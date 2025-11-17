using StudentTimeTrackerApp.Components.Account.Pages.Manage;
using StudentTimeTrackerApp.Entities;
using StudentTimeTrackerApp.Models;
using StudentTimeTrackerApp.Models.Entities;
using StudentTimeTrackerApp.Services;

//using StudentTimeTrackerApp.Services.StudentService as studentManager;

namespace StudentTimeTrackerApp.Models
{
    //public class UserDTO
    //{
    //}

    /// <summary>
    /// Used by Messaging, this class is a Data Transfer Object (DTO) for User information.
    /// This way both Instructors and Students can be represented in the same way, and protects sensitive columns.
    /// </summary>
    public class UserDTO //dto_User
    {
        // TODO: Add Prefix and Suffix back in later
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        //public string Prefix { get; set; }
        //public string Suffix { get; set; }
        public string? Email { get; set; }


        public UserDTO(string userId,
                        string firstName,
                        string lastName,
                        //string prefix,
                        //string suffix,
                        string email
                        )
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            //Prefix = prefix;
            //Suffix = suffix;
            Email = email;
        }


        private readonly StudentService _studentService;
        private readonly InstructorService _instructorService;

        /// <summary>
        /// Constructor with dependency injection of services. To be followed up with MakeDTOFromUserId()
        /// </summary>
        /// <param name="studentService"></param>
        /// <param name="instructorService"></param>
        public UserDTO(StudentService studentService, InstructorService instructorService)
        {
            // dependency injection of services
            _studentService = studentService;
            _instructorService = instructorService;
        }

        public UserDTO MakeDTOFromUserId(string userId)
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



        // create a DTO instance, just given the userId
        public UserDTO(string userId, StudentService studentService, InstructorService instructorService)
        {
            // dependency injection of services
            _studentService = studentService;
            _instructorService = instructorService;

            // try to get info using student service
            Student? stu = _studentService.GetStudentByUserId(userId);
            Instructor? ins = null;
            if (stu == null)
            {
                ins = _instructorService.GetInstructorByUserId(userId);
            }


            if (!(stu == null))
            {
                UserId = userId;
                FirstName = stu.FirstName;
                LastName = stu.LastName;

                //Email = email;
            }
            else if (!(ins == null))
            {
                UserId = userId;
                FirstName = ins.FirstName;
                LastName = ins.LastName;

                //Email = email;
            }
            else
            {
                throw new NotImplementedException("User not found from ID");
            }
            // if that fails, try to get info using instructor service
        }


        // method to get full name
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

    }

}
