namespace App.Application.CustomExceptions
{
    public class InvalidCredentialsException(string message) : Exception(message);
}
