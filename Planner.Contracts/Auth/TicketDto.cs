﻿namespace Planner.Contracts.Auth;

/// <summary>
/// Тикет для авторизации
/// </summary>
public class TicketDto
{
    /// <summary>
    /// Идентификатор тикета
    /// </summary>
    public string TicketId { get; set; } = string.Empty;
}