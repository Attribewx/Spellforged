using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFlyingEnemy : MonoBehaviour
{
    [SerializeField, Header("Movement Setup")] 
    public float speed;
    public float bubbleSize;
    public bool isPathFinder;
    public bool facingRight;
    private Vector2 veloc;
    private float startSpeed;

    [Header("Movement Variables"), Space(10)]
    public float pathfindingDist;
    public float pathMulti = 3;
    public float pathingTimeThing = .5f;

    [HideInInspector] public float goUpF;
    [HideInInspector] public float goDownF;
    [HideInInspector] public float goRightF;
    [HideInInspector] public float goLeftF;

    public GameObject player;
    private Rigidbody2D rig;
    private Vector3 startScale;
    private Vector3 startNegScale;

    private float[] jery;
    private float lof;

    private float upD = 3;
    private float downD = 3;
    private float leftD = 3;
    private float rightD = 3;

    private float dirLock = -1;
    private float timer = 0;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        jery = new float[4];
        rig = GetComponent<Rigidbody2D>();
        startScale = transform.localScale;
        startNegScale = new Vector3(-startScale.x, startScale.y, 1);
        startSpeed = speed;
    }

    // Update is called once per frame
    public void Update()
    {
        if(isPathFinder)
        {

            //// DISTANCE FROM DIRECTIONS \\\\
            RaycastHit2D upDir = Physics2D.Raycast(transform.position, Vector2.up, pathfindingDist, 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Ground")));
            RaycastHit2D downDir = Physics2D.Raycast(transform.position, Vector2.down, pathfindingDist, 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Ground")));
            RaycastHit2D leftDir = Physics2D.Raycast(transform.position, Vector2.left, pathfindingDist, 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Ground")));
            RaycastHit2D rightDir = Physics2D.Raycast(transform.position, Vector2.right, pathfindingDist, 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Ground")));
            
            
            if (upDir)
                upD = upDir.distance;
            else
                upD = pathfindingDist;
            if (downDir)
                downD = downDir.distance;
            else
                downD = pathfindingDist;
            if (leftDir)
                leftD = leftDir.distance;
            else
                leftD = pathfindingDist;
            if (rightDir)
                rightD = rightDir.distance;
            else
                rightD = pathfindingDist;


            //// PLAYER CHECK RAYCASTS \\\\
            goUpF = PlayerCheck(upDir, Vector3.up);
            jery[0] = goUpF;
            goLeftF = PlayerCheck(leftDir, Vector3.left);
            jery[1] = goLeftF;
            goDownF = PlayerCheck(downDir, Vector3.down);
            jery[2] = goDownF;
            goRightF = PlayerCheck(rightDir, Vector3.right);
            jery[3] = goRightF;

            lof = LowestFloat(jery);


            directionTune();

            //// PERSONAL SPACE CHECK \\\\
            BubbleCheck();

            //// FLIP SPRITE \\\\
            if(rig.velocity.x > 0 && facingRight)
            {
                transform.localScale = startScale;
            }
            else if(rig.velocity.x < 0 && facingRight)
            {
                transform.localScale = startNegScale;
            }

            if (rig.velocity.x > 0 && !facingRight)
            {
                transform.localScale = startNegScale;
            }
            else if(rig.velocity.x < 0 && !facingRight)
            {
                transform.localScale = startScale;
            }
        }


    }

    void FixedUpdate()
    {
            rig.AddForce(veloc);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,pathfindingDist + pathfindingDist * pathMulti);
        Color shortie = Color.red;

        if(lof == goUpF)
            Debug.DrawRay(transform.position, Vector2.up * upD, shortie);
        else
            Debug.DrawRay(transform.position, Vector2.up * upD, Color.green);
        if (lof == goDownF)
            Debug.DrawRay(transform.position, Vector2.down * downD, shortie);
        else
            Debug.DrawRay(transform.position, Vector2.down * downD, Color.green);
        if (lof == goLeftF)
            Debug.DrawRay(transform.position, Vector2.left * leftD, shortie);
        else
            Debug.DrawRay(transform.position, Vector2.left * leftD, Color.green);
        if (lof == goRightF)
            Debug.DrawRay(transform.position, Vector2.right * rightD, shortie);
        else
            Debug.DrawRay(transform.position, Vector2.right * rightD, Color.green);


        if (player)
        {
            Debug.DrawRay(transform.position + Vector3.up * upD, player.transform.position - transform.position - Vector3.up * upD, Color.magenta);
            Debug.DrawRay(transform.position + Vector3.left * leftD, player.transform.position - transform.position - Vector3.left * leftD, Color.magenta);
            Debug.DrawRay(transform.position + Vector3.right * rightD, player.transform.position - transform.position - Vector3.right * rightD, Color.magenta);
            Debug.DrawRay(transform.position + Vector3.down * downD, player.transform.position - transform.position - Vector3.down * downD, Color.magenta);
        }
        Gizmos.DrawWireSphere(transform.position, bubbleSize);
        
    }

    private void directionTune()
    {
        RaycastHit2D rayman = Physics2D.Raycast(transform.position, player.transform.position - transform.position, pathfindingDist + pathfindingDist * pathMulti, 1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Ground")));
        if(rayman)
        {
            if(rayman.collider.tag == player.tag)
            {
                veloc = (player.transform.position - transform.position).normalized * speed;
                dirLock = -1;
            }
            else if(lof == jery[0] && Time.time > timer)
            {
                dirLock = 0;
                timer = Time.time + pathingTimeThing;
            }
            else if (lof == jery[1] && Time.time > timer)
            {
                dirLock = 1;
                timer = Time.time + pathingTimeThing;
            }
            else if (lof == jery[2] && Time.time > timer)
            {
                dirLock = 2;
                timer = Time.time + pathingTimeThing;
            }
            else if (lof == jery[3] && Time.time > timer)
            {
                dirLock = 3;
                timer = Time.time + pathingTimeThing;
            }
            else
            {
                veloc = Vector2.zero;
            }

            if (Time.time > timer)
            {
                dirLock = -1;
            }

            switch(dirLock)
            {
                case -1:
                    timer = Time.time;
                    break;

                case 0:
                    veloc = Vector2.up * speed;
                    break;

                case 1:
                    veloc = Vector2.left * speed;
                    break;

                case 2:
                    veloc = Vector2.down * speed;
                    break;

                case 3:
                    veloc = Vector2.right * speed;
                    break;
            }
        }
        else
        {
            veloc = Vector2.zero;
        }
}

    private void BubbleCheck()
    {
        RaycastHit2D[] twoClose = Physics2D.CircleCastAll(transform.position, bubbleSize, Vector2.zero, 0, 1 << (LayerMask.NameToLayer("Ground")));
        foreach (RaycastHit2D item in twoClose)
        {
            veloc += -(item.point - new Vector2(transform.position.x, transform.position.y)).normalized * speed;
            //veloc += new Vector2(-(item.point - new Vector2(transform.position.x, transform.position.y)).normalized.x, -(item.point - new Vector2(transform.position.x, transform.position.y)).normalized.y) * speed;
        }
    }

    private float LowestFloat(float[] tom)
    {
        float lowF = pathfindingDist * pathMulti;
        for (int i = 0; i < tom.Length; i++)
        {
            if(tom[i] < lowF && tom[i] > 0)
            {
                lowF = tom[i];
            }
        }
        return lowF;
    }

    public void SetSpeed(float speef)
    {
        speed = speef;
    }

    public void SetDirection(Vector2 vec)
    {
        veloc = vec.normalized * speed;
    }

    public void SetStartSpeed()
    {
        speed = startSpeed;
    }

    public void SetScale(int dir)
    {
        if (dir > 0)
            transform.localScale = startScale;
        else
            transform.localScale = startNegScale;
    }

    private float PlayerCheck(RaycastHit2D dir, Vector3 dir2)
    {
        if(dir.collider)
        {
            RaycastHit2D playerHit = Physics2D.Raycast(dir.point - (new Vector2(dir2.x, dir2.y) * .1f),
                player.transform.position - new Vector3(dir.point.x, dir.point.y, 0),
                pathfindingDist * pathMulti,
                1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Ground")));

            if(playerHit)
            {
                if (playerHit.collider.tag == "Player")
                {
                    return playerHit.distance;
                }
            }
            return -1;
        }
        else
        {
            RaycastHit2D playerHit = Physics2D.Raycast(transform.position + dir2 * pathfindingDist,
                player.transform.position - (transform.position + (dir2 * pathfindingDist)),
                pathfindingDist * pathMulti,
                1 << (LayerMask.NameToLayer("Player")) | 1 << (LayerMask.NameToLayer("Ground")));

            if (playerHit)
            {
                if (playerHit.collider.tag == "Player")
                {
                    return playerHit.distance;
                }
            }
                    return -1;
        }
    }
}
