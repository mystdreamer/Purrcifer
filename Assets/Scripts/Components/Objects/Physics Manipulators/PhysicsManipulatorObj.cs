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
public static class PhysicsManipulatorObj
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
