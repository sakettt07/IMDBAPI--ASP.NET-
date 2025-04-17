# 🎬 IMDb Backend

A powerful backend system for an IMDb-like application built with ASP.NET Core. This project supports user authentication, movie management, reviews, actor/producer/genre creation, and file storage via Supabase.

## 🚀 Project Overview

This backend application allows users to:

- Register and login with JWT-based authentication
- Add, update, delete movies
- Add actors, producers, and genres
- Upload movie posters (stored in Supabase)
- Write reviews for the movies they added
- View all movies and their associated details

Built using a clean and maintainable architecture by applying the **Business Code Structure**, this project separates concerns into multiple layers to handle models, business logic, and database interaction effectively.

---

## 🧰 Tech Stack

- **Framework**: ASP.NET Core
- **Language**: C#
- **Database**: SQL Server
- **ORMs**:
  - Dapper (for lightweight, performant queries)
  - Entity Framework Core (for quick scaffolding & migrations)
- **Authentication**: JWT (JSON Web Tokens)
- **Cloud Storage**: [Supabase](https://supabase.io/)
- **API Documentation**: Swagger (via Swashbuckle)
- **Architecture**: Multi-layered architecture
  - **Domain Layer**: All request/response models
  - **Service Layer**: Business logic implementation
  - **Repository Layer**: Database operations via Dapper

---

---

## 🔐 Authentication

- Uses JWT for secure token generation and user session management
- Token is issued on login and used for all protected routes
- Passwords are securely hashed using industry best practices

---

## 🖼️ Supabase Integration

- Used for storing and retrieving movie posters
- Users can upload images through the backend
- URLs are stored alongside movie data

---

## ✅ Features

- [x] User Authentication (JWT)
- [x] Add/Edit/Delete Movies
- [x] Add Actors, Producers, Genres
- [x] Write Reviews
- [x] Upload Movie Posters to Supabase
- [x] Business Layer Separation (Clean Architecture)
- [x] SQL Server with Dapper for high performance

---

Clone the Project and have a look Cheers!
