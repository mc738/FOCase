namespace FOCase.Store.V1

[<RequireQualifiedAccess>]
module DocumentVersions =

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
        Operations.selectDocumentVersionRecord ctx [ "WHERE id = @0" ] [ id ]

    let getLatest (ctx: SqliteContext) (documentId: string) =
        Operations.selectDocumentVersionRecord
            ctx
            [ "WHERE document_id = @0 ORDER BY version DESC LIMIT 1;" ]
            [ documentId ]

    let add
        (ctx: SqliteContext)
        (id: IdType option)
        (documentId: string)
        (version: int)
        (rawData: byte array)
        (encryptionType: EncryptionType)
        (compressionType: CompressionType)
        =
        let ms = new MemoryStream(rawData)
        let hash = ms.GetSHA256Hash()

        ({ Id = getId id
           DocumentId = documentId
           Version = version
           RawData = BlobField.FromStream ms
           Hash = hash
           CreatedOn = getTimestamp ()
           EncryptionType = encryptionType.Serialize()
           CompressionType = compressionType.Serialize()
           Active = true }
        : Parameters.NewDocumentVersion)
        |> Operations.insertDocumentVersion ctx
    
    let tryAddLatest (ctx: SqliteContext) (id: IdType option) (documentId: string) (rawData: byte array) (encryptionType: EncryptionType) (compressionType: CompressionType) =
        match Documents.get ctx documentId, getLatest ctx documentId with
        | Some _, Some ldv -> add ctx id documentId (ldv.Version + 1) rawData encryptionType compressionType |> Ok
        | Some _, None -> add ctx id documentId 1 rawData encryptionType compressionType |> Ok
        | None, _ -> Error $"Document `{documentId}` does not exist"
        
    let getAll (ctx: SqliteContext) =
        Operations.selectDocumentVersionRecord ctx [] []

    let getAllActive (ctx: SqliteContext) =
        Operations.selectDocumentVersionRecord ctx [ "WHERE active = TRUE" ] []

    let activate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE document_versions SET active = TRUE WHERE id = @0", [ id ])

    let deactivate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE document_versions SET active = FALSE WHERE id = @0", [ id ])

    // *** Metadata ***
    
    let getMetadataValue (ctx: SqliteContext) (documentVersionId: string) (key: string) =
        Operations.selectDocumentVersionMetadataItemRecord ctx [ "WHERE document_version_id = @0 AND item_key = @1" ] [ documentVersionId; key ]

    let addMetadataValue (ctx: SqliteContext) (document_versionId: string) (key: string) (value: string) =
        ({ DocumentVersionId = document_versionId
           ItemKey = key
           ItemValue = value
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewDocumentVersionMetadataItem)
        |> Operations.insertDocumentVersionMetadataItem ctx

    let tryAddMetadataValue (ctx: SqliteContext) (documentVersionId: string) (key: string) (value: string) =
        match getMetadataValue ctx documentVersionId key with
        | Some _ -> Error $"Metadata value `{key}` already exists for document version `{documentVersionId}`"
        | None -> addMetadataValue ctx documentVersionId key value |> Ok

    let updateMetadataValue (ctx: SqliteContext) (documentVersionId: string) (key: string) (value: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE document_version_metadata SET item_value = @0 WHERE document_version_id = @1 AND item_key = @2",
            [ value; documentVersionId; key ]
        )
        |> ignore

    let tryUpdateMetadataValue (ctx: SqliteContext) (documentVersionId: string) (key: string) (value: string) =
        match getMetadataValue ctx documentVersionId key with
        | Some _ -> updateMetadataValue ctx documentVersionId key value |> Ok
        | None -> Error $"Metadata value `{key}` does not exist for document version `{documentVersionId}`"

    let addOrUpdateMetadataValue (ctx: SqliteContext) (documentVersionId: string) (key: string) (value: string) =
        match getMetadataValue ctx documentVersionId key with
        | Some _ -> updateMetadataValue ctx documentVersionId key value
        | None -> addMetadataValue ctx documentVersionId key value

    let activateMetadataItem (ctx: SqliteContext) (documentVersionId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE document_version_metadata SET active = TRUE WHERE document_version_id = @0 AND item_key = @1",
            [ documentVersionId; key ]
        )

    let deactivateMetadataItem (ctx: SqliteContext) (documentVersionId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE document_version_metadata SET active = FALSE WHERE document_version_id = @0 AND item_key = @1",
            [ documentVersionId; key ]
        )

    // *** Tags ***

    let getDocumentVersionTag (ctx: SqliteContext) (documentVersionId: string) (tag: string) =
        Operations.selectDocumentVersionTagRecord ctx [ "WHERE document_version_id = @0 AND tag = @1" ] [ documentVersionId; tag ]

    let getAllDocumentVersionTags (ctx: SqliteContext) (documentVersionId: string) =
        Operations.selectDocumentVersionTagRecords ctx [ "WHERE document_version_id = @0" ] [ documentVersionId ]

    let getAllActiveDocumentVersionTags (ctx: SqliteContext) (documentVersionId: string) =
        Operations.selectDocumentVersionTagRecords ctx [ "WHERE document_version_id = @0 AND active = TRUE" ] [ documentVersionId ]

    let addDocumentVersionTag (ctx: SqliteContext) (documentVersionId: string) (tag: string) =
        ({ DocumentVersionId = documentVersionId
           Tag = tag
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewDocumentVersionTag)
        |> Operations.insertDocumentVersionTag ctx

    let tryAddDocumentVersionTag (ctx: SqliteContext) (documentVersionId: string) (tag: string) =
        match Tags.get ctx tag, get ctx documentVersionId, getDocumentVersionTag ctx documentVersionId tag with
        | None, _, _ -> Error $"Tag `{tag}` not found"
        | _, None, _ -> Error $"Document version `{documentVersionId}` not found"
        | _, _, Some _ -> Error $"Tag `{tag}` already attached to document version `{documentVersionId}`"
        | Some l, Some n, None -> addDocumentVersionTag ctx n.Id l.Name |> Ok

    let activateDocumentVersionTag (ctx: SqliteContext) (documentVersionId: string) (tag: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE document_version_tags SET active = TRUE WHERE document_version_id = @0 AND tag = @1",
            [ documentVersionId; tag ]
        )

    let deactivateDocumentVersionTag (ctx: SqliteContext) (documentVersionId: string) (tag: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE document_version_tags SET active = FALSE WHERE document_version_id = @0 AND tag = @1",
            [ documentVersionId; tag ]
        )
