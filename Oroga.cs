using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oroga : MonoBehaviour
{

    public GameObject Goof;
    public float fireRate = 3f;
    public float nextTimeToFire = 0;
    public Transform playerLocation;
    private bool inRange = false;
    public float range = 8;
    [SerializeField] private LayerMask kegs;


    // Start is called before the first frame update
    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(gameObject.transform.position.x - playerLocation.position.x) < range)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }

        if(nextTimeToFire < Time.time && inRange)
        {
            nextTimeToFire = Time.time + fireRate;
            StartCoroutine(Shoot(.2f));
        }
    }

    IEnumerator Shoot(float seconds)
    {
        RaycastHit2D locate = Physics2D.Raycast(playerLocation.position, Vector2.down, 30, kegs);
        yield return new WaitForSeconds(seconds);
        Instantiate(Goof, locate.point + new Vector2(0, .9f), Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, new Vector3(range, 0));
        Gizmos.DrawRay(transform.position, new Vector3(-range, 0));
    }
}
