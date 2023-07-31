namespace FOCase.Store.V1

[<RequireQualifiedAccess>]
module Connections =

    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store
    open FOCase.Store.V1.Persistence

    let add
        (ctx: SqliteContext)
        (id: IdType option)
        (name: string)
        (fromNode: string)
        (toNode: string)
        (twoWay: bool)
        =
        ({ Id = getId id
           Name = name
           FromNode = fromNode
           ToNode = toNode
           TwoWay = twoWay
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewConnection)
        |> Operations.insertConnection ctx

    let get (ctx: SqliteContext) (id: string) =
        Operations.selectConnectionRecord ctx [ "WHERE id = @0" ] [ id ]

    let getActiveFromNode (ctx: SqliteContext) (nodeId: string) =
        Operations.selectConnectionRecord ctx [ "WHERE from_node = @0 AND active = TRUE" ] [ nodeId ]

    let getActiveToNode (ctx: SqliteContext) (nodeId: string) =
        Operations.selectConnectionRecord ctx [ "WHERE to_node = @0 AND active = TRUE" ] [ nodeId ]
