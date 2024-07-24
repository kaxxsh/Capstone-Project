# Twitter Clone

This is a Twitter clone application built with .NET and SQL for the backend, and Next.js for the frontend. The application includes features such as user registration, login, posting tweets, and following/unfollowing users.

## Table of Contents

- [Features](#features)
- [Backend Setup](#backend-setup)
  - [Project Setup](#project-setup)
  - [Database Design](#database-design)
  - [Entity Framework Core Configuration](#entity-framework-core-configuration)
  - [Repositories and Services](#repositories-and-services)
  - [Controllers](#controllers)
  - [Authentication and Authorization](#authentication-and-authorization)
  - [API Documentation with Swagger](#api-documentation-with-swagger)
- [Frontend Setup](#frontend-setup)
  - [Project Setup](#project-setup-1)
  - [Configure Tailwind CSS](#configure-tailwind-css)
  - [Page Structure](#page-structure)
  - [State Management](#state-management)
  - [API Integration](#api-integration)
  - [Authentication](#authentication)
- [Deployment](#deployment)
- [Security Considerations](#security-considerations)

## Features

- User registration and login
- Posting tweets
- Viewing tweets from followed users
- likes tweets from users
- Following and unfollowing users
- User profiles

## Backend Setup

### Project Setup

1. **Create a new .NET Web API project:**
   ```bash
   dotnet new webapi -n TwitterCloneAPI
   ```

2. **Install necessary packages:**
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
   dotnet add package Swashbuckle.AspNetCore
   ```

### Repositories and Services

Create repositories for data access and services for business logic.

### Controllers

- **UserController:** Handles user registration, login, profile management.
- **TweetController:** Handles tweet creation, deletion, fetching tweets.
- **FollowController:** Manages following/unfollowing users.

### Authentication and Authorization

Use JWT for token-based authentication and secure endpoints using the `[Authorize]` attribute.

### API Documentation with Swagger

Enable Swagger in `Startup.cs`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "TwitterCloneAPI", Version = "v1" });
    });
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TwitterCloneAPI v1");
    });
}
```

## Frontend Setup

### Project Setup

1. **Create a new Next.js project:**
   ```bash
   npx create-next-app twitter-clone-frontend
   ```

2. **Install necessary packages:**
   ```bash
   npm install axios tailwindcss
   ```

### Configure Tailwind CSS

1. **Install Tailwind CSS:**
   ```bash
   npx tailwindcss init -p
   ```

2. **Configure `tailwind.config.js`:**
   ```javascript
   module.exports = {
     content: [
       './pages/**/*.{js,ts,jsx,tsx}',
       './components/**/*.{js,ts,jsx,tsx}',
     ],
     theme: {
       extend: {},
     },
     plugins: [],
   }
   ```

3. **Add Tailwind directives to `globals.css`:**
   ```css
   @tailwind base;
   @tailwind components;
   @tailwind utilities;
   ```

### Page Structure

- **Pages:** Home, Profile, Login, Register
- **Components:** Navbar, Tweet, TweetForm, UserList

### State Management

Use React Context API or a state management library like Redux for managing user authentication state and tweets.

### API Integration

1. **Set up Axios instance:**
   ```javascript
   import axios from 'axios';

   const api = axios.create({
       baseURL: 'http://localhost:5000/api',
   });

   export default api;
   ```

2. **Fetch data in pages/components:**
   ```javascript
   useEffect(() => {
       api.get('/tweets')
           .then(response => setTweets(response.data))
           .catch(error => console.error(error));
   }, []);
   ```

### Authentication

Implement login and registration forms. Store JWT tokens in cookies or local storage. Protect routes based on authentication status.

## Deployment

- **Backend:** Deploy on a cloud service like Azure or AWS.
- **Frontend:** Deploy on Vercel or another static site hosting service.

## Security Considerations

- Ensure secure password storage (hashing).
- Validate all inputs to prevent SQL injection.
- Implement proper CORS policies.
