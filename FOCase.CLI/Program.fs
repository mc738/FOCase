open System

module Utils =


    type Option =
        { Name: string
          Details: string option
          AcceptedValues: string list }

    let printLines (lines: string list) = lines |> List.iter Console.WriteLine

    let optionPrompt (prompt: string) (options: Option list) =
        Console.WriteLine prompt

        let printOptions _ =
            options
            |> List.iteri (fun i o ->
                let acceptedValues = o.AcceptedValues |> String.concat " / "

                printfn $"{i + 1}. {o.Name} ({acceptedValues})"
                o.Details |> Option.iter (fun d -> printfn $"\t{d}"))


        let rec handler _ =
            printOptions ()

            let input = Console.ReadLine()

            let selectedOption =
                options
                |> List.tryFind (fun o ->
                    o.AcceptedValues
                    |> List.exists (fun av -> System.String.Equals(input, av, StringComparison.OrdinalIgnoreCase)))

            match selectedOption with
            | Some so -> so
            | None ->
                printfn $"Unknown option: {input}"
                handler ()

        handler ()

let banner =
    [ "▄████  ████▄ ▄█▄    ██      ▄▄▄▄▄   ▄███▄   "
      "█▀   ▀ █   █ █▀ ▀▄  █ █    █     ▀▄ █▀   ▀  "
      "█▀▀    █   █ █   ▀  █▄▄█ ▄  ▀▀▀▀▄   ██▄▄    "
      "█      ▀████ █▄  ▄▀ █  █  ▀▄▄▄▄▀    █▄   ▄▀ "
      " █           ▀███▀     █            ▀███▀   "
      "  ▀                   █                     "
      "                     ▀                      "
      ""
      "Version: 0.1 " ]

let initialOptions =
    [ "Choose an option:"; "1. New case (new/n/1)"; "2. Load case (load/l/2)" ]


banner |> List.iter Console.WriteLine
initialOptions |> List.iter Console.WriteLine

let option =
    Utils.optionPrompt
        "Choose an option:"
        [ { Name = "New case"
            Details = None
            AcceptedValues = [ "new"; "n" ] }
          { Name = "New case"
            Details = None
            AcceptedValues = [ "load"; "l" ] } ]

printfn $"Selected option: {option.Name}"

Console.ReadLine()
