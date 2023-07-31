namespace FOCase.Core

module FileTypes =

    [<RequireQualifiedAccess>]
    type FileType =
        // General
        | Binary
        | Text
        // Data
        | Json
        | Xml
        | Csv
        // Documents
        | Pdf
        // Web
        | Html
        | Css
        | JavaScript
        // Audio
        | Mp3
        | Wma
        | RealAudio
        | Wav
        // Images
        | Gif
        | Jpeg
        | Png
        | Tiff
        | Svg
        | WebP
        // Video
        | Mpeg
        | Mp4
        | QuickTime
        | Wmv
        | WebM

        static member TryDeserialize(str: string) =
            match str.ToLower() with
            | "bin"
            | "exe"
            | "binary" -> Ok FileType.Binary
            | "text" -> Ok FileType.Text
            | "json" -> Ok FileType.Json
            | "xml" -> Ok FileType.Xml
            | "csv" -> Ok FileType.Csv
            | "pdf" -> Ok FileType.Pdf
            | "html" -> Ok FileType.Html
            | "css" -> Ok FileType.Css
            | "js"
            | "javascript" -> Ok FileType.JavaScript
            | "mp3" -> Ok FileType.Mp3
            | "wma" -> Ok FileType.Wma
            | "realaudio" -> Ok FileType.RealAudio
            | "wav" -> Ok FileType.Wav
            | "gif" -> Ok FileType.Gif
            | "jpeg" -> Ok FileType.Jpeg
            | "png" -> Ok FileType.Png
            | "tiff" -> Ok FileType.Tiff
            | "svg" -> Ok FileType.Svg
            | "webp" -> Ok FileType.WebP
            | "mpeg" -> Ok FileType.Mpeg
            | "mp4" -> Ok FileType.Mp4
            | "quicktime" -> Ok FileType.QuickTime
            | "wmv" -> Ok FileType.Wmv
            | "webm" -> Ok FileType.WebM
            | _ -> Error $"Unknown file type `{str}`"

        static member Deserialize(str: string) =
            match FileType.TryDeserialize str with
            | Ok ft -> ft
            | Error _ -> Binary

        static member All() =
            [ FileType.Binary
              FileType.Text
              FileType.Json
              FileType.Xml
              FileType.Csv
              FileType.Pdf
              FileType.Html
              FileType.Css
              FileType.JavaScript
              FileType.Mp3
              FileType.Wma
              FileType.RealAudio
              FileType.Wav
              FileType.Gif
              FileType.Jpeg
              FileType.Png
              FileType.Tiff
              FileType.Svg
              FileType.WebP
              FileType.Mpeg
              FileType.Mp4
              FileType.QuickTime
              FileType.Wmv
              FileType.WebM ]

        member ft.Serialize() =
            match ft with
            | Binary -> "binary"
            | Text -> "text"
            | Json -> "json"
            | Xml -> "xml"
            | Csv -> "csv"
            | Pdf -> "pdf"
            | Html -> "html"
            | Css -> "css"
            | JavaScript -> "javascript"
            | Mp3 -> "mp3"
            | Wma -> "wma"
            | RealAudio -> "realaudio"
            | Wav -> "wav"
            | Gif -> "gif"
            | Jpeg -> "jpeg"
            | Png -> "png"
            | Tiff -> "tiff"
            | Svg -> "svg"
            | WebP -> "webp"
            | Mpeg -> "mpeg"
            | Mp4 -> "mp4"
            | QuickTime -> "quicktime"
            | Wmv -> "wmv"
            | WebM -> "webm"

        member ft.GetExtension() =
            match ft with
            | Binary -> ".bin"
            | Text -> ".txt"
            | Json -> ".json"
            | Xml -> ".xml"
            | Csv -> ".csv"
            | Pdf -> ".pdf"
            | Html -> ".html"
            | Css -> ".css"
            | JavaScript -> ".js"
            | Mp3 -> ".mp3"
            | Wma -> ".wma"
            | RealAudio -> "ra"
            | Wav -> ".wav"
            | Gif -> ".gif"
            | Jpeg -> ".jpeg"
            | Png -> ".png"
            | Tiff -> ".tiff"
            | Svg -> ".svg"
            | WebP -> ".webp"
            | Mpeg -> ".mpeg"
            | Mp4 -> ".mp4"
            | QuickTime -> ".quicktime"
            | Wmv -> ".wmv"
            | WebM -> ".webm"

        member ft.GetContentType() =
            match ft with
            | Binary -> "application/octet-stream"
            | Text -> "text/plain"
            | Json -> "application/json"
            | Xml -> "application/xml"
            | Csv -> "text/csv"
            | Pdf -> "application/pdf"
            | Html -> "text/html"
            | Css -> "text/css"
            | JavaScript -> "application/javascript"
            | Mp3 -> "audio/mpeg"
            | Wma -> "audio/x-ms-wma"
            | RealAudio -> "audio/vnd.rn-realaudio"
            | Wav -> "audio/x-wav"
            | Gif -> "image/gif"
            | Jpeg -> "image/jpeg"
            | Png -> "image/png"
            | Tiff -> "image/tiff"
            | Svg -> "image/svg+xml"
            | WebP -> "image/webp"
            | Mpeg -> "video/mpeg"
            | Mp4 -> "video/mp4"
            | QuickTime -> "video/quicktime"
            | Wmv -> "video/x-ms-wmv"
            | WebM -> "video/webm"
