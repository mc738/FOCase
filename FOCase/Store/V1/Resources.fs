namespace FOCase.Store.V1

[<RequireQualifiedAccess>]
module Resources =
    
    open Freql.Sqlite
    open FOCase.Core
    open FOCase.Store.V1.Persistence

    // *** General ***

    let add (ctx: SqliteContext) (id: IdType option) (name: string) =
        ({ Id = getId id
           Name = name
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewResource)
        |> Operations.insertResource ctx

    let get (ctx: SqliteContext) (id: string) =
        Operations.selectResourceRecord ctx [ "WHERE id = @0" ] [ id ]
        
    let getAll (ctx:SqliteContext) =
        Operations.selectResourceRecords ctx [] []
        
    let getAllActive (ctx: SqliteContext) =
        Operations.selectResourceRecords ctx [ "WHERE active = TRUE" ] []
    
    let activate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resources SET active = TRUE WHERE id = @0", [ id ])

    let deactivate (ctx: SqliteContext) (id: string) =
        ctx.ExecuteVerbatimNonQueryAnon("UPDATE resources SET active = FALSE WHERE id = @0", [ id ])

    // *** Metadata ***
    
    let getMetadataValue (ctx: SqliteContext) (resourceId: string) (key: string) =
        Operations.selectResourceMetadataItemRecord ctx [ "WHERE resource_id = @0 AND item_key = @1" ] [ resourceId; key ]

    let addMetadataValue (ctx: SqliteContext) (resourceId: string) (key: string) (value: string) =
        ({ ResourceId = resourceId
           ItemKey = key
           ItemValue = value
           CreatedOn = getTimestamp ()
           Active = true }
        : Parameters.NewResourceMetadataItem)
        |> Operations.insertResourceMetadataItem ctx

    let tryAddMetadataValue (ctx: SqliteContext) (resourceId: string) (key: string) (value: string) =
        match getMetadataValue ctx resourceId key with
        | Some _ -> Error $"Metadata value `{key}` already exists for resource `{resourceId}`"
        | None -> addMetadataValue ctx resourceId key value |> Ok

    let updateMetadataValue (ctx: SqliteContext) (resourceId: string) (key: string) (value: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE resource_metadata SET item_value = @0 WHERE resource_id = @1 AND item_key = @2",
            [ value; resourceId; key ]
        )
        |> ignore

    let tryUpdateMetadataValue (ctx: SqliteContext) (resourceId: string) (key: string) (value: string) =
        match getMetadataValue ctx resourceId key with
        | Some _ -> updateMetadataValue ctx resourceId key value |> Ok
        | None -> Error $"Metadata value `{key}` does not exist for resource `{resourceId}`"

    let addOrUpdateMetadataValue (ctx: SqliteContext) (resourceId: string) (key: string) (value: string) =
        match getMetadataValue ctx resourceId key with
        | Some _ -> updateMetadataValue ctx resourceId key value
        | None -> addMetadataValue ctx resourceId key value

    let activateMetadataItem (ctx: SqliteContext) (resourceId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE resource_metadata SET active = TRUE WHERE resource_id = @0 AND item_key = @1",
            [ resourceId; key ]
        )

    let deactivateMetadataItem (ctx: SqliteContext) (resourceId: string) (key: string) =
        ctx.ExecuteVerbatimNonQueryAnon(
            "UPDATE resource_metadata SET active = FALSE WHERE resource_id = @0 AND item_key = @1",
            [ resourceId; key ]
        )