using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activatables : MonoBehaviour
{
    [SerializeField]protected bool multiActivational = true;
    protected bool hasCooldown;

    protected float cooldownTime;
    private float cooldown;

    protected Rigidbody2D rig;
    protected BoxCollider2D boxy;

    public virtual void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        boxy = GetComponent<BoxCollider2D>();
    }

    protected void SetCooldown()
    {
        cooldown = Time.time + cooldownTime;
    }

    protected bool CheckCooldown()
    {
        if (cooldown < Time.time)
            return true;
        else
            return false;
    }

    protected void MoveRig(Vector3 dir, float speed)
    {
        rig.velocity = dir * speed;
    }

}
