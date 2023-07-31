namespace FOCase.Store.V1

open System.Reflection.Emit

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

    let getNodeLabel (ctx: SqliteContext) (nodeId: string) (label: string) =
        Operations.selectNodeLabelRecord ctx [ "WHERE node_id = @0 AND label = @1" ] [ nodeId; label ]

    let addNodeLabel (ctx: SqliteContext) (nodeId: string) (label: string) (weight: decimal) =
        ({ NodeId = nodeId
           Label = label
           Weight = weight
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeLabel)
        |> Operations.insertNodeLabel ctx

    let tryAddNodeLabel (ctx: SqliteContext) (nodeId: string) (label: string) (weight: decimal) =
        match Labels.get ctx label, get ctx nodeId, getNodeLabel ctx nodeId label with
        | None, _, _ -> Error $"Label `{label}` not found"
        | _, None, _ -> Error $"Node `{nodeId}` not found"
        | _, _, Some _ -> Error $"Label `{label}` already attached to node `{nodeId}`"
        | Some l, Some n, None -> addNodeLabel ctx n.Id l.Name weight |> Ok

    let getNodeTag (ctx: SqliteContext) (nodeId: string) (label: string) =
        Operations.selectNodeTagRecord ctx [ "WHERE node_id = @0 AND tag = @1" ] [ nodeId; label ]

    let addNodeTag (ctx: SqliteContext) (nodeId: string) (tag: string) =
        ({ NodeId = nodeId
           Tag = tag
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeTag)
        |> Operations.insertNodeTag ctx

    let tryAddNodeTag (ctx: SqliteContext) (nodeId: string) (tag: string) =
        match Tags.get ctx tag, get ctx nodeId, getNodeTag ctx nodeId tag with
        | None, _, _ -> Error $"Tag `{tag}` not found"
        | _, None, _ -> Error $"Node `{nodeId}` not found"
        | _, _, Some _ -> Error $"Tag `{tag}` already attached to node `{nodeId}`"
        | Some l, Some n, None -> addNodeTag ctx n.Id l.Name |> Ok
