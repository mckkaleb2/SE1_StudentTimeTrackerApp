# Student Time Tracker App
This app allows students to log their time entries with location logging at clock-in.
Instructors create courses, which students may join, and view the timecards of students
enrolled in thier courses. 
The app also includes a messaging function that includes chats between indivuals within the same course and course group chats.

---
### Group Members
- Quinn Daniels
- Ronan Eanes
- Kaleb McKinney
- Jacob Robidas

### Deployment Guide

After cloning or downloading the source code, use the included Dockerfile to 
build a Docker image to the deploy in the location of your choice:
`
docker build -t <image_name>:<tag_verion>
`
where <image_name> and <tag_version> are the name you want to give the image and the 
version you wish to assign to it. Thay can also be ommitted if you don't want to specify:
`
docker build
`
Follow the documentation of your prefered deployment target to use the docker image. 