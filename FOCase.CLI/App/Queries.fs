namespace FOCase.CLI.App

module Queries =
    
    type Query =
        {
            Target: QueryTarget
        }
        
    and QueryTarget =
        | Nodes
        | Connections
        | Resources
        | Documents
    
    let parse () =
        ()

