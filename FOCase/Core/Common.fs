module FOCase.Core

open System

[<AutoOpen>]
module Common =

    [<RequireQualifiedAccess>]
    type IdType =
        | Specific of string
        | Generated

        static member Create(?id: string) =
            match id with
            | Some v -> Specific v
            | None -> Generated.Generate()

        member idt.GetId() =
            match idt with
            | Specific id -> id
            | Generated -> System.Guid.NewGuid().ToString("n")

        member idt.Generate() =
            match idt with
            | Specific id -> Specific id
            | Generated -> idt.GetId() |> Specific

    [<RequireQualifiedAccess>]
    type LabelWeightComparison =
        | Equal of Value: decimal
        | NotEqual of Value: decimal
        | GreaterThan of Value: decimal
        | GreaterThanOrEqual of Value: decimal
        | LessThan of Value: decimal
        | LessThanOrEqual of Value: decimal
        | Not of Comparison: LabelWeightComparison
        | And of ComparisonA: LabelWeightComparison * ComparisonB: LabelWeightComparison 
        | Or of ComparisonA: LabelWeightComparison * ComparisonB: LabelWeightComparison 
        | Any of Comparisons: LabelWeightComparison list
        | All of Comparisons: LabelWeightComparison list
        | None of Comparisons: LabelWeightComparison list
        | WildCard
        
        static member Default = LabelWeightComparison.WildCard
        
        static member IsTrue = Equal 1m
        
        static member IsFalse = Equal 0m
        