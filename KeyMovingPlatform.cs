using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMovingPlatform : MovingPlatform
{

    private Vector3 startPOS;
    private Vector3 speedTracked;
    [SerializeField, Header("Key Settings")] private Vector3 targLocation;

    [SerializeField] private float stoppedTime; 
    
    private bool goingToTarg;
    new public void Start()
    {
        base.Start();
        startPOS = transform.position;
        speedTracked = speed;
        speed = Vector3.zero;
    }

    new public void FixedUpdate()
    {
        base.FixedUpdate();
        if(Vector3.Distance(transform.position, targLocation) < .5f && goingToTarg)
        {
            speed = Vector3.zero;
            StartCoroutine(GoToStartPOS(stoppedTime));
        }
        if (Vector3.Distance(transform.position, startPOS) < .5f && !goingToTarg)
            speed = Vector3.zero;
    }

    public void GoToTarget()
    {
        if (Vector3.Distance(transform.position, targLocation) > .5f)
        {
            goingToTarg = true;
            Vector3 direction = targLocation - transform.position;
            speed = direction.normalized * speedTracked.x;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(targLocation, .1f);
    }

    IEnumerator GoToStartPOS(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        goingToTarg = false;
        Vector3 direction = startPOS - transform.position;
        speed = direction.normalized * speedTracked.x;
    }
}
