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
