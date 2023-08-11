namespace FOCase.CLI.App

[<AutoOpen>]
module Impl =

    [<AutoOpen>]
    module private Internal =

        let initialOptions =
            ({ Prompt = ""
               Marker = None
               Items =
                 [ { Name = "New case"
                     Details = None
                     AcceptedValues = [ "new"; "n" ] }
                   { Name = "New case"
                     Details = None
                     AcceptedValues = [ "load"; "l" ] } ] }
            : OptionPrompt)

        ()

    let run _ =

        printBanner ()
        let mode = optionPrompt initialOptions



        ()


    ()
