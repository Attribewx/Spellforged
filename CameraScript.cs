using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject followtarget;
    private Vector3 targetArea;
    public float spid;

    public static bool activeCam;

    public BoxCollider2D boundas;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    private Camera cam;
    private float halfHeight;
    private float halfWidth;
    private int skipCheckl;

    // Start is called before the first frame update
    void Start()
    {
        if (!activeCam)
        {
            activeCam = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        minBounds = boundas.bounds.min;
        maxBounds = boundas.bounds.max;

        cam = GetComponent<Camera>();
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        if(skipCheckl < 0)
        targetArea = new Vector3(followtarget.transform.position.x, followtarget.transform.position.y, transform.position.z);
        else
        skipCheckl = skipCheckl--;

        if(boundas == null)
        {
            boundas = FindObjectOfType<Bounds>().GetComponent<BoxCollider2D>();
            minBounds = boundas.bounds.min;
            maxBounds = boundas.bounds.max;
        }

    }

    private void FixedUpdate()
    {
        if (skipCheckl < 0)
        {
            transform.position = Vector3.Lerp(transform.position, targetArea, spid * Time.deltaTime);


            float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
            float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);


            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
        else
        {
            skipCheckl--;
        }
    }

    public void SetBoundaries(BoxCollider2D newBounds)
    {
        newBounds = boundas;

        if(boundas != null)
        {
            minBounds = boundas.bounds.min;
            maxBounds = boundas.bounds.max;
        }

    }

    public void SnapToPlayer()
    {
        transform.position = new Vector3(followtarget.transform.position.x, followtarget.transform.position.y, transform.position.z);
        targetArea = new Vector3(followtarget.transform.position.x, followtarget.transform.position.y, transform.position.z);
        skipCheckl = 6;
    }
}
