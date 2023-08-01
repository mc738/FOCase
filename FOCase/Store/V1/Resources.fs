namespace FOCase.Store.V1

[<RequireQualifiedAccess>]
module Resources =
    
    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store.V1.Persistence

    // *** General ***

    let add (ctx: SqliteContext) (id: IdType option) (name: string) =
        ({ Id = getId id
           Name = name
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewResource)
        |> Operations.insertResource ctx

    let get (ctx: SqliteContext) (id: string) =
        Operations.selectResourceRecord ctx [ "WHERE id = @0" ] [ id ]
        
    let getAll (ctx:SqliteContext) =
        Operations.selectResourceRecords ctx [] []
        
    let getAllActive (ctx: SqliteContext) =
        Operations.selectResourceRecords ctx [ "WHERE active = TRUE" ] []
    
    let activate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resources SET active = TRUE WHERE id = @0", [ id ])

    let deactivate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resources SET active = FALSE WHERE id = @0", [ id ])
