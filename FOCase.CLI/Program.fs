open System

module Utils =
    
    
    type Option =
        {
            Name: string
            Details: string option
            AcceptedValues: string
        }
    
    let printLines (lines: string list) = lines |> List.iter Console.WriteLine

    let optionPrompt (prompt: string) (options: Option list) =
        Console.WriteLine prompt
        
        options |> List.mapi (fun i o -> printfn $"{i + 1}. {}" )
        
        
        
        
        
        ()

let banner =
    [
        "▄████  ████▄ ▄█▄    ██      ▄▄▄▄▄   ▄███▄   "
        "█▀   ▀ █   █ █▀ ▀▄  █ █    █     ▀▄ █▀   ▀  "
        "█▀▀    █   █ █   ▀  █▄▄█ ▄  ▀▀▀▀▄   ██▄▄    "
        "█      ▀████ █▄  ▄▀ █  █  ▀▄▄▄▄▀    █▄   ▄▀ "
        " █           ▀███▀     █            ▀███▀   "
        "  ▀                   █                     "
        "                     ▀                      "
        ""
        "Version: 0.1 "
    ]

let initialOptions =
    [
        "Choose an option:"
        "1. New case (new/n/1)"
        "2. Load case (load/l/2)"
    ]


banner |> List.iter Console.WriteLine
initialOptions |> List.iter Console.WriteLine

Console.ReadLine ()

