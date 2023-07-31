namespace FOCase.Core

module Encryption =

    [<RequireQualifiedAccess>]
    type EncryptionType =
        | None
        | Aes

        static member TryDeserialize(str: string) =
            match str.ToLower() with
            | "none" -> Ok None
            | "aes" -> Ok Aes
            | _ -> Error $"Unknown encryption type: `{str}`"

        static member Deserialize(str: string) =
            match EncryptionType.TryDeserialize str with
            | Ok et -> et
            | Error _ -> None

        static member All() = [ None; Aes ]

        member et.Serialize() =
            match et with
            | None -> "none"
            | Aes -> "aes"
