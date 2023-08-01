namespace FOCase.Store.V1


[<RequireQualifiedAccess>]
module Documents =

    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store.V1.Persistence

    let add (ctx: SqliteContext) (id: IdType option) (name: string) =
        ({ Id = getId id; Name = name }: Parameters.NewDocument)
        |> Operations.insertDocument ctx

    let get (ctx: SqliteContext) (id: string) =
        Operations.selectDocumentRecord ctx [ "WHERE id = @0" ] [ id ]


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
