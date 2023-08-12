namespace FOCase.CLI.App

open System
open System.IO
open System.Reflection.Metadata
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

        let handleDocumentContext (documentCtx: DocumentStateContext) = ()
        
        let handleResourceContext (resourceCtx: ResourceStateContext) = ()
        
        let runLoop (ctx: SqliteContext) =
            let rec loop (state: ApplicationState) =
                match state.CurrentStateItem.Context with
                | StateContext.Case -> handleCaseContext ()

                | StateContext.Node nodeStateContext -> handleNodeContext nodeStateContext
                | StateContext.Connection connectionStateContext -> handleConnectionContext connectionStateContext
                | StateContext.Document documentStateContext -> handleDocumentContext documentStateContext
                | StateContext.Resource resourceStateContext -> handleResourceContext resourceStateContext


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
