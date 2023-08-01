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
