namespace FOCase.Store.V1

[<RequireQualifiedAccess>]
module Resources =
    
    open System.IO
    open FsToolbox.Extensions.Streams
    open FsToolbox.Extensions.Strings
    open Freql.Core.Common.Types
    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store.V1.Persistence

    // *** General ***

    let add (ctx: SqliteContext) (id: IdType option) (name: string) =
        ({ Id = getId id
           Name = name
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewResource)
        |> Operations.insertResource ctx

    let get (ctx: SqliteContext) (id: string) =
        Operations.selectResourceRecord ctx [ "WHERE id = @0" ] [ id ]
        
    let getAll (ctx:SqliteContext) =
        Operations.selectResourceRecords ctx [] []
        
    let getAllActive (ctx: SqliteContext) =
        Operations.selectResourceRecords ctx [ "WHERE active = TRUE" ] []
    
    let activate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resources SET active = TRUE WHERE id = @0", [ id ])

    let deactivate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resources SET active = FALSE WHERE id = @0", [ id ])

    // *** Metadata ***
    
    let getMetadataValue (ctx: SqliteContext) (resourceId: string) (key: string) =
        Operations.selectResourceMetadataItemRecord ctx [ "WHERE resource_id = @0 AND item_key = @1" ] [ resourceId; key ]

    let addMetadataValue (ctx: SqliteContext) (resourceId: string) (key: string) (value: string) =
        ({ ResourceId = resourceId
           ItemKey = key
           ItemValue = value
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewResourceMetadataItem)
        |> Operations.insertResourceMetadataItem ctx

    let tryAddMetadataValue (ctx: SqliteContext) (resourceId: string) (key: string) (value: string) =
        match getMetadataValue ctx resourceId key with
        | Some _ -> Error $"Metadata value `{key}` already exists for resource `{resourceId}`"
        | None -> addMetadataValue ctx resourceId key value |> Ok

    let updateMetadataValue (ctx: SqliteContext) (resourceId: string) (key: string) (value: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE resource_metadata SET item_value = @0 WHERE resource_id = @1 AND item_key = @2",
            [ value; resourceId; key ]
        )
        |> ignore

    let tryUpdateMetadataValue (ctx: SqliteContext) (resourceId: string) (key: string) (value: string) =
        match getMetadataValue ctx resourceId key with
        | Some _ -> updateMetadataValue ctx resourceId key value |> Ok
        | None -> Error $"Metadata value `{key}` does not exist for resource `{resourceId}`"

    let addOrUpdateMetadataValue (ctx: SqliteContext) (resourceId: string) (key: string) (value: string) =
        match getMetadataValue ctx resourceId key with
        | Some _ -> updateMetadataValue ctx resourceId key value
        | None -> addMetadataValue ctx resourceId key value

    let activateMetadataItem (ctx: SqliteContext) (resourceId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE resource_metadata SET active = TRUE WHERE resource_id = @0 AND item_key = @1",
            [ resourceId; key ]
        )

    let deactivateMetadataItem (ctx: SqliteContext) (resourceId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE resource_metadata SET active = FALSE WHERE resource_id = @0 AND item_key = @1",
            [ resourceId; key ]
        )
        
    // *** Tags ***

    let getResourceTag (ctx: SqliteContext) (resourceId: string) (tag: string) =
        Operations.selectResourceTagRecord ctx [ "WHERE resource_id = @0 AND tag = @1" ] [ resourceId; tag ]

    let getAllResourceTags (ctx: SqliteContext) (resourceId: string) =
        Operations.selectResourceTagRecords ctx [ "WHERE resource_id = @0" ] [ resourceId ]

    let getAllActiveResourceTags (ctx: SqliteContext) (resourceId: string) =
        Operations.selectResourceTagRecords ctx [ "WHERE resource_id = @0 AND active = TRUE" ] [ resourceId ]

    let addResourceTag (ctx: SqliteContext) (resourceId: string) (tag: string) =
        ({ ResourceId = resourceId
           Tag = tag
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewResourceTag)
        |> Operations.insertResourceTag ctx

    let tryAddResourceTag (ctx: SqliteContext) (resourceId: string) (tag: string) =
        match Tags.get ctx tag, get ctx resourceId, getResourceTag ctx resourceId tag with
        | None, _, _ -> Error $"Tag `{tag}` not found"
        | _, None, _ -> Error $"Resource `{resourceId}` not found"
        | _, _, Some _ -> Error $"Tag `{tag}` already attached to resource `{resourceId}`"
        | Some l, Some n, None -> addResourceTag ctx n.Id l.Name |> Ok

    let activateResourceTag (ctx: SqliteContext) (resourceId: string) (tag: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE resource_tags SET active = TRUE WHERE resource_id = @0 AND tag = @1",
            [ resourceId; tag ]
        )

    let deactivateResourceTag (ctx: SqliteContext) (resourceId: string) (tag: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE resource_tags SET active = FALSE WHERE resource_id = @0 AND tag = @1",
            [ resourceId; tag ]
        )
        
    // *** Notes ***

    let getNote (ctx: SqliteContext) (noteId: string) =
        Operations.selectResourceNoteRecord ctx [ "WHERE note_id = @0" ] [ noteId ]

    let getAllActiveNotes (ctx: SqliteContext) (resourceId: string) =
        Operations.selectResourceNoteRecord ctx [ "WHERE resource_id = @0 AND active = TRUE;" ] [ resourceId ]

    let getAllNotes (ctx: SqliteContext) (resourceId: string) =
        Operations.selectResourceNoteRecord ctx [ "WHERE resource_id = @0" ] [ resourceId ]

    let getLatestNoteVersion (ctx: SqliteContext) (noteId: string) =
        Operations.selectResourceNoteVersionRecord
            ctx
            [ "WHERE resource_note_id = @0 ORDER BY version DESC LIMIT 1" ]
            [ noteId ]

    let getLatestActiveNoteVersion (ctx: SqliteContext) (noteId: string) =
        Operations.selectResourceNoteVersionRecord
            ctx
            [ "WHERE resource_note_id = @0 AND active = TRUE ORDER BY version DESC LIMIT 1" ]
            [ noteId ]

    let activateNote (ctx: SqliteContext) (noteId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_notes SET active = TRUE WHERE id = @0", [ noteId ])

    let deactivateNote (ctx: SqliteContext) (noteId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_notes SET active = FALSE WHERE id = @0", [ noteId ])

    let activateNoteVersion (ctx: SqliteContext) (noteVersionId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_note_versions SET active = TRUE WHERE id = @0", [ noteVersionId ])

    let deactivateNoteVersion (ctx: SqliteContext) (noteVersionId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_note_versions SET active = FALSE WHERE id = @0", [ noteVersionId ])


    let addNote (ctx: SqliteContext) (id: IdType option) (resourceId: string) =
        ({ Id = getId id
           ResourceId = resourceId
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewResourceNote)
        |> Operations.insertResourceNote ctx

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
           ResourceNoteId = noteId
           Version = version
           Title = title
           Note = BlobField.FromStream ms
           Hash = hash
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewResourceNoteVersion)
        |> Operations.insertResourceNoteVersion ctx

    let tryAddLatestNoteVersion
        (ctx: SqliteContext)
        (id: IdType option)
        (noteId: string)
        (title: string)
        (note: string)
        =
        match getLatestNoteVersion ctx noteId with
        | Some lnv -> addNoteVersion ctx id noteId (lnv.Version + 1) title note |> Ok
        | None -> Error $"Resource note `{noteId}` does not exist"

    let addNewNote (ctx: SqliteContext) (resourceId: string) (title: string) (note: string) =
        let id = IdType.Create()
        addNote ctx (Some id) resourceId
        addNoteVersion ctx None (id.GetId()) 1 title note

    let tryAddNewNote (ctx: SqliteContext) (resourceId: string) (title: string) (note: string) =
        match get ctx resourceId with
        | Some _ -> addNewNote ctx resourceId title note |> Ok
        | None -> Error $"Resource `{resourceId}` does not exist"