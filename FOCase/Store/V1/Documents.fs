namespace FOCase.Store.V1


[<RequireQualifiedAccess>]
module Documents =

    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store.V1.Persistence

    // *** General ***

    let add (ctx: SqliteContext) (id: IdType option) (name: string) =
        ({ Id = getId id
           Name = name
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewDocument)
        |> Operations.insertDocument ctx

    let get (ctx: SqliteContext) (id: string) =
        Operations.selectDocumentRecord ctx [ "WHERE id = @0" ] [ id ]

    let getAll (ctx: SqliteContext) =
        Operations.selectDocumentRecord ctx [] []

    let getAllActive (ctx: SqliteContext) =
        Operations.selectDocumentRecord ctx [ "WHERE active = TRUE" ] []

    let activate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE documents SET active = TRUE WHERE id = @0", [ id ])

    let deactivate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE documents SET active = FALSE WHERE id = @0", [ id ])

    // *** Meta data ***
    let getMetadataValue (ctx: SqliteContext) (documentId: string) (key: string) =
        Operations.selectDocumentMetadataItemRecord
            ctx
            [ "WHERE document_id = @0 AND item_key = @1" ]
            [ documentId; key ]

    let addMetadataValue (ctx: SqliteContext) (documentId: string) (key: string) (value: string) =
        ({ DocumentId = documentId
           ItemKey = key
           ItemValue = value
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewDocumentMetadataItem)
        |> Operations.insertDocumentMetadataItem ctx

    let tryAddMetadataValue (ctx: SqliteContext) (documentId: string) (key: string) (value: string) =
        match getMetadataValue ctx documentId key with
        | Some _ -> Error $"Metadata value `{key}` already exists for document `{documentId}`"
        | None -> addMetadataValue ctx documentId key value |> Ok

    let updateMetadataValue (ctx: SqliteContext) (documentId: string) (key: string) (value: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE document_metadata SET item_value = @0 WHERE document_id = @1 AND item_key = @2",
            [ value; documentId; key ]
        )
        |> ignore

    let tryUpdateMetadataValue (ctx: SqliteContext) (documentId: string) (key: string) (value: string) =
        match getMetadataValue ctx documentId key with
        | Some _ -> updateMetadataValue ctx documentId key value |> Ok
        | None -> Error $"Metadata value `{key}` does not exist for document `{documentId}`"

    let addOrUpdateMetadataValue (ctx: SqliteContext) (documentId: string) (key: string) (value: string) =
        match getMetadataValue ctx documentId key with
        | Some _ -> updateMetadataValue ctx documentId key value
        | None -> addMetadataValue ctx documentId key value

    let activateMetadataItem (ctx: SqliteContext) (documentId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE document_metadata SET active = TRUE WHERE document_id = @0 AND item_key = @1",
            [ documentId; key ]
        )

    let deactivateMetadataItem (ctx: SqliteContext) (documentId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE document_metadata SET active = FALSE WHERE document_id = @0 AND item_key = @1",
            [ documentId; key ]
        )

    // *** Tags ***

    let getDocumentTag (ctx: SqliteContext) (documentId: string) (tag: string) =
        Operations.selectDocumentTagRecord ctx [ "WHERE document_id = @0 AND tag = @1" ] [ documentId; tag ]

    let getAllDocumentTags (ctx: SqliteContext) (documentId: string) =
        Operations.selectDocumentTagRecords ctx [ "WHERE document_id = @0" ] [ documentId ]

    let getAllActiveDocumentTags (ctx: SqliteContext) (documentId: string) =
        Operations.selectDocumentTagRecords ctx [ "WHERE document_id = @0 AND active = TRUE" ] [ documentId ]

    let addDocumentTag (ctx: SqliteContext) (documentId: string) (tag: string) =
        ({ DocumentId = documentId
           Tag = tag
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewDocumentTag)
        |> Operations.insertDocumentTag ctx

    let tryAddDocumentTag (ctx: SqliteContext) (documentId: string) (tag: string) =
        match Tags.get ctx tag, get ctx documentId, getDocumentTag ctx documentId tag with
        | None, _, _ -> Error $"Tag `{tag}` not found"
        | _, None, _ -> Error $"Document `{documentId}` not found"
        | _, _, Some _ -> Error $"Tag `{tag}` already attached to document `{documentId}`"
        | Some l, Some n, None -> addDocumentTag ctx n.Id l.Name |> Ok

    let activateDocumentTag (ctx: SqliteContext) (documentId: string) (tag: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE document_tags SET active = TRUE WHERE document_id = @0 AND tag = @1",
            [ documentId; tag ]
        )

    let deactivateDocumentTag (ctx: SqliteContext) (documentId: string) (tag: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE document_tags SET active = FALSE WHERE document_id = @0 AND tag = @1",
            [ documentId; tag ]
        )