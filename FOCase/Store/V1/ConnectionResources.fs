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

    
    ()

