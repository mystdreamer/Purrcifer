
using Purrcifer.Data.Defaults;
using Purrcifer.Entity.HotsDots;

/// <summary>
/// Interface for passing entity damage. 
/// </summary>
public interface IEntityInterface
{
    float Health { get; set; }

    bool IsAlive { get; }

    HealOverTime SetHot { set; }

    DamageOverTime SetDot {  set; }
}