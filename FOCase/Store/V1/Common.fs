namespace FOCase.Store.V1

open FOCase.Core

[<AutoOpen>]
module Common =

    open System
    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Core.Compression
    open FOCase.Core.Encryption
    open FOCase.Core.FileTypes
    open FOCase.Store.V1.Persistence

    module Internal =

        let createTables (ctx: SqliteContext) =
            [ // Top level
              Records.MetadataItem.CreateTableSql()
              Records.Node.CreateTableSql()
              Records.Document.CreateTableSql()
              Records.Label.CreateTableSql()
              Records.Resource.CreateTableSql()
              Records.Tag.CreateTableSql()
              Records.CompressionType.CreateTableSql()
              Records.FileType.CreateTableSql()
              Records.EncryptionType.CreateTableSql()
              // Second level
              Records.Connection.CreateTableSql()
              Records.ExternalConnection.CreateTableSql()
              // Nodes
              Records.NodeLabel.CreateTableSql()
              Records.NodeTag.CreateTableSql()
              Records.NodeMetadataItem.CreateTableSql()
              Records.NodeNote.CreateTableSql()
              Records.NodeNoteVersion.CreateTableSql()
              // Connections
              Records.ConnectionLabel.CreateTableSql()
              Records.ConnectionTag.CreateTableSql()
              Records.ConnectionMetadataItem.CreateTableSql()
              Records.ConnectionNote.CreateTableSql()
              Records.ConnectionNoteVersion.CreateTableSql()
              // External connections (5) 26
              Records.ExternalConnectionLabel.CreateTableSql()
              Records.ExternalConnectionTag.CreateTableSql()
              Records.ExternalConnectionMetadataItem.CreateTableSql()
              Records.ExternalConnectionNote.CreateTableSql()
              Records.ExternalConnectionNoteVersion.CreateTableSql()
              // Documents
              Records.DocumentMetadataItem.CreateTableSql()
              Records.DocumentTag.CreateTableSql()
              Records.DocumentNote.CreateTableSql()
              Records.DocumentNoteVersion.CreateTableSql()
              // Document versions
              Records.DocumentVersion.CreateTableSql()
              Records.DocumentVersionMetadataItem.CreateTableSql()
              Records.DocumentVersionTag.CreateTableSql()
              Records.DocumentVersionNote.CreateTableSql()
              Records.DocumentVersionNoteVersion.CreateTableSql()
              // Resources
              Records.ResourceMetadataItem.CreateTableSql()
              Records.ResourceTag.CreateTableSql()
              Records.ResourceNote.CreateTableSql()
              Records.ResourceNoteVersion.CreateTableSql()
              // Resource versions
              Records.ResourceVersion.CreateTableSql()
              Records.ResourceVersionMetadataItem.CreateTableSql()
              Records.ResourceVersionTag.CreateTableSql()
              Records.ResourceVersionNote.CreateTableSql()
              Records.ResourceVersionNoteVersion.CreateTableSql()
              // Node documents
              Records.NodeDocument.CreateTableSql()
              Records.NodeDocumentMetadataItem.CreateTableSql()
              Records.NodeDocumentNote.CreateTableSql()
              Records.NodeDocumentNoteVersion.CreateTableSql()
              // Node resources
              Records.NodeResource.CreateTableSql()
              Records.NodeResourceMetadataItem.CreateTableSql()
              Records.NodeResourceNote.CreateTableSql()
              Records.NodeResourceNoteVersion.CreateTableSql()
              // Connection documents
              Records.ConnectionDocument.CreateTableSql()
              Records.ConnectionDocumentMetadataItem.CreateTableSql()
              Records.ConnectionDocumentNote.CreateTableSql()
              Records.ConnectionDocumentNoteVersion.CreateTableSql()
              // Connection resources
              Records.ConnectionResource.CreateTableSql()
              Records.ConnectionResourceMetadataItem.CreateTableSql()
              Records.ConnectionResourceNote.CreateTableSql()
              Records.ConnectionResourceNoteVersion.CreateTableSql()
              // External connection documents
              Records.ExternalConnectionDocument.CreateTableSql()
              Records.ExternalConnectionDocumentMetadataItem.CreateTableSql()
              Records.ExternalConnectionDocumentNote.CreateTableSql()
              Records.ExternalConnectionDocumentNoteVersion.CreateTableSql()
              // External connection resources
              Records.ExternalConnectionResource.CreateTableSql()
              Records.ExternalConnectionResourceMetadataItem.CreateTableSql()
              Records.ExternalConnectionResourceNote.CreateTableSql()
              Records.ExternalConnectionResourceNoteVersion.CreateTableSql() ]
            |> List.iter (ctx.ExecuteSqlNonQuery >> ignore)

            ctx

        let seedData (ctx: SqliteContext) =
            CompressionType.All()
            |> List.iter (fun ct ->
                ({ Name = ct.Serialize() }: Parameters.NewCompressionType)
                |> Operations.insertCompressionType ctx)

            EncryptionType.All()
            |> List.iter (fun et ->
                ({ Name = et.Serialize() }: Parameters.NewEncryptionType)
                |> Operations.insertEncryptionType ctx)

            FileType.All()
            |> List.iter (fun ft ->
                ({ Name = ft.Serialize()
                   Extension = ft.GetExtension()
                   ContentType = ft.GetContentType() }
                : Parameters.NewFileType)
                |> Operations.insertFileType ctx)

            ctx

        let initialize (ctx: SqliteContext) =
            ctx.ExecuteInTransaction(createTables >> seedData)
            
     
    let getTimestamp _ = DateTime.UtcNow       
            
    let getId (id: IdType option) = (id |> Option.defaultWith (fun _ -> IdType.Create())).GetId()
    
    
    let labelWeightComparisonToSql (initialParameterIndex: int) (comparison: LabelWeightComparison) =
        let rec handler (parameterIndex: int) (com: LabelWeightComparison) =
            ()
        
        
        handler initialParameterIndex comparison