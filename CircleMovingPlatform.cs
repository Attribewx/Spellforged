using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovingPlatform : MovingPlatform
{

    private float startTime;
    [SerializeField]private float speedMultiplier = 50;
    [SerializeField] private float sizeScalar = 2;
    [SerializeField] private float fineTuner = 4;
    [SerializeField] private float fineTuner2 = 4;
    [SerializeField] private float fineTuner3 = 4;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        startTime = Time.time;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {

        float x = Mathf.Sin((Time.time - startTime) * sizeScalar);
        float y = Mathf.Cos((Time.time - startTime) * sizeScalar);

        speed = new Vector2(x, y).normalized * speedMultiplier;
        base.FixedUpdate();
    }

    private void OnDrawGizmos()
    {
        float x = Mathf.Sin((Time.time - startTime) * sizeScalar);
        float y = Mathf.Cos((Time.time - startTime) * sizeScalar);

        Vector2 center = new Vector2(y, -x);
        speed = new Vector2(x, y).normalized * speedMultiplier;

        Gizmos.DrawWireSphere(transform.position + (Vector3)center / fineTuner2 * speedMultiplier / sizeScalar, Mathf.Sqrt(speed.magnitude / Mathf.Abs(sizeScalar))/fineTuner);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.right * fineTuner3, Mathf.Sqrt(speed.magnitude / Mathf.Abs(sizeScalar)) / fineTuner);
    }
}
