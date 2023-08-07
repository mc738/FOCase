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

