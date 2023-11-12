namespace ProjectX.Exceptions;

public class UserRoleAlreadyExistsException : Exception
{
    public UserRoleAlreadyExistsException(string message) : base(message)
    {
    }
}