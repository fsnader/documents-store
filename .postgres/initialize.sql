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

CREATE TABLE IF NOT EXISTS "DocumentPermission" (
  "DocumentId" INTEGER NOT NULL,
  "UserId" INTEGER,
  "GroupId" INTEGER,
  PRIMARY KEY ("DocumentId", "UserId", "GroupId"),
  FOREIGN KEY ("DocumentId") REFERENCES "Document" ("Id") ON DELETE CASCADE,
  FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE,
  FOREIGN KEY ("GroupId") REFERENCES "Group" ("Id") ON DELETE CASCADE
);
