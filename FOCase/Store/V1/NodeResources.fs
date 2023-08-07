namespace FOCase.Store.V1

[<RequireQualifiedAccess>]
module NodeResources =
    
    open System.IO
    open Freql.Core.Common.Types
    open FsToolbox.Extensions.Strings
    open FsToolbox.Extensions.Streams
    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store.V1.Persistence
        
    
    // *** Metadata ***

    let getMetadataValue (ctx: SqliteContext) (nodeResourceId: string) (key: string) =
        Operations.selectNodeResourceMetadataItemRecord ctx [ "WHERE node_resource_id = @0 AND item_key = @1" ] [ nodeResourceId; key ]

    let addMetadataValue (ctx: SqliteContext) (nodeResourceId: string) (key: string) (value: string) =
        ({ NodeResourceId = nodeResourceId
           ItemKey = key
           ItemValue = value
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeResourceMetadataItem)
        |> Operations.insertNodeResourceMetadataItem ctx

    let tryAddMetadataValue (ctx: SqliteContext) (nodeResourceId: string) (key: string) (value: string) =
        match getMetadataValue ctx nodeResourceId key with
        | Some _ -> Error $"Metadata value `{key}` already exists for node resource `{nodeResourceId}`"
        | None -> addMetadataValue ctx nodeResourceId key value |> Ok

    let updateMetadataValue (ctx: SqliteContext) (nodeResourceId: string) (key: string) (value: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_resource_metadata SET item_value = @0 WHERE node_resource_id = @1 AND item_key = @2",
            [ value; nodeResourceId; key ]
        )
        |> ignore

    let tryUpdateMetadataValue (ctx: SqliteContext) (nodeResourceId: string) (key: string) (value: string) =
        match getMetadataValue ctx nodeResourceId key with
        | Some _ -> updateMetadataValue ctx nodeResourceId key value |> Ok
        | None -> Error $"Metadata value `{key}` does not exist for node resource `{nodeResourceId}`"

    let addOrUpdateMetadataValue (ctx: SqliteContext) (nodeResourceId: string) (key: string) (value: string) =
        match getMetadataValue ctx nodeResourceId key with
        | Some _ -> updateMetadataValue ctx nodeResourceId key value
        | None -> addMetadataValue ctx nodeResourceId key value

    let activateMetadataItem (ctx: SqliteContext) (nodeResourceId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_resource_metadata SET active = TRUE WHERE node_resource_id = @0 AND item_key = @1",
            [ nodeResourceId; key ]
        )

    let deactivateMetadataItem (ctx: SqliteContext) (nodeResourceId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_resource_metadata SET active = FALSE WHERE node_resource_id = @0 AND item_key = @1",
            [ nodeResourceId; key ]
        )


    // *** Notes ***

    let getNote (ctx: SqliteContext) (noteId: string) =
        Operations.selectNodeResourceNoteRecord ctx [ "WHERE note_id = @0" ] [ noteId ]

    let getAllActiveNotes (ctx: SqliteContext) (nodeResourceId: string) =
        Operations.selectNodeResourceNoteRecord ctx [ "WHERE node_resource_id = @0 AND active = TRUE;" ] [ nodeResourceId ]

    let getAllNotes (ctx: SqliteContext) (nodeResourceId: string) =
        Operations.selectNodeResourceNoteRecord ctx [ "WHERE node_resource_id = @0" ] [ nodeResourceId ]

    let getLatestNoteVersion (ctx: SqliteContext) (noteId: string) =
        Operations.selectNodeResourceNoteVersionRecord
            ctx
            [ "WHERE node_resource_note_id = @0 ORDER BY version DESC LIMIT 1" ]
            [ noteId ]

    let getLatestActiveNoteVersion (ctx: SqliteContext) (noteId: string) =
        Operations.selectNodeResourceNoteVersionRecord
            ctx
            [ "WHERE node_resource_note_id = @0 AND active = TRUE ORDER BY version DESC LIMIT 1" ]
            [ noteId ]

    let activateNote (ctx: SqliteContext) (noteId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE node_resource_notes SET active = TRUE WHERE id = @0", [ noteId ])

    let deactivateNote (ctx: SqliteContext) (noteId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE node_resource_notes SET active = FALSE WHERE id = @0", [ noteId ])

    let activateNoteVersion (ctx: SqliteContext) (noteVersionId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE node_resource_note_versions SET active = TRUE WHERE id = @0", [ noteVersionId ])

    let deactivateNoteVersion (ctx: SqliteContext) (noteVersionId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE node_resource_note_versions SET active = FALSE WHERE id = @0", [ noteVersionId ])


    let addNote (ctx: SqliteContext) (id: IdType option) (nodeResourceId: string) =
        ({ Id = getId id
           NodeResourceId = nodeResourceId
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeResourceNote)
        |> Operations.insertNodeResourceNote ctx

    let addNoteVersion
        (ctx: SqliteContext)
        (id: IdType option)
        (noteId: string)
        (version: int)
        (title: string)
        (note: string)
        =
        use ms = new MemoryStream(note.ToUtf8Bytes())
        let hash = ms.GetSHA256Hash()

        ({ Id = getId id
           NodeResourceNoteId = noteId
           Version = version
           Title = title
           Note = BlobField.FromStream ms
           Hash = hash
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeResourceNoteVersion)
        |> Operations.insertNodeResourceNoteVersion ctx

    let tryAddLatestNoteVersion
        (ctx: SqliteContext)
        (id: IdType option)
        (noteId: string)
        (title: string)
        (note: string)
        =
        match getLatestNoteVersion ctx noteId with
        | Some lnv -> addNoteVersion ctx id noteId (lnv.Version + 1) title note |> Ok
        | None -> Error $"Node resource note `{noteId}` does not exist"

    let addNewNote (ctx: SqliteContext) (nodeResourceId: string) (title: string) (note: string) =
        let id = IdType.Create()
        addNote ctx (Some id) nodeResourceId
        addNoteVersion ctx None (id.GetId()) 1 title note

    let tryAddNewNote (ctx: SqliteContext) (nodeResourceId: string) (title: string) (note: string) =
        match get ctx node_resourceId with
        | Some _ -> addNewNote ctx nodeResourceId title note |> Ok
        | None -> Error $"Node resource `{nodeResourceId}` does not exist"