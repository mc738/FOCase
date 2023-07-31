namespace FOCase.Store.V1

open FOCase.Store.V1.Persistence
open Freql.Sqlite

[<RequireQualifiedAccess>]
module Tags =

    let get (ctx: SqliteContext) (name: string) =
        Operations.selectTagRecord ctx [ "WHERE name = @0" ] [ name ]

    let add (ctx: SqliteContext)


    ()
