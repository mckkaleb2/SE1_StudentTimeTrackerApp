# Student Time Tracker App
This app allows students to log their time entries with location logging at clock-in.
Instructors create courses, which students may join, and view the timecards of students
enrolled in their courses. 
The app also includes a messaging function that includes chats between individuals within the same course and course group chats.

---


Link to live demo (available until 12/01/2025 after class time): [Live Demo](https://se1-studenttimetrackerapp.fly.dev)

---
### Group Members
- Quinn Daniels
- Ronan Eanes
- Kaleb McKinney
- Jacob Robidas

### Installation Guide

You can find our latest user releases, at https://github.com/mckkaleb2/SE1_StudentTimeTrackerApp/releases/ . <br/> Upon installing the version for your Operating System, unzip the folder, and click the shortcut to start the backend of the app. From there, simply navigate to `http://localhost:5000` within a web browser.


### Docker Deployment Guide

After cloning or downloading the source code, first create an `appsettings.json` file based on the `appsettings.template.txt` file, and replace the `"DefaultConnection"` with the connection string for your database. In our version, we used a PostGre database hosted through Neon. After the initial step, use the included Dockerfile to 
build a Docker image to the deploy in the location of your choice:  <br/>
`
docker build -t <image_name>:<tag_version>
` <br/>
where `<image_name>` and `<tag_version>` are the name you want to give the image and the 
version you wish to assign to it. They can also be omitted if you don't want to specify:  <br/>
`
docker build
` <br/>
Follow the documentation of your preferred deployment target to use the docker image. 
To run the image:  <br/>
`
docker run <image_name>
`
