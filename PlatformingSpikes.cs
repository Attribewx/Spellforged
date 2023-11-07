using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingSpikes : MonoBehaviour
{
    private Animator transition;
    private Health playerHealth;
    private Movement canMove;
    private CameraScript cam;
    private float timer;
    [SerializeField] private float damage;
    [SerializeField] private float waitTime;
    [SerializeField, Tooltip("Return position for the player")] private Transform returnPos;

    void Start()
    {
        transition = GetComponent<Animator>();
        playerHealth = FindObjectOfType<Movement>().GetComponent<Health>();
        canMove = FindObjectOfType<Movement>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && timer <= Time.time)
        {
            transition.SetTrigger("Start");
            playerHealth.TakeDamage(damage, 100, 800, ElementType.Physical);
            timer = Time.time + waitTime;
        }
    }

    public void MovePlayer()
    {
        canMove.dashTimeLeft = 0;
        canMove.jTest = false;
        canMove.transform.position = returnPos.position;
        cam.transform.position = new Vector3(returnPos.position.x, returnPos.position.y, cam.transform.position.z);
    }
}
