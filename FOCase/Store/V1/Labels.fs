namespace FOCase.Store.V1

[<RequireQualifiedAccess>]
module Labels =

    open Freql.Sqlite
    open FOCase.Store.V1.Persistence

    let get (ctx: SqliteContext) (name: string) =
        Operations.selectLabelRecord ctx [ "WHERE name = @0" ] [ name ]

    let add (ctx: SqliteContext) (name: string) =
        ({ Name = name
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewLabel)
        |> Operations.insertLabel ctx

    let tryAdd (ctx: SqliteContext) (name: string) =
        match get ctx name with
        | Some _ -> Error $"Label `{name}` already exists"
        | None -> add ctx name |> Ok

    let activate (ctx: SqliteContext) (name: string) (includeRelated: bool) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE labels SET active = TRUE WHERE name = @0", [ name ])
        |> ignore

        match includeRelated with
        | true ->
            ctx.ExecuteVerbatimNonQueryAnon("UPDATE connection_labels SET active = TRUE WHERE tag = @0", [ name ])
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon(
                "UPDATE external_connection_labels SET active = TRUE WHERE tag = @0",
                [ name ]
            )
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon("UPDATE node_labels SET active = TRUE WHERE tag = @0", [ name ])
            |> ignore
        | false -> ()


    /// <summary>
    /// Deactivate a specific label.
    /// </summary>
    /// <param name="ctx">The SqliteContext for the store.</param>
    /// <param name="name">The label name.</param>
    /// <param name="includeRelated">Deactivate any usages of the label in the store (for example connection labels, node labels etc.)</param>
    let deactivate (ctx: SqliteContext) (name: string) (includeRelated: bool) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE labels SET active = FALSE WHERE name = @0", [ name ])
        |> ignore

        match includeRelated with
        | true ->
            ctx.ExecuteVerbatimNonQueryAnon("UPDATE connection_s SET active = FALSE WHERE tag = @0", [ name ])
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon(
                "UPDATE external_connection_labels SET active = FALSE WHERE tag = @0",
                [ name ]
            )
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon("UPDATE node_labels SET active = FALSE WHERE tag = @0", [ name ])
            |> ignore
        | false -> ()
