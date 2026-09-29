# tian - Python Learning Platform
## Note
The project is no longer maintained
## Description
This web application is intended to teach absolute beginners Python programming in an engaging and interactive manner. With step-by-step lessons, quizzes, and hands-on practice, users will be able to master Python fundamentals and begin coding with confidence.
## Screenshots
<img src="lessons_page.png" alt="drawing" width="100%"/>

<img src="profile_page.png" alt="drawing" width="100%"/>

## Demonstration video
Link: https://www.youtube.com/watch?v=9IbWCHOr5sM
## Main Features
-   **Account system:** Users can create an account by entering their email address and password. They can also make changes to their account information, such as their profile picture and username.

-   **Lessons:** The study program is made up of lessons. Each lesson lasts about 3 to 10 minutes and includes a variety of slides. There's a theory slide with text and an image, a question slide with four answer options, and a coding question slide with an embedded code editor.

-   **System administration:** A user with the role of adminstartor can create and delete slides and lessons. Additionally, users can be created, deleted, and edited. All of this is done on specific pages that require administrator privileges.

-   **Achievement system:** users can earn achievements, such as "First steps" when they complete their first lesson. This system encourages the user to test his limits and progress in his studies. The achievements can be viewed on the profile page.
## Main Tech Used
- ASP.NET Core

- HTML

- CSS

- Java Script (JS)
## Installation
This section shows how to run thr web app locally on your pc.
### Video Guide
This video is very detailed so you get less problems:
[video link]
### Text Guide
- Install [Visual Studio](https://visualstudio.microsoft.com)
- In the Visual Studio Installer select ".NET desktop development" and "ASP.NET and web development" workloads.
- Install [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server)
- Install the two databases from here: [Link to Drive](https://drive.google.com/drive/folders/1xLLMWMRSm_MfHhAakuFg92R1WldMh8mq?usp=sharing) CHECK THAT THIS IS ACCESSIBLE FROM THE WEB
- Install [SQL Server Management Studio](https://learn.microsoft.com/en-us/ssms/install/install)
- Select the "Basic" installation type and follow the prompts to complete the installation.
- Open SQL Server Management Studio on your computer
- Click File -> Connect Object Explorer
- Set the Server name and enter localhost as the name of the server
- Tick the Trust Server Certificate checkbox
- Click Connect
- In the Object Explorer, click on the SQL Server instance to expand it
- Right-click on the Databases folder and select Import Data-tier Application
- Click Next on the Introduction screen
- Click Browse and select the AppDb.bacpac
- Click Next and then click Finish to start the import process
- Repeat the process for the second database
- Open Visual Studio and click Clone a repository
- Enter the following URL in the Repository location field: https://github.com/Diezaaa/tian.git
- Click Clone to download the repository to your local machine
- If prompted in the Solution Explorer that the project is targeting a different version of .NET, click Yes to retarget the project to the installed version of .NET
- Click on the "Run" button to start the application
- You can use the following credentials to log in:
    - Admin:
      - Username: Dieza
      - Password: 1234567DAniel_
    - Regular user:
      - Username: CoolJohn123
      - Password: 1234567JOhn_

      VERIFY THAT THE WEB APP WORKS ON MY LAPTOP
## Important Point About Architecture

The app uses the MVC architecture pattern.