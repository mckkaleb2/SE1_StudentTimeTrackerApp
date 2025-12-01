Glossary:
 * API (Application Programming Interface)
    - A standardized method for enabling communication between software components, such as the Blazor frontend and FastAPI backend.
 * Attribute
    - A property or column of an entity in the database (e.g., FirstName, CourseId, etc.).
 * Blazor
    - A .NET-based web UI framework used for the application's frontend.
 * Entity
    - A data model in the system represented as a table in the database (e.g., User, Message).
 * ERD (Entity-Relationship Diagram)
    - A diagram showing entities, their attributes, and how they relate to each other.
 * Foreign Key (FK)
    - A database field that references a Primary Key in another table to define a relationship.
 * Primary Key (PK)
    - A unique identifier for each record in a database table.
 * UI (User Interface)
    - The visual part of the application that the user interacts with, such as the Blazor pages.


Development Environment & Setup:
 * Microsoft Visual Studio & and Visual Studio Code were used for the Integrated Development Environment.
 * Blazor framework was used for the frontend. 
 * Used Postgres for the database.
 * Implemented SignalR for the messaging.

Coding Standards:
 * Naming Conventions
    - Used pascal casing for names of methods, classes, and pages.
 * Spacing
    - Kept most lines short enough to be fully visible when in full-screen.
    - A few areas were shortened to be fully visible when window was only half the screen.
 * Documentation
    - Each class and method has documentation.
    - Some comments throughout certain pages and classes to explain thought processes and functionality.
 * Error Handling
    - Try-catches are used to stop exceptions from stopping the application.
    - In CourseService.cs, when creating/finding a course object, exiting the method before errors occur is the method used.

Project Management Tool:
 * Trello was used to track the backlog of tasks, what needed to be done, when they were completed, and who will be working on each task.

Architecture:
 - Client-server
    - Users register on personal machine
    - User machine communicates with Postgres database
    - All operations performed follow this format Client machine wants to do something -> communication with database occurs -> database gives the return (success or error report).

Classes:
 * Instructor 
    - Represents an instructor.
    - Holds data to identify that individual instructor. 
    - Instructor objects are linked to Course objects and, indirectly, to Students.
 * Student
    - Represents a student, holding data to identify that individual student.
    - Student objects are linked to Course and, indirectly, Instructor objects.
 * Course
    - The object used for Courses. 
    - Stores data to identify it and to link it to both Student and Instructor objects.
 * TimeEntry
    - Represents an individual time entry, a single punch in/out.
    - Multiple of these objects make up a full timecard.
 * Timecard
    - Represents an instance of a timecard. 
    - Holds multiple time entries. 
    - Used to keep track of student whereabouts.

What the pages do:
 * Login/Register
    - Creates a new EF Core user and assigns whether the user is a Student or an Instructor. When the user is created, so is a Student/Instructor object respectively. 
 * Home
    - This is the general view for both Student and Instructor objects. The View is different depending on the user category.
    - Students see the timeclock. 
    - Instructors see a create Course page.
 * View Timecards (Instructor)
    - On the Instructor side, there is an extra page where the Instructor may select a Course of which that he/she is a member to then view all the Timecards of all Students who are a member of that Course.
    - The Timecards for the Students are pulled from the database when the Course is selected.
 * Messages
    - The messages page is on the Student side. Students are able to select a Course, and then a particular Student within that Course, to send messages with that Student.
    - Messages are pulled from the database when the Student is selected to show all previous messages sent.
 * Timesheet
    - The Timesheet page is on the Student side. It shows the timeclock just like the Home page, but it also shows the Student all the Timecards he/she has created.


Database:
 - The database is made in Postgres.
 - There is a table for each of the objects aside from Timesheet.
 - ERD linked below
 - ![ERD for Database](/wwwroot/erd.png)