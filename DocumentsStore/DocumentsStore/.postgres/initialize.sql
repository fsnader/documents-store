CREATE TYPE "UserRole" AS ENUM ('Regular', 'Manager', 'Admin');

CREATE TABLE IF NOT EXISTS "User" (
  "Id" SERIAL PRIMARY KEY,
  "Name" TEXT NOT NULL,
  "Email" TEXT NOT NULL UNIQUE,
  "Role" "UserRole" NOT NULL
);


CREATE TABLE IF NOT EXISTS "Group" (
  "Id" SERIAL PRIMARY KEY,
  "Name" TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS "UserGroup" (
  "UserId" INTEGER NOT NULL,
  "GroupId" INTEGER NOT NULL,
  PRIMARY KEY ("UserId", "GroupId"),
  FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE,
  FOREIGN KEY ("GroupId") REFERENCES "Group" ("Id") ON DELETE CASCADE
);

CREATE TYPE "DocumentCategory" AS ENUM ('General', 'Technical', 'Legal');

CREATE TABLE IF NOT EXISTS "Document" (
  "Id" SERIAL PRIMARY KEY,
  "UserId" INTEGER NOT NULL,
  "PostedDate" TIMESTAMP WITH TIME ZONE NOT NULL,
  "Name" TEXT NOT NULL,
  "Description" TEXT,
  "Category" "DocumentCategory" NOT NULL,
  "Content" TEXT NOT NULL,
  FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE
);

CREATE TABLE "DocumentUserPermission" (
    "DocumentId" INTEGER NOT NULL,
    "UserId" INTEGER NOT NULL,
    PRIMARY KEY ("DocumentId", "UserId"),
    FOREIGN KEY ("UserId") REFERENCES "User" ("Id"),
    FOREIGN KEY ("DocumentId") REFERENCES "Document" ("Id")
);

CREATE TABLE "DocumentGroupPermission" (
    "DocumentId" INTEGER NOT NULL,
    "GroupId" INTEGER NOT NULL,
    PRIMARY KEY ("DocumentId", "GroupId"),
    FOREIGN KEY ("GroupId") REFERENCES "Group" ("Id"),
    FOREIGN KEY ("DocumentId") REFERENCES "Document" ("Id")
);

CREATE OR REPLACE PROCEDURE "CreateDocument"(
    user_id INT,
    name TEXT,
    description TEXT,
    category TEXT,
    content TEXT,
    posted_date TIMESTAMP WITH TIME ZONE,
    authorized_users INT[],
    authorized_groups INT[],
    OUT document_id INT
)
language plpgsql
as $$
DECLARE
    id INTEGER;
BEGIN
INSERT INTO "Document" ("UserId", "Name", "Description", "Category", "Content", "PostedDate")
VALUES (user_id, name, description, category::"DocumentCategory", content, posted_date)
    RETURNING "Id" INTO document_id;

FOREACH id IN ARRAY authorized_users
        LOOP
            INSERT INTO "DocumentUserPermission" ("DocumentId", "UserId")
            VALUES (document_id, user_id);
END LOOP;

    FOREACH id IN ARRAY authorized_groups
        LOOP
            INSERT INTO "DocumentGroupPermission" ("DocumentId", "GroupId")
            VALUES (document_id, id);
END LOOP;
END; $$