using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{

    private BoxCollider2D box;
    private CameraScript theCam;
    

    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        theCam = FindObjectOfType<CameraScript>();
        theCam.SetBoundaries(box);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
