using Microsoft.AspNetCore.Identity;

namespace Api.DTO
{
    public class CustomEmailErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = "Email is already in use."
            };
        }
        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = "Password mismatch"
            };
        }
        public override IdentityError InvalidUserName(string? userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = "Invalid username"
            };
        }
        public override IdentityError InvalidEmail(string? email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = "Invalid email"
            };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = "Duplicate username"
            };
        }
    }
}
