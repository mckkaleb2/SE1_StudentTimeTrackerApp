using Microsoft.EntityFrameworkCore;
using StudentTimeTrackerApp.Data;
using StudentTimeTrackerApp.Entities;
using StudentTimeTrackerApp.Models.Entities;
using StudentTimeTrackerApp.Models;

namespace StudentTimeTrackerApp.Services
{

    //NOTE: Retrieval methods may need to be async depending on how they are used in the future.

    /// <summary>
    /// The purpose of this service is to handle user and message operations for the message page
    /// </summary>
    public class MessagePageService
    {
        private readonly ApplicationDbContext _context; // Add a field for the context
        private readonly StudentService _studentService;
        private readonly InstructorService _instructorService;


        public MessagePageService(ApplicationDbContext context,
                                  InstructorService instructorService,
                                  StudentService studentService) // Inject the context via constructor
        {
            _context = context;
            _instructorService = instructorService;
            _studentService = studentService;
        }

        /// <summary>
        /// Get all of the courses associated with a user, either as an instructor or as a student
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ICollection<Course>? GetCoursesByUserId(string userId)
        {
            // get the courses that are taught by the user, if any
            var taughtCourses = _context.Courses
                .Where(c => c.Instructors.Any(i => i.UserId == userId))
                .ToList();

            // get the courses that are enrolled in by the user, if any
            var enrolledCourses = _context.Courses
                .Where(c => c.Students.Any(s => s.UserID == userId))
                .ToList();

            // create a new list that combines both lists without duplicates
            var allCourses = taughtCourses.Union(enrolledCourses).ToList();

            return allCourses;
        }

