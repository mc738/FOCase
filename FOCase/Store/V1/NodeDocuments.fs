namespace FOCase.Store.V1

[<RequireQualifiedAccess>]
module NodeDocuments =

    open System.IO
    open Freql.Core.Common.Types
    open FsToolbox.Extensions.Strings
    open FsToolbox.Extensions.Streams
    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store.V1.Persistence
        
    let get (ctx: SqliteContext) (nodeDocumentId: string) =
        Operations.selectNodeDocumentRecord ctx [ "WHERE id = @0" ] [ nodeDocumentId ]
        
    // *** Metadata ***

    let getMetadataValue (ctx: SqliteContext) (node_documentId: string) (key: string) =
        Operations.selectNodeDocumentMetadataItemRecord ctx [ "WHERE node_document_id = @0 AND item_key = @1" ] [ node_documentId; key ]

    let addMetadataValue (ctx: SqliteContext) (nodeDocumentId: string) (key: string) (value: string) =
        ({ NodeDocumentId = nodeDocumentId
           ItemKey = key
           ItemValue = value
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeDocumentMetadataItem)
        |> Operations.insertNodeDocumentMetadataItem ctx

    let tryAddMetadataValue (ctx: SqliteContext) (nodeDocumentId: string) (key: string) (value: string) =
        match getMetadataValue ctx nodeDocumentId key with
        | Some _ -> Error $"Metadata value `{key}` already exists for node document `{nodeDocumentId}`"
        | None -> addMetadataValue ctx nodeDocumentId key value |> Ok

    let updateMetadataValue (ctx: SqliteContext) (nodeDocumentId: string) (key: string) (value: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_document_metadata SET item_value = @0 WHERE node_document_id = @1 AND item_key = @2",
            [ value; nodeDocumentId; key ]
        )
        |> ignore

    let tryUpdateMetadataValue (ctx: SqliteContext) (nodeDocumentId: string) (key: string) (value: string) =
        match getMetadataValue ctx nodeDocumentId key with
        | Some _ -> updateMetadataValue ctx nodeDocumentId key value |> Ok
        | None -> Error $"Metadata value `{key}` does not exist for node document `{nodeDocumentId}`"

    let addOrUpdateMetadataValue (ctx: SqliteContext) (nodeDocumentId: string) (key: string) (value: string) =
        match getMetadataValue ctx nodeDocumentId key with
        | Some _ -> updateMetadataValue ctx nodeDocumentId key value
        | None -> addMetadataValue ctx nodeDocumentId key value

    let activateMetadataItem (ctx: SqliteContext) (nodeDocumentId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_document_metadata SET active = TRUE WHERE node_document_id = @0 AND item_key = @1",
            [ nodeDocumentId; key ]
        )

    let deactivateMetadataItem (ctx: SqliteContext) (nodeDocumentId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_document_metadata SET active = FALSE WHERE node_document_id = @0 AND item_key = @1",
            [ nodeDocumentId; key ]
        )
        
    
    // *** Notes ***

    let getNote (ctx: SqliteContext) (noteId: string) =
        Operations.selectNodeDocumentNoteRecord ctx [ "WHERE note_id = @0" ] [ noteId ]

    let getAllActiveNotes (ctx: SqliteContext) (nodeDocumentId: string) =
        Operations.selectNodeDocumentNoteRecord ctx [ "WHERE node_document_id = @0 AND active = TRUE;" ] [ nodeDocumentId ]

    let getAllNotes (ctx: SqliteContext) (nodeDocumentId: string) =
        Operations.selectNodeDocumentNoteRecord ctx [ "WHERE node_document_id = @0" ] [ nodeDocumentId ]

    let getLatestNoteVersion (ctx: SqliteContext) (noteId: string) =
        Operations.selectNodeDocumentNoteVersionRecord
            ctx
            [ "WHERE node_document_note_id = @0 ORDER BY version DESC LIMIT 1" ]
            [ noteId ]

    let getLatestActiveNoteVersion (ctx: SqliteContext) (noteId: string) =
        Operations.selectNodeDocumentNoteVersionRecord
            ctx
            [ "WHERE node_document_note_id = @0 AND active = TRUE ORDER BY version DESC LIMIT 1" ]
            [ noteId ]

    let activateNote (ctx: SqliteContext) (noteId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE node_document_notes SET active = TRUE WHERE id = @0", [ noteId ])

    let deactivateNote (ctx: SqliteContext) (noteId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE node_document_notes SET active = FALSE WHERE id = @0", [ noteId ])

    let activateNoteVersion (ctx: SqliteContext) (noteVersionId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE node_document_note_versions SET active = TRUE WHERE id = @0", [ noteVersionId ])

    let deactivateNoteVersion (ctx: SqliteContext) (noteVersionId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE node_document_note_versions SET active = FALSE WHERE id = @0", [ noteVersionId ])

    let addNote (ctx: SqliteContext) (id: IdType option) (nodeDocumentId: string) =
        ({ Id = getId id
           NodeDocumentId = nodeDocumentId
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeDocumentNote)
        |> Operations.insertNodeDocumentNote ctx

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
           NodeDocumentNoteId = noteId
           Version = version
           Title = title
           Note = BlobField.FromStream ms
           Hash = hash
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeDocumentNoteVersion)
        |> Operations.insertNodeDocumentNoteVersion ctx

    let tryAddLatestNoteVersion
        (ctx: SqliteContext)
        (id: IdType option)
        (noteId: string)
        (title: string)
        (note: string)
        =
        match getLatestNoteVersion ctx noteId with
        | Some lnv -> addNoteVersion ctx id noteId (lnv.Version + 1) title note |> Ok
        | None -> Error $"NodeDocument note `{noteId}` does not exist"

    let addNewNote (ctx: SqliteContext) (nodeDocumentId: string) (title: string) (note: string) =
        let id = IdType.Create()
        addNote ctx (Some id) nodeDocumentId
        addNoteVersion ctx None (id.GetId()) 1 title note

    let tryAddNewNote (ctx: SqliteContext) (nodeDocumentId: string) (title: string) (note: string) =
        match get ctx nodeDocumentId with
        | Some _ -> addNewNote ctx nodeDocumentId title note |> Ok
        | None -> Error $"Node document `{nodeDocumentId}` does not exist"
