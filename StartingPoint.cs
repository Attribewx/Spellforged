using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPoint : MonoBehaviour
{

    public Movement player;
    public CameraScript camo;
    public LoadArea lastMovement;
    public AudioManager ardino;
    public string loadName;
    public string songName;
    public string stopName;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        camo = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
    }

    void Start()
    {
        if(!AudioManager.arduino.IsPlaying(songName))
        {
            if(stopName != "" && AudioManager.arduino.IsPlaying(stopName))
            {
                AudioManager.arduino.Stop(stopName);
            }

            AudioManager.arduino.Play(songName);
        }

        player.dashTimeLeft = 0;
        player.jumpKeyDown = false;

        if (player.pointCrow == loadName)
        {
            player.transform.position = transform.position;
            //camo.transform.position = new Vector3(transform.position.x, transform.position.y, camo.transform.position.z);
            camo.SnapToPlayer();
        }
    }

    public void ResetPos()
    {
        player.dashTimeLeft = 0;
        player.jumpKeyDown = false;

        if (player.pointCrow == loadName)
        {
            player.transform.position = transform.position;
            camo.transform.position = new Vector3(transform.position.x, transform.position.y, camo.transform.position.z);
        }
    }
}
