# Final Implementation Plan: Application Logic (Trees & Nodes)

This document represents the final state of the implementation plan for the core hierarchical tree management logic and unit testing within the **Application Layer**.

## Status: COMPLETED (2026-02-19)

## 1. Research & Scaffolding [COMPLETED]
- [x] **Audit Context**: Review `JournalChallenge.Application.Core` for the existing `ICommand`, `IQuery`, and `Result` interfaces.
- [x] **Error Mapping Strategy**:
    - Application layer returns `Result<T, IError<DomainError>>`.
    - `IRestCustomResultsHandler` will map `ErrorType.Secure` to `SecureException` and other errors to `Exception`.
- [x] **Define DTOs (Application Layer)**:
    - `NodeDto`: Recursive model with `long Id`, `string Name`, and `IEnumerable<NodeDto> Children`. Matches the `MNode` schema.

## 2. Feature: Get Entire Tree (`GetTreeQuery`) [COMPLETED]
- [x] **Logic**:
    1.  Search for a `Tree` entity by `Name`.
    2.  If the tree does not exist, create a new `Tree` record (auto-creation requirement).
    3.  Return the `Tree`'s root node and all its descendants.
- [x] **Outcome**: A structured `NodeDto` representing the entire tree as defined in the `MNode` schema.

## 3. Feature: Create Node (`CreateNodeCommand`) [COMPLETED]
- [x] **Logic**:
    1.  **Get or Create Tree**: Search for a `Tree` by `treeName`. If not found, create it.
    2.  If `parentNodeId` is null (Root Node attempt):
        - **Single Root Check**: Verify if the tree already has a root node. If so, throw a `SecureException` (message: "A tree cannot have more than one root node").
    3.  If `parentNodeId` is provided:
        - Verify the parent node exists and belongs to the specified tree.
    4.  **Sibling Uniqueness Check**: Ensure no existing sibling (nodes with the same `ParentId`) has the same name.
    5.  Create and persist the new `Node`.
- [x] **Error Handling**: Throw or return a `SecureException` if sibling names conflict, the parent ID is invalid, or a second root node is attempted.

## 4. Feature: Rename Node (`RenameNodeCommand`) [COMPLETED]
- [x] **Logic**:
    1.  Find the `Node` by `nodeId`.
    2.  Check for name uniqueness among its current siblings (excluding the node itself).
    3.  Update the `Name` property.

## 5. Feature: Delete Node (`DeleteNodeCommand`) [COMPLETED]
- [x] **Logic**:
    1.  Locate the node by `nodeId` including its children.
    2.  Check if the node has children and `isForcedDeletion` is false.
    3.  If it has children and not forced: throw a `SecureException("You have to delete all children nodes first")`.
    4.  Otherwise, perform a recursive delete (leveraging `DeleteBehavior.Cascade` if configured, or manual traversal if required).

## 6. Handler Interface Pattern [COMPLETED]
- [x] **Implementation**: Each handler now implements a dedicated interface (e.g., `IGetTreeQueryHandler`) as per architectural conventions.

## 7. Unit Testing (Application Logic) [COMPLETED]
- [x] **Verify Implementation**: Correctness of the application services and commands.
- [x] **Test Cases**:
    - **GetTree**: Verify auto-creation when a tree name is not found.
    - **CreateNode**: 
        - Success when adding a root node (to a new or empty tree).
        - Success when adding a child node to a valid parent.
        - Failure when adding a second root node to a tree that already has one.
        - Failure when adding a node with a name that already exists among siblings.
        - Failure when `parentNodeId` belongs to a different tree.
    - **RenameNode**:
        - Success for a valid rename.
        - Failure if the new name conflicts with an existing sibling.
    - **DeleteNode**:
        - Success for leaf nodes.
        - Success for nodes with descendants when `isForcedDeletion` is true.
        - Failure (SecureException) for nodes with descendants when `isForcedDeletion` is false.
        
## 8. Alignment Fixes [COMPLETED]
- [x] **Resolve Delete Contradiction**: Introduced `isForcedDeletion` parameter. If false/missing, throw `SecureException` when children exist. If true, perform recursive delete. (Updated in `spec.md`).
- [x] **Align GetTree Return Type**: Using `NodeDto` representing the root exactly as defined in the Swagger `MNode` schema (no `TreeId/Name` at the root).
- [x] **Define Auto-Creation Strategy**: Implement a shared `GetOrCreateTree` logic to ensure consistency if `node.create` is called before the first `get` call.
- [x] **Root Node Structure**: Enforced a **Single Root per Tree** constraint. If `parentNodeId` is null, verify no existing root exists for the tree.
- [x] **DTO Mapping**: `NodeDto` properties will naturally map to Swagger via standard JSON serialization.
