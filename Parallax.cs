using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public Transform[] backgrounds;
    private float[] parallaxScalsa;
    public float smoothing = 1f;

    private Transform cam;
    private Vector3 prevCamPos;

    public Vector3 entrance1;
    public Vector3 entrance2;
    public Vector3 entrance3;
    public Vector3 entrance4;

    public bool doYScroll = false;

    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        if(entrance1 == Vector3.zero)
            prevCamPos = cam.position;
        else
        {
            prevCamPos = (entrance1 + entrance2) / 2;
            if(entrance3 != Vector3.zero)
            {
                prevCamPos = (entrance1 + entrance2 + entrance3) / 3;
            }
            if (entrance4 != Vector3.zero)
            {
                prevCamPos = (entrance1 + entrance2 + entrance3 + entrance4) / 4;
            }
        }

        parallaxScalsa = new float[backgrounds.Length];
        for(int i = 0; i < backgrounds.Length;i++)
        {
            parallaxScalsa[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (prevCamPos.x - cam.position.x) * parallaxScalsa[i];

            float backgroundTargPosX = backgrounds[i].position.x + parallax;

            float backgroundTargPosY = backgrounds[i].position.y;

            if (doYScroll)
            {
                float parallaxY = (prevCamPos.y - cam.position.y) * parallaxScalsa[i];
                 backgroundTargPosY = backgrounds[i].position.y + parallaxY;
            }

            Vector3 backgroundTargPos = new Vector3(backgroundTargPosX, backgroundTargPosY, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargPos, smoothing * Time.deltaTime);
        }

        prevCamPos = cam.position;
    }
           
}
