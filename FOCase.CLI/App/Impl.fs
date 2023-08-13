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

        [<RequireQualifiedAccess>]
        type OperationResult =
            | NoStateChange
            | PushState of StateContext
            | PopState
            | Exit

        let handleClear _ =
            Console.Clear ()
            OperationResult.NoStateChange
        
        let handleCaseContext (ctx: SqliteContext) = OperationResult.NoStateChange

        let handleNodeContext (ctx: SqliteContext) (nodeCtx: NodeStateContext) = OperationResult.NoStateChange

        let handleConnectionContext (ctx: SqliteContext) (connectionCtx: ConnectionStateContext) =
            OperationResult.NoStateChange

        let handleDocumentContext (ctx: SqliteContext) (documentCtx: DocumentStateContext) =
            OperationResult.NoStateChange

        let handleResourceContext (ctx: SqliteContext) (resourceCtx: ResourceStateContext) =
            OperationResult.NoStateChange

        let runLoop (ctx: SqliteContext) =
            let rec loop (state: ApplicationState) =
                let result =
                    match state.CurrentStateItem.Context with
                    | StateContext.Case -> handleCaseContext ctx
                    | StateContext.Node nodeStateContext -> handleNodeContext ctx nodeStateContext
                    | StateContext.Connection connectionStateContext ->
                        handleConnectionContext ctx connectionStateContext
                    | StateContext.Document documentStateContext -> handleDocumentContext ctx documentStateContext
                    | StateContext.Resource resourceStateContext -> handleResourceContext ctx resourceStateContext

                match result with
                | OperationResult.NoStateChange -> loop state
                | OperationResult.PushState stateContext -> loop (state.PushState stateContext)
                | OperationResult.PopState -> loop (state.PopState())
                | OperationResult.Exit -> ()

            loop (ApplicationState.Create())


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
