namespace FOCase.Store.V1

[<RequireQualifiedAccess>]
module Tags =

    open Freql.Sqlite
    open FOCase.Store.V1.Persistence

    let get (ctx: SqliteContext) (name: string) =
        Operations.selectTagRecord ctx [ "WHERE name = @0" ] [ name ]

    let add (ctx: SqliteContext) (name: string) =
        ({ Name = name
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewLabel)
        |> Operations.insertLabel ctx

    let tryAdd (ctx: SqliteContext) (name: string) =
        match get ctx name with
        | Some _ -> Error $"Tag `{name}` already exists"
        | None -> add ctx name |> Ok

    let activate (ctx: SqliteContext) (name: string) (includeRelated: bool) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE labels SET active = TRUE WHERE name = @0", [ name ])
        |> ignore

        match includeRelated with
        | true ->
            ctx.ExecuteVerbatimNonQueryAnon("UPDATE connection_tags SET active = TRUE WHERE tag = @0", [ name ])
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon("UPDATE document_tags SET active = TRUE WHERE tag = @0", [ name ])
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon("UPDATE document_version_tags SET active = TRUE WHERE tag = @0", [ name ])
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon(
                "UPDATE external_connection_tags SET active = TRUE WHERE tag = @0",
                [ name ]
            )
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon("UPDATE node_tags SET active = TRUE WHERE tag = @0", [ name ])
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_tags SET active = TRUE WHERE tag = @0", [ name ])
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_version_tags SET active = TRUE WHERE tag = @0", [ name ])
            |> ignore
        | false -> ()


    /// <summary>
    /// Deactivate a specific tag.
    /// </summary>
    /// <param name="ctx">The SqliteContext for the store.</param>
    /// <param name="name">The tag name.</param>
    /// <param name="includeRelated">Deactivate any usages of the tag in the store (for example connection tags, node tags etc.)</param>
    let deactivate (ctx: SqliteContext) (name: string) (includeRelated: bool) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE labels SET active = FALSE WHERE name = @0", [ name ])
        |> ignore
        
        match includeRelated with
        | true ->
            ctx.ExecuteVerbatimNonQueryAnon("UPDATE connection_tags SET active = FALSE WHERE tag = @0", [ name ])
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon("UPDATE document_tags SET active = FALSE WHERE tag = @0", [ name ])
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon("UPDATE document_version_tags SET active = FALSE WHERE tag = @0", [ name ])
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon(
                "UPDATE external_connection_tags SET active = FALSE WHERE tag = @0",
                [ name ]
            )
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon("UPDATE node_tags SET active = FALSE WHERE tag = @0", [ name ])
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_tags SET active = FALSE WHERE tag = @0", [ name ])
            |> ignore

            ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_version_tags SET active = FALSE WHERE tag = @0", [ name ])
            |> ignore
        | false -> ()
