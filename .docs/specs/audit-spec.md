# Audit Specification

This document defines the standard for audit reports across all tasks in the project. Audits are required at the conclusion of each task to verify implementation against the project's specifications and architectural standards.

## 1. File Naming Convention
Audit files MUST be stored in the `audit/` subdirectory of the corresponding task folder.
The filename MUST follow this pattern:
`audit-{NNN}-{YYYY-MM-DD}-{HH-mm}.md`

Where:
- `NNN`: 3-digit sequential number of the audit for that task (starting at `001`).
- `YYYY-MM-DD`: Current date.
- `HH-mm`: Current time (24-hour format).

Example: `audit-001-2026-02-19-16-45.md`

## 2. Document Structure
Each audit report MUST follow this structure:

### Title
`# Audit: {Task Name} ({NNN})`

### Metadata
- **Date**: {YYYY-MM-DD} {HH-mm}
- **Status**: [COMPLETED | PARTIAL | FAILED]
- **Scope**: Brief description of the work audited (e.g., "Presentation Layer", "Infrastructural changes").

### Sections
1. **Summary of Work**: High-level overview of the implementation.
2. **Key Features Implemented**: Bulleted list of specific features or changes.
3. **Verification & Testing**: Details on how the implementation was verified (unit tests, manual testing, build checks).
4. **Architectural Notes**: Observations on how the work aligns with Clean Architecture and existing project patterns.
5. **Conclusion**: Final assessment of the task status.

## 3. Audit Protocol
- **Persistence**: Every implementation MUST be followed by an audit.
- **Verification**: Audits are not just documentation; they require actual verification (running tests, builds, or manual checks).
- **History**: Audit files are immutable once committed; if changes are needed, a new sequential audit file MUST be created.
