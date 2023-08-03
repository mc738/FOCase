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

    // *** Metadata ***

    let getMetadataValue (ctx: SqliteContext) (resourceVersionId: string) (key: string) =
        Operations.selectResourceVersionMetadataItemRecord
            ctx
            [ "WHERE resource_version_id = @0 AND item_key = @1" ]
            [ resourceVersionId; key ]

    let addMetadataValue (ctx: SqliteContext) (resourceVersionId: string) (key: string) (value: string) =
        ({ ResourceVersionId = resourceVersionId
           ItemKey = key
           ItemValue = value
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewResourceVersionMetadataItem)
        |> Operations.insertResourceVersionMetadataItem ctx

    let tryAddMetadataValue (ctx: SqliteContext) (resourceVersionId: string) (key: string) (value: string) =
        match getMetadataValue ctx resourceVersionId key with
        | Some _ -> Error $"Metadata value `{key}` already exists for resource version `{resourceVersionId}`"
        | None -> addMetadataValue ctx resourceVersionId key value |> Ok

    let updateMetadataValue (ctx: SqliteContext) (resourceVersionId: string) (key: string) (value: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE resource_version_metadata SET item_value = @0 WHERE resource_version_id = @1 AND item_key = @2",
            [ value; resourceVersionId; key ]
        )
        |> ignore

    let tryUpdateMetadataValue (ctx: SqliteContext) (resourceVersionId: string) (key: string) (value: string) =
        match getMetadataValue ctx resourceVersionId key with
        | Some _ -> updateMetadataValue ctx resourceVersionId key value |> Ok
        | None -> Error $"Metadata value `{key}` does not exist for resource version `{resourceVersionId}`"

    let addOrUpdateMetadataValue (ctx: SqliteContext) (resourceVersionId: string) (key: string) (value: string) =
        match getMetadataValue ctx resourceVersionId key with
        | Some _ -> updateMetadataValue ctx resourceVersionId key value
        | None -> addMetadataValue ctx resourceVersionId key value

    let activateMetadataItem (ctx: SqliteContext) (resourceVersionId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE resource_version_metadata SET active = TRUE WHERE resource_version_id = @0 AND item_key = @1",
            [ resourceVersionId; key ]
        )

    let deactivateMetadataItem (ctx: SqliteContext) (resourceVersionId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE resource_version_metadata SET active = FALSE WHERE resource_version_id = @0 AND item_key = @1",
            [ resourceVersionId; key ]
        )
