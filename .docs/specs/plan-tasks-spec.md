# Task Handling Specification

This document defines the standard workflow for planning, executing, and finalizing tasks within the JournalChallenge project. This process ensures transparency, traceability, and architectural consistency.

## 1. Task Initialization
Every significant feature, bug fix, or refactoring MUST be managed as a discrete task.

1.  **Directory Creation**: Create a new directory in `.docs/tasks/` named `task-<short-description>`.
    - Example: `.docs/tasks/task-fix-tech-debt/`
2.  **Initial Plan**: Create `implementation-plan.md` in the task directory. This file serves as the "source of truth" for what needs to be done.

## 2. Implementation Plan Structure
The `implementation-plan.md` MUST include:

### Title
`# Implementation Plan: {Task Name}`

### Task Goals
A high-level list of the primary objectives. Use `[DONE]` markers as goals are achieved.

### Subtasks
Detailed, actionable steps broken down by component or layer (e.g., Domain, Application, Infrastructure, Presentation).
- Each subtask should have an **Objective** and **Action** list.
- Use checkboxes `[ ]` to track progress.

### Verification
A list of specific steps to verify the implementation, including unit tests, integration tests, and manual validation steps.

## 3. Execution Phase
- **Iterative Workflow**: Work through subtasks sequentially.
- **Updating the Plan**: As subtasks are completed, update the `implementation-plan.md` by checking off boxes and adding `[DONE]` to section titles.
- **Course Correction**: If the implementation strategy changes during the task, update the plan to reflect the new direction.

## 4. Audit Phase
Once all subtasks are complete, a mandatory audit MUST be performed.

1.  **Audit Directory**: Ensure an `audit/` folder exists within the task directory.
2.  **Audit Report**: Create an audit file following the naming and structure defined in [audit-spec.md](audit-spec.md).
    - Format: `audit-{NNN}-{YYYY-MM-DD}-{HH-mm}.md`
3.  **Verification**: The audit MUST confirm that all goals in the plan have been met and that the implementation adheres to the project's architectural standards.

## 5. Task Finalization
After a successful audit, create a `final-implementation.md` in the task directory.

1.  **Summary**: Provide a concise summary of the changes made.
2.  **Status**: Mark the task as `COMPLETED` with the date.
3.  **Files Changed**: List the key files that were modified or created.
4.  **Verification Check**: A final checklist confirming build success, test passes, and documentation updates.

## 6. Task Lifecycle Summary
`Plan (implementation-plan.md)` -> `Act (Code & Tests)` -> `Audit (audit/audit-001-...)` -> `Finalize (final-implementation.md)`
