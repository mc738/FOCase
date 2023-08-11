namespace FOCase.CLI.App

module State =

    [<RequireQualifiedAccess>]
    type StateContext =
        | Case
        | Node of NodeStateContext
        | Connection of ConnectionStateContext
        | Document of DocumentStateContext
        | Resource of ResourceStateContext

    and NodeStateContext = { Id: string }

    and ConnectionStateContext = { Id: string }

    and DocumentStateContext = { Id: string }

    and ResourceStateContext = { Id: string }

    type StateItem =
        { ParentState: StateItem option
          Context: StateContext }

    type ApplicationState =
        { CurrentStateItem: StateItem }

        static member Create() =
            { CurrentStateItem =
                { ParentState = None
                  Context = StateContext.Case } }

        member s.PushState(state: StateContext) =
            { s with
                CurrentStateItem =
                    { ParentState = Some s.CurrentStateItem
                      Context = state } }

        member s.PopState() =
            match s.CurrentStateItem.ParentState with
            | Some ps -> { s with CurrentStateItem = ps }
            | None -> s
