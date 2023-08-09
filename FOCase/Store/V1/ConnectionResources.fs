namespace FOCase.Store.V1

[<RequireQualifiedAccess>]
module ConnectionResources =
    
    open System.IO
    open Freql.Core.Common.Types
    open FsToolbox.Extensions.Strings
    open FsToolbox.Extensions.Streams
    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store.V1.Persistence
        
    let get (ctx: SqliteContext) (connectionResourceId: string) =
        Operations.selectConnectionResourceRecord ctx [ "WHERE id = @0" ] [ connectionResourceId ]
        
    let getAllForConnection (ctx:SqliteContext) (connectionId: string) =
        Operations.selectConnectionResourceRecords ctx [ "WHERE connection_id = @0" ] [ connectionId ]
        
    let getAllForResourceVersion (ctx:SqliteContext) (resourceVersionId: string) =
        Operations.selectConnectionResourceRecords ctx [ "WHERE resource_version_id = @0" ] [ resourceVersionId ]
    
    // *** Metadata ***

    let getMetadataValue (ctx: SqliteContext) (connectionResourceId: string) (key: string) =
        Operations.selectConnectionResourceMetadataItemRecord ctx [ "WHERE connection_resource_id = @0 AND item_key = @1" ] [ connectionResourceId; key ]

    let addMetadataValue (ctx: SqliteContext) (connectionResourceId: string) (key: string) (value: string) =
        ({ ConnectionResourceId = connectionResourceId
           ItemKey = key
           ItemValue = value
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewConnectionResourceMetadataItem)
        |> Operations.insertConnectionResourceMetadataItem ctx

    let tryAddMetadataValue (ctx: SqliteContext) (connectionResourceId: string) (key: string) (value: string) =
        match getMetadataValue ctx connectionResourceId key with
        | Some _ -> Error $"Metadata value `{key}` already exists for node resource `{connectionResourceId}`"
        | None -> addMetadataValue ctx connectionResourceId key value |> Ok

    let updateMetadataValue (ctx: SqliteContext) (connectionResourceId: string) (key: string) (value: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_resource_metadata SET item_value = @0 WHERE connection_resource_id = @1 AND item_key = @2",
            [ value; connectionResourceId; key ]
        )
        |> ignore

    let tryUpdateMetadataValue (ctx: SqliteContext) (connectionResourceId: string) (key: string) (value: string) =
        match getMetadataValue ctx connectionResourceId key with
        | Some _ -> updateMetadataValue ctx connectionResourceId key value |> Ok
        | None -> Error $"Metadata value `{key}` does not exist for node resource `{connectionResourceId}`"

    let addOrUpdateMetadataValue (ctx: SqliteContext) (connectionResourceId: string) (key: string) (value: string) =
        match getMetadataValue ctx connectionResourceId key with
        | Some _ -> updateMetadataValue ctx connectionResourceId key value
        | None -> addMetadataValue ctx connectionResourceId key value

    let activateMetadataItem (ctx: SqliteContext) (connectionResourceId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_resource_metadata SET active = TRUE WHERE connection_resource_id = @0 AND item_key = @1",
            [ connectionResourceId; key ]
        )

    let deactivateMetadataItem (ctx: SqliteContext) (connectionResourceId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_resource_metadata SET active = FALSE WHERE connection_resource_id = @0 AND item_key = @1",
            [ connectionResourceId; key ]
        )

    // *** Notes ***

    let getNote (ctx: SqliteContext) (noteId: string) =
        Operations.selectConnectionResourceNoteRecord ctx [ "WHERE note_id = @0" ] [ noteId ]

    let getAllActiveNotes (ctx: SqliteContext) (connectionResourceId: string) =
        Operations.selectConnectionResourceNoteRecord ctx [ "WHERE connection_resource_id = @0 AND active = TRUE;" ] [ connectionResourceId ]

    let getAllNotes (ctx: SqliteContext) (connectionResourceId: string) =
        Operations.selectConnectionResourceNoteRecord ctx [ "WHERE connection_resource_id = @0" ] [ connectionResourceId ]

    let getLatestNoteVersion (ctx: SqliteContext) (noteId: string) =
        Operations.selectConnectionResourceNoteVersionRecord
            ctx
            [ "WHERE connection_resource_note_id = @0 ORDER BY version DESC LIMIT 1" ]
            [ noteId ]

    let getLatestActiveNoteVersion (ctx: SqliteContext) (noteId: string) =
        Operations.selectConnectionResourceNoteVersionRecord
            ctx
            [ "WHERE connection_resource_note_id = @0 AND active = TRUE ORDER BY version DESC LIMIT 1" ]
            [ noteId ]

    let activateNote (ctx: SqliteContext) (noteId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE connection_resource_notes SET active = TRUE WHERE id = @0", [ noteId ])

    let deactivateNote (ctx: SqliteContext) (noteId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE connection_resource_notes SET active = FALSE WHERE id = @0", [ noteId ])

    let activateNoteVersion (ctx: SqliteContext) (noteVersionId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE connection_resource_note_versions SET active = TRUE WHERE id = @0", [ noteVersionId ])

    let deactivateNoteVersion (ctx: SqliteContext) (noteVersionId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE connection_resource_note_versions SET active = FALSE WHERE id = @0", [ noteVersionId ])


    let addNote (ctx: SqliteContext) (id: IdType option) (connectionResourceId: string) =
        ({ Id = getId id
           ConnectionResourceId = connectionResourceId
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewConnectionResourceNote)
        |> Operations.insertConnectionResourceNote ctx

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
           ConnectionResourceNoteId = noteId
           Version = version
           Title = title
           Note = BlobField.FromStream ms
           Hash = hash
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewConnectionResourceNoteVersion)
        |> Operations.insertConnectionResourceNoteVersion ctx

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

    let addNewNote (ctx: SqliteContext) (connectionResourceId: string) (title: string) (note: string) =
        let id = IdType.Create()
        addNote ctx (Some id) connectionResourceId
        addNoteVersion ctx None (id.GetId()) 1 title note

    let tryAddNewNote (ctx: SqliteContext) (connectionResourceId: string) (title: string) (note: string) =
        match get ctx connectionResourceId with
        | Some _ -> addNewNote ctx connectionResourceId title note |> Ok
        | None -> Error $"Node resource `{connectionResourceId}` does not exist"