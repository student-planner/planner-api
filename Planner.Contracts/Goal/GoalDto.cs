﻿using Planner.Models;

namespace Planner.Contracts.Goal;

/// <summary>
/// Модель данных для отображения задач
/// </summary>
public class GoalDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Крайний срок выполнения
    /// </summary>
    public DateTime? Deadline { get; set; }

    /// <summary>
    /// Трудоёмкость в секундах
    /// </summary>
    public double? Labor { get; set; }

    /// <summary>
    /// Приоритет
    /// </summary>
    public GoalPriority? Priority { get; set; }
    
    /// <summary>
    /// Статус
    /// </summary>
    public GoalStatus Status { get; set; }
}
