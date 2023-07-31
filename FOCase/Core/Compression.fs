namespace FOCase.Core

module Compression =
    
    [<RequireQualifiedAccess>]
    type CompressionType =
        | None
        | GZip

        static member TryDeserialize(str: string) =
            match str.ToLower() with
            | "none" -> Ok None
            | "gzip" -> Ok GZip
            | _ -> Error $"Unknown compression type: `{str}`"

        static member Deserialize(str: string) =
            match CompressionType.TryDeserialize str with
            | Ok ct -> ct
            | Error _ -> None

        static member All() = [ None; GZip ]

        member et.Serialize() =
            match et with
            | None -> "none"
            | GZip -> "gzip"

