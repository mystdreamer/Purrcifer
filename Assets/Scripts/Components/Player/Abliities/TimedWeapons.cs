using Purrcifer.Data.Player;
using System.Collections;
using UnityEngine;
using Input = UnityEngine.Input;

public abstract class TimedWeapons : MonoBehaviour
{
    private PlayerInputs _inputs;
    internal bool _canFire = true;

    public void OnEnable()
    {
        _inputs = GameManager.PlayerInputs;
    }

    public void Update()
    {
        if (Input.GetKeyDown(_inputs.ctlr_y) | Input.GetKeyDown(_inputs.key_a_up))
            Attack(Vector3.up);
        if (Input.GetKeyDown(_inputs.ctlr_a) | Input.GetKeyDown(_inputs.key_a_down))
            Attack(Vector3.down);
        if (Input.GetKeyDown(_inputs.ctlr_x) | Input.GetKeyDown(_inputs.key_a_left))
            Attack(Vector3.left);
        if (Input.GetKeyDown(_inputs.ctlr_b) | Input.GetKeyDown(_inputs.key_a_right))
            Attack(Vector3.right);
    }

    internal abstract void Attack(Vector3 direction);

    internal IEnumerator WeaponDisposer(GameObject prefab, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(prefab);
    }

    internal IEnumerator CoolDown(float cooldownTime)
    {
        _canFire = false;
        float fireRate = Mathf.Clamp(cooldownTime - GameManager.Instance.PlayerState.AttackRate, 0F, 100F);
        yield return new WaitForSeconds(fireRate);
        _canFire = true;
    }
}
