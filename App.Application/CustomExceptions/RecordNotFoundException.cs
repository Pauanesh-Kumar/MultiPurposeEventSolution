namespace App.Application.CustomExceptions
{
    public class RecordNotFoundException(string message) : Exception(message)
    {
    }
}
