using Purrcifer.Data.Defaults;
using Purrcifer.Object.PhysicsManipulators;
using UnityEngine;

#region Abstract Base Classes. 
public abstract class PhysicsManipulator : MonoBehaviour
{
    internal bool _manipulatorActive = false;

    public bool Enabled
    {
        get => _manipulatorActive;
        set => _manipulatorActive = value;
    }
}

public abstract class PhysicsManipulatorCD : PhysicsManipulator
{

    public abstract PhysicsFX Effect { get; }

    public abstract Vector3 Direction { get; set; }

    public abstract float Force { get; set; }

    public abstract Vector3 ManipulatorPosition { get; set; }

    private void Update()
    {
        if (_manipulatorActive) Effect.ApplyEffect();
    }
}

public abstract class PhysicsManipulatorPoint : PhysicsManipulator
{

    public abstract PhysicsFX Effect { get; }

    public abstract float Force { get; set; }

    public abstract Transform Center { get; set; }

    private void Update()
    {
        if (_manipulatorActive) Effect.ApplyEffect();
    }
}

public abstract class PhysicsManipulatorCDWave : PhysicsManipulatorCD
{
    public ObjectEventTicker eventTicker;

    public float TickRate
    {
        get => eventTicker._tickRate;
        set => eventTicker._tickRate = value;
    }

    private void Update()
    {
        if (_manipulatorActive && eventTicker.TickComplete)
        {
            Effect.ApplyEffect();
            eventTicker.TickComplete = false;
        }
    }
}

public abstract class PhysicsManipulatorPointWave : PhysicsManipulatorPoint
{
    public ObjectEventTicker eventTicker;

    public float TickRate { 
        get => eventTicker._tickRate; 
        set => eventTicker._tickRate = value; 
    }

    private void Update()
    {
        if (_manipulatorActive && eventTicker.TickComplete)
        {
            Effect.ApplyEffect();
            eventTicker.TickComplete = false;
        }
    }
}
#endregion

public class PhysicsManipulator_PushCD : PhysicsManipulatorCD
{
    public PhysFXPushDirection pushDirection;

    public override Vector3 Direction
    {
        get => pushDirection.direction;
        set => pushDirection.direction = value.normalized;
    }

    public override float Force
    {
        get => pushDirection.force;
        set => pushDirection.force = value;
    }

    public override Vector3 ManipulatorPosition
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    public override PhysicsFX Effect => (PhysicsFX)pushDirection;

    public static GameObject BuildObject()
    {
        GameObject temp = new GameObject("Physics Manipulator - Pull Towards.");
        temp.AddComponent<PhysicsManipulator_PushCD>();
        return temp;
    }
}

public class PhysicsManipulator_PushCDWave : PhysicsManipulatorCDWave
{
    public PhysFXPushDirection pushDirection;

    public override PhysicsFX Effect => throw new System.NotImplementedException();

    public override Vector3 Direction
    {
        get => pushDirection.direction;
        set => pushDirection.direction = value.normalized;
    }

    public override float Force
    {
        get => pushDirection.force;
        set => pushDirection.force = value;
    }

    public override Vector3 ManipulatorPosition
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    private void Update()
    {
        if (_manipulatorActive)
            pushDirection.ApplyEffect();
    }

    public static GameObject BuildObject()
    {
        GameObject temp = new GameObject("Physics Manipulator - Pull Towards.");
        PhysicsManipulator_PushCDWave tempPCDW = temp.AddComponent<PhysicsManipulator_PushCDWave>();
        tempPCDW.eventTicker = temp.AddComponent<ObjectEventTicker>();
        return temp;
    }
}

public class PhysicsManipulator_PushFromPoint : PhysicsManipulatorPoint
{
    public PhysFXPushFromPoint pushDirection;

    public override PhysicsFX Effect => pushDirection;

    public override float Force
    {
        get => pushDirection.force;
        set => pushDirection.force = value;
    }

    public override Transform Center
    {
        get => pushDirection.center;
        set
        {
            gameObject.transform.position = value.position;
            pushDirection.center = value;
        }
    }

    public static GameObject BuildObject()
    {
        GameObject temp = new GameObject("Physics Manipulator - Pull Towards.");
        PhysicsManipulator_PushFromPoint tempPFP = temp.AddComponent<PhysicsManipulator_PushFromPoint>();
        tempPFP.Center = temp.transform;
        return temp;
    }
}

public class PhysicsManipulator_PullTowardsPoint : PhysicsManipulatorPoint
{
    public PhysFxPullTowardsPoint pullTowards;

    public override PhysicsFX Effect => pullTowards;

    public override float Force
    {
        get => pullTowards.force;
        set => pullTowards.force = value;
    }

    public override Transform Center
    {
        get => pullTowards.center;
        set
        {
            gameObject.transform.position = value.position;
            pullTowards.center = value;
        }
    }

    public static GameObject BuildObject()
    {
        GameObject temp = new GameObject("Physics Manipulator - Pull Towards.");
        PhysicsManipulatorPoint point = temp.AddComponent<PhysicsManipulator_PullTowardsPoint>();
        point.Center = point.transform;
        point.Force = 0;
        return temp;
    }
}

public class PhysicsManipulator_PushTowardsPointWave : PhysicsManipulatorPointWave
{
    public PhysFXPushFromPoint _pushPoint;

    public override PhysicsFX Effect => _pushPoint;

    public override float Force
    {
        get => _pushPoint.force;
        set => _pushPoint.force = value;
    }

    public override Transform Center
    {
        get => _pushPoint.center;
        set
        {
            gameObject.transform.position = value.position;
            _pushPoint.center = value;
        }
    }

    public static GameObject BuildObject()
    {
        GameObject temp = new GameObject("Physics Manipulator - Pull Towards.");
        temp.AddComponent<PhysicsManipulator_PushTowardsPointWave>();
        return temp;
    }
}

public class PhysicsManipulator_PullTowardsPointWave : PhysicsManipulatorPointWave
{
    public PhysFxPullTowardsPoint pullTowards;

    public override PhysicsFX Effect => pullTowards;

    public override float Force { 
        get => pullTowards.force; 
        set => pullTowards.force = value; 
    }

    public override Transform Center {
        get => pullTowards.center;
        set
        {
            gameObject.transform.position = value.position;
            pullTowards.center = value;
        }
    }

    public static GameObject BuildObject()
    {
        GameObject temp = new GameObject("Physics Manipulator - Pull Towards.");
        temp.AddComponent<PhysicsManipulator_PullTowardsPointWave>();
        return temp;
    }
}