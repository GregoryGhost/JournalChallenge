# Core Specification

## 1. Overview
Create an ASP.NET Core 8 REST API application. No UI is required. The application must use a Code-First approach with one of the following databases: **PostgreSQL (Preferred)**, MS SQL, or MySQL.

## 2. Database Design

### 2.1 Nodes of Independent Trees
- **Structure:** Hierarchical tree structure.
- **Independence:** Each tree is independent; nodes belong to a specific tree.
- **Constraints:**
    - Each node must belong to a single tree.
    - All child nodes must belong to the same tree as their parent.
    - Each node must have a mandatory `name` field.
    - Sibling nodes must have unique names (implied by "A new node name must be unique across all siblings" in Swagger).
    - **Note:** Additional fields necessary for ensuring tree independence may be added.

### 2.2 Exception Journal
- **Purpose:** Track **all** exceptions during REST API request processing.
- **Required Fields:**
    - **Unique Event ID**
    - **Timestamp**
    - **All query/body parameters**
    - **Stack trace** of the exception

## 3. Exception Handling

### 3.1 Custom Exception: `SecureException`
- Define a custom exception class named `SecureException`.
- **Behavior:** If a `SecureException` (or subclass) is thrown:
    1.  Store all details in the journal.
    2.  Respond with **HTTP 500**.
    3.  **Response Body Format:**
        ```json
        {
          "type": "name of exception",
          "id": "id of event",
          "data": {
            "message": "message of exception"
          }
        }
        ```
        **Example:**
        ```json
        {
          "type": "Secure",
          "id": "638136064526554554",
          "data": {
            "message": "You have to delete all children nodes first"
          }
        }
        ```

### 3.2 General Exceptions
- **Behavior:** For all other exceptions:
    1.  Log full details in the journal.
    2.  Respond with **HTTP 500**.
    3.  **Response Body Format:**
        ```json
        {
          "type": "Exception",
          "id": "id of event",
          "data": {
            "message": "Internal server error ID = id of event"
          }
        }
        ```
        **Example:**
        ```json
        {
          "type": "Exception",
          "id": "638136064187111634",
          "data": {
            "message": "Internal server error ID = 638136064187111634"
          }
        }
        ```

## 4. API Requirements
The REST API structure must replicate the provided Swagger definition as closely as possible.

### 4.1 Tree & Nodes (`user.tree`, `user.tree.node`)

| Method | Endpoint | Description | Parameters (Query) |
| :--- | :--- | :--- | :--- |
| **POST** | `/api.user.tree.get` | Returns entire tree. Creates it automatically if it doesn't exist. | `treeName` (string, required) |
| **POST** | `/api.user.tree.node.create` | Create a new node. If the tree doesn't exist, it will be created automatically. Parent ID is optional; if omitted, the node becomes the tree's single root. [See more for the creation rules](#411-node-creation-rules) | `treeName` (string, required)<br>`parentNodeId` (int64, optional)<br>`nodeName` (string, required) |
| **POST** | `/api.user.tree.node.delete` | Delete an existing node. If the node has descendants, it will only be deleted (along with all its descendants) if `isForcedDeletion` is true. [See more for the deletion rules](#412-node-deletion-rules) | `nodeId` (int64, required)<br>`isForcedDeletion` (boolean, optional) |
| **POST** | `/api.user.tree.node.rename` | Rename an existing node. New name must be unique among siblings. | `nodeId` (int64, required)<br>`newNodeName` (string, required) |

#### 4.1.1 Node creation rules
- **Auto-Creation**: If `treeName` is not found, a new tree is created automatically.
- **Single Root Constraint**: Each tree can have only **one** root node (where `ParentId` is null). If a second root node is attempted for the same tree, throw a `SecureException` with an appropriate message.
- **Sibling Uniqueness**: Node names must be unique among siblings within the same tree.

#### 4.1.2 Node deletion rules
- If a node has children and `isForcedDeletion` is **false** or **missing**: Throw a `SecureException` with the message: `"You have to delete all children nodes first"`.
- If a node has children and `isForcedDeletion` is **true**: Delete the node and all its descendants recursively.
- If a node has no children: Delete the node normally.

### 4.2 Journal (`user.journal`)

| Method | Endpoint | Description | Parameters |
| :--- | :--- | :--- | :--- |
| **POST** | `/api.user.journal.getRange` | Pagination API for journal entries. | **Query:** `skip` (int32), `take` (int32)<br>**Body:** `filter` (Object - optional) |
| **POST** | `/api.user.journal.getSingle` | Get information about a particular event by ID. | **Query:** `id` (int64, required) |

**Journal Filter Model:**
```json
{
  "from": "datetime",
  "to": "datetime",
  "search": "string"
}
```

### 4.3 Authentication (Optional) (`user.partner`)

| Method | Endpoint | Description | Parameters |
| :--- | :--- | :--- | :--- |
| **POST** | `/api.user.partner.rememberMe` | Saves user by unique code and returns auth token. | **Query:** `code` (string, required) |

## 5. Technical Constraints
- **Framework:** ASP.NET Core 8
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core (Code-First)
- **Architecture:** Clean Architecture.
