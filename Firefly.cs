using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : BaseFlyingEnemy
{

    public float dashDist;
    [SerializeField] private float dashTime = .5f;
    private float dashTimer = 0;
    private BoxCollider2D box;
    private bool isDashing;
    private float dashCooldown;

    new void Start()
    {
        base.Start();
        box = GetComponent<BoxCollider2D>();
        isPathFinder = true;
    }

    new void Update()
    {
        base.Update();
        Dash(Vector2.right);
        Dash(Vector2.left);

        if(dashTimer + dashTime < Time.time)
        {
            isDashing = false;
            SetStartSpeed();
            isPathFinder = true;
        }
    }

    private void Dash(Vector2 dir)
    {
        RaycastHit2D dashBox = Physics2D.BoxCast(transform.position, box.size/2, 0, dir, dashDist, 1 << LayerMask.NameToLayer("Player") | 1 << (LayerMask.NameToLayer("Ground")));
        if (dashBox && dashBox.collider.tag == "Player" && !isDashing && dashTimer + dashTime < Time.time && dashCooldown < Time.time)
        {
            isDashing = true;
            isPathFinder = false;
            dashTimer = Time.time;
            dashCooldown = Time.time + dashTime * 20;
            SetSpeed(speed * 7);
            SetDirection(dir);
            if(dir == Vector2.right)
            {
                SetScale(1);
            }
            else
            {
                SetScale(-1);
            }
        }
    }

}
