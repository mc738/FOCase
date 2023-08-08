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
    
    
    ()

