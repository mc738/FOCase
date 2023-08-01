namespace FOCase.Store.V1

open FOCase.Core.FileTypes

[<RequireQualifiedAccess>]
module ResourceVersions =

    open System.IO
    open FsToolbox.Extensions.Strings
    open FsToolbox.Extensions.Streams
    open Freql.Core.Common.Types
    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Core
    open FOCase.Core.Compression
    open FOCase.Core.Encryption
    open FOCase.Store.V1.Persistence

    // *** General ***

    let get (ctx: SqliteContext) (id: string) =
        Operations.selectResourceVersionRecord ctx [ "WHERE id = @0" ] [ id ]

    let getLatest (ctx: SqliteContext) (resourceId: string) =
        Operations.selectResourceVersionRecord
            ctx
            [ "WHERE resource_id = @0 ORDER BY version DESC LIMIT 1;" ]
            [ resourceId ]

    let add
        (ctx: SqliteContext)
        (id: IdType option)
        (resourceId: string)
        (version: int)
        (rawData: byte array)
        (fileType: FileType)
        (encryptionType: EncryptionType)
        (compressionType: CompressionType)
        =
        let ms = new MemoryStream(rawData)
        let hash = ms.GetSHA256Hash()

        ({ Id = getId id
           ResourceId = resourceId
           Version = version
           RawData = BlobField.FromStream ms
           Hash = hash
           CreatedOn = getTimestamp ()
           FileType = fileType.Serialize()
           EncryptionType = encryptionType.Serialize()
           CompressionType = compressionType.Serialize()
           Active = true }
        : Parameters.NewResourceVersion)
        |> Operations.insertResourceVersion ctx

    let tryAddLatest
        (ctx: SqliteContext)
        (id: IdType option)
        (resourceId: string)
        (rawData: byte array)
        (fileType: FileType)
        (encryptionType: EncryptionType)
        (compressionType: CompressionType)
        =
        match Documents.get ctx resourceId, getLatest ctx resourceId with
        | Some _, Some lrv ->
            add ctx id resourceId (lrv.Version + 1) rawData fileType encryptionType compressionType
            |> Ok
        | Some _, None -> add ctx id resourceId 1 rawData fileType encryptionType compressionType |> Ok
        | None, _ -> Error $"Resource `{resourceId}` does not exist"

    let getAll (ctx: SqliteContext) =
        Operations.selectResourceVersionRecords ctx [] []

    let getAllActive (ctx: SqliteContext) =
        Operations.selectResourceVersionRecords ctx [ "WHERE active = TRUE" ] []

    let activate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_versions SET active = TRUE WHERE id = @0", [ id ])

    let deactivate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_versions SET active = FALSE WHERE id = @0", [ id ])
