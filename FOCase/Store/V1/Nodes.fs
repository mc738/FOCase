namespace FOCase.Store.V1

[<RequireQualifiedAccess>]
module Nodes =

    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store.V1.Persistence

    let add (ctx: SqliteContext) (id: IdType option) (name: string) =
        ({ Id = getId id
           Name = name
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNode)
        |> Operations.insertNode ctx

    let get (ctx: SqliteContext) (id: string) =
        Operations.selectNodeRecord ctx [ "WHERE id = @0" ] [ id ]

    let getMetadataValue (ctx: SqliteContext) (nodeId: string) (key: string) =
        Operations.selectNodeMetadataItemRecord ctx [ "WHERE node_id = @0 AND item_key = @1" ] [ nodeId; key ]

    let addMetadataValue (ctx: SqliteContext) (nodeId: string) (key: string) (value: string) =
        ({ NodeId = nodeId
           ItemKey = key
           ItemValue = value
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeMetadataItem)
        |> Operations.insertNodeMetadataItem ctx

    let tryAddMetadataValue (ctx: SqliteContext) (nodeId: string) (key: string) (value: string) =
        match getMetadataValue ctx nodeId key with
        | Some _ -> Error $"Metadata value `{key}` already exists for node `{nodeId}`"
        | None -> addMetadataValue ctx nodeId key value |> Ok

    let addNodeLabel (ctx: SqliteContext) (nodeId: string) (label: string) (weight: decimal) =
        ({ NodeId = nodeId
           Label = label
           Weight = weight
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeLabel)
        |> Operations.insertNodeLabel ctx
