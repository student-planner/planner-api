namespace Planner.Contracts.Register;

public class RegisterCompleteDto
{
    /// <summary>
    /// Тикет
    /// </summary>
    public string TicketId { get; set; } = string.Empty;

    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Код подтверждения
    /// </summary>
    public string Code { get; set; } = string.Empty;
}
