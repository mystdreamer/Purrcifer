
using Purrcifer.Data.Defaults;

/// <summary>
/// Interface for passing entity damage. 
/// </summary>
public interface IEntityInterface
{
    float Health { get; set; }

    bool IsAlive { get; }
}