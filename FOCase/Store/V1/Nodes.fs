﻿namespace FOCase.Store.V1

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

    let getMetadataValue (ctx: SqliteContext) = ()
    
    let addMetadataValue (ctx: SqliteContext) (nodeId: string) (key: string) (value: string) =
        
        ()