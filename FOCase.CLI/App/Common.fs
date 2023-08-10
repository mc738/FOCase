module FOCase.CLI.App

[<AutoOpen>]
module Common =
    
    open System
    
    type OptionPromptItem =
        { Name: string
          Details: string option
          AcceptedValues: string list }

    type OptionPrompt =
        {
            Prompt: string
            Marker: string option
            Items: OptionPromptItem list
        }
    
    
    let printLines (lines: string list) = lines |> List.iter Console.WriteLine

    let optionPrompt (options: OptionPrompt) =
        
        let printOptions _ =
            options.Items
            |> List.iteri (fun i o ->
                let acceptedValues = o.AcceptedValues |> String.concat " / "

                printfn $"{i + 1}. {o.Name} ({acceptedValues})"
                o.Details |> Option.iter (fun d -> printfn $"\t{d}"))


        let rec handler _ =
            Console.WriteLine options.Prompt

            printOptions ()

            match options.Marker with
            | Some m -> printf $"{m} > "
            | None -> printf "> "
            
            let input = Console.ReadLine()

            match input with
            | "clr"
            | "clear" ->
                Console.Clear()
                handler ()
            | _ ->
                
                let selectedOption =
                    options.Items
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
    

