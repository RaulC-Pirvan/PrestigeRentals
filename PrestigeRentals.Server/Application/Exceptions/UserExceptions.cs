using System;

namespace PrestigeRentals.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when an attempt is made to register a user with an email that already exists.
    /// </summary>
    public class EmailAlreadyExistsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAlreadyExistsException"/> class.
        /// </summary>
        public EmailAlreadyExistsException() : base($"A user with this email address already exists.") { }
    }

    /// <summary>
    /// Exception thrown when the provided photo format is invalid.
    /// </summary>
    public class InvalidPhotoFormatException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPhotoFormatException"/> class.
        /// </summary>
        public InvalidPhotoFormatException() : base($"Invalid photo format.") { }
    }

    /// <summary>
    /// Exception thrown when a password is invalid (e.g., too short, missing special characters, etc.).
    /// </summary>
    public class InvalidPasswordException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPasswordException"/> class.
        /// </summary>
        public InvalidPasswordException() : base($"Invalid password.") { }
    }

    /// <summary>
    /// Exception thrown when a user is not found in the system.
    /// </summary>
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
        /// </summary>
        public UserNotFoundException() : base($"User not found.") { }
    }

    /// <summary>
    /// Exception thrown when an attempt is made to deactivate a user who is already deactivated.
    /// </summary>
    public class UserAlreadyDeactivatedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAlreadyDeactivatedException"/> class.
        /// </summary>
        public UserAlreadyDeactivatedException() : base($"User is already deactivated.") { }
    }

    /// <summary>
    /// Exception thrown when an attempt is made to activate a user who is already active.
    /// </summary>
    public class UserAlreadyActivatedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAlreadyActivatedException"/> class.
        /// </summary>
        public UserAlreadyActivatedException() : base($"User is already active.") { }
    }

    /// <summary>
    /// Exception thrown when an attempt is made to assign the admin role to a user who is already an admin.
    /// </summary>
    public class UserAlreadyAdminException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAlreadyAdminException"/> class.
        /// </summary>
        public UserAlreadyAdminException() : base($"User is already an admin.") { }
    }

    /// <summary>
    /// Exception thrown when an attempt is made to assign the user role to a user who is already a regular user.
    /// </summary>
    public class UserAlreadyUserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAlreadyUserException"/> class.
        /// </summary>
        public UserAlreadyUserException() : base($"User is already a user.") { }
    }
}
