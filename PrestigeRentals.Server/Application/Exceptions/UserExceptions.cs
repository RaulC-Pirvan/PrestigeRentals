using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException() : base($"An user with this email address already exists.") { }
    }

    public class InvalidPhotoFormatException : Exception
    {
        public InvalidPhotoFormatException() : base($"Invalid photo format.") { }
    }

    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base($"Invalid password.") { }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base($"User not found.") { }
    }

    public class UserAlreadyDeactivatedException : Exception
    {
        public UserAlreadyDeactivatedException() : base($"User is already deactivated.") { }
    }

    public class UserAlreadyActivatedException : Exception
    {
        public UserAlreadyActivatedException() : base($"User is already active.") { }
    }

    public class UserAlreadyAdminException : Exception
    {
        public UserAlreadyAdminException() : base($"User is already an admin.") { }
    }

    public class UserAlreadyUserException : Exception
    {
        public UserAlreadyUserException() : base($"User is already an user.") { } 
    }
}
