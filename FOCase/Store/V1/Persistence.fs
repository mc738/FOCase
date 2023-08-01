namespace FOCase.Store.V1.Persistence

open System
open System.Text.Json.Serialization
open Freql.Core.Common
open Freql.Sqlite

/// Module generated on 01/08/2023 19:57:08 (utc) via Freql.Tools.
[<RequireQualifiedAccess>]
module Records =
    /// A record representing a row in the table `compression_types`.
    type CompressionType =
        { [<JsonPropertyName("name")>] Name: string }
    
        static member Blank() =
            { Name = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE compression_types (
	name TEXT NOT NULL,
	CONSTRAINT compression_types_PK PRIMARY KEY (name)
)
        """
    
        static member SelectSql() = """
        SELECT
              compression_types.`name`
        FROM compression_types
        """
    
        static member TableName() = "compression_types"
    
    /// A record representing a row in the table `connection_document_metadata`.
    type ConnectionDocumentMetadataItem =
        { [<JsonPropertyName("connectionDocumentId")>] ConnectionDocumentId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ConnectionDocumentId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connection_document_metadata (
	connection_document_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT connection_document_metadata_PK PRIMARY KEY (connection_document_id,item_key),
	CONSTRAINT connection_document_metadata_FK FOREIGN KEY (connection_document_id) REFERENCES connection_documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              connection_document_metadata.`connection_document_id`,
              connection_document_metadata.`item_key`,
              connection_document_metadata.`item_value`,
              connection_document_metadata.`created_on`,
              connection_document_metadata.`active`
        FROM connection_document_metadata
        """
    
        static member TableName() = "connection_document_metadata"
    
    /// A record representing a row in the table `connection_document_note_versions`.
    type ConnectionDocumentNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionDocumentNoteId")>] ConnectionDocumentNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionDocumentNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connection_document_note_versions (
	id TEXT NOT NULL,
	connection_document_note_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	title TEXT NOT NULL,
	note BLOB NOT NULL,
	hash TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT connection_document_note_versions_PK PRIMARY KEY (id),
	CONSTRAINT connection_document_note_versions_UN UNIQUE (connection_document_note_id,version),
	CONSTRAINT connection_document_note_versions_FK FOREIGN KEY (connection_document_note_id) REFERENCES connection_document_notes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              connection_document_note_versions.`id`,
              connection_document_note_versions.`connection_document_note_id`,
              connection_document_note_versions.`version`,
              connection_document_note_versions.`created_on`,
              connection_document_note_versions.`title`,
              connection_document_note_versions.`note`,
              connection_document_note_versions.`hash`,
              connection_document_note_versions.`active`
        FROM connection_document_note_versions
        """
    
        static member TableName() = "connection_document_note_versions"
    
    /// A record representing a row in the table `connection_document_notes`.
    type ConnectionDocumentNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionDocumentId")>] ConnectionDocumentId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionDocumentId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connection_document_notes (
	id TEXT NOT NULL,
	connection_document_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT connection_document_notes_PK PRIMARY KEY (id),
	CONSTRAINT connection_document_notes_FK FOREIGN KEY (connection_document_id) REFERENCES connection_documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              connection_document_notes.`id`,
              connection_document_notes.`connection_document_id`,
              connection_document_notes.`created_on`,
              connection_document_notes.`active`
        FROM connection_document_notes
        """
    
        static member TableName() = "connection_document_notes"
    
    /// A record representing a row in the table `connection_documents`.
    type ConnectionDocument =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionId")>] ConnectionId: string
          [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionId = String.Empty
              DocumentVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connection_documents (
	id TEXT NOT NULL,
    connection_id TEXT NOT NULL,
	document_version_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT connection_documents_PK PRIMARY KEY (id),
	CONSTRAINT connection_documents_UN UNIQUE (connection_id,document_version_id),
	CONSTRAINT connection_documents_FK FOREIGN KEY (connection_id) REFERENCES connections(id),
	CONSTRAINT connection_documents_FK_1 FOREIGN KEY (document_version_id) REFERENCES documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              connection_documents.`id`,
              connection_documents.`connection_id`,
              connection_documents.`document_version_id`,
              connection_documents.`created_on`,
              connection_documents.`active`
        FROM connection_documents
        """
    
        static member TableName() = "connection_documents"
    
    /// A record representing a row in the table `connection_labels`.
    type ConnectionLabel =
        { [<JsonPropertyName("connectionId")>] ConnectionId: string
          [<JsonPropertyName("label")>] Label: string
          [<JsonPropertyName("weight")>] Weight: decimal
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ConnectionId = String.Empty
              Label = String.Empty
              Weight = 0m
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connection_labels (
	connection_id TEXT NOT NULL,
	label TEXT NOT NULL,
	weight REAL NOT NULL, 
	created_on TEXT NOT NULL, 
	active INTEGER NOT NULL,
	CONSTRAINT connection_labels_PK PRIMARY KEY (connection_id,label),
	CONSTRAINT connection_labels_FK FOREIGN KEY (connection_id) REFERENCES connections(id),
	CONSTRAINT connection_labels_FK_1 FOREIGN KEY (label) REFERENCES labels(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              connection_labels.`connection_id`,
              connection_labels.`label`,
              connection_labels.`weight`,
              connection_labels.`created_on`,
              connection_labels.`active`
        FROM connection_labels
        """
    
        static member TableName() = "connection_labels"
    
    /// A record representing a row in the table `connection_metadata`.
    type ConnectionMetadataItem =
        { [<JsonPropertyName("connectionId")>] ConnectionId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ConnectionId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connection_metadata (
	connection_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT connection_metadata_PK PRIMARY KEY (connection_id,item_key),
	CONSTRAINT connection_metadata_FK FOREIGN KEY (connection_id) REFERENCES connections(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              connection_metadata.`connection_id`,
              connection_metadata.`item_key`,
              connection_metadata.`item_value`,
              connection_metadata.`created_on`,
              connection_metadata.`active`
        FROM connection_metadata
        """
    
        static member TableName() = "connection_metadata"
    
    /// A record representing a row in the table `connection_note_versions`.
    type ConnectionNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionNoteId")>] ConnectionNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connection_note_versions (
	id TEXT NOT NULL,
	connection_note_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	title TEXT NOT NULL,
	note BLOB NOT NULL,
	hash TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT connection_note_versions_PK PRIMARY KEY (id),
	CONSTRAINT connection_note_versions_UN UNIQUE (connection_note_id,version),
	CONSTRAINT connection_note_versions_FK FOREIGN KEY (connection_note_id) REFERENCES connection_notes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              connection_note_versions.`id`,
              connection_note_versions.`connection_note_id`,
              connection_note_versions.`version`,
              connection_note_versions.`created_on`,
              connection_note_versions.`title`,
              connection_note_versions.`note`,
              connection_note_versions.`hash`,
              connection_note_versions.`active`
        FROM connection_note_versions
        """
    
        static member TableName() = "connection_note_versions"
    
    /// A record representing a row in the table `connection_notes`.
    type ConnectionNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionId")>] ConnectionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connection_notes (
	id TEXT NOT NULL,
	connection_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT connection_notes_PK PRIMARY KEY (id),
	CONSTRAINT connection_notes_FK FOREIGN KEY (connection_id) REFERENCES connections(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              connection_notes.`id`,
              connection_notes.`connection_id`,
              connection_notes.`created_on`,
              connection_notes.`active`
        FROM connection_notes
        """
    
        static member TableName() = "connection_notes"
    
    /// A record representing a row in the table `connection_resource_metadata`.
    type ConnectionResourceMetadataItem =
        { [<JsonPropertyName("connectionResourceId")>] ConnectionResourceId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ConnectionResourceId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connection_resource_metadata (
	connection_resource_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT connection_resource_metadata_PK PRIMARY KEY (connection_resource_id,item_key),
	CONSTRAINT connection_resource_metadata_FK FOREIGN KEY (connection_resource_id) REFERENCES connection_resources(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              connection_resource_metadata.`connection_resource_id`,
              connection_resource_metadata.`item_key`,
              connection_resource_metadata.`item_value`,
              connection_resource_metadata.`created_on`,
              connection_resource_metadata.`active`
        FROM connection_resource_metadata
        """
    
        static member TableName() = "connection_resource_metadata"
    
    /// A record representing a row in the table `connection_resource_note_versions`.
    type ConnectionResourceNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionResourceNoteId")>] ConnectionResourceNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionResourceNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connection_resource_note_versions (
	id TEXT NOT NULL,
	connection_resource_note_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	title TEXT NOT NULL,
	note BLOB NOT NULL,
	hash TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT connection_resource_note_versions_PK PRIMARY KEY (id),
	CONSTRAINT connection_resource_note_versions_UN UNIQUE (connection_resource_note_id,version),
	CONSTRAINT connection_resource_note_versions_FK FOREIGN KEY (connection_resource_note_id) REFERENCES connection_resource_notes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              connection_resource_note_versions.`id`,
              connection_resource_note_versions.`connection_resource_note_id`,
              connection_resource_note_versions.`version`,
              connection_resource_note_versions.`created_on`,
              connection_resource_note_versions.`title`,
              connection_resource_note_versions.`note`,
              connection_resource_note_versions.`hash`,
              connection_resource_note_versions.`active`
        FROM connection_resource_note_versions
        """
    
        static member TableName() = "connection_resource_note_versions"
    
    /// A record representing a row in the table `connection_resource_notes`.
    type ConnectionResourceNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionResourceId")>] ConnectionResourceId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionResourceId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connection_resource_notes (
	id TEXT NOT NULL,
	connection_resource_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT connection_resource_notes_PK PRIMARY KEY (id),
	CONSTRAINT connection_resource_notes_FK FOREIGN KEY (connection_resource_id) REFERENCES connection_resources(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              connection_resource_notes.`id`,
              connection_resource_notes.`connection_resource_id`,
              connection_resource_notes.`created_on`,
              connection_resource_notes.`active`
        FROM connection_resource_notes
        """
    
        static member TableName() = "connection_resource_notes"
    
    /// A record representing a row in the table `connection_resources`.
    type ConnectionResource =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionId")>] ConnectionId: string
          [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionId = String.Empty
              ResourceVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connection_resources (
	id TEXT NOT NULL,
    connection_id TEXT NOT NULL,
	resource_version_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT connection_resource_PK PRIMARY KEY (id),
	CONSTRAINT connection_resource_UN UNIQUE (connection_id,resource_version_id),
	CONSTRAINT connection_resource_FK FOREIGN KEY (connection_id) REFERENCES connections(id),
	CONSTRAINT connection_resource_FK_1 FOREIGN KEY (resource_version_id) REFERENCES resources(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              connection_resources.`id`,
              connection_resources.`connection_id`,
              connection_resources.`resource_version_id`,
              connection_resources.`created_on`,
              connection_resources.`active`
        FROM connection_resources
        """
    
        static member TableName() = "connection_resources"
    
    /// A record representing a row in the table `connection_tags`.
    type ConnectionTag =
        { [<JsonPropertyName("connectionId")>] ConnectionId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ConnectionId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connection_tags (
	connection_id TEXT NOT NULL,
	tag TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT connection_tags_PK PRIMARY KEY (connection_id,tag),
	CONSTRAINT connection_tags_FK FOREIGN KEY (connection_id) REFERENCES connections(id),
	CONSTRAINT connection_tags_FK_1 FOREIGN KEY (tag) REFERENCES tags(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              connection_tags.`connection_id`,
              connection_tags.`tag`,
              connection_tags.`created_on`,
              connection_tags.`active`
        FROM connection_tags
        """
    
        static member TableName() = "connection_tags"
    
    /// A record representing a row in the table `connections`.
    type Connection =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("fromNode")>] FromNode: string
          [<JsonPropertyName("toNode")>] ToNode: string
          [<JsonPropertyName("twoWay")>] TwoWay: bool
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              FromNode = String.Empty
              ToNode = String.Empty
              TwoWay = true
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE connections (
	id TEXT NOT NULL,
	name TEXT NOT NULL,
	from_node TEXT NOT NULL,
	to_node TEXT NOT NULL,
	two_way INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT connections_PK PRIMARY KEY (id),
	CONSTRAINT connections_FK FOREIGN KEY (from_node) REFERENCES nodes(id),
	CONSTRAINT connections_FK_1 FOREIGN KEY (to_node) REFERENCES nodes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              connections.`id`,
              connections.`name`,
              connections.`from_node`,
              connections.`to_node`,
              connections.`two_way`,
              connections.`created_on`,
              connections.`active`
        FROM connections
        """
    
        static member TableName() = "connections"
    
    /// A record representing a row in the table `document_metadata`.
    type DocumentMetadataItem =
        { [<JsonPropertyName("documentId")>] DocumentId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { DocumentId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE document_metadata (
	document_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT document_metadata_PK PRIMARY KEY (document_id,item_key),
	CONSTRAINT document_metadata_FK FOREIGN KEY (document_id) REFERENCES documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              document_metadata.`document_id`,
              document_metadata.`item_key`,
              document_metadata.`item_value`,
              document_metadata.`created_on`,
              document_metadata.`active`
        FROM document_metadata
        """
    
        static member TableName() = "document_metadata"
    
    /// A record representing a row in the table `document_note_versions`.
    type DocumentNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("documentNoteId")>] DocumentNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              DocumentNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE document_note_versions (
	id TEXT NOT NULL,
	document_note_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	title TEXT NOT NULL,
	note BLOB NOT NULL,
	hash TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT document_note_versions_PK PRIMARY KEY (id),
	CONSTRAINT document_note_versions_UN UNIQUE (document_note_id,version),
	CONSTRAINT document_note_versions_FK FOREIGN KEY (document_note_id) REFERENCES document_notes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              document_note_versions.`id`,
              document_note_versions.`document_note_id`,
              document_note_versions.`version`,
              document_note_versions.`created_on`,
              document_note_versions.`title`,
              document_note_versions.`note`,
              document_note_versions.`hash`,
              document_note_versions.`active`
        FROM document_note_versions
        """
    
        static member TableName() = "document_note_versions"
    
    /// A record representing a row in the table `document_notes`.
    type DocumentNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("documentId")>] DocumentId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              DocumentId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE document_notes (
	id TEXT NOT NULL,
	document_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT document_notes_PK PRIMARY KEY (id),
	CONSTRAINT document_notes_FK FOREIGN KEY (document_id) REFERENCES documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              document_notes.`id`,
              document_notes.`document_id`,
              document_notes.`created_on`,
              document_notes.`active`
        FROM document_notes
        """
    
        static member TableName() = "document_notes"
    
    /// A record representing a row in the table `document_tags`.
    type DocumentTag =
        { [<JsonPropertyName("documentId")>] DocumentId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { DocumentId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE document_tags (
	document_id TEXT NOT NULL,
	tag TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT document_tags_PK PRIMARY KEY (document_id,tag),
	CONSTRAINT document_tags_FK FOREIGN KEY (document_id) REFERENCES documents(id),
	CONSTRAINT document_tags_FK_1 FOREIGN KEY (tag) REFERENCES tags(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              document_tags.`document_id`,
              document_tags.`tag`,
              document_tags.`created_on`,
              document_tags.`active`
        FROM document_tags
        """
    
        static member TableName() = "document_tags"
    
    /// A record representing a row in the table `document_version_metadata`.
    type DocumentVersionMetadataItem =
        { [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { DocumentVersionId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE document_version_metadata (
	document_version_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT document_version_metadata_PK PRIMARY KEY (document_version_id,item_key),
	CONSTRAINT document_version_metadata_FK FOREIGN KEY (document_version_id) REFERENCES document_versions(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              document_version_metadata.`document_version_id`,
              document_version_metadata.`item_key`,
              document_version_metadata.`item_value`,
              document_version_metadata.`created_on`,
              document_version_metadata.`active`
        FROM document_version_metadata
        """
    
        static member TableName() = "document_version_metadata"
    
    /// A record representing a row in the table `document_version_note_versions`.
    type DocumentVersionNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("documentVersionNoteId")>] DocumentVersionNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              DocumentVersionNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE document_version_note_versions (
	id TEXT NOT NULL,
	document_version_note_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	title TEXT NOT NULL,
	note BLOB NOT NULL,
	hash TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT document_version_note_versions_PK PRIMARY KEY (id),
	CONSTRAINT document_version_note_versions_UN UNIQUE (document_version_note_id,version),
	CONSTRAINT document_version_note_versions_FK FOREIGN KEY (document_version_note_id) REFERENCES document_version_notes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              document_version_note_versions.`id`,
              document_version_note_versions.`document_version_note_id`,
              document_version_note_versions.`version`,
              document_version_note_versions.`created_on`,
              document_version_note_versions.`title`,
              document_version_note_versions.`note`,
              document_version_note_versions.`hash`,
              document_version_note_versions.`active`
        FROM document_version_note_versions
        """
    
        static member TableName() = "document_version_note_versions"
    
    /// A record representing a row in the table `document_version_notes`.
    type DocumentVersionNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              DocumentVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE document_version_notes (
	id TEXT NOT NULL,
	document_version_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT document_version_notes_PK PRIMARY KEY (id),
	CONSTRAINT document_version_notes_FK FOREIGN KEY (document_version_id) REFERENCES document_versions(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              document_version_notes.`id`,
              document_version_notes.`document_version_id`,
              document_version_notes.`created_on`,
              document_version_notes.`active`
        FROM document_version_notes
        """
    
        static member TableName() = "document_version_notes"
    
    /// A record representing a row in the table `document_version_tags`.
    type DocumentVersionTag =
        { [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { DocumentVersionId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE document_version_tags (
	document_version_id TEXT NOT NULL,
	tag TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT document_version_tags_PK PRIMARY KEY (document_version_id,tag),
	CONSTRAINT document_version_tags_FK FOREIGN KEY (document_version_id) REFERENCES document_versions(id),
	CONSTRAINT document_version_tags_FK_1 FOREIGN KEY (tag) REFERENCES tags(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              document_version_tags.`document_version_id`,
              document_version_tags.`tag`,
              document_version_tags.`created_on`,
              document_version_tags.`active`
        FROM document_version_tags
        """
    
        static member TableName() = "document_version_tags"
    
    /// A record representing a row in the table `document_versions`.
    type DocumentVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("documentId")>] DocumentId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("rawData")>] RawData: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("encryptionType")>] EncryptionType: string
          [<JsonPropertyName("compressionType")>] CompressionType: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              DocumentId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              RawData = BlobField.Empty()
              Hash = String.Empty
              EncryptionType = String.Empty
              CompressionType = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE document_versions (
	id TEXT NOT NULL,
	document_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	raw_data BLOB NOT NULL,
	hash TEXT NOT NULL,
	encryption_type TEXT NOT NULL,
	compression_type TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT document_versions_PK PRIMARY KEY (id),
	CONSTRAINT document_versions_UN UNIQUE (document_id,version),
	CONSTRAINT document_versions_FK FOREIGN KEY (document_id) REFERENCES documents(id),
	CONSTRAINT document_versions_FK_1 FOREIGN KEY (encryption_type) REFERENCES encryption_types(name),
	CONSTRAINT document_versions_FK_2 FOREIGN KEY (compression_type) REFERENCES compression_types(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              document_versions.`id`,
              document_versions.`document_id`,
              document_versions.`version`,
              document_versions.`created_on`,
              document_versions.`raw_data`,
              document_versions.`hash`,
              document_versions.`encryption_type`,
              document_versions.`compression_type`,
              document_versions.`active`
        FROM document_versions
        """
    
        static member TableName() = "document_versions"
    
    /// A record representing a row in the table `documents`.
    type Document =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE documents (
	id TEXT NOT NULL,
	name TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT documents_PK PRIMARY KEY (id)
)
        """
    
        static member SelectSql() = """
        SELECT
              documents.`id`,
              documents.`name`,
              documents.`created_on`,
              documents.`active`
        FROM documents
        """
    
        static member TableName() = "documents"
    
    /// A record representing a row in the table `encryption_types`.
    type EncryptionType =
        { [<JsonPropertyName("name")>] Name: string }
    
        static member Blank() =
            { Name = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE encryption_types (
	name TEXT NOT NULL,
	CONSTRAINT encryption_types_PK PRIMARY KEY (name)
)
        """
    
        static member SelectSql() = """
        SELECT
              encryption_types.`name`
        FROM encryption_types
        """
    
        static member TableName() = "encryption_types"
    
    /// A record representing a row in the table `events`.
    type EventItem =
        { [<JsonPropertyName("eventType")>] EventType: string
          [<JsonPropertyName("eventTimestamp")>] EventTimestamp: DateTime
          [<JsonPropertyName("eventBlob")>] EventBlob: string }
    
        static member Blank() =
            { EventType = String.Empty
              EventTimestamp = DateTime.UtcNow
              EventBlob = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE events (
	event_type TEXT NOT NULL,
	event_timestamp TEXT NOT NULL,
	event_blob TEXT NOT NULL
)
        """
    
        static member SelectSql() = """
        SELECT
              events.`event_type`,
              events.`event_timestamp`,
              events.`event_blob`
        FROM events
        """
    
        static member TableName() = "events"
    
    /// A record representing a row in the table `external_connection_document_metadata`.
    type ExternalConnectionDocumentMetadataItem =
        { [<JsonPropertyName("externalConnectionDocumentId")>] ExternalConnectionDocumentId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ExternalConnectionDocumentId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connection_document_metadata (
	external_connection_document_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT external_connection_document_metadata_PK PRIMARY KEY (external_connection_document_id,item_key),
	CONSTRAINT external_connection_document_metadata_FK FOREIGN KEY (external_connection_document_id) REFERENCES external_connection_documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connection_document_metadata.`external_connection_document_id`,
              external_connection_document_metadata.`item_key`,
              external_connection_document_metadata.`item_value`,
              external_connection_document_metadata.`created_on`,
              external_connection_document_metadata.`active`
        FROM external_connection_document_metadata
        """
    
        static member TableName() = "external_connection_document_metadata"
    
    /// A record representing a row in the table `external_connection_document_note_versions`.
    type ExternalConnectionDocumentNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionDocumentNoteId")>] ExternalConnectionDocumentNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionDocumentNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connection_document_note_versions (
	id TEXT NOT NULL,
	external_connection_document_note_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	title TEXT NOT NULL,
	note BLOB NOT NULL,
	hash TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT external_connection_document_note_versions_PK PRIMARY KEY (id),
	CONSTRAINT external_connection_document_note_versions_UN UNIQUE (external_connection_document_note_id,version),
	CONSTRAINT external_connection_document_note_versions_FK FOREIGN KEY (external_connection_document_note_id) REFERENCES external_connection_document_notes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connection_document_note_versions.`id`,
              external_connection_document_note_versions.`external_connection_document_note_id`,
              external_connection_document_note_versions.`version`,
              external_connection_document_note_versions.`created_on`,
              external_connection_document_note_versions.`title`,
              external_connection_document_note_versions.`note`,
              external_connection_document_note_versions.`hash`,
              external_connection_document_note_versions.`active`
        FROM external_connection_document_note_versions
        """
    
        static member TableName() = "external_connection_document_note_versions"
    
    /// A record representing a row in the table `external_connection_document_notes`.
    type ExternalConnectionDocumentNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionDocumentId")>] ExternalConnectionDocumentId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionDocumentId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connection_document_notes (
	id TEXT NOT NULL,
	external_connection_document_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT external_connection_document_notes_PK PRIMARY KEY (id),
	CONSTRAINT external_connection_document_notes_FK FOREIGN KEY (external_connection_document_id) REFERENCES external_connection_documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connection_document_notes.`id`,
              external_connection_document_notes.`external_connection_document_id`,
              external_connection_document_notes.`created_on`,
              external_connection_document_notes.`active`
        FROM external_connection_document_notes
        """
    
        static member TableName() = "external_connection_document_notes"
    
    /// A record representing a row in the table `external_connection_documents`.
    type ExternalConnectionDocument =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionId")>] ExternalConnectionId: string
          [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionId = String.Empty
              DocumentVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connection_documents (
	id TEXT NOT NULL,
    external_connection_id TEXT NOT NULL,
	document_version_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT external_connection_documents_PK PRIMARY KEY (id),
	CONSTRAINT external_connection_documents_UN UNIQUE (external_connection_id,document_version_id),
	CONSTRAINT external_connection_documents_FK FOREIGN KEY (external_connection_id) REFERENCES external_connections(id),
	CONSTRAINT external_connection_documents_FK_1 FOREIGN KEY (document_version_id) REFERENCES documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connection_documents.`id`,
              external_connection_documents.`external_connection_id`,
              external_connection_documents.`document_version_id`,
              external_connection_documents.`created_on`,
              external_connection_documents.`active`
        FROM external_connection_documents
        """
    
        static member TableName() = "external_connection_documents"
    
    /// A record representing a row in the table `external_connection_labels`.
    type ExternalConnectionLabel =
        { [<JsonPropertyName("externalConnectionId")>] ExternalConnectionId: string
          [<JsonPropertyName("label")>] Label: string
          [<JsonPropertyName("weight")>] Weight: decimal
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ExternalConnectionId = String.Empty
              Label = String.Empty
              Weight = 0m
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connection_labels (
	external_connection_id TEXT NOT NULL,
	label TEXT NOT NULL,
	weight REAL NOT NULL, 
	created_on TEXT NOT NULL, 
	active INTEGER NOT NULL,
	CONSTRAINT external_connection_labels_PK PRIMARY KEY (external_connection_id,label),
	CONSTRAINT external_connection_labels_FK FOREIGN KEY (external_connection_id) REFERENCES external_connections(id),
	CONSTRAINT external_connection_labels_FK_1 FOREIGN KEY (label) REFERENCES labels(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connection_labels.`external_connection_id`,
              external_connection_labels.`label`,
              external_connection_labels.`weight`,
              external_connection_labels.`created_on`,
              external_connection_labels.`active`
        FROM external_connection_labels
        """
    
        static member TableName() = "external_connection_labels"
    
    /// A record representing a row in the table `external_connection_metadata`.
    type ExternalConnectionMetadataItem =
        { [<JsonPropertyName("externalConnectionId")>] ExternalConnectionId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ExternalConnectionId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connection_metadata (
	external_connection_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT external_connection_metadata_PK PRIMARY KEY (external_connection_id,item_key),
	CONSTRAINT external_connection_metadata_FK FOREIGN KEY (external_connection_id) REFERENCES external_connections(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connection_metadata.`external_connection_id`,
              external_connection_metadata.`item_key`,
              external_connection_metadata.`item_value`,
              external_connection_metadata.`created_on`,
              external_connection_metadata.`active`
        FROM external_connection_metadata
        """
    
        static member TableName() = "external_connection_metadata"
    
    /// A record representing a row in the table `external_connection_note_versions`.
    type ExternalConnectionNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionNoteId")>] ExternalConnectionNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connection_note_versions (
	id TEXT NOT NULL,
	external_connection_note_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	title TEXT NOT NULL,
	note BLOB NOT NULL,
	hash TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT external_connection_note_versions_PK PRIMARY KEY (id),
	CONSTRAINT external_connection_note_versions_UN UNIQUE (external_connection_note_id,version),
	CONSTRAINT external_connection_note_versions_FK FOREIGN KEY (external_connection_note_id) REFERENCES external_connection_notes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connection_note_versions.`id`,
              external_connection_note_versions.`external_connection_note_id`,
              external_connection_note_versions.`version`,
              external_connection_note_versions.`created_on`,
              external_connection_note_versions.`title`,
              external_connection_note_versions.`note`,
              external_connection_note_versions.`hash`,
              external_connection_note_versions.`active`
        FROM external_connection_note_versions
        """
    
        static member TableName() = "external_connection_note_versions"
    
    /// A record representing a row in the table `external_connection_notes`.
    type ExternalConnectionNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionId")>] ExternalConnectionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connection_notes (
	id TEXT NOT NULL,
	external_connection_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT external_connection_notes_PK PRIMARY KEY (id),
	CONSTRAINT external_connection_notes_FK FOREIGN KEY (external_connection_id) REFERENCES external_connections(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connection_notes.`id`,
              external_connection_notes.`external_connection_id`,
              external_connection_notes.`created_on`,
              external_connection_notes.`active`
        FROM external_connection_notes
        """
    
        static member TableName() = "external_connection_notes"
    
    /// A record representing a row in the table `external_connection_resource_metadata`.
    type ExternalConnectionResourceMetadataItem =
        { [<JsonPropertyName("externalConnectionResourceId")>] ExternalConnectionResourceId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ExternalConnectionResourceId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connection_resource_metadata (
	external_connection_resource_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT external_connection_resource_metadata_PK PRIMARY KEY (external_connection_resource_id,item_key),
	CONSTRAINT external_connection_resource_metadata_FK FOREIGN KEY (external_connection_resource_id) REFERENCES external_connection_resources(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connection_resource_metadata.`external_connection_resource_id`,
              external_connection_resource_metadata.`item_key`,
              external_connection_resource_metadata.`item_value`,
              external_connection_resource_metadata.`created_on`,
              external_connection_resource_metadata.`active`
        FROM external_connection_resource_metadata
        """
    
        static member TableName() = "external_connection_resource_metadata"
    
    /// A record representing a row in the table `external_connection_resource_note_versions`.
    type ExternalConnectionResourceNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionResourceNoteId")>] ExternalConnectionResourceNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionResourceNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connection_resource_note_versions (
	id TEXT NOT NULL,
	external_connection_resource_note_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	title TEXT NOT NULL,
	note BLOB NOT NULL,
	hash TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT external_connection_resource_note_versions_PK PRIMARY KEY (id),
	CONSTRAINT external_connection_resource_note_versions_UN UNIQUE (external_connection_resource_note_id,version),
	CONSTRAINT external_connection_resource_note_versions_FK FOREIGN KEY (external_connection_resource_note_id) REFERENCES external_connection_resource_notes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connection_resource_note_versions.`id`,
              external_connection_resource_note_versions.`external_connection_resource_note_id`,
              external_connection_resource_note_versions.`version`,
              external_connection_resource_note_versions.`created_on`,
              external_connection_resource_note_versions.`title`,
              external_connection_resource_note_versions.`note`,
              external_connection_resource_note_versions.`hash`,
              external_connection_resource_note_versions.`active`
        FROM external_connection_resource_note_versions
        """
    
        static member TableName() = "external_connection_resource_note_versions"
    
    /// A record representing a row in the table `external_connection_resource_notes`.
    type ExternalConnectionResourceNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionResourceId")>] ExternalConnectionResourceId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionResourceId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connection_resource_notes (
	id TEXT NOT NULL,
	external_connection_resource_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT external_connection_resource_notes_PK PRIMARY KEY (id),
	CONSTRAINT external_connection_resource_notes_FK FOREIGN KEY (external_connection_resource_id) REFERENCES external_connection_resources(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connection_resource_notes.`id`,
              external_connection_resource_notes.`external_connection_resource_id`,
              external_connection_resource_notes.`created_on`,
              external_connection_resource_notes.`active`
        FROM external_connection_resource_notes
        """
    
        static member TableName() = "external_connection_resource_notes"
    
    /// A record representing a row in the table `external_connection_resources`.
    type ExternalConnectionResource =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionId")>] ExternalConnectionId: string
          [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionId = String.Empty
              ResourceVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connection_resources (
	id TEXT NOT NULL,
    external_connection_id TEXT NOT NULL,
	resource_version_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT external_connection_resource_PK PRIMARY KEY (id),
	CONSTRAINT external_connection_resource_UN UNIQUE (external_connection_id,resource_version_id),
	CONSTRAINT external_connection_resource_FK FOREIGN KEY (external_connection_id) REFERENCES external_connections(id),
	CONSTRAINT external_connection_resource_FK_1 FOREIGN KEY (resource_version_id) REFERENCES resources(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connection_resources.`id`,
              external_connection_resources.`external_connection_id`,
              external_connection_resources.`resource_version_id`,
              external_connection_resources.`created_on`,
              external_connection_resources.`active`
        FROM external_connection_resources
        """
    
        static member TableName() = "external_connection_resources"
    
    /// A record representing a row in the table `external_connection_tags`.
    type ExternalConnectionTag =
        { [<JsonPropertyName("externalConnectionId")>] ExternalConnectionId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ExternalConnectionId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connection_tags (
	external_connection_id TEXT NOT NULL,
	tag TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT external_connection_tags_PK PRIMARY KEY (external_connection_id,tag),
	CONSTRAINT external_connection_tags_FK FOREIGN KEY (external_connection_id) REFERENCES external_connections(id),
	CONSTRAINT external_connection_tags_FK_1 FOREIGN KEY (tag) REFERENCES tags(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connection_tags.`external_connection_id`,
              external_connection_tags.`tag`,
              external_connection_tags.`created_on`,
              external_connection_tags.`active`
        FROM external_connection_tags
        """
    
        static member TableName() = "external_connection_tags"
    
    /// A record representing a row in the table `external_connections`.
    type ExternalConnection =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("fromNode")>] FromNode: string
          [<JsonPropertyName("externalUri")>] ExternalUri: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              FromNode = String.Empty
              ExternalUri = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE external_connections (
	id TEXT NOT NULL,
	from_node TEXT NOT NULL,
	external_uri TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT external_connections_PK PRIMARY KEY (id),
	CONSTRAINT external_connections_FK FOREIGN KEY (from_node) REFERENCES nodes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_connections.`id`,
              external_connections.`from_node`,
              external_connections.`external_uri`,
              external_connections.`created_on`,
              external_connections.`active`
        FROM external_connections
        """
    
        static member TableName() = "external_connections"
    
    /// A record representing a row in the table `file_types`.
    type FileType =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("extension")>] Extension: string
          [<JsonPropertyName("contentType")>] ContentType: string }
    
        static member Blank() =
            { Name = String.Empty
              Extension = String.Empty
              ContentType = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE file_types (
	name TEXT NOT NULL,
	extension TEXT NOT NULL,
	content_type TEXT NOT NULL,
	CONSTRAINT file_types_PK PRIMARY KEY (name)
)
        """
    
        static member SelectSql() = """
        SELECT
              file_types.`name`,
              file_types.`extension`,
              file_types.`content_type`
        FROM file_types
        """
    
        static member TableName() = "file_types"
    
    /// A record representing a row in the table `labels`.
    type Label =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Name = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE labels (
	name TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT labels_PK PRIMARY KEY (name)
)
        """
    
        static member SelectSql() = """
        SELECT
              labels.`name`,
              labels.`created_on`,
              labels.`active`
        FROM labels
        """
    
        static member TableName() = "labels"
    
    /// A record representing a row in the table `metadata`.
    type MetadataItem =
        { [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE metadata (
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT metadata_PK PRIMARY KEY (item_key)
)
        """
    
        static member SelectSql() = """
        SELECT
              metadata.`item_key`,
              metadata.`item_value`,
              metadata.`created_on`,
              metadata.`active`
        FROM metadata
        """
    
        static member TableName() = "metadata"
    
    /// A record representing a row in the table `node_document_metadata`.
    type NodeDocumentMetadataItem =
        { [<JsonPropertyName("nodeDocumentId")>] NodeDocumentId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { NodeDocumentId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE node_document_metadata (
	node_document_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT node_document_metadata_PK PRIMARY KEY (node_document_id,item_key),
	CONSTRAINT node_document_metadata_FK FOREIGN KEY (node_document_id) REFERENCES node_documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_document_metadata.`node_document_id`,
              node_document_metadata.`item_key`,
              node_document_metadata.`item_value`,
              node_document_metadata.`created_on`,
              node_document_metadata.`active`
        FROM node_document_metadata
        """
    
        static member TableName() = "node_document_metadata"
    
    /// A record representing a row in the table `node_document_note_versions`.
    type NodeDocumentNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeDocumentNoteId")>] NodeDocumentNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeDocumentNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE node_document_note_versions (
	id TEXT NOT NULL,
	node_document_note_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	title TEXT NOT NULL,
	note BLOB NOT NULL,
	hash TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT node_document_note_versions_PK PRIMARY KEY (id),
	CONSTRAINT node_document_note_versions_UN UNIQUE (node_document_note_id,version),
	CONSTRAINT node_document_note_versions_FK FOREIGN KEY (node_document_note_id) REFERENCES node_document_notes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_document_note_versions.`id`,
              node_document_note_versions.`node_document_note_id`,
              node_document_note_versions.`version`,
              node_document_note_versions.`created_on`,
              node_document_note_versions.`title`,
              node_document_note_versions.`note`,
              node_document_note_versions.`hash`,
              node_document_note_versions.`active`
        FROM node_document_note_versions
        """
    
        static member TableName() = "node_document_note_versions"
    
    /// A record representing a row in the table `node_document_notes`.
    type NodeDocumentNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeDocumentId")>] NodeDocumentId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeDocumentId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE node_document_notes (
	id TEXT NOT NULL,
	node_document_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT node_document_notes_PK PRIMARY KEY (id),
	CONSTRAINT node_document_notes_FK FOREIGN KEY (node_document_id) REFERENCES node_documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_document_notes.`id`,
              node_document_notes.`node_document_id`,
              node_document_notes.`created_on`,
              node_document_notes.`active`
        FROM node_document_notes
        """
    
        static member TableName() = "node_document_notes"
    
    /// A record representing a row in the table `node_documents`.
    type NodeDocument =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeId = String.Empty
              DocumentVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE node_documents (
	id TEXT NOT NULL,
    node_id TEXT NOT NULL,
	document_version_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT node_documents_PK PRIMARY KEY (id),
	CONSTRAINT node_documents_UN UNIQUE (node_id,document_version_id),
	CONSTRAINT node_documents_FK FOREIGN KEY (node_id) REFERENCES nodes(id),
	CONSTRAINT node_documents_FK_1 FOREIGN KEY (document_version_id) REFERENCES documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_documents.`id`,
              node_documents.`node_id`,
              node_documents.`document_version_id`,
              node_documents.`created_on`,
              node_documents.`active`
        FROM node_documents
        """
    
        static member TableName() = "node_documents"
    
    /// A record representing a row in the table `node_labels`.
    type NodeLabel =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("label")>] Label: string
          [<JsonPropertyName("weight")>] Weight: decimal
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { NodeId = String.Empty
              Label = String.Empty
              Weight = 0m
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE node_labels (
	node_id TEXT NOT NULL,
	label TEXT NOT NULL,
	weight REAL NOT NULL, 
	created_on TEXT NOT NULL, 
	active INTEGER NOT NULL,
	CONSTRAINT node_labels_PK PRIMARY KEY (node_id,label),
	CONSTRAINT node_labels_FK FOREIGN KEY (node_id) REFERENCES nodes(id),
	CONSTRAINT node_labels_FK_1 FOREIGN KEY (label) REFERENCES labels(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_labels.`node_id`,
              node_labels.`label`,
              node_labels.`weight`,
              node_labels.`created_on`,
              node_labels.`active`
        FROM node_labels
        """
    
        static member TableName() = "node_labels"
    
    /// A record representing a row in the table `node_metadata`.
    type NodeMetadataItem =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { NodeId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE node_metadata (
	node_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT node_metadata_PK PRIMARY KEY (node_id,item_key),
	CONSTRAINT node_metadata_FK FOREIGN KEY (node_id) REFERENCES nodes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_metadata.`node_id`,
              node_metadata.`item_key`,
              node_metadata.`item_value`,
              node_metadata.`created_on`,
              node_metadata.`active`
        FROM node_metadata
        """
    
        static member TableName() = "node_metadata"
    
    /// A record representing a row in the table `node_note_versions`.
    type NodeNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeNoteId")>] NodeNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE node_note_versions (
	id TEXT NOT NULL,
	node_note_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	title TEXT NOT NULL,
	note BLOB NOT NULL,
	hash TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT node_note_versions_PK PRIMARY KEY (id),
	CONSTRAINT node_note_versions_UN UNIQUE (node_note_id,version),
	CONSTRAINT node_note_versions_FK FOREIGN KEY (node_note_id) REFERENCES node_notes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_note_versions.`id`,
              node_note_versions.`node_note_id`,
              node_note_versions.`version`,
              node_note_versions.`created_on`,
              node_note_versions.`title`,
              node_note_versions.`note`,
              node_note_versions.`hash`,
              node_note_versions.`active`
        FROM node_note_versions
        """
    
        static member TableName() = "node_note_versions"
    
    /// A record representing a row in the table `node_notes`.
    type NodeNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE node_notes (
	id TEXT NOT NULL,
	node_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT node_notes_PK PRIMARY KEY (id),
	CONSTRAINT node_notes_FK FOREIGN KEY (node_id) REFERENCES nodes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_notes.`id`,
              node_notes.`node_id`,
              node_notes.`created_on`,
              node_notes.`active`
        FROM node_notes
        """
    
        static member TableName() = "node_notes"
    
    /// A record representing a row in the table `node_resource_metadata`.
    type NodeResourceMetadataItem =
        { [<JsonPropertyName("nodeResourceId")>] NodeResourceId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { NodeResourceId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE node_resource_metadata (
	node_resource_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT node_resource_metadata_PK PRIMARY KEY (node_resource_id,item_key),
	CONSTRAINT node_resource_metadata_FK FOREIGN KEY (node_resource_id) REFERENCES node_resources(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_resource_metadata.`node_resource_id`,
              node_resource_metadata.`item_key`,
              node_resource_metadata.`item_value`,
              node_resource_metadata.`created_on`,
              node_resource_metadata.`active`
        FROM node_resource_metadata
        """
    
        static member TableName() = "node_resource_metadata"
    
    /// A record representing a row in the table `node_resource_note_versions`.
    type NodeResourceNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeResourceNoteId")>] NodeResourceNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeResourceNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE node_resource_note_versions (
	id TEXT NOT NULL,
	node_resource_note_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	title TEXT NOT NULL,
	note BLOB NOT NULL,
	hash TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT node_resource_note_versions_PK PRIMARY KEY (id),
	CONSTRAINT node_resource_note_versions_UN UNIQUE (node_resource_note_id,version),
	CONSTRAINT node_resource_note_versions_FK FOREIGN KEY (node_resource_note_id) REFERENCES node_resource_notes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_resource_note_versions.`id`,
              node_resource_note_versions.`node_resource_note_id`,
              node_resource_note_versions.`version`,
              node_resource_note_versions.`created_on`,
              node_resource_note_versions.`title`,
              node_resource_note_versions.`note`,
              node_resource_note_versions.`hash`,
              node_resource_note_versions.`active`
        FROM node_resource_note_versions
        """
    
        static member TableName() = "node_resource_note_versions"
    
    /// A record representing a row in the table `node_resource_notes`.
    type NodeResourceNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeResourceId")>] NodeResourceId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeResourceId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE node_resource_notes (
	id TEXT NOT NULL,
	node_resource_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT node_resource_notes_PK PRIMARY KEY (id),
	CONSTRAINT node_resource_notes_FK FOREIGN KEY (node_resource_id) REFERENCES node_resources(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_resource_notes.`id`,
              node_resource_notes.`node_resource_id`,
              node_resource_notes.`created_on`,
              node_resource_notes.`active`
        FROM node_resource_notes
        """
    
        static member TableName() = "node_resource_notes"
    
    /// A record representing a row in the table `node_resources`.
    type NodeResource =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeId = String.Empty
              ResourceVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE node_resources (
	id TEXT NOT NULL,
    node_id TEXT NOT NULL,
	resource_version_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT node_resource_PK PRIMARY KEY (id),
	CONSTRAINT node_resource_UN UNIQUE (node_id,resource_version_id),
	CONSTRAINT node_resource_FK FOREIGN KEY (node_id) REFERENCES nodes(id),
	CONSTRAINT node_resource_FK_1 FOREIGN KEY (resource_version_id) REFERENCES resources(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_resources.`id`,
              node_resources.`node_id`,
              node_resources.`resource_version_id`,
              node_resources.`created_on`,
              node_resources.`active`
        FROM node_resources
        """
    
        static member TableName() = "node_resources"
    
    /// A record representing a row in the table `node_tags`.
    type NodeTag =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { NodeId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE node_tags (
	node_id TEXT NOT NULL,
	tag TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT node_tags_PK PRIMARY KEY (node_id,tag),
	CONSTRAINT node_tags_FK FOREIGN KEY (node_id) REFERENCES nodes(id),
	CONSTRAINT node_tags_FK_1 FOREIGN KEY (tag) REFERENCES tags(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_tags.`node_id`,
              node_tags.`tag`,
              node_tags.`created_on`,
              node_tags.`active`
        FROM node_tags
        """
    
        static member TableName() = "node_tags"
    
    /// A record representing a row in the table `nodes`.
    type Node =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE nodes (
	id TEXT NOT NULL,
	name TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT nodes_PK PRIMARY KEY (id)
)
        """
    
        static member SelectSql() = """
        SELECT
              nodes.`id`,
              nodes.`name`,
              nodes.`created_on`,
              nodes.`active`
        FROM nodes
        """
    
        static member TableName() = "nodes"
    
    /// A record representing a row in the table `resource_metadata`.
    type ResourceMetadataItem =
        { [<JsonPropertyName("resourceId")>] ResourceId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ResourceId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE resource_metadata (
	resource_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT resource_metadata_PK PRIMARY KEY (resource_id,item_key),
	CONSTRAINT resource_metadata_FK FOREIGN KEY (resource_id) REFERENCES resources(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              resource_metadata.`resource_id`,
              resource_metadata.`item_key`,
              resource_metadata.`item_value`,
              resource_metadata.`created_on`,
              resource_metadata.`active`
        FROM resource_metadata
        """
    
        static member TableName() = "resource_metadata"
    
    /// A record representing a row in the table `resource_note_versions`.
    type ResourceNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("resourceNoteId")>] ResourceNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ResourceNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE resource_note_versions (
	id TEXT NOT NULL,
	resource_note_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	title TEXT NOT NULL,
	note BLOB NOT NULL,
	hash TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT resource_note_versions_PK PRIMARY KEY (id),
	CONSTRAINT resource_note_versions_UN UNIQUE (resource_note_id,version),
	CONSTRAINT resource_note_versions_FK FOREIGN KEY (resource_note_id) REFERENCES resource_notes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              resource_note_versions.`id`,
              resource_note_versions.`resource_note_id`,
              resource_note_versions.`version`,
              resource_note_versions.`created_on`,
              resource_note_versions.`title`,
              resource_note_versions.`note`,
              resource_note_versions.`hash`,
              resource_note_versions.`active`
        FROM resource_note_versions
        """
    
        static member TableName() = "resource_note_versions"
    
    /// A record representing a row in the table `resource_notes`.
    type ResourceNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("resourceId")>] ResourceId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ResourceId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE resource_notes (
	id TEXT NOT NULL,
	resource_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT resource_notes_PK PRIMARY KEY (id),
	CONSTRAINT resource_notes_FK FOREIGN KEY (resource_id) REFERENCES resources(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              resource_notes.`id`,
              resource_notes.`resource_id`,
              resource_notes.`created_on`,
              resource_notes.`active`
        FROM resource_notes
        """
    
        static member TableName() = "resource_notes"
    
    /// A record representing a row in the table `resource_tags`.
    type ResourceTag =
        { [<JsonPropertyName("resourceId")>] ResourceId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ResourceId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE resource_tags (
	resource_id TEXT NOT NULL,
	tag TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT resource_tags_PK PRIMARY KEY (resource_id,tag),
	CONSTRAINT resource_tags_FK FOREIGN KEY (resource_id) REFERENCES resources(id),
	CONSTRAINT resource_tags_FK_1 FOREIGN KEY (tag) REFERENCES tags(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              resource_tags.`resource_id`,
              resource_tags.`tag`,
              resource_tags.`created_on`,
              resource_tags.`active`
        FROM resource_tags
        """
    
        static member TableName() = "resource_tags"
    
    /// A record representing a row in the table `resource_version_metadata`.
    type ResourceVersionMetadataItem =
        { [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ResourceVersionId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE resource_version_metadata (
	resource_version_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT resource_version_metadata_PK PRIMARY KEY (resource_version_id,item_key),
	CONSTRAINT resource_version_metadata_FK FOREIGN KEY (resource_version_id) REFERENCES resource_versions(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              resource_version_metadata.`resource_version_id`,
              resource_version_metadata.`item_key`,
              resource_version_metadata.`item_value`,
              resource_version_metadata.`created_on`,
              resource_version_metadata.`active`
        FROM resource_version_metadata
        """
    
        static member TableName() = "resource_version_metadata"
    
    /// A record representing a row in the table `resource_version_note_versions`.
    type ResourceVersionNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("resourceVersionNoteId")>] ResourceVersionNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ResourceVersionNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE resource_version_note_versions (
	id TEXT NOT NULL,
	resource_version_note_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	title TEXT NOT NULL,
	note BLOB NOT NULL,
	hash TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT resource_version_note_versions_PK PRIMARY KEY (id),
	CONSTRAINT resource_version_note_versions_UN UNIQUE (resource_version_note_id,version),
	CONSTRAINT resource_version_note_versions_FK FOREIGN KEY (resource_version_note_id) REFERENCES resource_version_notes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              resource_version_note_versions.`id`,
              resource_version_note_versions.`resource_version_note_id`,
              resource_version_note_versions.`version`,
              resource_version_note_versions.`created_on`,
              resource_version_note_versions.`title`,
              resource_version_note_versions.`note`,
              resource_version_note_versions.`hash`,
              resource_version_note_versions.`active`
        FROM resource_version_note_versions
        """
    
        static member TableName() = "resource_version_note_versions"
    
    /// A record representing a row in the table `resource_version_notes`.
    type ResourceVersionNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ResourceVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE resource_version_notes (
	id TEXT NOT NULL,
	resource_version_id TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT resource_version_notes_PK PRIMARY KEY (id),
	CONSTRAINT resource_version_notes_FK FOREIGN KEY (resource_version_id) REFERENCES resource_versions(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              resource_version_notes.`id`,
              resource_version_notes.`resource_version_id`,
              resource_version_notes.`created_on`,
              resource_version_notes.`active`
        FROM resource_version_notes
        """
    
        static member TableName() = "resource_version_notes"
    
    /// A record representing a row in the table `resource_version_tags`.
    type ResourceVersionTag =
        { [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ResourceVersionId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE resource_version_tags (
	resource_version_id TEXT NOT NULL,
	tag TEXT NOT NULL,
	created_on TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT resource_version_tags_PK PRIMARY KEY (resource_version_id,tag),
	CONSTRAINT resource_version_tags_FK FOREIGN KEY (resource_version_id) REFERENCES resource_versions(id),
	CONSTRAINT resource_version_tags_FK_1 FOREIGN KEY (tag) REFERENCES tags(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              resource_version_tags.`resource_version_id`,
              resource_version_tags.`tag`,
              resource_version_tags.`created_on`,
              resource_version_tags.`active`
        FROM resource_version_tags
        """
    
        static member TableName() = "resource_version_tags"
    
    /// A record representing a row in the table `resource_versions`.
    type ResourceVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("resourceId")>] ResourceId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("rawData")>] RawData: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("fileType")>] FileType: string
          [<JsonPropertyName("encryptionType")>] EncryptionType: string
          [<JsonPropertyName("compressionType")>] CompressionType: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ResourceId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              RawData = BlobField.Empty()
              Hash = String.Empty
              FileType = String.Empty
              EncryptionType = String.Empty
              CompressionType = String.Empty
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE resource_versions (
	id TEXT NOT NULL,
	resource_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	raw_data BLOB NOT NULL,
	hash TEXT NOT NULL,
	file_type TEXT NOT NULL,
	encryption_type TEXT NOT NULL,
	compression_type TEXT NOT NULL,
	active INTEGER NOT NULL,
	CONSTRAINT resource_versions_PK PRIMARY KEY (id),
	CONSTRAINT resource_versions_UN UNIQUE (resource_id,version),
	CONSTRAINT resource_versions_FK FOREIGN KEY (resource_id) REFERENCES resources(id),
	CONSTRAINT resource_versions_FK_1 FOREIGN KEY (file_type) REFERENCES file_types(name),
	CONSTRAINT resource_versions_FK_2 FOREIGN KEY (encryption_type) REFERENCES encryption_types(name),
	CONSTRAINT resource_versions_FK_3 FOREIGN KEY (compression_type) REFERENCES compression_types(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              resource_versions.`id`,
              resource_versions.`resource_id`,
              resource_versions.`version`,
              resource_versions.`created_on`,
              resource_versions.`raw_data`,
              resource_versions.`hash`,
              resource_versions.`file_type`,
              resource_versions.`encryption_type`,
              resource_versions.`compression_type`,
              resource_versions.`active`
        FROM resource_versions
        """
    
        static member TableName() = "resource_versions"
    
    /// A record representing a row in the table `resources`.
    type Resource =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE resources (
	id TEXT NOT NULL,
	name TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT resources_PK PRIMARY KEY (id)
)
        """
    
        static member SelectSql() = """
        SELECT
              resources.`id`,
              resources.`name`,
              resources.`created_on`,
              resources.`active`
        FROM resources
        """
    
        static member TableName() = "resources"
    
    /// A record representing a row in the table `tags`.
    type Tag =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Name = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
        static member CreateTableSql() = """
        CREATE TABLE tags (
	name TEXT NOT NULL, created_on TEXT NOT NULL, active INTEGER NOT NULL,
	CONSTRAINT tags_PK PRIMARY KEY (name)
)
        """
    
        static member SelectSql() = """
        SELECT
              tags.`name`,
              tags.`created_on`,
              tags.`active`
        FROM tags
        """
    
        static member TableName() = "tags"
    

/// Module generated on 01/08/2023 19:57:08 (utc) via Freql.Tools.
[<RequireQualifiedAccess>]
module Parameters =
    /// A record representing a new row in the table `compression_types`.
    type NewCompressionType =
        { [<JsonPropertyName("name")>] Name: string }
    
        static member Blank() =
            { Name = String.Empty }
    
    
    /// A record representing a new row in the table `connection_document_metadata`.
    type NewConnectionDocumentMetadataItem =
        { [<JsonPropertyName("connectionDocumentId")>] ConnectionDocumentId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ConnectionDocumentId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `connection_document_note_versions`.
    type NewConnectionDocumentNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionDocumentNoteId")>] ConnectionDocumentNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionDocumentNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `connection_document_notes`.
    type NewConnectionDocumentNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionDocumentId")>] ConnectionDocumentId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionDocumentId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `connection_documents`.
    type NewConnectionDocument =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionId")>] ConnectionId: string
          [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionId = String.Empty
              DocumentVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `connection_labels`.
    type NewConnectionLabel =
        { [<JsonPropertyName("connectionId")>] ConnectionId: string
          [<JsonPropertyName("label")>] Label: string
          [<JsonPropertyName("weight")>] Weight: decimal
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ConnectionId = String.Empty
              Label = String.Empty
              Weight = 0m
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `connection_metadata`.
    type NewConnectionMetadataItem =
        { [<JsonPropertyName("connectionId")>] ConnectionId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ConnectionId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `connection_note_versions`.
    type NewConnectionNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionNoteId")>] ConnectionNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `connection_notes`.
    type NewConnectionNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionId")>] ConnectionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `connection_resource_metadata`.
    type NewConnectionResourceMetadataItem =
        { [<JsonPropertyName("connectionResourceId")>] ConnectionResourceId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ConnectionResourceId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `connection_resource_note_versions`.
    type NewConnectionResourceNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionResourceNoteId")>] ConnectionResourceNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionResourceNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `connection_resource_notes`.
    type NewConnectionResourceNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionResourceId")>] ConnectionResourceId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionResourceId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `connection_resources`.
    type NewConnectionResource =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("connectionId")>] ConnectionId: string
          [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ConnectionId = String.Empty
              ResourceVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `connection_tags`.
    type NewConnectionTag =
        { [<JsonPropertyName("connectionId")>] ConnectionId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ConnectionId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `connections`.
    type NewConnection =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("fromNode")>] FromNode: string
          [<JsonPropertyName("toNode")>] ToNode: string
          [<JsonPropertyName("twoWay")>] TwoWay: bool
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              FromNode = String.Empty
              ToNode = String.Empty
              TwoWay = true
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `document_metadata`.
    type NewDocumentMetadataItem =
        { [<JsonPropertyName("documentId")>] DocumentId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { DocumentId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `document_note_versions`.
    type NewDocumentNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("documentNoteId")>] DocumentNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              DocumentNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `document_notes`.
    type NewDocumentNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("documentId")>] DocumentId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              DocumentId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `document_tags`.
    type NewDocumentTag =
        { [<JsonPropertyName("documentId")>] DocumentId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { DocumentId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `document_version_metadata`.
    type NewDocumentVersionMetadataItem =
        { [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { DocumentVersionId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `document_version_note_versions`.
    type NewDocumentVersionNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("documentVersionNoteId")>] DocumentVersionNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              DocumentVersionNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `document_version_notes`.
    type NewDocumentVersionNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              DocumentVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `document_version_tags`.
    type NewDocumentVersionTag =
        { [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { DocumentVersionId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `document_versions`.
    type NewDocumentVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("documentId")>] DocumentId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("rawData")>] RawData: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("encryptionType")>] EncryptionType: string
          [<JsonPropertyName("compressionType")>] CompressionType: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              DocumentId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              RawData = BlobField.Empty()
              Hash = String.Empty
              EncryptionType = String.Empty
              CompressionType = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `documents`.
    type NewDocument =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `encryption_types`.
    type NewEncryptionType =
        { [<JsonPropertyName("name")>] Name: string }
    
        static member Blank() =
            { Name = String.Empty }
    
    
    /// A record representing a new row in the table `events`.
    type NewEventItem =
        { [<JsonPropertyName("eventType")>] EventType: string
          [<JsonPropertyName("eventTimestamp")>] EventTimestamp: DateTime
          [<JsonPropertyName("eventBlob")>] EventBlob: string }
    
        static member Blank() =
            { EventType = String.Empty
              EventTimestamp = DateTime.UtcNow
              EventBlob = String.Empty }
    
    
    /// A record representing a new row in the table `external_connection_document_metadata`.
    type NewExternalConnectionDocumentMetadataItem =
        { [<JsonPropertyName("externalConnectionDocumentId")>] ExternalConnectionDocumentId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ExternalConnectionDocumentId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `external_connection_document_note_versions`.
    type NewExternalConnectionDocumentNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionDocumentNoteId")>] ExternalConnectionDocumentNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionDocumentNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `external_connection_document_notes`.
    type NewExternalConnectionDocumentNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionDocumentId")>] ExternalConnectionDocumentId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionDocumentId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `external_connection_documents`.
    type NewExternalConnectionDocument =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionId")>] ExternalConnectionId: string
          [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionId = String.Empty
              DocumentVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `external_connection_labels`.
    type NewExternalConnectionLabel =
        { [<JsonPropertyName("externalConnectionId")>] ExternalConnectionId: string
          [<JsonPropertyName("label")>] Label: string
          [<JsonPropertyName("weight")>] Weight: decimal
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ExternalConnectionId = String.Empty
              Label = String.Empty
              Weight = 0m
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `external_connection_metadata`.
    type NewExternalConnectionMetadataItem =
        { [<JsonPropertyName("externalConnectionId")>] ExternalConnectionId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ExternalConnectionId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `external_connection_note_versions`.
    type NewExternalConnectionNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionNoteId")>] ExternalConnectionNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `external_connection_notes`.
    type NewExternalConnectionNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionId")>] ExternalConnectionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `external_connection_resource_metadata`.
    type NewExternalConnectionResourceMetadataItem =
        { [<JsonPropertyName("externalConnectionResourceId")>] ExternalConnectionResourceId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ExternalConnectionResourceId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `external_connection_resource_note_versions`.
    type NewExternalConnectionResourceNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionResourceNoteId")>] ExternalConnectionResourceNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionResourceNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `external_connection_resource_notes`.
    type NewExternalConnectionResourceNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionResourceId")>] ExternalConnectionResourceId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionResourceId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `external_connection_resources`.
    type NewExternalConnectionResource =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("externalConnectionId")>] ExternalConnectionId: string
          [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ExternalConnectionId = String.Empty
              ResourceVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `external_connection_tags`.
    type NewExternalConnectionTag =
        { [<JsonPropertyName("externalConnectionId")>] ExternalConnectionId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ExternalConnectionId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `external_connections`.
    type NewExternalConnection =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("fromNode")>] FromNode: string
          [<JsonPropertyName("externalUri")>] ExternalUri: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              FromNode = String.Empty
              ExternalUri = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `file_types`.
    type NewFileType =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("extension")>] Extension: string
          [<JsonPropertyName("contentType")>] ContentType: string }
    
        static member Blank() =
            { Name = String.Empty
              Extension = String.Empty
              ContentType = String.Empty }
    
    
    /// A record representing a new row in the table `labels`.
    type NewLabel =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Name = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `metadata`.
    type NewMetadataItem =
        { [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `node_document_metadata`.
    type NewNodeDocumentMetadataItem =
        { [<JsonPropertyName("nodeDocumentId")>] NodeDocumentId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { NodeDocumentId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `node_document_note_versions`.
    type NewNodeDocumentNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeDocumentNoteId")>] NodeDocumentNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeDocumentNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `node_document_notes`.
    type NewNodeDocumentNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeDocumentId")>] NodeDocumentId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeDocumentId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `node_documents`.
    type NewNodeDocument =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeId = String.Empty
              DocumentVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `node_labels`.
    type NewNodeLabel =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("label")>] Label: string
          [<JsonPropertyName("weight")>] Weight: decimal
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { NodeId = String.Empty
              Label = String.Empty
              Weight = 0m
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `node_metadata`.
    type NewNodeMetadataItem =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { NodeId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `node_note_versions`.
    type NewNodeNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeNoteId")>] NodeNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `node_notes`.
    type NewNodeNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `node_resource_metadata`.
    type NewNodeResourceMetadataItem =
        { [<JsonPropertyName("nodeResourceId")>] NodeResourceId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { NodeResourceId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `node_resource_note_versions`.
    type NewNodeResourceNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeResourceNoteId")>] NodeResourceNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeResourceNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `node_resource_notes`.
    type NewNodeResourceNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeResourceId")>] NodeResourceId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeResourceId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `node_resources`.
    type NewNodeResource =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              NodeId = String.Empty
              ResourceVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `node_tags`.
    type NewNodeTag =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { NodeId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `nodes`.
    type NewNode =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `resource_metadata`.
    type NewResourceMetadataItem =
        { [<JsonPropertyName("resourceId")>] ResourceId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ResourceId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `resource_note_versions`.
    type NewResourceNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("resourceNoteId")>] ResourceNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ResourceNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `resource_notes`.
    type NewResourceNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("resourceId")>] ResourceId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ResourceId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `resource_tags`.
    type NewResourceTag =
        { [<JsonPropertyName("resourceId")>] ResourceId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ResourceId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `resource_version_metadata`.
    type NewResourceVersionMetadataItem =
        { [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ResourceVersionId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `resource_version_note_versions`.
    type NewResourceVersionNoteVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("resourceVersionNoteId")>] ResourceVersionNoteId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("title")>] Title: string
          [<JsonPropertyName("note")>] Note: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ResourceVersionNoteId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              Title = String.Empty
              Note = BlobField.Empty()
              Hash = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `resource_version_notes`.
    type NewResourceVersionNote =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ResourceVersionId = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `resource_version_tags`.
    type NewResourceVersionTag =
        { [<JsonPropertyName("resourceVersionId")>] ResourceVersionId: string
          [<JsonPropertyName("tag")>] Tag: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { ResourceVersionId = String.Empty
              Tag = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `resource_versions`.
    type NewResourceVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("resourceId")>] ResourceId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("rawData")>] RawData: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("fileType")>] FileType: string
          [<JsonPropertyName("encryptionType")>] EncryptionType: string
          [<JsonPropertyName("compressionType")>] CompressionType: string
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              ResourceId = String.Empty
              Version = 0
              CreatedOn = DateTime.UtcNow
              RawData = BlobField.Empty()
              Hash = String.Empty
              FileType = String.Empty
              EncryptionType = String.Empty
              CompressionType = String.Empty
              Active = true }
    
    
    /// A record representing a new row in the table `resources`.
    type NewResource =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
    /// A record representing a new row in the table `tags`.
    type NewTag =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime
          [<JsonPropertyName("active")>] Active: bool }
    
        static member Blank() =
            { Name = String.Empty
              CreatedOn = DateTime.UtcNow
              Active = true }
    
    
/// Module generated on 01/08/2023 19:57:08 (utc) via Freql.Tools.
[<RequireQualifiedAccess>]
module Operations =

    let buildSql (lines: string list) = lines |> String.concat Environment.NewLine

    /// Select a `Records.CompressionType` from the table `compression_types`.
    /// Internally this calls `context.SelectSingleAnon<Records.CompressionType>` and uses Records.CompressionType.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectCompressionTypeRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectCompressionTypeRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.CompressionType.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.CompressionType>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.CompressionType>` and uses Records.CompressionType.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectCompressionTypeRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectCompressionTypeRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.CompressionType.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.CompressionType>(sql, parameters)
    
    let insertCompressionType (context: SqliteContext) (parameters: Parameters.NewCompressionType) =
        context.Insert("compression_types", parameters)
    
    /// Select a `Records.ConnectionDocumentMetadataItem` from the table `connection_document_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.ConnectionDocumentMetadataItem>` and uses Records.ConnectionDocumentMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionDocumentMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionDocumentMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionDocumentMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ConnectionDocumentMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ConnectionDocumentMetadataItem>` and uses Records.ConnectionDocumentMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionDocumentMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionDocumentMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionDocumentMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ConnectionDocumentMetadataItem>(sql, parameters)
    
    let insertConnectionDocumentMetadataItem (context: SqliteContext) (parameters: Parameters.NewConnectionDocumentMetadataItem) =
        context.Insert("connection_document_metadata", parameters)
    
    /// Select a `Records.ConnectionDocumentNoteVersion` from the table `connection_document_note_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.ConnectionDocumentNoteVersion>` and uses Records.ConnectionDocumentNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionDocumentNoteVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionDocumentNoteVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionDocumentNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ConnectionDocumentNoteVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ConnectionDocumentNoteVersion>` and uses Records.ConnectionDocumentNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionDocumentNoteVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionDocumentNoteVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionDocumentNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ConnectionDocumentNoteVersion>(sql, parameters)
    
    let insertConnectionDocumentNoteVersion (context: SqliteContext) (parameters: Parameters.NewConnectionDocumentNoteVersion) =
        context.Insert("connection_document_note_versions", parameters)
    
    /// Select a `Records.ConnectionDocumentNote` from the table `connection_document_notes`.
    /// Internally this calls `context.SelectSingleAnon<Records.ConnectionDocumentNote>` and uses Records.ConnectionDocumentNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionDocumentNoteRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionDocumentNoteRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionDocumentNote.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ConnectionDocumentNote>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ConnectionDocumentNote>` and uses Records.ConnectionDocumentNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionDocumentNoteRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionDocumentNoteRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionDocumentNote.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ConnectionDocumentNote>(sql, parameters)
    
    let insertConnectionDocumentNote (context: SqliteContext) (parameters: Parameters.NewConnectionDocumentNote) =
        context.Insert("connection_document_notes", parameters)
    
    /// Select a `Records.ConnectionDocument` from the table `connection_documents`.
    /// Internally this calls `context.SelectSingleAnon<Records.ConnectionDocument>` and uses Records.ConnectionDocument.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionDocumentRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionDocumentRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionDocument.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ConnectionDocument>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ConnectionDocument>` and uses Records.ConnectionDocument.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionDocumentRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionDocumentRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionDocument.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ConnectionDocument>(sql, parameters)
    
    let insertConnectionDocument (context: SqliteContext) (parameters: Parameters.NewConnectionDocument) =
        context.Insert("connection_documents", parameters)
    
    /// Select a `Records.ConnectionLabel` from the table `connection_labels`.
    /// Internally this calls `context.SelectSingleAnon<Records.ConnectionLabel>` and uses Records.ConnectionLabel.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionLabelRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionLabelRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionLabel.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ConnectionLabel>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ConnectionLabel>` and uses Records.ConnectionLabel.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionLabelRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionLabelRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionLabel.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ConnectionLabel>(sql, parameters)
    
    let insertConnectionLabel (context: SqliteContext) (parameters: Parameters.NewConnectionLabel) =
        context.Insert("connection_labels", parameters)
    
    /// Select a `Records.ConnectionMetadataItem` from the table `connection_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.ConnectionMetadataItem>` and uses Records.ConnectionMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ConnectionMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ConnectionMetadataItem>` and uses Records.ConnectionMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ConnectionMetadataItem>(sql, parameters)
    
    let insertConnectionMetadataItem (context: SqliteContext) (parameters: Parameters.NewConnectionMetadataItem) =
        context.Insert("connection_metadata", parameters)
    
    /// Select a `Records.ConnectionNoteVersion` from the table `connection_note_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.ConnectionNoteVersion>` and uses Records.ConnectionNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionNoteVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionNoteVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ConnectionNoteVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ConnectionNoteVersion>` and uses Records.ConnectionNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionNoteVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionNoteVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ConnectionNoteVersion>(sql, parameters)
    
    let insertConnectionNoteVersion (context: SqliteContext) (parameters: Parameters.NewConnectionNoteVersion) =
        context.Insert("connection_note_versions", parameters)
    
    /// Select a `Records.ConnectionNote` from the table `connection_notes`.
    /// Internally this calls `context.SelectSingleAnon<Records.ConnectionNote>` and uses Records.ConnectionNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionNoteRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionNoteRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionNote.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ConnectionNote>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ConnectionNote>` and uses Records.ConnectionNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionNoteRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionNoteRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionNote.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ConnectionNote>(sql, parameters)
    
    let insertConnectionNote (context: SqliteContext) (parameters: Parameters.NewConnectionNote) =
        context.Insert("connection_notes", parameters)
    
    /// Select a `Records.ConnectionResourceMetadataItem` from the table `connection_resource_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.ConnectionResourceMetadataItem>` and uses Records.ConnectionResourceMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionResourceMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionResourceMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionResourceMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ConnectionResourceMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ConnectionResourceMetadataItem>` and uses Records.ConnectionResourceMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionResourceMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionResourceMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionResourceMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ConnectionResourceMetadataItem>(sql, parameters)
    
    let insertConnectionResourceMetadataItem (context: SqliteContext) (parameters: Parameters.NewConnectionResourceMetadataItem) =
        context.Insert("connection_resource_metadata", parameters)
    
    /// Select a `Records.ConnectionResourceNoteVersion` from the table `connection_resource_note_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.ConnectionResourceNoteVersion>` and uses Records.ConnectionResourceNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionResourceNoteVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionResourceNoteVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionResourceNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ConnectionResourceNoteVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ConnectionResourceNoteVersion>` and uses Records.ConnectionResourceNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionResourceNoteVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionResourceNoteVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionResourceNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ConnectionResourceNoteVersion>(sql, parameters)
    
    let insertConnectionResourceNoteVersion (context: SqliteContext) (parameters: Parameters.NewConnectionResourceNoteVersion) =
        context.Insert("connection_resource_note_versions", parameters)
    
    /// Select a `Records.ConnectionResourceNote` from the table `connection_resource_notes`.
    /// Internally this calls `context.SelectSingleAnon<Records.ConnectionResourceNote>` and uses Records.ConnectionResourceNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionResourceNoteRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionResourceNoteRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionResourceNote.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ConnectionResourceNote>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ConnectionResourceNote>` and uses Records.ConnectionResourceNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionResourceNoteRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionResourceNoteRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionResourceNote.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ConnectionResourceNote>(sql, parameters)
    
    let insertConnectionResourceNote (context: SqliteContext) (parameters: Parameters.NewConnectionResourceNote) =
        context.Insert("connection_resource_notes", parameters)
    
    /// Select a `Records.ConnectionResource` from the table `connection_resources`.
    /// Internally this calls `context.SelectSingleAnon<Records.ConnectionResource>` and uses Records.ConnectionResource.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionResourceRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionResourceRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionResource.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ConnectionResource>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ConnectionResource>` and uses Records.ConnectionResource.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionResourceRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionResourceRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionResource.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ConnectionResource>(sql, parameters)
    
    let insertConnectionResource (context: SqliteContext) (parameters: Parameters.NewConnectionResource) =
        context.Insert("connection_resources", parameters)
    
    /// Select a `Records.ConnectionTag` from the table `connection_tags`.
    /// Internally this calls `context.SelectSingleAnon<Records.ConnectionTag>` and uses Records.ConnectionTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionTagRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionTagRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionTag.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ConnectionTag>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ConnectionTag>` and uses Records.ConnectionTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionTagRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionTagRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ConnectionTag.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ConnectionTag>(sql, parameters)
    
    let insertConnectionTag (context: SqliteContext) (parameters: Parameters.NewConnectionTag) =
        context.Insert("connection_tags", parameters)
    
    /// Select a `Records.Connection` from the table `connections`.
    /// Internally this calls `context.SelectSingleAnon<Records.Connection>` and uses Records.Connection.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Connection.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.Connection>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.Connection>` and uses Records.Connection.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectConnectionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectConnectionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Connection.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.Connection>(sql, parameters)
    
    let insertConnection (context: SqliteContext) (parameters: Parameters.NewConnection) =
        context.Insert("connections", parameters)
    
    /// Select a `Records.DocumentMetadataItem` from the table `document_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.DocumentMetadataItem>` and uses Records.DocumentMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.DocumentMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.DocumentMetadataItem>` and uses Records.DocumentMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.DocumentMetadataItem>(sql, parameters)
    
    let insertDocumentMetadataItem (context: SqliteContext) (parameters: Parameters.NewDocumentMetadataItem) =
        context.Insert("document_metadata", parameters)
    
    /// Select a `Records.DocumentNoteVersion` from the table `document_note_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.DocumentNoteVersion>` and uses Records.DocumentNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentNoteVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentNoteVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.DocumentNoteVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.DocumentNoteVersion>` and uses Records.DocumentNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentNoteVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentNoteVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.DocumentNoteVersion>(sql, parameters)
    
    let insertDocumentNoteVersion (context: SqliteContext) (parameters: Parameters.NewDocumentNoteVersion) =
        context.Insert("document_note_versions", parameters)
    
    /// Select a `Records.DocumentNote` from the table `document_notes`.
    /// Internally this calls `context.SelectSingleAnon<Records.DocumentNote>` and uses Records.DocumentNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentNoteRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentNoteRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentNote.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.DocumentNote>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.DocumentNote>` and uses Records.DocumentNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentNoteRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentNoteRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentNote.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.DocumentNote>(sql, parameters)
    
    let insertDocumentNote (context: SqliteContext) (parameters: Parameters.NewDocumentNote) =
        context.Insert("document_notes", parameters)
    
    /// Select a `Records.DocumentTag` from the table `document_tags`.
    /// Internally this calls `context.SelectSingleAnon<Records.DocumentTag>` and uses Records.DocumentTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentTagRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentTagRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentTag.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.DocumentTag>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.DocumentTag>` and uses Records.DocumentTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentTagRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentTagRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentTag.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.DocumentTag>(sql, parameters)
    
    let insertDocumentTag (context: SqliteContext) (parameters: Parameters.NewDocumentTag) =
        context.Insert("document_tags", parameters)
    
    /// Select a `Records.DocumentVersionMetadataItem` from the table `document_version_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.DocumentVersionMetadataItem>` and uses Records.DocumentVersionMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersionMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.DocumentVersionMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.DocumentVersionMetadataItem>` and uses Records.DocumentVersionMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersionMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.DocumentVersionMetadataItem>(sql, parameters)
    
    let insertDocumentVersionMetadataItem (context: SqliteContext) (parameters: Parameters.NewDocumentVersionMetadataItem) =
        context.Insert("document_version_metadata", parameters)
    
    /// Select a `Records.DocumentVersionNoteVersion` from the table `document_version_note_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.DocumentVersionNoteVersion>` and uses Records.DocumentVersionNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionNoteVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionNoteVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersionNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.DocumentVersionNoteVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.DocumentVersionNoteVersion>` and uses Records.DocumentVersionNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionNoteVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionNoteVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersionNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.DocumentVersionNoteVersion>(sql, parameters)
    
    let insertDocumentVersionNoteVersion (context: SqliteContext) (parameters: Parameters.NewDocumentVersionNoteVersion) =
        context.Insert("document_version_note_versions", parameters)
    
    /// Select a `Records.DocumentVersionNote` from the table `document_version_notes`.
    /// Internally this calls `context.SelectSingleAnon<Records.DocumentVersionNote>` and uses Records.DocumentVersionNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionNoteRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionNoteRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersionNote.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.DocumentVersionNote>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.DocumentVersionNote>` and uses Records.DocumentVersionNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionNoteRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionNoteRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersionNote.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.DocumentVersionNote>(sql, parameters)
    
    let insertDocumentVersionNote (context: SqliteContext) (parameters: Parameters.NewDocumentVersionNote) =
        context.Insert("document_version_notes", parameters)
    
    /// Select a `Records.DocumentVersionTag` from the table `document_version_tags`.
    /// Internally this calls `context.SelectSingleAnon<Records.DocumentVersionTag>` and uses Records.DocumentVersionTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionTagRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionTagRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersionTag.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.DocumentVersionTag>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.DocumentVersionTag>` and uses Records.DocumentVersionTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionTagRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionTagRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersionTag.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.DocumentVersionTag>(sql, parameters)
    
    let insertDocumentVersionTag (context: SqliteContext) (parameters: Parameters.NewDocumentVersionTag) =
        context.Insert("document_version_tags", parameters)
    
    /// Select a `Records.DocumentVersion` from the table `document_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.DocumentVersion>` and uses Records.DocumentVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.DocumentVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.DocumentVersion>` and uses Records.DocumentVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.DocumentVersion>(sql, parameters)
    
    let insertDocumentVersion (context: SqliteContext) (parameters: Parameters.NewDocumentVersion) =
        context.Insert("document_versions", parameters)
    
    /// Select a `Records.Document` from the table `documents`.
    /// Internally this calls `context.SelectSingleAnon<Records.Document>` and uses Records.Document.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Document.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.Document>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.Document>` and uses Records.Document.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Document.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.Document>(sql, parameters)
    
    let insertDocument (context: SqliteContext) (parameters: Parameters.NewDocument) =
        context.Insert("documents", parameters)
    
    /// Select a `Records.EncryptionType` from the table `encryption_types`.
    /// Internally this calls `context.SelectSingleAnon<Records.EncryptionType>` and uses Records.EncryptionType.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEncryptionTypeRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectEncryptionTypeRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EncryptionType.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.EncryptionType>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.EncryptionType>` and uses Records.EncryptionType.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEncryptionTypeRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectEncryptionTypeRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EncryptionType.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.EncryptionType>(sql, parameters)
    
    let insertEncryptionType (context: SqliteContext) (parameters: Parameters.NewEncryptionType) =
        context.Insert("encryption_types", parameters)
    
    /// Select a `Records.EventItem` from the table `events`.
    /// Internally this calls `context.SelectSingleAnon<Records.EventItem>` and uses Records.EventItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEventItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectEventItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EventItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.EventItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.EventItem>` and uses Records.EventItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEventItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectEventItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EventItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.EventItem>(sql, parameters)
    
    let insertEventItem (context: SqliteContext) (parameters: Parameters.NewEventItem) =
        context.Insert("events", parameters)
    
    /// Select a `Records.ExternalConnectionDocumentMetadataItem` from the table `external_connection_document_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnectionDocumentMetadataItem>` and uses Records.ExternalConnectionDocumentMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionDocumentMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionDocumentMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionDocumentMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnectionDocumentMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnectionDocumentMetadataItem>` and uses Records.ExternalConnectionDocumentMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionDocumentMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionDocumentMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionDocumentMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnectionDocumentMetadataItem>(sql, parameters)
    
    let insertExternalConnectionDocumentMetadataItem (context: SqliteContext) (parameters: Parameters.NewExternalConnectionDocumentMetadataItem) =
        context.Insert("external_connection_document_metadata", parameters)
    
    /// Select a `Records.ExternalConnectionDocumentNoteVersion` from the table `external_connection_document_note_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnectionDocumentNoteVersion>` and uses Records.ExternalConnectionDocumentNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionDocumentNoteVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionDocumentNoteVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionDocumentNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnectionDocumentNoteVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnectionDocumentNoteVersion>` and uses Records.ExternalConnectionDocumentNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionDocumentNoteVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionDocumentNoteVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionDocumentNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnectionDocumentNoteVersion>(sql, parameters)
    
    let insertExternalConnectionDocumentNoteVersion (context: SqliteContext) (parameters: Parameters.NewExternalConnectionDocumentNoteVersion) =
        context.Insert("external_connection_document_note_versions", parameters)
    
    /// Select a `Records.ExternalConnectionDocumentNote` from the table `external_connection_document_notes`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnectionDocumentNote>` and uses Records.ExternalConnectionDocumentNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionDocumentNoteRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionDocumentNoteRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionDocumentNote.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnectionDocumentNote>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnectionDocumentNote>` and uses Records.ExternalConnectionDocumentNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionDocumentNoteRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionDocumentNoteRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionDocumentNote.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnectionDocumentNote>(sql, parameters)
    
    let insertExternalConnectionDocumentNote (context: SqliteContext) (parameters: Parameters.NewExternalConnectionDocumentNote) =
        context.Insert("external_connection_document_notes", parameters)
    
    /// Select a `Records.ExternalConnectionDocument` from the table `external_connection_documents`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnectionDocument>` and uses Records.ExternalConnectionDocument.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionDocumentRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionDocumentRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionDocument.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnectionDocument>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnectionDocument>` and uses Records.ExternalConnectionDocument.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionDocumentRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionDocumentRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionDocument.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnectionDocument>(sql, parameters)
    
    let insertExternalConnectionDocument (context: SqliteContext) (parameters: Parameters.NewExternalConnectionDocument) =
        context.Insert("external_connection_documents", parameters)
    
    /// Select a `Records.ExternalConnectionLabel` from the table `external_connection_labels`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnectionLabel>` and uses Records.ExternalConnectionLabel.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionLabelRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionLabelRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionLabel.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnectionLabel>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnectionLabel>` and uses Records.ExternalConnectionLabel.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionLabelRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionLabelRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionLabel.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnectionLabel>(sql, parameters)
    
    let insertExternalConnectionLabel (context: SqliteContext) (parameters: Parameters.NewExternalConnectionLabel) =
        context.Insert("external_connection_labels", parameters)
    
    /// Select a `Records.ExternalConnectionMetadataItem` from the table `external_connection_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnectionMetadataItem>` and uses Records.ExternalConnectionMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnectionMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnectionMetadataItem>` and uses Records.ExternalConnectionMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnectionMetadataItem>(sql, parameters)
    
    let insertExternalConnectionMetadataItem (context: SqliteContext) (parameters: Parameters.NewExternalConnectionMetadataItem) =
        context.Insert("external_connection_metadata", parameters)
    
    /// Select a `Records.ExternalConnectionNoteVersion` from the table `external_connection_note_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnectionNoteVersion>` and uses Records.ExternalConnectionNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionNoteVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionNoteVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnectionNoteVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnectionNoteVersion>` and uses Records.ExternalConnectionNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionNoteVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionNoteVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnectionNoteVersion>(sql, parameters)
    
    let insertExternalConnectionNoteVersion (context: SqliteContext) (parameters: Parameters.NewExternalConnectionNoteVersion) =
        context.Insert("external_connection_note_versions", parameters)
    
    /// Select a `Records.ExternalConnectionNote` from the table `external_connection_notes`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnectionNote>` and uses Records.ExternalConnectionNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionNoteRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionNoteRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionNote.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnectionNote>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnectionNote>` and uses Records.ExternalConnectionNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionNoteRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionNoteRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionNote.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnectionNote>(sql, parameters)
    
    let insertExternalConnectionNote (context: SqliteContext) (parameters: Parameters.NewExternalConnectionNote) =
        context.Insert("external_connection_notes", parameters)
    
    /// Select a `Records.ExternalConnectionResourceMetadataItem` from the table `external_connection_resource_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnectionResourceMetadataItem>` and uses Records.ExternalConnectionResourceMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionResourceMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionResourceMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionResourceMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnectionResourceMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnectionResourceMetadataItem>` and uses Records.ExternalConnectionResourceMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionResourceMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionResourceMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionResourceMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnectionResourceMetadataItem>(sql, parameters)
    
    let insertExternalConnectionResourceMetadataItem (context: SqliteContext) (parameters: Parameters.NewExternalConnectionResourceMetadataItem) =
        context.Insert("external_connection_resource_metadata", parameters)
    
    /// Select a `Records.ExternalConnectionResourceNoteVersion` from the table `external_connection_resource_note_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnectionResourceNoteVersion>` and uses Records.ExternalConnectionResourceNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionResourceNoteVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionResourceNoteVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionResourceNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnectionResourceNoteVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnectionResourceNoteVersion>` and uses Records.ExternalConnectionResourceNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionResourceNoteVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionResourceNoteVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionResourceNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnectionResourceNoteVersion>(sql, parameters)
    
    let insertExternalConnectionResourceNoteVersion (context: SqliteContext) (parameters: Parameters.NewExternalConnectionResourceNoteVersion) =
        context.Insert("external_connection_resource_note_versions", parameters)
    
    /// Select a `Records.ExternalConnectionResourceNote` from the table `external_connection_resource_notes`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnectionResourceNote>` and uses Records.ExternalConnectionResourceNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionResourceNoteRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionResourceNoteRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionResourceNote.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnectionResourceNote>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnectionResourceNote>` and uses Records.ExternalConnectionResourceNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionResourceNoteRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionResourceNoteRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionResourceNote.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnectionResourceNote>(sql, parameters)
    
    let insertExternalConnectionResourceNote (context: SqliteContext) (parameters: Parameters.NewExternalConnectionResourceNote) =
        context.Insert("external_connection_resource_notes", parameters)
    
    /// Select a `Records.ExternalConnectionResource` from the table `external_connection_resources`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnectionResource>` and uses Records.ExternalConnectionResource.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionResourceRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionResourceRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionResource.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnectionResource>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnectionResource>` and uses Records.ExternalConnectionResource.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionResourceRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionResourceRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionResource.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnectionResource>(sql, parameters)
    
    let insertExternalConnectionResource (context: SqliteContext) (parameters: Parameters.NewExternalConnectionResource) =
        context.Insert("external_connection_resources", parameters)
    
    /// Select a `Records.ExternalConnectionTag` from the table `external_connection_tags`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnectionTag>` and uses Records.ExternalConnectionTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionTagRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionTagRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionTag.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnectionTag>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnectionTag>` and uses Records.ExternalConnectionTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionTagRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionTagRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnectionTag.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnectionTag>(sql, parameters)
    
    let insertExternalConnectionTag (context: SqliteContext) (parameters: Parameters.NewExternalConnectionTag) =
        context.Insert("external_connection_tags", parameters)
    
    /// Select a `Records.ExternalConnection` from the table `external_connections`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalConnection>` and uses Records.ExternalConnection.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnection.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalConnection>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalConnection>` and uses Records.ExternalConnection.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalConnectionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalConnectionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalConnection.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalConnection>(sql, parameters)
    
    let insertExternalConnection (context: SqliteContext) (parameters: Parameters.NewExternalConnection) =
        context.Insert("external_connections", parameters)
    
    /// Select a `Records.FileType` from the table `file_types`.
    /// Internally this calls `context.SelectSingleAnon<Records.FileType>` and uses Records.FileType.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFileTypeRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectFileTypeRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.FileType.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.FileType>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.FileType>` and uses Records.FileType.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFileTypeRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectFileTypeRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.FileType.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.FileType>(sql, parameters)
    
    let insertFileType (context: SqliteContext) (parameters: Parameters.NewFileType) =
        context.Insert("file_types", parameters)
    
    /// Select a `Records.Label` from the table `labels`.
    /// Internally this calls `context.SelectSingleAnon<Records.Label>` and uses Records.Label.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectLabelRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectLabelRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Label.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.Label>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.Label>` and uses Records.Label.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectLabelRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectLabelRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Label.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.Label>(sql, parameters)
    
    let insertLabel (context: SqliteContext) (parameters: Parameters.NewLabel) =
        context.Insert("labels", parameters)
    
    /// Select a `Records.MetadataItem` from the table `metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.MetadataItem>` and uses Records.MetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.MetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.MetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.MetadataItem>` and uses Records.MetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.MetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.MetadataItem>(sql, parameters)
    
    let insertMetadataItem (context: SqliteContext) (parameters: Parameters.NewMetadataItem) =
        context.Insert("metadata", parameters)
    
    /// Select a `Records.NodeDocumentMetadataItem` from the table `node_document_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeDocumentMetadataItem>` and uses Records.NodeDocumentMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeDocumentMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeDocumentMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeDocumentMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeDocumentMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeDocumentMetadataItem>` and uses Records.NodeDocumentMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeDocumentMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeDocumentMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeDocumentMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeDocumentMetadataItem>(sql, parameters)
    
    let insertNodeDocumentMetadataItem (context: SqliteContext) (parameters: Parameters.NewNodeDocumentMetadataItem) =
        context.Insert("node_document_metadata", parameters)
    
    /// Select a `Records.NodeDocumentNoteVersion` from the table `node_document_note_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeDocumentNoteVersion>` and uses Records.NodeDocumentNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeDocumentNoteVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeDocumentNoteVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeDocumentNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeDocumentNoteVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeDocumentNoteVersion>` and uses Records.NodeDocumentNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeDocumentNoteVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeDocumentNoteVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeDocumentNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeDocumentNoteVersion>(sql, parameters)
    
    let insertNodeDocumentNoteVersion (context: SqliteContext) (parameters: Parameters.NewNodeDocumentNoteVersion) =
        context.Insert("node_document_note_versions", parameters)
    
    /// Select a `Records.NodeDocumentNote` from the table `node_document_notes`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeDocumentNote>` and uses Records.NodeDocumentNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeDocumentNoteRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeDocumentNoteRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeDocumentNote.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeDocumentNote>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeDocumentNote>` and uses Records.NodeDocumentNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeDocumentNoteRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeDocumentNoteRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeDocumentNote.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeDocumentNote>(sql, parameters)
    
    let insertNodeDocumentNote (context: SqliteContext) (parameters: Parameters.NewNodeDocumentNote) =
        context.Insert("node_document_notes", parameters)
    
    /// Select a `Records.NodeDocument` from the table `node_documents`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeDocument>` and uses Records.NodeDocument.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeDocumentRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeDocumentRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeDocument.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeDocument>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeDocument>` and uses Records.NodeDocument.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeDocumentRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeDocumentRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeDocument.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeDocument>(sql, parameters)
    
    let insertNodeDocument (context: SqliteContext) (parameters: Parameters.NewNodeDocument) =
        context.Insert("node_documents", parameters)
    
    /// Select a `Records.NodeLabel` from the table `node_labels`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeLabel>` and uses Records.NodeLabel.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeLabelRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeLabelRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeLabel.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeLabel>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeLabel>` and uses Records.NodeLabel.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeLabelRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeLabelRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeLabel.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeLabel>(sql, parameters)
    
    let insertNodeLabel (context: SqliteContext) (parameters: Parameters.NewNodeLabel) =
        context.Insert("node_labels", parameters)
    
    /// Select a `Records.NodeMetadataItem` from the table `node_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeMetadataItem>` and uses Records.NodeMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeMetadataItem>` and uses Records.NodeMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeMetadataItem>(sql, parameters)
    
    let insertNodeMetadataItem (context: SqliteContext) (parameters: Parameters.NewNodeMetadataItem) =
        context.Insert("node_metadata", parameters)
    
    /// Select a `Records.NodeNoteVersion` from the table `node_note_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeNoteVersion>` and uses Records.NodeNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeNoteVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeNoteVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeNoteVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeNoteVersion>` and uses Records.NodeNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeNoteVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeNoteVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeNoteVersion>(sql, parameters)
    
    let insertNodeNoteVersion (context: SqliteContext) (parameters: Parameters.NewNodeNoteVersion) =
        context.Insert("node_note_versions", parameters)
    
    /// Select a `Records.NodeNote` from the table `node_notes`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeNote>` and uses Records.NodeNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeNoteRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeNoteRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeNote.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeNote>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeNote>` and uses Records.NodeNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeNoteRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeNoteRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeNote.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeNote>(sql, parameters)
    
    let insertNodeNote (context: SqliteContext) (parameters: Parameters.NewNodeNote) =
        context.Insert("node_notes", parameters)
    
    /// Select a `Records.NodeResourceMetadataItem` from the table `node_resource_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeResourceMetadataItem>` and uses Records.NodeResourceMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeResourceMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeResourceMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeResourceMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeResourceMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeResourceMetadataItem>` and uses Records.NodeResourceMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeResourceMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeResourceMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeResourceMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeResourceMetadataItem>(sql, parameters)
    
    let insertNodeResourceMetadataItem (context: SqliteContext) (parameters: Parameters.NewNodeResourceMetadataItem) =
        context.Insert("node_resource_metadata", parameters)
    
    /// Select a `Records.NodeResourceNoteVersion` from the table `node_resource_note_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeResourceNoteVersion>` and uses Records.NodeResourceNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeResourceNoteVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeResourceNoteVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeResourceNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeResourceNoteVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeResourceNoteVersion>` and uses Records.NodeResourceNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeResourceNoteVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeResourceNoteVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeResourceNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeResourceNoteVersion>(sql, parameters)
    
    let insertNodeResourceNoteVersion (context: SqliteContext) (parameters: Parameters.NewNodeResourceNoteVersion) =
        context.Insert("node_resource_note_versions", parameters)
    
    /// Select a `Records.NodeResourceNote` from the table `node_resource_notes`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeResourceNote>` and uses Records.NodeResourceNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeResourceNoteRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeResourceNoteRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeResourceNote.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeResourceNote>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeResourceNote>` and uses Records.NodeResourceNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeResourceNoteRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeResourceNoteRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeResourceNote.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeResourceNote>(sql, parameters)
    
    let insertNodeResourceNote (context: SqliteContext) (parameters: Parameters.NewNodeResourceNote) =
        context.Insert("node_resource_notes", parameters)
    
    /// Select a `Records.NodeResource` from the table `node_resources`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeResource>` and uses Records.NodeResource.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeResourceRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeResourceRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeResource.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeResource>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeResource>` and uses Records.NodeResource.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeResourceRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeResourceRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeResource.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeResource>(sql, parameters)
    
    let insertNodeResource (context: SqliteContext) (parameters: Parameters.NewNodeResource) =
        context.Insert("node_resources", parameters)
    
    /// Select a `Records.NodeTag` from the table `node_tags`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeTag>` and uses Records.NodeTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeTagRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeTagRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeTag.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeTag>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeTag>` and uses Records.NodeTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeTagRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeTagRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeTag.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeTag>(sql, parameters)
    
    let insertNodeTag (context: SqliteContext) (parameters: Parameters.NewNodeTag) =
        context.Insert("node_tags", parameters)
    
    /// Select a `Records.Node` from the table `nodes`.
    /// Internally this calls `context.SelectSingleAnon<Records.Node>` and uses Records.Node.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Node.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.Node>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.Node>` and uses Records.Node.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Node.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.Node>(sql, parameters)
    
    let insertNode (context: SqliteContext) (parameters: Parameters.NewNode) =
        context.Insert("nodes", parameters)
    
    /// Select a `Records.ResourceMetadataItem` from the table `resource_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.ResourceMetadataItem>` and uses Records.ResourceMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ResourceMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ResourceMetadataItem>` and uses Records.ResourceMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ResourceMetadataItem>(sql, parameters)
    
    let insertResourceMetadataItem (context: SqliteContext) (parameters: Parameters.NewResourceMetadataItem) =
        context.Insert("resource_metadata", parameters)
    
    /// Select a `Records.ResourceNoteVersion` from the table `resource_note_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.ResourceNoteVersion>` and uses Records.ResourceNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceNoteVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceNoteVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ResourceNoteVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ResourceNoteVersion>` and uses Records.ResourceNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceNoteVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceNoteVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ResourceNoteVersion>(sql, parameters)
    
    let insertResourceNoteVersion (context: SqliteContext) (parameters: Parameters.NewResourceNoteVersion) =
        context.Insert("resource_note_versions", parameters)
    
    /// Select a `Records.ResourceNote` from the table `resource_notes`.
    /// Internally this calls `context.SelectSingleAnon<Records.ResourceNote>` and uses Records.ResourceNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceNoteRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceNoteRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceNote.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ResourceNote>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ResourceNote>` and uses Records.ResourceNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceNoteRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceNoteRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceNote.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ResourceNote>(sql, parameters)
    
    let insertResourceNote (context: SqliteContext) (parameters: Parameters.NewResourceNote) =
        context.Insert("resource_notes", parameters)
    
    /// Select a `Records.ResourceTag` from the table `resource_tags`.
    /// Internally this calls `context.SelectSingleAnon<Records.ResourceTag>` and uses Records.ResourceTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceTagRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceTagRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceTag.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ResourceTag>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ResourceTag>` and uses Records.ResourceTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceTagRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceTagRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceTag.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ResourceTag>(sql, parameters)
    
    let insertResourceTag (context: SqliteContext) (parameters: Parameters.NewResourceTag) =
        context.Insert("resource_tags", parameters)
    
    /// Select a `Records.ResourceVersionMetadataItem` from the table `resource_version_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.ResourceVersionMetadataItem>` and uses Records.ResourceVersionMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceVersionMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceVersionMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceVersionMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ResourceVersionMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ResourceVersionMetadataItem>` and uses Records.ResourceVersionMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceVersionMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceVersionMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceVersionMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ResourceVersionMetadataItem>(sql, parameters)
    
    let insertResourceVersionMetadataItem (context: SqliteContext) (parameters: Parameters.NewResourceVersionMetadataItem) =
        context.Insert("resource_version_metadata", parameters)
    
    /// Select a `Records.ResourceVersionNoteVersion` from the table `resource_version_note_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.ResourceVersionNoteVersion>` and uses Records.ResourceVersionNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceVersionNoteVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceVersionNoteVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceVersionNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ResourceVersionNoteVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ResourceVersionNoteVersion>` and uses Records.ResourceVersionNoteVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceVersionNoteVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceVersionNoteVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceVersionNoteVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ResourceVersionNoteVersion>(sql, parameters)
    
    let insertResourceVersionNoteVersion (context: SqliteContext) (parameters: Parameters.NewResourceVersionNoteVersion) =
        context.Insert("resource_version_note_versions", parameters)
    
    /// Select a `Records.ResourceVersionNote` from the table `resource_version_notes`.
    /// Internally this calls `context.SelectSingleAnon<Records.ResourceVersionNote>` and uses Records.ResourceVersionNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceVersionNoteRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceVersionNoteRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceVersionNote.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ResourceVersionNote>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ResourceVersionNote>` and uses Records.ResourceVersionNote.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceVersionNoteRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceVersionNoteRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceVersionNote.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ResourceVersionNote>(sql, parameters)
    
    let insertResourceVersionNote (context: SqliteContext) (parameters: Parameters.NewResourceVersionNote) =
        context.Insert("resource_version_notes", parameters)
    
    /// Select a `Records.ResourceVersionTag` from the table `resource_version_tags`.
    /// Internally this calls `context.SelectSingleAnon<Records.ResourceVersionTag>` and uses Records.ResourceVersionTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceVersionTagRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceVersionTagRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceVersionTag.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ResourceVersionTag>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ResourceVersionTag>` and uses Records.ResourceVersionTag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceVersionTagRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceVersionTagRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceVersionTag.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ResourceVersionTag>(sql, parameters)
    
    let insertResourceVersionTag (context: SqliteContext) (parameters: Parameters.NewResourceVersionTag) =
        context.Insert("resource_version_tags", parameters)
    
    /// Select a `Records.ResourceVersion` from the table `resource_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.ResourceVersion>` and uses Records.ResourceVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ResourceVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ResourceVersion>` and uses Records.ResourceVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ResourceVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ResourceVersion>(sql, parameters)
    
    let insertResourceVersion (context: SqliteContext) (parameters: Parameters.NewResourceVersion) =
        context.Insert("resource_versions", parameters)
    
    /// Select a `Records.Resource` from the table `resources`.
    /// Internally this calls `context.SelectSingleAnon<Records.Resource>` and uses Records.Resource.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Resource.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.Resource>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.Resource>` and uses Records.Resource.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectResourceRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectResourceRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Resource.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.Resource>(sql, parameters)
    
    let insertResource (context: SqliteContext) (parameters: Parameters.NewResource) =
        context.Insert("resources", parameters)
    
    /// Select a `Records.Tag` from the table `tags`.
    /// Internally this calls `context.SelectSingleAnon<Records.Tag>` and uses Records.Tag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectTagRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectTagRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Tag.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.Tag>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.Tag>` and uses Records.Tag.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectTagRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectTagRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Tag.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.Tag>(sql, parameters)
    
    let insertTag (context: SqliteContext) (parameters: Parameters.NewTag) =
        context.Insert("tags", parameters)
    