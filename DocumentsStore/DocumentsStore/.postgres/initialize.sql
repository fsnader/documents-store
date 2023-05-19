CREATE TYPE user_role AS ENUM ('Regular', 'Manager', 'Admin');

CREATE TABLE IF NOT EXISTS users (
  id SERIAL PRIMARY KEY,
  name TEXT NOT NULL,
  email TEXT NOT NULL UNIQUE,
  role user_role NOT NULL
);


CREATE TABLE IF NOT EXISTS groups (
  id SERIAL PRIMARY KEY,
  name TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS user_groups (
  user_id INTEGER NOT NULL,
  group_id INTEGER NOT NULL,
  PRIMARY KEY (user_id, group_id),
  FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE,
  FOREIGN KEY (group_id) REFERENCES groups (id) ON DELETE CASCADE
);

CREATE TYPE document_category AS ENUM ('General', 'Technical', 'Legal');

CREATE TABLE IF NOT EXISTS documents (
  id SERIAL PRIMARY KEY,
  user_id INTEGER NOT NULL,
  posted_date TIMESTAMP WITH TIME ZONE NOT NULL,
  name TEXT NOT NULL,
  description TEXT,
  category document_category NOT NULL,
  content TEXT NOT NULL,
  FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE TABLE document_user_permissions (
    document_id INTEGER NOT NULL,
    user_id INTEGER NOT NULL,
    PRIMARY KEY (document_id, user_id),
    FOREIGN KEY (user_id) REFERENCES users (id)  ON DELETE CASCADE,
    FOREIGN KEY (document_id) REFERENCES documents (id)  ON DELETE CASCADE
);

CREATE TABLE document_group_permissions (
    document_id INTEGER NOT NULL,
    group_id INTEGER NOT NULL,
    PRIMARY KEY (document_id, group_id),
    FOREIGN KEY (group_id) REFERENCES groups (id)  ON DELETE CASCADE,
    FOREIGN KEY (document_id) REFERENCES documents (id)  ON DELETE CASCADE
);

CREATE OR REPLACE PROCEDURE create_document(
    user_id INT,
    name TEXT,
    description TEXT,
    category TEXT,
    content TEXT,
    posted_date TIMESTAMP WITH TIME ZONE,
    authorized_users INT[],
    authorized_groups INT[],
    INOUT document_id INT
)
    language plpgsql
as $$
DECLARE
    id INTEGER;
BEGIN
    INSERT INTO documents (user_id, name, description, category, content, posted_date)
    VALUES (user_id, name, description, category::document_category, content, posted_date)
        RETURNING id INTO document_id;
    
    FOREACH id IN ARRAY authorized_users
            LOOP
                INSERT INTO document_user_permissions (document_id, user_id)
                VALUES (document_id, user_id);
    END LOOP;
    
        FOREACH id IN ARRAY authorized_groups
            LOOP
                INSERT INTO document_group_permissions (document_id, group_id)
                VALUES (document_id, id);
    END LOOP;
END; $$;

-- INSERT INITIAL ADMIN USER
INSERT INTO users (name, email, role)
VALUES ('admin', 'admin@admin.com', 'Admin'::user_role);