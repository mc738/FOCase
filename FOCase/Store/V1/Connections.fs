namespace FOCase.Store.V1

[<RequireQualifiedAccess>]
module Connections =

    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store
    open FOCase.Store.V1.Persistence

    // *** General ***
    
    let add
        (ctx: SqliteContext)
        (id: IdType option)
        (name: string)
        (fromConnection: string)
        (toConnection: string)
        (twoWay: bool)
        =
        ({ Id = getId id
           Name = name
           FromNode = fromConnection
           ToNode = toConnection
           TwoWay = twoWay
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewConnection)
        |> Operations.insertConnection ctx

    let get (ctx: SqliteContext) (id: string) =
        Operations.selectConnectionRecord ctx [ "WHERE id = @0" ] [ id ]

    let getActiveFromConnection (ctx: SqliteContext) (connectionId: string) =
        Operations.selectConnectionRecord ctx [ "WHERE from_node = @0 AND active = TRUE" ] [ connectionId ]

    let getActiveToConnection (ctx: SqliteContext) (connectionId: string) =
        Operations.selectConnectionRecord ctx [ "WHERE to_node = @0 AND active = TRUE" ] [ connectionId ]

    // *** Metadata ***

    let getMetadataValue (ctx: SqliteContext) (connectionId: string) (key: string) =
        Operations.selectConnectionMetadataItemRecord ctx [ "WHERE connection_id = @0 AND item_key = @1" ] [ connectionId; key ]

    let addMetadataValue (ctx: SqliteContext) (connectionId: string) (key: string) (value: string) =
        ({ ConnectionId = connectionId
           ItemKey = key
           ItemValue = value
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewConnectionMetadataItem)
        |> Operations.insertConnectionMetadataItem ctx

    let tryAddMetadataValue (ctx: SqliteContext) (connectionId: string) (key: string) (value: string) =
        match getMetadataValue ctx connectionId key with
        | Some _ -> Error $"Metadata value `{key}` already exists for connection `{connectionId}`"
        | None -> addMetadataValue ctx connectionId key value |> Ok

    let updateMetadataValue (ctx: SqliteContext) (connectionId: string) (key: string) (value: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_metadata SET item_value = @0 WHERE connection_id = @1 AND item_key = @2",
            [ value; connectionId; key ]
        )
        |> ignore

    let tryUpdateMetadataValue (ctx: SqliteContext) (connectionId: string) (key: string) (value: string) =
        match getMetadataValue ctx connectionId key with
        | Some _ -> updateMetadataValue ctx connectionId key value |> Ok
        | None -> Error $"Metadata value `{key}` does not exist for connection `{connectionId}`"

    let addOrUpdateMetadataValue (ctx: SqliteContext) (connectionId: string) (key: string) (value: string) =
        match getMetadataValue ctx connectionId key with
        | Some _ -> updateMetadataValue ctx connectionId key value
        | None -> addMetadataValue ctx connectionId key value

    let activateMetadataItem (ctx: SqliteContext) (connectionId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_metadata SET active = TRUE WHERE connection_id = @0 AND item_key = @1",
            [ connectionId; key ]
        )

    let deactivateMetadataItem (ctx: SqliteContext) (connectionId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_metadata SET active = FALSE WHERE connection_id = @0 AND item_key = @1",
            [ connectionId; key ]
        )

    // *** Labels

    let getConnectionLabel (ctx: SqliteContext) (connectionId: string) (label: string) =
        Operations.selectConnectionLabelRecord ctx [ "WHERE connection_id = @0 AND label = @1" ] [ connectionId; label ]

    let getAllConnectionLabels (ctx: SqliteContext) (connectionId: string) =
        Operations.selectConnectionLabelRecords ctx [ "WHERE connection_id = @0" ] [ connectionId ]

    let getAllActiveConnectionLabels (ctx: SqliteContext) (connectionId: string) =
        Operations.selectConnectionLabelRecords ctx [ "WHERE connection_id = @0 AND active = TRUE" ] [ connectionId ]

    let addConnectionLabel (ctx: SqliteContext) (connectionId: string) (label: string) (weight: decimal) =
        ({ ConnectionId = connectionId
           Label = label
           Weight = weight
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewConnectionLabel)
        |> Operations.insertConnectionLabel ctx

    let tryAddConnectionLabel (ctx: SqliteContext) (connectionId: string) (label: string) (weight: decimal) =
        match Labels.get ctx label, get ctx connectionId, getConnectionLabel ctx connectionId label with
        | None, _, _ -> Error $"Label `{label}` not found"
        | _, None, _ -> Error $"Connection `{connectionId}` not found"
        | _, _, Some _ -> Error $"Label `{label}` already attached to connection `{connectionId}`"
        | Some l, Some n, None -> addConnectionLabel ctx n.Id l.Name weight |> Ok

    let activateConnectionLabel (ctx: SqliteContext) (connectionId: string) (label: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_labels SET active = TRUE WHERE connection_id = @0 AND label = @1",
            [ connectionId; label ]
        )

    let deactivateConnectionLabel (ctx: SqliteContext) (connectionId: string) (label: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_labels SET active = FALSE WHERE connection_id = @0 AND label = @1",
            [ connectionId; label ]
        )

    let updateConnectionLabelWeight (ctx: SqliteContext) (connectionId: string) (label: string) (weight: decimal) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_labels SET weight = @0 WHERE connection_id = @1 AND label = @2",
            [ weight; connectionId; label ]
        )

    // *** Tags ***

    let getConnectionTag (ctx: SqliteContext) (connectionId: string) (tag: string) =
        Operations.selectConnectionTagRecord ctx [ "WHERE connection_id = @0 AND tag = @1" ] [ connectionId; tag ]

    let getAllConnectionTags (ctx: SqliteContext) (connectionId: string) =
        Operations.selectConnectionTagRecords ctx [ "WHERE connection_id = @0" ] [ connectionId ]

    let getAllActiveConnectionTags (ctx: SqliteContext) (connectionId: string) =
        Operations.selectConnectionTagRecords ctx [ "WHERE connection_id = @0 AND active = TRUE" ] [ connectionId ]

    let addConnectionTag (ctx: SqliteContext) (connectionId: string) (tag: string) =
        ({ ConnectionId = connectionId
           Tag = tag
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewConnectionTag)
        |> Operations.insertConnectionTag ctx

    let tryAddConnectionTag (ctx: SqliteContext) (connectionId: string) (tag: string) =
        match Tags.get ctx tag, get ctx connectionId, getConnectionTag ctx connectionId tag with
        | None, _, _ -> Error $"Tag `{tag}` not found"
        | _, None, _ -> Error $"Connection `{connectionId}` not found"
        | _, _, Some _ -> Error $"Tag `{tag}` already attached to connection `{connectionId}`"
        | Some l, Some n, None -> addConnectionTag ctx n.Id l.Name |> Ok

    let activateConnectionTag (ctx: SqliteContext) (connectionId: string) (tag: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_tags SET active = TRUE WHERE connection_id = @0 AND tag = @1",
            [ connectionId; tag ]
        )

    let deactivateConnectionTag (ctx: SqliteContext) (connectionId: string) (tag: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE connection_tags SET active = FALSE WHERE connection_id = @0 AND tag = @1",
            [ connectionId; tag ]
        )
    