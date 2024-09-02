using UnityEngine;

public abstract class AbsConsumable : MonoBehaviour
{
    internal bool collected;

    private void OnCollisionEnter(Collision collision)
    {
        if (collected)
            return;
        else if (collision.gameObject.tag == "Player")
            ConsumableEffect();
    }

    internal abstract void ConsumableEffect();
}
