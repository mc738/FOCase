open System

module Utils =


    type Option =
        { Name: string
          Details: string option
          AcceptedValues: string list }

    let printLines (lines: string list) = lines |> List.iter Console.WriteLine

    let optionPrompt (prompt: string) (marker: string option) (options: Option list) =
        
        let printOptions _ =
            options
            |> List.iteri (fun i o ->
                let acceptedValues = o.AcceptedValues |> String.concat " / "

                printfn $"{i + 1}. {o.Name} ({acceptedValues})"
                o.Details |> Option.iter (fun d -> printfn $"\t{d}"))


        let rec handler _ =
            Console.WriteLine prompt

            printOptions ()

            match marker with
            | Some m -> printf $"{marker} > "
            | None -> printf "> "
            
            let input = Console.ReadLine()

            match input with
            | "clr"
            | "clear" ->
                Console.Clear()
                handler ()
            | _ ->
                
                let selectedOption =
                    options
                    |> List.tryFind (fun o ->
                        o.AcceptedValues
                        |> List.exists (fun av -> System.String.Equals(input, av, StringComparison.OrdinalIgnoreCase)))

                match selectedOption with
                | Some so -> so
                | None ->
                    printfn $"Unknown option: {input}"
                    printfn ""
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

banner |> List.iter Console.WriteLine

let option =
    Utils.optionPrompt
        "Choose an option:"
        None
        [ { Name = "New case"
            Details = None
            AcceptedValues = [ "new"; "n" ] }
          { Name = "New case"
            Details = None
            AcceptedValues = [ "load"; "l" ] } ]

printfn $"Selected option: {option.Name}"
