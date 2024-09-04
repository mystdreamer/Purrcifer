using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum ItemType
{
    STOPWATCH = 0,
    SWORD = 1,
}

public class SwordAttack : MonoBehaviour
{
    private const int RADIUS = 180;
    private const float LENGTH = 2;
    private const float OFFSET = 0.55F;
    private ItemType _type = ItemType.SWORD;
    [SerializeField] private GameObject _swordPrefab;
    public int detectionPoints = 10;
    public Vector3 Direction = Vector3.zero;
    public Vector2[] directions;

    public void Start()
    {
        RegisterDelegates();
    }

    #region Delegate Registration. 
    private void RegisterDelegates()
    {
        PlayerInputSys.Instance.GetKey(PInputIdentifier.ACTION_UP).DoAction += UpInput;
        PlayerInputSys.Instance.GetKey(PInputIdentifier.ACTION_DOWN).DoAction += DownInput;
        PlayerInputSys.Instance.GetKey(PInputIdentifier.ACTION_RIGHT).DoAction += RightInput;
        PlayerInputSys.Instance.GetKey(PInputIdentifier.ACTION_LEFT).DoAction += LeftInput;
        PlayerInputSys.Instance.GetButton(PInputIdentifier.ACTION_UP).DoAction += UpInput;
        PlayerInputSys.Instance.GetButton(PInputIdentifier.ACTION_DOWN).DoAction += DownInput;
        PlayerInputSys.Instance.GetButton(PInputIdentifier.ACTION_RIGHT).DoAction += RightInput;
        PlayerInputSys.Instance.GetButton(PInputIdentifier.ACTION_LEFT).DoAction += LeftInput;
    }

    private void DeRegisterDelegates()
    {
        PlayerInputSys.Instance.GetKey(PInputIdentifier.ACTION_UP).DoAction -= UpInput;
        PlayerInputSys.Instance.GetKey(PInputIdentifier.ACTION_DOWN).DoAction -= DownInput;
        PlayerInputSys.Instance.GetKey(PInputIdentifier.ACTION_RIGHT).DoAction -= RightInput;
        PlayerInputSys.Instance.GetKey(PInputIdentifier.ACTION_LEFT).DoAction -= LeftInput;
        PlayerInputSys.Instance.GetButton(PInputIdentifier.ACTION_UP).DoAction -= UpInput;
        PlayerInputSys.Instance.GetButton(PInputIdentifier.ACTION_DOWN).DoAction -= DownInput;
        PlayerInputSys.Instance.GetButton(PInputIdentifier.ACTION_RIGHT).DoAction -= RightInput;
        PlayerInputSys.Instance.GetButton(PInputIdentifier.ACTION_LEFT).DoAction -= LeftInput;
    }
    #endregion

    #region Input Functions. 
    private void ResolveVector(Vector3 input) => GetDirections(input);

    private void UpInput(bool result)
    {
        if (result)
            GetDirections(Vector3.up);
    }

    private void DownInput(bool result)
    {
        if (result)
            GetDirections(Vector3.down);
    }

    private void LeftInput(bool result)
    {
        if (result)
            GetDirections(Vector3.left);
    }

    private void RightInput(bool result)
    {
        if (result) Attack(Vector3.right);
    }
    #endregion

    public void Attack(Vector3 vector)
    {
        if (vector == Vector3.zero) return;

        Vector3[] directions = GetDirections(vector);
        Ray ray; 
        RaycastHit[] hits;
        IEntityInterface iEntity;
        float castingLength = (LENGTH - OFFSET);

        //Get hits for each vector direction. 
        for (int i = 0; i < directions.Length; i++)
        {
            ray = new Ray(transform.position + (directions[i].normalized * OFFSET), directions[i].normalized * castingLength);
            hits = Physics.RaycastAll(vector, directions[i]);
            
            //Check if hits are damageable interfaces. 
            for (int j = 0; j < hits.Length; j++)
            {
                if (Vector3.Distance(transform.position, vector) < LENGTH)
                {
                    if ((iEntity = gameObject.GetComponent<IEntityInterface>()) != null)
                    {
                        iEntity.Health -= 1; 
                    }
                }
            }
        }
    }

    private Vector3[] GetDirections(Vector3 direction)
    {
        Vector3 initalRay = Quaternion.Euler(0, -(RADIUS / 2), 0) * direction;
        Quaternion rotQuat;
        List<Vector3> _directions = new List<Vector3>();
        float step = RADIUS / detectionPoints;

        _directions.Add(initalRay);

        for (int i = 0; i <= detectionPoints; i++)
        {
            rotQuat = Quaternion.Euler(0, step * i, 0);
            _directions.Add(rotQuat * initalRay);
        }
        return _directions.ToArray();
    }

    public void OnDrawGizmos()
    {
        Vector3 dir = Vector3.right;
        Vector3[] dirList = GetDirections(dir);

        List<RaycastHit> hits = new List<RaycastHit>();

        foreach (Vector3 direction in dirList)
        {
            Ray ray = new Ray(transform.position + (direction.normalized * OFFSET), direction.normalized * (LENGTH - OFFSET));
            Gizmos.DrawRay(ray);
            hits.AddRange(Physics.RaycastAll(ray));
        }

        foreach (RaycastHit hit in hits)
        {
            Gizmos.DrawSphere(hit.point, 0.025F);
        }
    }
}
