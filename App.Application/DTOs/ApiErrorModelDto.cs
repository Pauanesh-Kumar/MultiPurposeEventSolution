namespace App.Application.DTOs;

public class ApiErrorModelDto
{
    public int StatusCode { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Detail { get; set; }
    public string? StackTrace { get; set; }
    public string? Instance { get; set; }
}