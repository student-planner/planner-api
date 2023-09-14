using System.ComponentModel.DataAnnotations;

namespace Planner.Contracts.Register;

public class RegisterStartDto
{
    /// <summary>
    /// Электронная почта
    /// </summary>
    [Required(ErrorMessage = "Не указан адрес электронной почты")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Информация об устройстве
    /// </summary>
    public string? DeviceDescription { get; set; } = string.Empty;
}
