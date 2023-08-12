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

        let newInputPrompt =
            ({ Prompt = "Enter a case name:"
               Marker = None }
            : InputPrompt)
            
        let loadInputPrompt =
            ({ Prompt = "Enter a case path:"
               Marker = None }
            : InputPrompt)

        
        let handleCaseContext _ = ()

        let handleNodeContext (nodeCtx: NodeStateContext) = ()
        
        let handleConnectionContext (connectionCtx: ConnectionStateContext) = ()

        let runLoop (ctx: SqliteContext) =
            let rec loop (state: ApplicationState) =
                match state.CurrentStateItem.Context with
                | StateContext.Case -> handleCaseContext ()

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
