using Purrcifer.Object.PhysicsManipulators;
using Purrcifer.PhysManipulation.BaseClass;
using UnityEngine;

/// <summary>
/// Used to specify a type of physics manipulator. 
/// </summary>
public enum PhysicsManipulatorType
{
    /// <summary>
    /// Generate a Physics manipulator that uses a set direction to push the player. 
    /// </summary>
    PUSH_CD,
    /// <summary>
    /// Generate a Physics manipulator that uses a set direction in waves to push the player. 
    /// </summary>
    PUSH_CD_WAVE,
    /// <summary>
    /// Generate a Physics manipulator that uses a point to push the player. 
    /// </summary>
    PUSH_FROM_POINT,
    /// <summary>
    /// Generate a Physics manipulator that uses a point to pull the player. 
    /// </summary>
    PULL_TO_POINT,
    /// <summary>
    /// Generate a Physics manipulator that uses a point to push the player in waves. 
    /// </summary>
    PUSH_TO_POINT_WAVE,
    /// <summary>
    /// Generate a Physics manipulator that uses a point to pull the player in waves. 
    /// </summary>
    PULL_TO_POINT_WAVE
}

/// <summary>
/// Static class used to generate physics manipulators. 
/// </summary>
public static class PhysicsManipulatorFactory
{
    /// <summary>
    /// Generates and returns an ObjectPhysicsManipulator
    /// </summary>
    /// <param name="type"> The type to generate. </param>
    public static GameObject GetManipulator(PhysicsManipulatorType type)
    {
        switch (type)
        {
            case PhysicsManipulatorType.PUSH_CD:
                return PhysicsManipulator_PushCD.BuildObject();
            case PhysicsManipulatorType.PUSH_CD_WAVE:
                return PhysicsManipulator_PushCDWave.BuildObject();
            case PhysicsManipulatorType.PUSH_FROM_POINT:
                return PhysicsManipulator_PushFromPoint.BuildObject();
            case PhysicsManipulatorType.PULL_TO_POINT:
                return PhysicsManipulator_PullTowardsPoint.BuildObject();
            case PhysicsManipulatorType.PUSH_TO_POINT_WAVE:
                return PhysicsManipulator_PushTowardsPointWave.BuildObject();
            case PhysicsManipulatorType.PULL_TO_POINT_WAVE:
                return PhysicsManipulator_PullTowardsPointWave.BuildObject();
        }
        return null;
    }
}

#region Abstract Base Classes. 

namespace Purrcifer.PhysManipulation.BaseClass
{
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

        public abstract ObjectPhysicsFX Effect { get; }

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

        public abstract ObjectPhysicsFX Effect { get; }

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
}

#endregion

#region Manipulators. 
public class PhysicsManipulator_PushCD : PhysicsManipulatorCD
{
    public ObjectPhysFXPushDirection pushDirection;

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

    public override ObjectPhysicsFX Effect => (ObjectPhysicsFX)pushDirection;

    public static GameObject BuildObject()
    {
        GameObject temp = new GameObject("Physics Manipulator - Pull Towards.");
        temp.AddComponent<PhysicsManipulator_PushCD>();
        return temp;
    }
}

public class PhysicsManipulator_PushCDWave : PhysicsManipulatorCDWave
{
    public ObjectPhysFXPushDirection pushDirection;

    public override ObjectPhysicsFX Effect => throw new System.NotImplementedException();

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
    public ObjectPhysFXPushFromPoint pushDirection;

    public override ObjectPhysicsFX Effect => pushDirection;

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
    public ObjectPhysFxPullTowardsPoint pullTowards;

    public override ObjectPhysicsFX Effect => pullTowards;

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
    public ObjectPhysFXPushFromPoint _pushPoint;

    public override ObjectPhysicsFX Effect => _pushPoint;

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
    public ObjectPhysFxPullTowardsPoint pullTowards;

    public override ObjectPhysicsFX Effect => pullTowards;

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
        temp.AddComponent<PhysicsManipulator_PullTowardsPointWave>();
        return temp;
    }
}
#endregion