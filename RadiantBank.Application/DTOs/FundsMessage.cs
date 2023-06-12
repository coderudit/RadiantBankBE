namespace RadiantBank.Application.DTOs;

public record FundsMessage
{
    public bool Status { get; set; }
    public string Message { get; set; } = default!;

    public FundsMessage(bool status, string message)
    {
        Status = status;
        Message = message;
    }
}