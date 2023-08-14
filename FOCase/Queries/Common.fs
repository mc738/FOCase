namespace FOCase.Queries

[<AutoOpen>]
module Common =
    
    
    type Query = { Target: QueryTarget }

    and QueryTarget =
        | Nodes
        | Connections
        | Resources
        | Documents

    and [<RequireQualifiedAccess>] QueryCondition =
        | HasTag of Tag: string
        | Label of LabelWeightCondition
        | Not of Condition: QueryCondition
        | And of ConditionA: QueryCondition * ConditionB: QueryCondition
        | Or of ConditionA: QueryCondition * ConditionB: QueryCondition
        | Any of Conditions: QueryCondition list
        | All of Conditions: QueryCondition list
        | None of Conditions: QueryCondition list

    and LabelWeightCondition =
        | Equals of Value: decimal
        | NotEqual of Value: decimal
        | GreaterThan of Value: decimal
        | GreaterThanOrEqual of Value: decimal
        | LessThan of Value: decimal
        | LessThanOrEqual of Value: decimal
    
    ()

