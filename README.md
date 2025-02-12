[![TianLogo](https://i.postimg.cc/SQvx4V0H/TianLogo.png)](https://postimg.cc/14DQcG3K)

# Tian - Python Learning Platform

Welcome to the Python Learning Platform! This web application is designed to help absolute beginners learn Python programming in an engaging and interactive way. With step-by-step lessons, quizzes, and hands-on practice, users will be able to master Python fundamentals and start coding confidently.

**Note:** This project is a **practice project** created for my personal learning and development. It serves as a way for me to apply and improve my programming skills, particularly in web development.

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Getting Started](#getting-started)
4. [Technologies Used](#technologies-used)
5. [License](#license)
6. [Contact](#contact)

## Introduction

The Python Learning Platform is designed to teach Python programming in a fun, interactive, and easy-to-understand way. Each lesson is broken into multiple slides, covering theory, code examples, multiple-choice questions, and practical coding exercises. The platform is ideal for individuals who are completely new to programming and want to learn Python from the ground up.

### Key Features:
- **Step-by-step lessons:** Learn Python concepts gradually with short lessons.
- **Interactive code editor:** Practice coding directly within the platform.
- **Quizzes and assessments:** Reinforce learning with quizzes at the end of each lesson.
- **Engaging user experience:** A fun and non-intimidating learning environment.

## Features

- **User Authentication:**
  - Register and log in to track progress.
  - Option to resume lessons from where you left off.

- **Interactive Coding Exercises:**
  - Edit and run Python code in an integrated code editor.
  - Real-time feedback on code execution. (In development)

- **Multi-Slide Lessons:**
  - Theoretical slides with clear explanations.
  - Code example slides that demonstrate concepts.
  - Quiz slides to test knowledge with multiple-choice questions.

- **Practice Mode:**
  - Practice exercises with instant feedback to help reinforce concepts. (In development)

- **Progress Tracking:**
  - Track your learning progress with visual statistics. (Partially completed)

## Getting Started

### Prerequisites

Before running the project, ensure you have the following software installed:

- **Git**: To clone the repository from GitHub, you will need Git installed. You can download it from [here](https://git-scm.com/downloads)
.

- **.NET 8 SDK**: Since the project uses .NET, you will need the .NET SDK installed to run commands like dotnet restore, dotnet ef, and dotnet run. You can download it from [here](https://dotnet.microsoft.com/en-us/download).

- **SQL Server**: You need to have SQL Server installed, as the project requires two databases. Download it from [here](https://download.microsoft.com/download/c/c/9/cc9c6797-383c-4b24-8920-dc057c1de9d3/SQL2022-SSEI-Dev.exe).

- **MSSM**: You should've an interface for creating\editing\DBs. You can downlaod it from [here] ([https://download.microsoft.com/download/9/b/e/9bee9f00-2ee2-429a-9462-c9bc1ce14c28/SSMS-Setup-ENU.exe](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)).

To get started with the Python learning platform, follow these steps:

### Clone the Repository

First, clone the repository to your local machine:

```powershell
git clone https://github.com/Diezaaa/TianPythonLearningPlatfrom.git
cd TianPythonLearningPlatfrom
cd TianLearningPlatform
```

Install required packages

```powershell
dotnet restore
```

This project requires two databases. One is used for storing user data, and the other is for storing all app's info except user-related data

Create the databases:

- Database 1: AppDb - for stroing all app's info except user-related data.
- Database 2: UserDb - for storing user-related data. 

Open the appsettings.json file and update the connection strings for both databases:

```json
  "ConnectionStrings": {
    "AppConnection": "Server=your_server;Database=AppDb;Trusted_Connection=True;TrustServerCertificate=true",
    "UserConnection": "Server=your_server;DataBase=UserDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
```

Run the migrations for both databases:

```powershell
dotnet ef database update --context UserDbContext
dotnet ef database update --context ApplicationDbContext
```

Run the app:

```powershell
dotnet run
```

## Technologies-used

This project uses the following technologies:

### Backend:
- ASP.NET Core: A cross-platform, high-performance framework for building modern web applications.
- C#: The programming language used for server-side development.

### Frontend:
- CSS & HTML: For structuring and styling the web pages.
- JavaScript: Programming language used for front-end interactivity.

## License
Copyright © 2024 Daniel Gertsykov. All Rights Reserved.
Unauthorized use, modification, or distribution is prohibited.


## Contact
If you have any questions, feel free to reach out:

Email: kotvesopogah@gmail.com <br/>
GitHub: [Diezaaa](https://github.com/Diezaaa)
