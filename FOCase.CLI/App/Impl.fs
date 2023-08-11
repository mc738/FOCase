namespace FOCase.CLI.App

open Freql.Sqlite

[<AutoOpen>]
module Impl =

    [<AutoOpen>]
    module private Internal =

        let initialOptions =
            ({ Prompt = ""
               Marker = None
               Items =
                 [ { Name = "New case"
                     Value = "new"
                     Details = None
                     AcceptedValues = [ "new"; "n" ] }
                   { Name = "Load case"
                     Value = "load"
                     Details = None
                     AcceptedValues = [ "load"; "l" ] } ] }
            : OptionPrompt)

        ()

        let runLoop (ctx:SqliteContext) = ()
    
    
    let run _ =

        printBanner ()
        let mode = optionPrompt initialOptions

        match mode.Value with
        | "new" -> ()
        | "load" -> ()
        | _ -> ()


        ()


    ()
