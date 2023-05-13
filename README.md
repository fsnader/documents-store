# Document Management API
This API provides endpoints for document management, group management, and user management.
The app is built using .NET 7, Dapper and PostgreSQL.

## Database setup

To run the API and its associated tests, you will need to have Docker installed on your machine. Once you have Docker installed, you can run the following command to start the PostgreSQL server and pgAdmin container:

```bash
docker-compose up -d
```
This will start both the PostgreSQL server and a pgAdmin server running at http://localhost:5050. You can use pgAdmin to interact with the database directly if needed.

On the first container run, the database schema will be created automatically. If you need to make any changes to the schema or initialize the database in any other way, you can modify the initialize.sql script located in the .postgres directory.

To run the integration tests, make sure the containers are running and then execute the tests using Visual Studio or the command line. Please note that the integration tests will create, update, and delete data from the database, so it's recommended to use a test database or reset the container after running the tests.

## Authentication

Authentication is required for all endpoints except for the POST /api/auth/signup and POST /api/auth/login endpoints.
Users must include an Authorization header with a JWT token to access protected endpoints.

- POST /api/auth/signup: Allows users to sign up and create a new account.
- POST /api/auth/login: Allows users to log in and receive a JWT token.

Users can have 3 roles:
- Regular user: can download documents.
- Manager user: can upload and download documents.
- Admin user: can CRUD users and groups, upload and download documents.

## Endpoints
![swagger](/images/api.png?raw=true)
### Documents
- POST /api/documents: Allows authenticated users to upload a new document.
- GET /api/documents: Allows authenticated users to retrieve a list of documents.
- GET /api/documents/{id}: Allows authenticated users to retrieve a specific document by ID.
- POST /api/documents/{id}/users/{userId}: Adds a user permission for the specified document by id
- POST /api/documents/{id}/groups/{groupId}: Adds a group permission for the specified document by id
- DELETE /api/documents/{id}/users/{userId}: Removes a user permission for the specified document by id
- DELETE /api/documents/{id}/groups/{groupId}: Removes a group permission for the specified document by id
### Groups
- GET /api/groups: Allows only admin users to retrieve a list of groups.
- POST /api/groups: Allows only admin users to create a new group.
- GET /api/groups/{id}: Allows only admin users to retrieve a specific group by ID.
- PUT /api/groups/{id}: Allows only admin users to update a specific group by ID.
- DELETE /api/groups/{id}: Allows only admin users to delete a specific group by ID.

### Users
- GET /api/users: Allows only admin users to retrieve a list of users.
- POST /api/users: Allows only admin users to create a new user.
- GET /api/users/{id}: Allows only admin users to retrieve a specific user by ID.
- PUT /api/users/{id}: Allows only admin users to update a specific user by ID.
- DELETE /api/users/{id}: Allows only admin users to delete a specific user by ID.
- GET /api/users/{id}/groups: Allows only admin users to retrieve a list of groups that a specific user belongs to.
- POST /api/users/{userId}/groups/{groupId}: Allows only admin users to add a user to a group.
- DELETE /api/users/{userId}/groups/{groupId}: Allows only admin users to remove a user from a group.

## Testing
This project includes both unit tests and integration tests to ensure the quality and functionality of the API.

### Unit Tests
The DocumentsStore.UnitTests project contains a suite of unit tests for testing the individual components of the API. These tests are designed to test the logic of the code in isolation, without relying on external dependencies. The tests are written using xUnit and can be run from Visual Studio or using the command line.

### Integration Tests
The DocumentsStore.IntegrationTests project contains a suite of integration tests for testing the API endpoints. These tests are designed to test the functionality of the API as a whole, including its interactions with external dependencies such as databases and other APIs. The tests are written using xUnit and can be run from Visual Studio or using the command line.

