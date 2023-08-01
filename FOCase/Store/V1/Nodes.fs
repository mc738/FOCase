namespace FOCase.Store.V1

open System.Reflection.Emit
open FOCase.Core
open Microsoft.FSharp.Core

[<RequireQualifiedAccess>]
module Nodes =

    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store.V1.Persistence

    // *** General ***

    let add (ctx: SqliteContext) (id: IdType option) (name: string) =
        ({ Id = getId id
           Name = name
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNode)
        |> Operations.insertNode ctx

    let get (ctx: SqliteContext) (id: string) =
        Operations.selectNodeRecord ctx [ "WHERE id = @0" ] [ id ]

    let activate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE nodes SET active = TRUE WHERE id = @0", [ id ])

    let deactivate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE nodes SET active = FALSE WHERE id = @0", [ id ])

    // *** Metadata ***

    let getMetadataValue (ctx: SqliteContext) (nodeId: string) (key: string) =
        Operations.selectNodeMetadataItemRecord ctx [ "WHERE node_id = @0 AND item_key = @1" ] [ nodeId; key ]

    let addMetadataValue (ctx: SqliteContext) (nodeId: string) (key: string) (value: string) =
        ({ NodeId = nodeId
           ItemKey = key
           ItemValue = value
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeMetadataItem)
        |> Operations.insertNodeMetadataItem ctx

    let tryAddMetadataValue (ctx: SqliteContext) (nodeId: string) (key: string) (value: string) =
        match getMetadataValue ctx nodeId key with
        | Some _ -> Error $"Metadata value `{key}` already exists for node `{nodeId}`"
        | None -> addMetadataValue ctx nodeId key value |> Ok

    let updateMetadataValue (ctx: SqliteContext) (nodeId: string) (key: string) (value: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_metadata SET item_value = @0 WHERE node_id = @1 AND item_key = @2",
            [ value; nodeId; key ]
        )
        |> ignore

    let tryUpdateMetadataValue (ctx: SqliteContext) (nodeId: string) (key: string) (value: string) =
        match getMetadataValue ctx nodeId key with
        | Some _ -> updateMetadataValue ctx nodeId key value |> Ok
        | None -> Error $"Metadata value `{key}` does not exist for node `{nodeId}`"

    let addOrUpdateMetadataValue (ctx: SqliteContext) (nodeId: string) (key: string) (value: string) =
        match getMetadataValue ctx nodeId key with
        | Some _ -> updateMetadataValue ctx nodeId key value
        | None -> addMetadataValue ctx nodeId key value

    let activateMetadataItem (ctx: SqliteContext) (nodeId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_metadata SET active = TRUE WHERE node_id = @0 AND item_key = @1",
            [ nodeId; key ]
        )

    let deactivateMetadataItem (ctx: SqliteContext) (nodeId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_metadata SET active = FALSE WHERE node_id = @0 AND item_key = @1",
            [ nodeId; key ]
        )

    // *** Labels

    let getNodeLabel (ctx: SqliteContext) (nodeId: string) (label: string) =
        Operations.selectNodeLabelRecord ctx [ "WHERE node_id = @0 AND label = @1" ] [ nodeId; label ]

    let getAllNodeLabels (ctx: SqliteContext) (nodeId: string) =
        Operations.selectNodeLabelRecords ctx [ "WHERE node_id = @0" ] [ nodeId ]
    
    let getAllActiveNodeLabels (ctx: SqliteContext) (nodeId: string) =
        Operations.selectNodeLabelRecords ctx [ "WHERE node_id = @0 AND active = TRUE" ] [ nodeId ]
        
    let addNodeLabel (ctx: SqliteContext) (nodeId: string) (label: string) (weight: decimal) =
        ({ NodeId = nodeId
           Label = label
           Weight = weight
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeLabel)
        |> Operations.insertNodeLabel ctx

    let tryAddNodeLabel (ctx: SqliteContext) (nodeId: string) (label: string) (weight: decimal) =
        match Labels.get ctx label, get ctx nodeId, getNodeLabel ctx nodeId label with
        | None, _, _ -> Error $"Label `{label}` not found"
        | _, None, _ -> Error $"Node `{nodeId}` not found"
        | _, _, Some _ -> Error $"Label `{label}` already attached to node `{nodeId}`"
        | Some l, Some n, None -> addNodeLabel ctx n.Id l.Name weight |> Ok

    let activateNodeLabel (ctx: SqliteContext) (nodeId: string) (label: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_labels SET active = TRUE WHERE node_id = @0 AND label = @1",
            [ nodeId; label ]
        )

    let deactivateNodeLabel (ctx: SqliteContext) (nodeId: string) (label: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_labels SET active = FALSE WHERE node_id = @0 AND label = @1",
            [ nodeId; label ]
        )

    let updateNodeLabelWeight (ctx: SqliteContext) (nodeId: string) (label: string) (weight: decimal) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_labels SET weight = @0 WHERE node_id = @1 AND label = @2",
            [ weight; nodeId; label ]
        )

    // *** Tags ***

    let getNodeTag (ctx: SqliteContext) (nodeId: string) (tag: string) =
        Operations.selectNodeTagRecord ctx [ "WHERE node_id = @0 AND tag = @1" ] [ nodeId; tag ]

    let getAllNodeTags (ctx: SqliteContext) (nodeId: string) =
        Operations.selectNodeTagRecords ctx [ "WHERE node_id = @0" ] [ nodeId ]
    
    let getAllActiveNodeTags (ctx: SqliteContext) (nodeId: string) =
        Operations.selectNodeTagRecords ctx [ "WHERE node_id = @0 AND active = TRUE" ] [ nodeId ]
        
    let getAllNodeTags (ctx: SqliteContext) (nodeId: string) =
        Operations.selectNodeTagRecords ctx [ "WHERE node_id = @0" ] [ nodeId ]
    
    let addNodeTag (ctx: SqliteContext) (nodeId: string) (tag: string) =
        ({ NodeId = nodeId
           Tag = tag
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewNodeTag)
        |> Operations.insertNodeTag ctx

    let tryAddNodeTag (ctx: SqliteContext) (nodeId: string) (tag: string) =
        match Tags.get ctx tag, get ctx nodeId, getNodeTag ctx nodeId tag with
        | None, _, _ -> Error $"Tag `{tag}` not found"
        | _, None, _ -> Error $"Node `{nodeId}` not found"
        | _, _, Some _ -> Error $"Tag `{tag}` already attached to node `{nodeId}`"
        | Some l, Some n, None -> addNodeTag ctx n.Id l.Name |> Ok

    let activateNodeTag (ctx: SqliteContext) (nodeId: string) (tag: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_tags SET active = TRUE WHERE node_id = @0 AND tag = @1",
            [ nodeId; tag ]
        )

    let deactivateNodeTag (ctx: SqliteContext) (nodeId: string) (tag: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE node_tags SET active = FALSE WHERE node_id = @0 AND tag = @1",
            [ nodeId; tag ]
        )

    // *** Notes ***

    let getNote (ctx: SqliteContext) (noteId: string) =
        Operations.selectNodeNoteRecord ctx [ "WHERE note_id = @0" ] [ noteId ]
    
    let getAllActiveNotes (ctx: SqliteContext) (nodeId: string) =
        Operations.selectNodeNoteRecord ctx [ "WHERE node_id = @0 AND active = TRUE;" ] [ nodeId ]
        
    let getAllNotes (ctx: SqliteContext) (nodeId: string) =
        Operations.selectNodeNoteRecord ctx [ "WHERE node_id = @0" ] [ nodeId ]
        
    let getLatestNoteVersion (ctx: SqliteContext) (noteId: string) =
        Operations.selectNodeNoteVersionRecord ctx [ "WHERE node_note_id = @0 ORDER BY version DESC LIMIT 1" ] [ noteId ]
    
    let addNote (ctx: SqliteContext) (id: IdType option) (nodeId: string) (note: string) =
        ({})

        ()
