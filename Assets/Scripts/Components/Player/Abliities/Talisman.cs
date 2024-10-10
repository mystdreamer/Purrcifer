using UnityEngine;

public class Talisman : MonoBehaviour
{

    public GameObject talismanPrefab;
    public float reuseTime = 0.5F;
    public float currentReuseTime = 0; 

    void Update()
    {
        if(reuseTime > 0)
            currentReuseTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && currentReuseTime <= 0) {
            if (GameManager.Instance.PlayerState.Talismans > 0)
            {
                currentReuseTime = reuseTime;
                GameManager.Instance.PlayerState.Talismans--;
                GameObject talisman = GameObject.Instantiate(talismanPrefab);
                talisman.transform.position = gameObject.transform.position;
            }
        }
    }
}