        /// <summary>
        /// Asynchronously retrieves a collection of courses associated with a specific user.
        /// </summary>
        /// <remarks>This method combines courses where the user is an instructor and courses where the
        /// user is a student,  ensuring no duplicates in the resulting collection.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of courses  that
        /// the user is either teaching or enrolled in. If the user is not associated with any courses,  the collection
        /// will be empty.</returns>
        public async Task<ICollection<Course>> GetCoursesByUserIdAsync(string userId)
        {
            List<Course> allCourses = new();
            try
            {
                // get the courses that are taught by the user, if any
                //var taughtCourses = await _context.Courses
                //    .Where(c => c.Instructors.Any(i => i.UserId == userId))
                //    .ToListAsync();

                List<Course> taughtCourses = _context.Instructors
                    .Where(i => i.UserId == userId)
                    .SelectMany(i => i.Courses).ToList();
                //.ToListAsync();

                // get the courses that are enrolled in by the user, if any
                List<Course> enrolledCourses = _context.Students
                    //.Where(c => c.Students.Any(s => s.UserID == userId))
                    //.ToListAsync();
                    .Where(s => s.UserID == userId)
                    .SelectMany(s => s.Courses)
                    .ToList();


                // create a new list that combines both lists without duplicates
                allCourses = taughtCourses.Union(enrolledCourses).ToList();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return allCourses;
        }



        /// <summary>
        /// Retrieves a collection of students enrolled in the specified course.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course whose students are to be retrieved.</param>
        /// <returns>A collection of <see cref="Student"/> objects representing the students enrolled in the course. Returns <see
        /// langword="null"/> if the course does not exist or has no students.</returns>
        public ICollection<UserDTO>? GetStudentDTOsByCourseId(int courseId)
        {
            var students = _context.Courses
                .Where(c => c.Id == courseId)
                .SelectMany(c => c.Students)
                .Include(s => s.User)
                .ToList();

            var studentDtos = students.Select(s => new UserDTO(
                s.UserID,
                s.FirstName,
                s.LastName,
                //s.Prefix != null ? s.Prefix.ToString() : string.Empty,
                //s.Suffix != null ? s.Suffix.ToString() : string.Empty,
                s.User.Email = ""
            )).ToList();
            return studentDtos;
        }

        /// <summary>
        /// Asynchronously retrieves a collection of students enrolled in the specified course.
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<ICollection<UserDTO>?> GetStudentDTOsByCourseIdAsync(int courseId)
        {
            ICollection<UserDTO>? studentDtos = await _context.Courses
                .Where(c => c.Id == courseId)
                .SelectMany(c => c.Students)
                .Include(s => s.User)
                    .Select(s => new UserDTO(
                        s.UserID,
                        s.FirstName,
                        s.LastName,
                        //s.Prefix != null ? s.Prefix.ToString() : string.Empty,
                        //s.Suffix != null ? s.Suffix.ToString() : string.Empty,
                        s.User.Email ?? ""
                    ))
                .ToListAsync();

            return studentDtos;
        }



        public ICollection<UserDTO>? GetInstructorDTOsByCourseId(int courseId)
        {
            var instructors = _context.Courses
                .Where(c => c.Id == courseId)
                .SelectMany(c => c.Instructors)
                .Include(i => i.User)
                .ToList();


            ICollection<UserDTO>? instructorDtos = instructors
                .Select(i => new UserDTO(
                                i.UserId,
                                i.FirstName,
                                i.LastName,
                                //i.Prefix != null ? i.Prefix.ToString() : string.Empty,
                                //i.Suffix != null ? i.Suffix.ToString() : string.Empty,
                                i.User.Email = ""
                ))
                .ToList();
            return instructorDtos;
        }

        /// <summary>
        /// async version of GetInstructorDTOsByCourseId
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<ICollection<UserDTO>?> GetInstructorDTOsByCourseIdAsync(int courseId)
        {
            ICollection<UserDTO>? instructorDtos = await _context.Courses
                .Where(c => c.Id == courseId)
                .SelectMany(c => c.Instructors)
                .Include(i => i.User)
                .Select(i => new UserDTO(
                                i.UserId,
                                i.FirstName,
                                i.LastName,
                                //i.Prefix != null ? i.Prefix.ToString() : string.Empty,
                                //i.Suffix != null ? i.Suffix.ToString() : string.Empty,
                                i.User.Email ?? ""
                ))
                .ToListAsync();

            return instructorDtos;
        }



        /// <summary>
        /// [TODO : make sure that instructors show up at the top of this list]
        /// Attain a list of all users (students and instructors) associated with a course
        /// 
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IList<UserDTO>? GetUsersByCourseId(int courseId)
        {
            var instructorDtos = GetInstructorDTOsByCourseId(courseId);

            var studentDtos = GetStudentDTOsByCourseId(courseId);

            if (instructorDtos == null || studentDtos == null)
            {
                Console.WriteLine("ONE OF THE FOLLOWING IS NULL:");
                Console.WriteLine(instructorDtos);
                Console.WriteLine(studentDtos);
            }
            var allUsers = instructorDtos.Union(studentDtos).ToList();

            // TODO: Sort the list so that instructors are at the top
            // TODO: Cover what happens if one of these is null
            return allUsers;
        }


        public UserDTO GetDTOFromUserId(string userId)
        {
            Student? stu = _studentService.GetStudentByUserId(userId);
            Instructor? ins = null;
            // if that fails, try to get info using instructor service
            if (stu == null)
            {
                ins = _instructorService.GetInstructorByUserId(userId);
            }
            UserDTO userOut = new UserDTO(_studentService, _instructorService);

            if (!(stu == null))
            {
                userOut.UserId = userId;
                userOut.FirstName = stu.FirstName;
                userOut.LastName = stu.LastName;

                //Email = email;
            }
            else if (!(ins == null))
            {
                userOut.UserId = userId;
                userOut.FirstName = ins.FirstName;
                userOut.LastName = ins.LastName;

                //Email = email;
            }
            else
            {
                throw new NotImplementedException("User not found from ID");
            }

            return userOut;

        }

        public async Task<UserDTO> GetDTOFromUserIdAsync(string userId)
        {
            //var waiter = Task.Run(() => _studentService.GetStudentByUserId(userId));
            //Student? stu = await waiter;
            Student? stu = await _studentService.GetStudentByUserIdAsync(userId);
            Instructor? ins = null;
            // if that fails, try to get info using instructor service
            if (stu == null)
            {
                ins = await _instructorService.GetInstructorByUserIdAsync(userId);
            }
            UserDTO userOut = new UserDTO(_studentService, _instructorService);

            if (!(stu == null))
            {
                userOut.UserId = userId;
                userOut.FirstName = stu.FirstName;
                userOut.LastName = stu.LastName;

                //Email = email;
            }
            else if (!(ins == null))
            {
                userOut.UserId = userId;
                userOut.FirstName = ins.FirstName;
                userOut.LastName = ins.LastName;

                //Email = email;
            }
            else
            {
                throw new NotImplementedException("User not found from ID");
            }

            return userOut;

        }



        public string? GetFullNameFromUserId(string userId)
        {
            UserDTO data = GetDTOFromUserId(userId);
            string output = data.GetFullName();
            return output;
        }

        public async Task<string?> GetFullNameFromUserIdAsync(string userId)
        {
            //var data = GetDTOFromUserIdAsync(userId);
            var data = await GetDTOFromUserIdAsync(userId);

            //await Task.WhenAll(data);
            
            //string output = data.Result.GetFullName();
            string output = data.GetFullName();
            return output;
        }

        public List<string?> GetFullNameFromUserIdList(List<string> userIds)
        {
            //List<UserDTO> tempusers = new();
            List<string?> tempnames = new();
            foreach (var user in userIds)
            {
                var tempDto = GetFullNameFromUserId(user);
                tempnames.Add(tempDto);
            }
            return tempnames;
        }


        public async Task<List<string?>> GetFullNameFromUserIdListAsync(List<string> userIds)
        {
            //List<UserDTO> tempusers = new();
            List<string?> tempnames = new();
            foreach(var user in userIds)
            {
                //var tempDto = GetFullNameFromUserIdAsync(user);
                var tempDto = await GetFullNameFromUserIdAsync(user);
                //await Task.WhenAll(tempDto);
                //tempnames.Add(tempDto.Result);
                tempnames.Add(tempDto);
            }
            return tempnames;
        }

        /// <summary>
        /// async version of GetUsersByCourseId
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<IList<UserDTO>?> GetUsersByCourseIdAsync(int courseId)
        {
            var instructorDtos = await GetInstructorDTOsByCourseIdAsync(courseId);
            //await Task.WhenAll(instructorDtos);

            var studentDtos = await GetStudentDTOsByCourseIdAsync(courseId);
            //await Task.WhenAll(studentDtos);

            //if (instructorDtos.Result == null || studentDtos.Result == null)
            if (instructorDtos == null || studentDtos == null)
            {
                Console.WriteLine("ONE OF THE FOLLOWING IS NULL:");
                Console.WriteLine(instructorDtos);
                Console.WriteLine(studentDtos);
            }

            //var allUsers = instructorDtos.Result.Union(studentDtos.Result).ToList(); 
            var allUsers = instructorDtos.Union(studentDtos).ToList(); 
            //var result = await GetUnion(instructorDtos, studentDtos);


            // TODO: Sort the list so that instructors are at the top
            // TODO: Cover what happens if one of these is null
            return allUsers;
        }



        //-----------------

        /// <summary>
        /// Retrieves messages exchanged between two users within a specific course. If the second User is null, or string.Empty, then it should be from a group chat
        /// </summary>
        /// <param name="userId1"></param>
        /// <param name="userId2"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public ICollection<Message> GetMessagesBetweenUsersInCourse(string userId1, string? userId2, int courseId)
        {
            if (string.IsNullOrEmpty(userId2) || string.IsNullOrWhiteSpace(userId2))
            {
                userId2 = string.Empty;
            }


            var messages = _context.Messages
                .Where(m => m.CourseId == courseId &&
                            ((m.Sender == userId1 && m.Recipient == userId2) ||
                             (m.Sender == userId2 && m.Recipient == userId1)))
                .OrderBy(m => m.Timestamp)
                .ToList();
            return messages;
        }

        /// <summary>
        /// async version of GetMessagesBetweenUsersInCourse
        /// </summary>
        /// <param name="userId1"></param>
        /// <param name="userId2"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<ICollection<Message>> GetMessagesBetweenUsersInCourseAsync(string userId1, string? userId2, int courseId)
        {
            if (string.IsNullOrEmpty(userId2) || string.IsNullOrWhiteSpace(userId2))
            {
                userId2 = string.Empty;
            }


            var messages =  await _context.Messages
                //.ToAsyncEnumerable()
                .Where(m => m.CourseId == courseId &&
                            ((m.Sender == userId1 && m.Recipient == userId2) ||
                             (m.Sender == userId2 && m.Recipient == userId1)))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
            return messages;
        }



        // NOTE: Skipping update functions for messages, as they are generally not editable once sent.
        public void AddMessageToDatabase(string senderId, string? recipientId, int courseId, string body)
        {

            var message = new Message
            {
                Sender = senderId,
                Recipient = recipientId ?? string.Empty,
                CourseId = courseId,
                Body = body,
                Timestamp = DateTime.UtcNow
            };
            _context.Messages.Add(message);
            _context.SaveChanges();
        }

        /// <summary>
        /// async version of AddMessageToDatabase
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="recipientId"></param>
        /// <param name="courseId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task AddMessageToDatabaseAsync(string senderId, string? recipientId, int courseId, string body)
        {

            var message = new Message
            {
                Sender = senderId,
                Recipient = recipientId ?? string.Empty,
                CourseId = courseId,
                Body = body,
                Timestamp = DateTime.UtcNow
            };
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }



        public void DeleteMessage(int messageId)
        {
            var message = _context.Messages.FirstOrDefault(m => m.Id == messageId);
            if (message != null)
            {
                _context.Messages.Remove(message);
                _context.SaveChanges();
            }
        }


        /// <summary>
        /// async version of DeleteMessage
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task DeleteMessageAsync(int messageId)
        {
            var message = _context.Messages.FirstOrDefault(m => m.Id == messageId);
            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }

        // Get List of Enrolled courses for Student


        // Get list of users associated with a course, instructors first, then students


        // Get message history between two users, also using the CourseID


        // to message, you need 2x userIds (unless if sending to a group chat), and a CourseId, as well as the body


    }
}
