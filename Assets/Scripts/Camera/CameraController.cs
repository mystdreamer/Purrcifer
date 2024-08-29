using System.Collections;
using UnityEngine;

public enum CameraBehaviour
{
    TARGET_STEP, 
    LOCK,
    TRANSVERSE
}

public class CameraController : MonoBehaviour
{
    public Camera cameraTarget;
    public GameObject playerObj;
    public float moveWidth;
    public float moveHeight;
    public CameraBehaviour currentBehaviour = CameraBehaviour.TARGET_STEP;
    public bool isStepping = false; 

    private Vector3 PlayerPosition =>
        (playerObj != null) ? playerObj.transform.position : transform.position;
    private Vector3 PlayerPositionX =>
        (playerObj != null) ? new Vector3(playerObj.transform.position.x, 0, 0) : new Vector3(transform.position.x, 0, 0);
    private Vector3 PlayerPositionZ =>
        (playerObj != null) ? new Vector3(0, 0, playerObj.transform.position.z) : new Vector3(0, 0, transform.position.z);

    private float GetMinX => -moveWidth / 2;
    private float GetMaxX => moveWidth / 2;
    private float GetMinZ => -moveHeight / 2;
    private float GetMaxZ => moveHeight / 2;

    private bool InsideX => PlayerPosition.x > transform.position.x + GetMinX && PlayerPositionX.x < transform.position.x + GetMaxX;
    private bool InsideZ => PlayerPosition.z > transform.position.z + GetMinZ && PlayerPosition.z < transform.position.z + GetMaxZ;

    public float distX;
    public float distZ;
    public Vector3 diffVecX;
    public Vector3 diffVecZ;

    public void Update()
    {
        switch (currentBehaviour)
        {
            case CameraBehaviour.TARGET_STEP:
                if(!isStepping)
                    CameraStepper();
                break;
            case CameraBehaviour.LOCK:
                break;
            case CameraBehaviour.TRANSVERSE:
                break;
        }
    }

    private void CameraStepper()
    {
        //Cache player if not cached. 
        if (playerObj == null)
            playerObj = GameManager.Instance.playerCurrent;

        if (!InsideX)
        {
            //Get the direction vector. 
            diffVecX = PlayerPositionX - new Vector3(transform.position.x, 0, 0);
            distX = Vector3.Distance(new Vector3(transform.position.x, 0, 0), PlayerPositionX);
            diffVecZ.Normalize();

            //Determine axis on which the player has breached AABB.
            if (Mathf.Sign(diffVecX.x) * distX < moveWidth / 2)
                StartCoroutine(StepCamera(transform.position, new Vector3(-moveWidth, 0, 0)));
            else if (Mathf.Sign(diffVecX.x) * distX > moveWidth / 2)
                StartCoroutine(StepCamera(transform.position, new Vector3(moveWidth, 0, 0)));
        }

        if (!InsideZ)
        {
            //Get the direction vector. 
            diffVecZ = PlayerPositionZ - new Vector3(0, 0, transform.position.z);
            distZ = Vector3.Distance(new Vector3(0, 0, transform.position.z), PlayerPositionZ);
            diffVecX.Normalize();

            if (Mathf.Sign(diffVecZ.z) * distZ < moveHeight / 2)
                StartCoroutine(StepCamera(transform.position, new Vector3(0, 0, -moveHeight)));
            if (Mathf.Sign(diffVecZ.z) * distZ > moveHeight / 2)
                StartCoroutine(StepCamera(transform.position, new Vector3(0, 0, moveHeight)));
        }
    }

    private IEnumerator StepCamera(Vector3 initial, Vector3 stepVector)
    {
        isStepping = true;
        float currentdt = 0;
        Vector3 currentStep = Vector3.zero; 

        while(currentStep != initial + stepVector)
        {
            currentdt += Time.deltaTime;
            currentStep = Vector3.Lerp(initial, initial + stepVector, currentdt);
            transform.position = currentStep;
            yield return new WaitForEndOfFrame(); 
        }

        isStepping = false; 
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawSphere(transform.position + new Vector3(GetMinX, 0, 0), 0.5f);
        Gizmos.DrawSphere(transform.position + new Vector3(GetMaxX, 0, 0), 0.5f);
        Gizmos.DrawSphere(transform.position + new Vector3(0, 0, GetMinZ), 0.5f);
        Gizmos.DrawSphere(transform.position + new Vector3(0, 0, GetMaxZ), 0.5f);
    }
}
