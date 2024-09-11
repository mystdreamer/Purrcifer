using UnityEngine;

namespace Purrcifer.Object.PhysicsManipulators
{
    public abstract class PhysicsFX : IRoomObjectEffect
    {
        private PlayerMovementSys _movementSys;

        internal PlayerMovementSys MovementSys
        {
            get
            {
                if (_movementSys == null)
                    _movementSys = GameManager.Instance.PlayerMovementSys;
                return _movementSys;
            }
        }

        public abstract void ApplyEffect();

        public Vector3 GetABDirection(Vector3 pointA, Vector3 pointB)
        {
            return pointA - pointB.normalized;
        }
    }

    [System.Serializable]
    public class PhysFXPushDirection : PhysicsFX
    {
        public Vector3 direction;
        public float force;

        public override void ApplyEffect()
        {
            if (MovementSys != null)
            {
                MovementSys.RigidBody.AddForce(direction.normalized * force);
            }
        }
    }

    [System.Serializable]
    public class PhysFXPushFromPoint : PhysicsFX
    {
        public Transform center;
        public float force;

        public override void ApplyEffect()
        {
            if(center == null)
            {
                Debug.Log("Center not set to object.");
                return;
            }

            if(MovementSys == null)
            {
                Debug.Log("MovementSys does not exist.");
                return;
            }

            Debug.Log("Applying force push.");
            //Generate the vector between the two points. 
            Vector3 vectorBetween =
                GetABDirection(GameManager.Instance.PlayerTransform.position, center.position);
            MovementSys.ApplyForce = vectorBetween * force;
        }
    }

    [System.Serializable]
    public class PhysFxPullTowardsPoint : PhysicsFX
    {
        public Transform center;
        public float force;

        public override void ApplyEffect()
        {
            if (MovementSys != null && center != null)
            {
                //Generate the vector between the two points. 
                Transform playerTransform = GameManager.Instance.PlayerTransform;
                Vector3 vectorBetween = center.position - playerTransform.position;
                MovementSys.ApplyForce = vectorBetween.normalized * force;
            }
        }
    }
}
