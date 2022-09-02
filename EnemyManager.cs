using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public string[] deadBoies = new string[0];
    public string[] deadBaus = new string[0];
    public static bool activeMan;

    // Start is called before the first frame update
    void Start()
    {
        if (!activeMan)
        {
            activeMan = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDeadBoi(GameObject deathBoi)
    {
        string[] temps = new string[deadBoies.Length + 1];
        for (int i = 0; i < deadBoies.Length; i++)
        {
            temps[i] = deadBoies[i];
        }
        temps[deadBoies.Length] = deathBoi.name;
        deadBoies = temps;
    }

    public void AddDeadBoiss(GameObject deathBoi)
    {
        string[] temps = new string[deadBaus.Length + 1];
        for (int i = 0; i < deadBaus.Length; i++)
        {
            temps[i] = deadBaus[i];
        }
        temps[deadBaus.Length] = deathBoi.name;
        deadBaus = temps;
    }

    public void ClearBois()
    {
        deadBoies = new string[0];
    }

}
