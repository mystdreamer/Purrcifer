using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class OrbitalWeapons : MonoBehaviour
{
    public float radius = 2F;
    public float currentRotation = 0f;
    public float speed = 0.0025f;
    public float rotationAmountPerTick = 10;
    public GameObject targetObject;

    internal void UpdateRotation()
    {
        //Wrap rotation. 
        currentRotation = WrapRotation(currentRotation + rotationAmountPerTick * speed);

        //Calculate the current rotation. 
        float angleRad = Mathf.Rad2Deg * currentRotation;
        Vector3 rotVec = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)).normalized * radius;

        //Apply position transform. 
        gameObject.transform.position = targetObject.transform.position + rotVec;

        //Calculate the look at vector. 
        gameObject.transform.LookAt(targetObject.transform.position);
    }

    private float WrapRotation(float value)
    {
        if (currentRotation > 360)
            return 0;

        if (currentRotation < 0)
            return 360;

        return value;
    }

    public void OnCollisionEnter(Collision collision) => ApplyAttack(collision.gameObject);
    public void OnCollisionStay(Collision collision) => ApplyAttack(collision.gameObject);
    public void OnTriggerEnter(Collider other) => ApplyAttack(other.gameObject);
    public void OnTriggerStay(Collider other) => ApplyAttack(other.gameObject);

    public abstract void ApplyAttack(GameObject obj);

}
