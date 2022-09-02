using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treaper : MonoBehaviour
{

    public int attacks = 0;
    public bool isAttacking = false;
    public Animator claudia;
    public GameObject groundBean;
    public GameObject airBean;
    public Transform groundBeanArea;
    public Transform airBeanArea;
    public Vector2 potatoSpeed;
    public Vector2 animalFries;
    public int attackRange = 1;
    public float cooldown = 1f;
    private float timeTillCoold = 0f;
    public bool firing = false;
    public bool hasFired = false;

    // Start is called before the first frame update
    void Start()
    {
        claudia = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timeTillCoold -= Time.deltaTime;
        if(timeTillCoold <= 0)
        {
            timeTillCoold = cooldown;
            attacks = Random.Range(1, attackRange);
        }

        claudia.SetInteger("Attack", attacks);
        if(attacks == 0)
        {
            hasFired = false;
        }

        if (attacks == 1 && !isAttacking)
        {
            if (firing && !hasFired)
            {
                Attack1();
                firing = false;
            }
        }

        if (attacks == 2 && !isAttacking)
        {
            if(firing && !hasFired)
            {
                Attack2();
                firing = false;
            }
        }
    }

    void Attack1()
    {
        GameObject movingBean = Instantiate(groundBean, groundBeanArea.position, Quaternion.identity);
        Rigidbody2D movingBody = movingBean.GetComponent<Rigidbody2D>();
        if (gameObject.transform.localScale.x < 0)
        {
            movingBody.AddForce(-potatoSpeed);
            movingBean.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            movingBody.AddForce(potatoSpeed);
        }
        isAttacking = false;
        hasFired = true;
    }

    void Attack2()
    {
        GameObject movingBean = Instantiate(airBean, airBeanArea.position, Quaternion.identity);
        Rigidbody2D movingBody = movingBean.GetComponent<Rigidbody2D>();
        if (gameObject.transform.localScale.x < 0)
        { 
            movingBody.AddForce(-animalFries);
            movingBean.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            movingBody.AddForce(animalFries);
        }
        isAttacking = false;
        hasFired = true;
    }
}
