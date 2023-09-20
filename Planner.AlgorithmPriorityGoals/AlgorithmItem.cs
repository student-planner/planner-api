namespace Planner.AlgorithmPriorityGoals;

public class AlgorithmItem
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Крайний срок выполнения
    /// </summary>
    public DateTime Deadline { get; set; }
    
    /// <summary>
    /// Трудоёмкость
    /// </summary>
    public double Labor { get; set; }
    
    /// <summary>
    /// Приоритет
    /// </summary>
    public int Priority { get; set; }
    
    /// <summary>
    /// Идентификатор самой приоритетной задачи
    /// </summary>
    public int DependsPriority { get; set; }
    
    /// <summary>
    /// Коллекция идентификаторов зависимых задач
    /// </summary>
    public ICollection<Guid> DependsIds { get; set; } = new List<Guid>();
    
    /// <summary>
    /// Преобразовать в строку
    /// </summary>
    public override string ToString() =>
        $"AlgorithmItem: Id: {Id}, Deadline: {Deadline}, Labor: {Labor}, Priority: {Priority}, DependsPriority: {DependsPriority}, DependsIds: {DependsIds}";
}