# Implementation Plan: Validation for Trees & Nodes Application

This task involves implementing `FluentValidation` validators for the existing Journal handlers and adding unit tests for these validators using `FluentValidation.TestHelper`.

## 1. Research & Scaffolding
- **Packages**: Ensure `FluentValidation` and its test helper are available in the Application and Tests projects.
- **Project Context**: Review the existing commands and queries in `JournalChallenge.Application/Journal`.

## 2. Validator Implementation (Application Layer)
For each handler, implement an `AbstractValidator<T>` in its respective feature folder.

### **2.1 `GetTreeQueryValidator`**
- **Rule**: `TreeName` must not be empty.

### **2.2 `CreateNodeCommandValidator`**
- **Rule**: `TreeName` must not be empty.
- **Rule**: `NodeName` must not be empty.
- **Note**: `ParentNodeId` is optional (null allowed for root), but if provided, it could be checked for `> 0` if appropriate.

### **2.3 `RenameNodeCommandValidator`**
- **Rule**: `NodeId` must be `> 0`.
- **Rule**: `NewNodeName` must not be empty.

### **2.4 `DeleteNodeCommandValidator`**
- **Rule**: `NodeId` must be `> 0`.

## 3. Unit Testing (Validation Tests)
Implement test classes in `JournalChallenge.Application.Tests` using `TestValidate` and `ShouldHaveValidationErrorFor`.

### **3.1 `GetTreeQueryValidationTests`**
- Test case: Empty `TreeName` should fail.
- Test case: Valid `TreeName` should pass.

### **3.2 `CreateNodeCommandValidationTests`**
- Test case: Empty `TreeName` should fail.
- Test case: Empty `NodeName` should fail.
- Test case: Valid command should pass.

### **3.3 `RenameNodeCommandValidationTests`**
- Test case: `NodeId` <= 0 should fail.
- Test case: Empty `NewNodeName` should fail.
- Test case: Valid command should pass.

### **3.4 `DeleteNodeCommandValidationTests`**
- Test case: `NodeId` <= 0 should fail.
- Test case: Valid command should pass.

## 4. Verification
- **Build**: Ensure all projects compile.
- **Test**: Run all tests in `JournalChallenge.Application.Tests` to verify both validation and existing business logic.
