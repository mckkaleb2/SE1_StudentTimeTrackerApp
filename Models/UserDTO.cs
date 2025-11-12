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
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Prefix { get; set; }
        //public string Suffix { get; set; }
        public string Email { get; set; }


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


    }

}
