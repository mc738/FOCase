namespace FOCase.Store.V1

[<RequireQualifiedAccess>]
module ConnectionDocuments =
    
    open System.IO
    open Freql.Core.Common.Types
    open FsToolbox.Extensions.Strings
    open FsToolbox.Extensions.Streams
    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store.V1.Persistence
    
    // *** Metadata ***

    let getMetadataValue (ctx: SqliteContext) (connection_documentId: string) (key: string) =
        Operations.selectConnectionDocumentMetadataItemRecord ctx [ "WHERE connection_document_id = @0 AND item_key = @1" ] [ connection_documentId; key ]

    let addMetadataValue (ctx: SqliteContext) (connectionDocumentId: string) (key: string) (value: string) =
        ({ ConnectionDocumentId = connectionDocumentId
           ItemKey = key
           ItemValue = value
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewConnectionDocumentMetadataItem)
        |> Operations.insertConnectionDocumentMetadataItem ctx

    let tryAddMetadataValue (ctx: SqliteContext) (connectionDocumentId: string) (key: string) (value: string) =
        match getMetadataValue ctx connectionDocumentId key with
        | Some _ -> Error $"Metadata value `{key}` already exists for node document `{connectionDocumentId}`"
        | None -> addMetadataValue ctx connectionDocumentId key value |> Ok

    let updateMetadataValue (ctx: SqliteContext) (connectionDocumentId: string) (key: string) (value: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_document_metadata SET item_value = @0 WHERE connection_document_id = @1 AND item_key = @2",
            [ value; connectionDocumentId; key ]
        )
        |> ignore

    let tryUpdateMetadataValue (ctx: SqliteContext) (connectionDocumentId: string) (key: string) (value: string) =
        match getMetadataValue ctx connectionDocumentId key with
        | Some _ -> updateMetadataValue ctx connectionDocumentId key value |> Ok
        | None -> Error $"Metadata value `{key}` does not exist for node document `{connectionDocumentId}`"

    let addOrUpdateMetadataValue (ctx: SqliteContext) (connectionDocumentId: string) (key: string) (value: string) =
        match getMetadataValue ctx connectionDocumentId key with
        | Some _ -> updateMetadataValue ctx connectionDocumentId key value
        | None -> addMetadataValue ctx connectionDocumentId key value

    let activateMetadataItem (ctx: SqliteContext) (connectionDocumentId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_document_metadata SET active = TRUE WHERE connection_document_id = @0 AND item_key = @1",
            [ connectionDocumentId; key ]
        )

    let deactivateMetadataItem (ctx: SqliteContext) (connectionDocumentId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_document_metadata SET active = FALSE WHERE connection_document_id = @0 AND item_key = @1",
            [ connectionDocumentId; key ]
        )
    

    
    // *** Notes ***

    let getNote (ctx: SqliteContext) (noteId: string) =
        Operations.selectConnectionDocumentNoteRecord ctx [ "WHERE note_id = @0" ] [ noteId ]

    let getAllActiveNotes (ctx: SqliteContext) (connectionDocumentId: string) =
        Operations.selectConnectionDocumentNoteRecord ctx [ "WHERE connection_document_id = @0 AND active = TRUE;" ] [ connectionDocumentId ]

    let getAllNotes (ctx: SqliteContext) (connectionDocumentId: string) =
        Operations.selectConnectionDocumentNoteRecord ctx [ "WHERE connection_document_id = @0" ] [ connectionDocumentId ]

    let getLatestNoteVersion (ctx: SqliteContext) (noteId: string) =
        Operations.selectConnectionDocumentNoteVersionRecord
            ctx
            [ "WHERE connection_document_note_id = @0 ORDER BY version DESC LIMIT 1" ]
            [ noteId ]

    let getLatestActiveNoteVersion (ctx: SqliteContext) (noteId: string) =
        Operations.selectConnectionDocumentNoteVersionRecord
            ctx
            [ "WHERE connection_document_note_id = @0 AND active = TRUE ORDER BY version DESC LIMIT 1" ]
            [ noteId ]

    let activateNote (ctx: SqliteContext) (noteId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE connection_document_notes SET active = TRUE WHERE id = @0", [ noteId ])

    let deactivateNote (ctx: SqliteContext) (noteId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE connection_document_notes SET active = FALSE WHERE id = @0", [ noteId ])

    let activateNoteVersion (ctx: SqliteContext) (noteVersionId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE connection_document_note_versions SET active = TRUE WHERE id = @0", [ noteVersionId ])

    let deactivateNoteVersion (ctx: SqliteContext) (noteVersionId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE connection_document_note_versions SET active = FALSE WHERE id = @0", [ noteVersionId ])

    let addNote (ctx: SqliteContext) (id: IdType option) (connectionDocumentId: string) =
        ({ Id = getId id
           ConnectionDocumentId = connectionDocumentId
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewConnectionDocumentNote)
        |> Operations.insertConnectionDocumentNote ctx

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
           ConnectionDocumentNoteId = noteId
           Version = version
           Title = title
           Note = BlobField.FromStream ms
           Hash = hash
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewConnectionDocumentNoteVersion)
        |> Operations.insertConnectionDocumentNoteVersion ctx

    let tryAddLatestNoteVersion
        (ctx: SqliteContext)
        (id: IdType option)
        (noteId: string)
        (title: string)
        (note: string)
        =
        match getLatestNoteVersion ctx noteId with
        | Some lnv -> addNoteVersion ctx id noteId (lnv.Version + 1) title note |> Ok
        | None -> Error $"ConnectionDocument note `{noteId}` does not exist"

    let addNewNote (ctx: SqliteContext) (connectionDocumentId: string) (title: string) (note: string) =
        let id = IdType.Create()
        addNote ctx (Some id) connectionDocumentId
        addNoteVersion ctx None (id.GetId()) 1 title note

    let tryAddNewNote (ctx: SqliteContext) (connectionDocumentId: string) (title: string) (note: string) =
        match get ctx connectionDocumentId with
        | Some _ -> addNewNote ctx connectionDocumentId title note |> Ok
        | None -> Error $"Node document `{connectionDocumentId}` does not exist"

