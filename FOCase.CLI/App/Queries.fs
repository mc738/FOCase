namespace FOCase.CLI.App

module Queries =
    
    type Query =
        {
            Target: QueryTarget
        }
        
    and QueryTarget =
        | Nodes
        | Connections
        | Resources
        | Documents
        
    and [<RequireQualifiedAccess>] QueryCondition =
        | HasTag of Tag : string
        | Not of Condition: QueryCondition
        | And of ConditionA: QueryCondition * ConditionB: QueryCondition
        | Or of ConditionA: QueryCondition * ConditionB: QueryCondition
        | Any of Conditions: QueryCondition list
        | All of Conditions: QueryCondition list
        | None of Conditions: QueryCondition list
        
    
    
    let parse () =
        ()

