namespace FOCase.Store.V1


module NodeDocuments =

    open System.IO
    open Freql.Core.Common.Types
    open FsToolbox.Extensions.Strings
    open FsToolbox.Extensions.Streams
    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store.V1.Persistence
        
    //let get 
    
    //()

    // *** Metadata ***

    let getMetadataValue (ctx: SqliteContext) (node_documentId: string) (key: string) =
        Operations.selectNodeDocumentMetadataItemRecord ctx [ "WHERE node_document_id = @0 AND item_key = @1" ] [ node_documentId; key ]

    let addMetadataValue (ctx: SqliteContext) (node_documentId: string) (key: string) (value: string) =
        ({ NodeDocumentId = node_documentId
           ItemKey = key
           ItemValue = value
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeDocumentMetadataItem)
        |> Operations.insertNodeDocumentMetadataItem ctx

    let tryAddMetadataValue (ctx: SqliteContext) (node_documentId: string) (key: string) (value: string) =
        match getMetadataValue ctx node_documentId key with
        | Some _ -> Error $"Metadata value `{key}` already exists for node_document `{node_documentId}`"
        | None -> addMetadataValue ctx node_documentId key value |> Ok

    let updateMetadataValue (ctx: SqliteContext) (node_documentId: string) (key: string) (value: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_document_metadata SET item_value = @0 WHERE node_document_id = @1 AND item_key = @2",
            [ value; node_documentId; key ]
        )
        |> ignore

    let tryUpdateMetadataValue (ctx: SqliteContext) (node_documentId: string) (key: string) (value: string) =
        match getMetadataValue ctx node_documentId key with
        | Some _ -> updateMetadataValue ctx node_documentId key value |> Ok
        | None -> Error $"Metadata value `{key}` does not exist for node_document `{node_documentId}`"

    let addOrUpdateMetadataValue (ctx: SqliteContext) (node_documentId: string) (key: string) (value: string) =
        match getMetadataValue ctx node_documentId key with
        | Some _ -> updateMetadataValue ctx node_documentId key value
        | None -> addMetadataValue ctx node_documentId key value

    let activateMetadataItem (ctx: SqliteContext) (node_documentId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_document_metadata SET active = TRUE WHERE node_document_id = @0 AND item_key = @1",
            [ node_documentId; key ]
        )

    let deactivateMetadataItem (ctx: SqliteContext) (node_documentId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_document_metadata SET active = FALSE WHERE node_document_id = @0 AND item_key = @1",
            [ node_documentId; key ]
        )
