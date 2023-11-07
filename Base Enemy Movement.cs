using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyMovement : MonoBehaviour
{

    public Rigidbody2D regigigas;
    public GameObject playPoPiPo;
    public float speed;
    public float detectionRange;
    public float detectionGroundRange = 3;
    public float detectionGroundCRange = 2;
    public Vector3 lastIdlePos;
    public bool inRange;
    public bool inLOS;
    public bool isRangeIdle = true;
    public bool isLOSIdle = true;
    public bool nearGround;
    public bool reallyNGround;
    public bool steepGround;
    public bool isGrounded;
    public bool isAttacking;
    public bool isFloorInFront;
    public bool isFacingRight = true;
    public float attackingTimer;
    public float maxAttackTime;
    public float groundChkFront;
    public float distToGrnd;
    public BoxCollider2D boxy;
    public Animator animo;
    public float groundDist;

    public void Start()
    {
        regigigas = GetComponent<Rigidbody2D>();
        playPoPiPo = GameObject.FindGameObjectWithTag("Player");
        lastIdlePos = transform.position;
        boxy = GetComponent<BoxCollider2D>();
        animo = GetComponent<Animator>();
    }

    public void Update()
    {

        if(attackingTimer >= 0)
        {
            attackingTimer -= Time.deltaTime;
        }

        if (IsGrounds())
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }



        if (Physics2D.OverlapCircle(gameObject.transform.position, detectionRange, 1 << (LayerMask.NameToLayer("Player"))))
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }


        RaycastHit2D forwardGCheckClose = Physics2D.Raycast(gameObject.transform.position, gameObject.transform.right, detectionGroundCRange, (1 << LayerMask.NameToLayer("Ground")));
        if (forwardGCheckClose)
        {
            reallyNGround = true;
        }
        else
        {
            reallyNGround = false;
        }

        RaycastHit2D forwardGCheck = Physics2D.Raycast(gameObject.transform.position, gameObject.transform.right, detectionGroundRange, (1 << LayerMask.NameToLayer("Ground")));
        if(forwardGCheck)
        {
            nearGround = true;
            distToGrnd = Mathf.Abs(transform.position.x - forwardGCheck.point.x);
        }
        else
        {
            nearGround = false;
            distToGrnd = 1f;
        }


        RaycastHit2D DiagonalChek = Physics2D.Raycast(gameObject.transform.position, (gameObject.transform.right + (Vector3.up * 1.25f * (2/distToGrnd))), 2f * detectionGroundRange, (1 << LayerMask.NameToLayer("Ground")));
        if(DiagonalChek)
        {
            steepGround = true;
        }
        else
        {
            steepGround = false;
        }


        RaycastHit2D hitio = Physics2D.Raycast(gameObject.transform.position, playPoPiPo.transform.position - gameObject.transform.position, 100f, (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground")));
        if(hitio && hitio.collider.tag == "Player")
        {
            inLOS = true;
        }
        else
        {
            inLOS = false;
        }

        RaycastHit2D floorCheck = new RaycastHit2D();
        if(isFacingRight)
        {
            floorCheck = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + groundChkFront, gameObject.transform.position.y), Vector2.down, groundDist, (1 << LayerMask.NameToLayer("Ground")));
        }
        else
        {
            floorCheck = Physics2D.Raycast(new Vector2(gameObject.transform.position.x - groundChkFront, gameObject.transform.position.y), Vector2.down, groundDist, (1 << LayerMask.NameToLayer("Ground")));
        }
        if (floorCheck)
        {
            isFloorInFront = true;
        }
        else
        {
            isFloorInFront = false;
        }


        if (inRange)
        {
            isRangeIdle = false;
        }
        else
        {
            isRangeIdle = true;
        }

        if(inLOS)
        {
            isLOSIdle = false;
        }
        else
        {
            isLOSIdle = true;
        }

    }

    public virtual void IsRangeIdle()
    {

    }

    public virtual void IsLOSIdle()
    {

    }

    public virtual void InRange()
    {
        //Does Something
    }

    public virtual void InLOS()
    {

    }

    public void OnDrawGizmos()
    {
        Color z = new Color(.5f, 0, 1);
        Color x = Color.gray;
        Color a = Color.black;
        Color g = Color.cyan;
        Color c = Color.red;
        if (inRange && inLOS)
        {
            c = Color.green;
        }
        else if (!inRange && inLOS)
        {
            c = Color.blue;
        }
        else if (inRange && !inLOS)
        {
            c = Color.yellow;
        }
        else
        {
            c = Color.red;
        }

        if(nearGround)
        {
            g = Color.magenta;
        }else
        {
            g = Color.red;
        }

        if(reallyNGround)
        {
            a = Color.white;
        }else
        {
            a = Color.black;
        }

        if(steepGround)
        {
            x = new Color(255, 64, 0);
        }
        else
        {
            x = Color.gray;
        }

        if(isFloorInFront)
        {
            z = new Color(1f, 0, .5f);
        }
        else
        {
            z = new Color(.5f, 0, 1);
        }
        Transform t = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Debug.DrawRay(t.position, gameObject.transform.position - t.transform.position, c);
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.right * detectionGroundRange, g);
        Debug.DrawRay(gameObject.transform.position + new Vector3(0,.25f,0), gameObject.transform.right * detectionGroundCRange, a);
        Debug.DrawRay(gameObject.transform.position, (gameObject.transform.right + (Vector3.up * 1.25f * (2/distToGrnd))) * 2 * detectionGroundRange, x);
        if(isFacingRight)
            Debug.DrawRay(new Vector2(gameObject.transform.position.x + groundChkFront, gameObject.transform.position.y), Vector2.down * groundDist, z);
        else
            Debug.DrawRay(new Vector2(gameObject.transform.position.x - groundChkFront, gameObject.transform.position.y), Vector2.down * groundDist, z);
        
    }

    public bool IsGrounds()
    {
            RaycastHit2D BillieRayCyrus = Physics2D.BoxCast(boxy.bounds.center + new Vector3(0, -.001f, 0), new Vector2(boxy.bounds.size.x - .3f, boxy.bounds.size.y), 0f, Vector2.down, 0, 1 << (LayerMask.NameToLayer("Ground")));
            return BillieRayCyrus;
    }




}
