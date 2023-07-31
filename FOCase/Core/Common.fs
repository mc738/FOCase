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
