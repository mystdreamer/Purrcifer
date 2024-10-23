using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class SacDagger : MonoBehaviour {

    public float radius = 2F;
    public float currentRotation = 0f;
    public float speed = 0.2f;
    public float rotationStep = 10;
    public GameObject target;
    public int damage;

    private float Damage
    {
        get
        {
            return damage * GameManager.Instance.PlayerState.DamageMultiplier;
        }
    }

    private void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        currentRotation += rotationStep * speed;

        //Wrap rotation. 
        if (currentRotation > 360)
            currentRotation = 0;

        if (currentRotation < 0)
            currentRotation = 0;


        //Calculate the current rotation. 
        float angleRad = Mathf.Rad2Deg * currentRotation;
        Vector3 rotVec = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)).normalized * radius;
        
        //Apply position transform. 
        gameObject.transform.position = target.transform.position + rotVec;

        //Calculate the look at vector. 
        gameObject.transform.LookAt(target.transform.position);
    }

    public void OnCollisionEnter(Collision collision)
    {
        ResolveCollision(collision.gameObject);
    }

    public void OnCollisionStay(Collision collision)
    {
        ResolveCollision(collision.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        ResolveCollision(other.gameObject);
    }

    public void OnTriggerStay(Collider other)
    {
        ResolveCollision(other.gameObject);
    }

    private void ResolveCollision(GameObject collisionObject)
    {
        Enemy enemy = collisionObject.GetComponent<Enemy>();
        Boss boss = collisionObject.GetComponent<Boss>();

        if (enemy != null) enemy.CurrentHealth -= Damage;
        if (boss != null) boss.Health -= Damage;
    }

    //Deathblossom is a girls girl with a giant moist flower. 
}
