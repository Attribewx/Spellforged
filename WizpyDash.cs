using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizpyDash : MonoBehaviour
{

    public Rigidbody2D rig;
    public Animator aim;
    public GameObject boi;
    public float speed;
    public float speed2;
    public float circleSize;
    public float timeChange;
    public float width;
    public float height;
    public Vector3 startCoord;
    public bool isLaunching;
    public float tim;
    public float timeToFight;
    public float timeToWait;


    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        aim = GetComponent<Animator>();
        boi = GameObject.FindGameObjectWithTag("Player");
        startCoord = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timeChange += Time.deltaTime * speed2;

        if(Vector3.Distance(boi.transform.position, gameObject.transform.position) < 7 && !isLaunching && timeToFight < Time.time)
        {
                StartCoroutine(PrepareForLiftOff());
        }
        else if(!isLaunching)
        {
            aim.SetFloat("Speed", 0);
            Rotata();
        }

    }

    void Rotata()
    {
        float x = Mathf.Sin(timeChange) * width + startCoord.x;
        float y = Mathf.Cos(timeChange) * height + startCoord.y;

        transform.position = new Vector3(x, y, transform.position.z);
    }


    IEnumerator PrepareForLiftOff()
    {
        timeToFight = Time.time + timeToWait;
        isLaunching = true;
        rig.velocity = (boi.transform.position - gameObject.transform.position).normalized * speed;
        aim.SetFloat("Horizontal", rig.velocity.x);
        aim.SetFloat("Vertical", rig.velocity.y);
        aim.SetFloat("Speed", rig.velocity.sqrMagnitude);
        yield return new WaitForSeconds(tim);
        rig.velocity = Vector2.zero;
        startCoord = transform.position;
        isLaunching = false;
    }
}
