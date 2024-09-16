using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLedgeContact : MovingPlatform
{
    private bool hasContacted;
    private Vector3 startPOS;
    [SerializeField, Header("Platform Movement"), Space(10)]
    private Vector3 targPOS;
    private Vector3 speedTracked;
    private Vector3 dir;
    [SerializeField] private float stopTime;
    private bool goingLeft;
    private bool cooling;
    [SerializeField] private GameObject targPosIcon;
    
    new void Start()
    {
        base.Start();
        startPOS = transform.position;
        dir = (targPOS - startPOS).normalized;

        if(dir.x < 0)
        {
            goingLeft = true;
        }

        speedTracked = speed;
        speed = Vector3.zero;

        if (targPosIcon)
            Instantiate(targPosIcon, targPOS, Quaternion.identity);
    }

    new void OnCollisionEnter2D(Collision2D collision)
    {
        if (!cooling)
        {
            hasContacted = true;
            speed = dir * speedTracked.x;
            ChangeLightColors(changeLightColor);
            ChangeMaterial(changeMaterial);

        }
        base.OnCollisionEnter2D(collision);


    }

    void Update()
    {
        if(hasContacted)
        {
            if(goingLeft && transform.position.x < targPOS.x && !cooling)
            {
                StartCoroutine(PlatformCooldown(stopTime));
            }else if (!goingLeft && transform.position.x > targPOS.x && !cooling)
            {
                StartCoroutine(PlatformCooldown(stopTime));
            }else if(cooling && goingLeft && transform.position.x > startPOS.x)
            {
                speed = Vector3.zero;
                cooling = false;
                ChangeMaterial(startMaterial);
                ChangeLightColors(startLightColor);
            }
            else if (cooling && !goingLeft && transform.position.x < startPOS.x)
            {
                speed = Vector3.zero;
                cooling = false;
                ChangeMaterial(startMaterial);
                ChangeLightColors(startLightColor);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(targPOS, .1f);
    }

    IEnumerator PlatformCooldown(float time)
    {
        cooling = true;
        speed = Vector3.zero;
        yield return new WaitForSeconds(time);
        speed = -dir * speedTracked.x;

    }
}
