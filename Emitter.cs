using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    [SerializeField, Header("Emission Setup")] private float emissionRate = 1;
    private float nextEmission;
    private float emissionTime;
    [SerializeField] private GameObject emission;
    [SerializeField] private Vector2 emissionSpeed;
    [SerializeField] private float destroyTime = 5;

    [SerializeField, Header("Emission Variance")] private float xRange = 0;
    [SerializeField] private float yRange = 0;


    void Start()
    {
        nextEmission = emissionRate;
    }

    // Update is called once per frame
    void Update()
    {
        emissionTime += Time.deltaTime;

        if (emissionTime > nextEmission)
        {
            Emit();
        }
    }

    void Emit()
    {
        float rngX = 0;
        float rngY = 0;
        if(xRange != 0 || yRange != 0)
        {
            rngX = Random.Range(-xRange, xRange);
            rngY = Random.Range(-yRange, yRange);
        }

        Vector3 rngOffset = new Vector3(rngX, rngY);

        nextEmission += emissionRate;
        GameObject emissary = Instantiate(emission, transform.position + rngOffset,Quaternion.identity);
        Destroy(emissary, destroyTime);
        if (emissary.GetComponent<Rigidbody2D>())
        {
            Rigidbody2D rig = emissary.GetComponent<Rigidbody2D>();
            rig.velocity = emissionSpeed;
        }
    }
}
