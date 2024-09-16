using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoaft : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject ghoaft;
    public bool makeGoatl = false;

    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(makeGoatl)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                GameObject currentGhoaft = Instantiate(ghoaft, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhoaft.GetComponent<SpriteRenderer>().sprite = currentSprite;
                currentGhoaft.transform.localScale = this.transform.localScale;
                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhoaft, 1);
            }
        }
    }
}
