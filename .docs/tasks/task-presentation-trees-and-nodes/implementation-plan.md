# Implementation Plan: Presentation Layer (Trees & Nodes)

This task focuses on implementing the REST API controllers for tree and node management, ensuring they align with the Swagger specification and correctly utilize the established exception handling and result mapping infrastructure.

## 1. Research & Scaffolding
- **Audit Context**: Review `spec.md` for exact route names and parameters.
- **Result Handling**: Ensure `IRestCustomResultsHandler` is correctly injected to map `Result` failures to the required exceptions (triggering the global middleware).

## 2. Controller Implementation
Following the **Feature-per-Folder** organization (though controllers usually stay in the Controllers folder, the logic they call is feature-based).

### 2.1 Tree Controller (`TreeController`)
- **Route**: `api.user.tree.get` (POST)
- **Logic**: Calls `IGetTreeQueryHandler`.
- **Parameters**: `treeName` (Query).

### 2.2 Node Controller (`NodeController`)
- **Route**: `api.user.tree.node.create` (POST)
    - **Parameters**: `treeName` (Query), `parentNodeId` (Query, optional), `nodeName` (Query).
- **Route**: `api.user.tree.node.rename` (POST)
    - **Parameters**: `nodeId` (Query), `newNodeName` (Query).
- **Route**: `api.user.tree.node.delete` (POST)
    - **Parameters**: `nodeId` (Query), `isForcedDeletion` (Query, optional).

## 3. Subtasks
See [subtasks/index.md](subtasks/index.md) for the atomic breakdown.

## 4. Verification
- **Integration**: Use the `.http` file to perform a full lifecycle test:
    1. Get a non-existent tree (verify auto-creation).
    2. Create a root node (verify single root constraint).
    3. Create child nodes.
    4. Rename nodes (verify sibling uniqueness).
    5. Delete nodes without force (verify `SecureException`).
    6. Delete nodes with force.
- **Journal Check**: Verify that validation errors and `SecureException` events are correctly logged in the `ExceptionJournal`.
