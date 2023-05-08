# documents-store
A .NET 7 API to store and retrieve documents

Login/Admin:
- Users CRUD
- Groups CRUD

Documents:
- Upload
- Edit (grant/remove access)
- Download

Users
Groups
Roles

Tests:
- Unit
- E2E

Database:
- Database Modeling
- Database setup

Document:
    - Id
    - UserId (user who created)
    - PostedDate
    - Name
    - Description
    - Category

// Don't need to be mapped to the code
DocumentPermissions:
    - DocumentId
    - GroupId (nullable)
    - UserId (nullable)

User:
    - Id
    - Name
    - Email
    - Password // TODO
    - Role

Group
    - Id
    - Name

// Don't need to be mapped to the code
GroupUsers
    - GroupId
    - UserId