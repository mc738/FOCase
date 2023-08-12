namespace FOCase.CLI.App

open System
open System.IO
open FOCase.CLI.App.State

[<AutoOpen>]
module Impl =
        
    open Freql.Sqlite
    open FOCase.CLI.App.State
    
    [<AutoOpen>]
    module private Internal =

        let tryGetEnvValue (name: string) =
            let v = Environment.GetEnvironmentVariable(name)
            
            match String.IsNullOrWhiteSpace v |> not with
            | true -> Some v
            | false -> None
        
        
        let resolvePath (value: string) =
            // Check if the path is fully qualified (i.e C://path) or just a name.
            match Path.IsPathFullyQualified value with
            | true -> value
            | false ->
                match Path.IsPathRooted value with
                | true -> Path.GetFullPath value
                | false ->            
                    match tryGetEnvValue "FOCASE_PATH" with
                    | Some p -> Path.Combine(p, value)
                    | None -> Path.Combine(Environment.CurrentDirectory, value)
            
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
        
        

        let handleCaseContext _ =
            ()
            
        let handleNodeContext (nodeCtx: NodeStateContext) =
            ()
        
        let runLoop (ctx:SqliteContext) =
            let rec loop (state: ApplicationState) =
                match state.CurrentStateItem.Context with
                | StateContext.Case ->
                    handleCaseContext ()
                    
                | StateContext.Node nodeStateContext -> handleNodeContext nodeStateContext
                | StateContext.Connection connectionStateContext -> failwith "todo"
                | StateContext.Document documentStateContext -> failwith "todo"
                | StateContext.Resource resourceStateContext -> failwith "todo"
                
                
                ()
            
            loop (ApplicationState.Create())
            
            ()
    
    
    let run _ =

        printBanner ()
        let mode = optionPrompt initialOptions

        match mode.Value with
        | "new" ->
            
            
            
            ()
        | "load" -> ()
        | _ -> ()


        ()


    ()
