namespace App.Application.CustomExceptions;

public class UnAuthorizedException(string message) : Exception(message);