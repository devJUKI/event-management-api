# Event Management API
Events Management API which requirements were given by ChatGPT (Minimal APIs, Result Pattern).

**❓ Prompt given to ChatGPT**:

`I want to practice creating REST APIs as a mid-level developer. Can you provide me with requirements for an API that I could develop?`

**🤖 ChatGPT**:

`Since you're a mid-level developer, here's an API project idea that will push your skills while staying realistic in scope. The following project idea covers core RESTful principles, CRUD operations, authentication, and data relationships.`

## 📜 API Idea: Event Management System
This API allows users to **create**, **manage**, and **attend** events. It includes multiple interconnected entities, offering robust CRUD functionality, authentication, and validations to ensure secure and efficient event management.

## Core Functionalities

### 🧑‍💻 **User Management**
- Register, login, and manage user profiles.
- **CRUD operations** for users (excluding deletion for security purposes).

### 📅 **Event Management**
- **CRUD operations** for creating and managing events:
  - **Title**: Name of the event.
  - **Date**: Date and time of the event.
  - **Location**: Where the event is held.
  - **Description**: Details about the event.
- Events can be marked as:
  - **Public**: Accessible to all users.
  - **Private**: Restricted to specific users.

### 👥 **Attendance Management**
- Users can mark themselves as attending an event.
- Retrieve **lists of attendees** for each event.

### 🏷️ **Categories/Tags**
- Assign **categories** or **tags** to events (e.g., "Sports," "Conference," "Meetup").
- Filter events by category or tag to make discovery easier.

### 🔍 **Search and Filter**
- **Search** events by name or description.
- **Filter** events by:
  - Date
  - Location
  - Category

### 🔐 **Authentication**
- Secure endpoints using **token-based authentication** (e.g., **JWT**).
- Only authenticated users can:
  - Create events.
  - Mark attendance.

## Suggested Entities

### User

```json
{
  "id": 1,
  "username": "john_doe",
  "email": "john@example.com",
  "passwordHash": "hashed_password",
  "createdAt": "2024-01-01T10:00:00Z"
}
```

### Event

```json
{
  "id": 1,
  "title": "Tech Conference 2024",
  "description": "Annual tech conference for developers.",
  "location": "Vilnius, Lithuania",
  "date": "2024-07-15T09:00:00Z",
  "createdBy": 1,
  "isPublic": true,
  "categories": ["Tech", "Conference"]
}
```

### Attendance

```json
{
  "id": 1,
  "eventId": 1,
  "userId": 2,
  "status": "attending",  // Other options: "interested", "not attending"
  "timestamp": "2024-06-01T14:30:00Z"
}
```

## API Endpoints

### Authentication

- **POST** `/auth/register`  
  Register a new user.

- **POST** `/auth/login`  
  Login and receive a token.

### User Management

- **GET** `/users/{id}`  
  Get user profile.

- **PUT** `/users/{id}`  
  Update user details.

### Event Management

- **GET** `/events`  
  List all events (with optional filters).

- **GET** `/events/{id}`  
  Get a single event.

- **POST** `/events`  
  Create a new event (authenticated users only).

- **PUT** `/events/{id}`  
  Update an event (only the creator).

- **DELETE** `/events/{id}`  
  Delete an event (only the creator).

### Attendance

- **POST** `/events/{id}/attend`  
  Mark attendance for an event.

- **GET** `/events/{id}/attendees`  
  Get a list of attendees.


### Categories/Tags
- **GET** `/categories`
  List all categories.

- **GET** `/events?category=Tech`
  Filter events by category.

## Technical Considerations

### Authentication
- **JWT** (JSON Web Tokens) for securing routes and handling user authentication.

### Database
- **SQL**: Use relational databases like **PostgreSQL** or **SQL Server** for structured data.
- **NoSQL**: Consider **MongoDB** for flexible, schema-less data storage.

### Framework
- **ASP.NET Core Web API** for building the API backend.

### ORM
- **Entity Framework Core** for ORM (Object-Relational Mapping), simplifying database operations.

### Validation
- **FluentValidation** or **built-in data annotations** for data validation and business logic checks.

### Documentation
- **Swagger/OpenAPI** for automatic API documentation, making it easy to test and understand endpoints.

### Testing
- Unit tests for core functionality using **XUnit** or **NUnit** for ensuring the reliability of the application.

## Optional Features to Enhance Your Skills

- **Pagination**: Implement pagination for listing large sets of events to improve performance and user experience.
  
- **Rate Limiting**: Prevent abuse of your API by limiting the number of requests from a single client in a given time period.
  
- **Email Notifications**: Send email notifications when users register or attend an event to improve engagement.

- **Role-Based Access Control (RBAC)**: Implement different permissions for **admin** and **regular users**, controlling what actions they can perform on the API.

---

This project covers a broad range of **REST API** concepts and will help you practice both core and advanced skills. If you'd like further details on any specific part, feel free to ask!

