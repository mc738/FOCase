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

    // *** Tags ***

    let getResourceVersionTag (ctx: SqliteContext) (resourceVersionId: string) (tag: string) =
        Operations.selectResourceVersionTagRecord ctx [ "WHERE resource_version_id = @0 AND tag = @1" ] [ resourceVersionId; tag ]

    let getAllResourceVersionTags (ctx: SqliteContext) (resourceVersionId: string) =
        Operations.selectResourceVersionTagRecords ctx [ "WHERE resource_version_id = @0" ] [ resourceVersionId ]

    let getAllActiveResourceVersionTags (ctx: SqliteContext) (resourceVersionId: string) =
        Operations.selectResourceVersionTagRecords ctx [ "WHERE resource_version_id = @0 AND active = TRUE" ] [ resourceVersionId ]

    let addResourceVersionTag (ctx: SqliteContext) (resourceVersionId: string) (tag: string) =
        ({ ResourceVersionId = resourceVersionId
           Tag = tag
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewResourceVersionTag)
        |> Operations.insertResourceVersionTag ctx

    let tryAddResourceVersionTag (ctx: SqliteContext) (resourceVersionId: string) (tag: string) =
        match Tags.get ctx tag, get ctx resourceVersionId, getResourceVersionTag ctx resourceVersionId tag with
        | None, _, _ -> Error $"Tag `{tag}` not found"
        | _, None, _ -> Error $"Resource version `{resourceVersionId}` not found"
        | _, _, Some _ -> Error $"Tag `{tag}` already attached to resource version `{resourceVersionId}`"
        | Some l, Some n, None -> addResourceVersionTag ctx n.Id l.Name |> Ok

    let activateResourceVersionTag (ctx: SqliteContext) (resourceVersionId: string) (tag: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE resource_version_tags SET active = TRUE WHERE resource_version_id = @0 AND tag = @1",
            [ resourceVersionId; tag ]
        )

    let deactivateResourceVersionTag (ctx: SqliteContext) (resourceVersionId: string) (tag: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE resource_version_tags SET active = FALSE WHERE resource_version_id = @0 AND tag = @1",
            [ resourceVersionId; tag ]
        )

    // *** Notes ***

    let getNote (ctx: SqliteContext) (noteId: string) =
        Operations.selectResourceVersionNoteRecord ctx [ "WHERE note_id = @0" ] [ noteId ]

    let getAllActiveNotes (ctx: SqliteContext) (resourceVersionId: string) =
        Operations.selectResourceVersionNoteRecord ctx [ "WHERE resource_version_id = @0 AND active = TRUE;" ] [ resourceVersionId ]

    let getAllNotes (ctx: SqliteContext) (resourceVersionId: string) =
        Operations.selectResourceVersionNoteRecord ctx [ "WHERE resource_version_id = @0" ] [ resourceVersionId ]

    let getLatestNoteVersion (ctx: SqliteContext) (noteId: string) =
        Operations.selectResourceVersionNoteVersionRecord
            ctx
            [ "WHERE resource_version_note_id = @0 ORDER BY version DESC LIMIT 1" ]
            [ noteId ]

    let getLatestActiveNoteVersion (ctx: SqliteContext) (noteId: string) =
        Operations.selectResourceVersionNoteVersionRecord
            ctx
            [ "WHERE resource_version_note_id = @0 AND active = TRUE ORDER BY version DESC LIMIT 1" ]
            [ noteId ]

    let activateNote (ctx: SqliteContext) (noteId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_version_notes SET active = TRUE WHERE id = @0", [ noteId ])

    let deactivateNote (ctx: SqliteContext) (noteId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_version_notes SET active = FALSE WHERE id = @0", [ noteId ])

    let activateNoteVersion (ctx: SqliteContext) (noteVersionId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_version_note_versions SET active = TRUE WHERE id = @0", [ noteVersionId ])

    let deactivateNoteVersion (ctx: SqliteContext) (noteVersionId: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resource_version_note_versions SET active = FALSE WHERE id = @0", [ noteVersionId ])


    let addNote (ctx: SqliteContext) (id: IdType option) (resourceVersionId: string) =
        ({ Id = getId id
           ResourceVersionId = resourceVersionId
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewResourceVersionNote)
        |> Operations.insertResourceVersionNote ctx

    let addNoteVersion
        (ctx: SqliteContext)
        (id: IdType option)
        (noteId: string)
        (version: int)
        (title: string)
        (note: string)
        =
        use ms = new MemoryStream(note.ToUtf8Bytes())
        let hash = ms.GetSHA256Hash()

        ({ Id = getId id
           ResourceVersionNoteId = noteId
           Version = version
           Title = title
           Note = BlobField.FromStream ms
           Hash = hash
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewResourceVersionNoteVersion)
        |> Operations.insertResourceVersionNoteVersion ctx

    let tryAddLatestNoteVersion
        (ctx: SqliteContext)
        (id: IdType option)
        (noteId: string)
        (title: string)
        (note: string)
        =
        match getLatestNoteVersion ctx noteId with
        | Some lnv -> addNoteVersion ctx id noteId (lnv.Version + 1) title note |> Ok
        | None -> Error $"ResourceVersion note `{noteId}` does not exist"

    let addNewNote (ctx: SqliteContext) (resourceVersionId: string) (title: string) (note: string) =
        let id = IdType.Create()
        addNote ctx (Some id) resourceVersionId
        addNoteVersion ctx None (id.GetId()) 1 title note

    let tryAddNewNote (ctx: SqliteContext) (resourceVersionId: string) (title: string) (note: string) =
        match get ctx resourceVersionId with
        | Some _ -> addNewNote ctx resourceVersionId title note |> Ok
        | None -> Error $"Resource version `{resourceVersionId}` does not exist"
