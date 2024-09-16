using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private LayerMask Layo;
    public Rigidbody2D wiz;
    public Animator anime;
    public Vector3 negaScale;
    public Vector3 scalsa;
    public EdgeCollider2D boxy;

    public bool isJumping = false;
    public float extraHeight = .7f;

    public float jPower = 25f;
    private float horizontalMove = 0;
    public float speed = 40;
    public float sprintSpeed;

    private float jumpCounter;
    public float jumpTime;

    public float maxFallSpeedz = -16f;

    private static bool playerExisto;
    private bool haveThePower = true;
    
    private float haveThePowerTimer;
    public float haveThePowerTimeToWait = .25f;

    public Ghoaft goaft;
    public float inputo;

    public Attacks movementMana;
    public int sprintMana = 15;

    public EdgeCollider2D boxyCrouch;
    public string pointCrow;

    public float dashTime;
    public float dashSpeed;
    public float dashTimeLeft;
    private float lastDash;
    public float dashCooldown = .5f;
    private int dashDir = 0;
    public bool isDashins;

    public bool isSwinging;
    public float inputoLock;

    public bool isSlamming = false;
    private Vector2[] colPoints;

    private float xLightning = 0;
    private float xPlatform = 0;
    public float airSpeed;
    public bool airLock;
    public float horizontalLock;
    private float platPower;
    public float yairSpeed;
    private bool hasntLanded;
    private float groundTimer;

    [SerializeField, Header("Jump Settings")] private float jumpCoyoteTime = .3f;
    private bool platformSwitcher = false;
    private float platformTimer = .2f;
    private float platformTimerStart;
    private float platformCoyoteX;
    private float platformCoyoteY;
    private float startCoyoteTime;
    public float jumpBuff = .1f;
    private float jumpBuffCount = 0;
    private bool isBuffering;
    private float xWindSpeed;
    private bool inWind;
    private float xWindDecaySpeed = 500;
    [SerializeField] private int jumpCount;
    private int jumpCountStart;

    public bool jumpKeyDown;
    public float jOverride;

    [SerializeField]
    private Transform camZoner;
    private Health helth;
    [SerializeField] private float SpikeDamage = 50;

    private float crouchTimer;
    private float upTimer;
    private bool dashPerkReset;

    void Start()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 300;
        helth = GetComponent<Health>();

        if (!playerExisto)
        {
            playerExisto = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        colPoints = boxy.points;

        startCoyoteTime = jumpCoyoteTime;
        platformTimerStart = platformTimer;
        jumpCount = jumpCountStart;
    }

    void Update()
    {
        DecreasePlatformTimer();

        DecreaseCoyoteTime();

        //Wind Speed Slower
        if(xWindSpeed != 0)
        {
            if(xWindSpeed > 0)
            {
                if(xWindSpeed - xWindDecaySpeed * Time.deltaTime < 0)
                {
                    xWindSpeed = 0;
                }
                else
                    xWindSpeed -= xWindDecaySpeed * Time.deltaTime;
            }
            else
            {
                if (xWindSpeed + xWindDecaySpeed * Time.deltaTime > 0)
                {
                    xWindSpeed = 0;
                }
                else
                    xWindSpeed += xWindDecaySpeed * Time.deltaTime;
            }
        }

        if(!isSlamming)
        {
            inputo = Input.GetAxisRaw("Horizontal");
        }

        DashInput();

        UpwardCamera();
        
        EnemyDashCheck();

        Crouch();

        if (isSwinging)
        {
            inputo = 0;
        }

        GravitySprint();


        //Scale code for turning left and right / mirroring left and right
        if (horizontalMove < 0 && !isDashins)
        {
            gameObject.transform.localScale = negaScale;
        }
        if (horizontalMove > 0 && !isDashins)
        {
            gameObject.transform.localScale = scalsa;
        }

        if (horizontalMove != 0)
        {
            anime.SetBool("isMoving", true);
        }
        else
        {
            goaft.makeGoatl = false;
            anime.SetBool("isMoving", false);
        }



        //GROUND CHECKS
        if (IsGrounds())
        {
            if(!inWind)
                SetXWindSpeed(0);
            anime.SetBool("IsGrounded", true);
            ChangePlatformForces(0, 0, true, false);
            airLock = false;
            airSpeed = 0;
            platPower = 0;
            yairSpeed = 0;
            hasntLanded = false;
            if (!(Input.GetAxisRaw("Vertical") < 0))
            {
                jPower = 11;
            }
        }
        else
        {
            anime.SetBool("IsGrounded", false);
            if(platformTimer < 0)
            ChangePlatformForces(airSpeed, yairSpeed, true, false);
            else 
            ChangePlatformForces(platformCoyoteX, platformCoyoteY, true, true);
        }





        if(Input.GetButtonDown("Jump"))
        {
            jumpBuffCount = jumpBuff;
        }
        else
        {
            jumpBuffCount -= Time.deltaTime;
        }



        if (!Input.GetButton("Jump") && jumpBuffCount < 0 && jumpKeyDown == true)
        {
            isJumping = false;
            jumpKeyDown = false;
        }
        if (jumpBuffCount >= 0 && jumpCoyoteTime > 0)
        {
            jumpKeyDown = true;
            isJumping = true;
            jumpCounter = jumpTime;
            //wiz.velocity = Vector2.up * (jPower);
            if(!Input.GetButton("Jump"))
            {
                isJumping = false; //YEET
            }
            jumpBuffCount = -12;
        }
        if (Input.GetButton("Jump") && isJumping == true)
        {
            if(jumpCounter > 0)
            {
                jumpKeyDown = true;
                //wiz.velocity = Vector2.up * (jPower + platPower);
                jumpCounter -= Time.deltaTime;
            }
            else
            {
                jumpKeyDown = false;
            }

        }
        if (Input.GetButtonUp("Jump"))
        {
            jumpKeyDown = false;
            isJumping = false;
            jOverride = 0;
        }
        anime.SetFloat("Jump Counter", groundTimer);






        if(wiz.velocity.y < maxFallSpeedz)
        {
            wiz.velocity = new Vector3 (wiz.velocity.x, maxFallSpeedz, 0);
        }
        anime.SetFloat("YVelocity", wiz.velocity.y);




        if(haveThePower)
        {
            CheckDash();
        }

        //// CAMERA DISTANCE ON SPEED \\\\
        if (crouchTimer == 0 && upTimer == 0)
        {
            if ((transform.localScale.x > 0 && wiz.velocity.x > 0) || (transform.localScale.x < 0 && wiz.velocity.x < 0))
            {
                float posCalc = Mathf.Clamp(Mathf.Abs(wiz.velocity.x), 6, 30);
                if (posCalc == 6)
                {
                    posCalc = posCalc / 20;
                }
                else
                {
                    posCalc = posCalc / 7.5f;
                }
                camZoner.localPosition = new Vector3(posCalc, 0, 0);
            }
            else
            {
                camZoner.localPosition = new Vector3(.3f, 0, 0);
            }
        }
        else if (crouchTimer == 0 && upTimer >= 1.5f)
        {
            if ((transform.localScale.x > 0 && wiz.velocity.x > 0) || (transform.localScale.x < 0 && wiz.velocity.x < 0))
            {
                float posCalc = Mathf.Clamp(Mathf.Abs(wiz.velocity.x), 6, 30);
                if (posCalc == 6)
                {
                    posCalc = posCalc / 20;
                }
                else
                {
                    posCalc = posCalc / 7.5f;
                }
                camZoner.localPosition = new Vector3(posCalc, 4, 0);
            }
            else
            {
                camZoner.localPosition = new Vector3(.3f, 4, 0);
            }
        }

        if(!haveThePower)
        {
            xPlatform = 0;
        }

    }

    void FixedUpdate()
    {
        if(haveThePower && isDashins == false)
        {
             wiz.velocity = new Vector3((horizontalMove + xLightning + xPlatform + xWindSpeed)* Time.fixedDeltaTime, wiz.velocity.y, 0);
            if(jumpKeyDown)
            {
                wiz.velocity = new Vector2(wiz.velocity.x, (jPower + platPower + jOverride));
            }
            if (isSwinging)
            {
                wiz.velocity = new Vector3(175 * inputoLock * Time.fixedDeltaTime, 0, 0);
            }
            if(isSlamming)
            {
                wiz.velocity = Vector2.zero;
            }
        }else
        {
            if(haveThePowerTimer < Time.time)
            {
                haveThePower = true;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Floor")
        {
            jumpCount = jumpCountStart;
        }
    }

    public bool IsGrounds()
    {
        if(boxy.enabled == true)
        {
            RaycastHit2D BillieRayCyrus = Physics2D.BoxCast(boxy.bounds.center + new Vector3(0, -extraHeight,0), boxy.bounds.size, 0f, Vector2.down, 0, Layo);
            if(BillieRayCyrus)
            {
                groundTimer += Time.deltaTime;
            }else
            {
                groundTimer = 0;
            }
            return BillieRayCyrus;
        }
        else
        {
            RaycastHit2D BillieRayCyrus = Physics2D.BoxCast(boxyCrouch.bounds.center + new Vector3(0, -extraHeight, 0), boxyCrouch.bounds.size, 0f, Vector2.down, 0, Layo);
            if (BillieRayCyrus)
            {
                groundTimer += Time.deltaTime;
            }
            else
            {
                groundTimer = 0;
            }
            return BillieRayCyrus;
        }
    }

    public bool IsOverheadAirborne()
    {
        RaycastHit2D boxChekBot = Physics2D.BoxCast(boxy.bounds.center, boxy.bounds.size, 0, Vector2.up, .2f, 1 << LayerMask.NameToLayer("Ground"));
        if (boxChekBot)
        {
            return boxChekBot;
        }else
        {
            return false;
        }
    }

    public void AddKnockback(float knockbackX, float knockbackY)
    {
        if(isDashins)
        {
            dashTimeLeft = 0;
            wiz.gravityScale = 4;
        }
        wiz.velocity = Vector2.zero;
        xPlatform = 0;
        isJumping = false;
        haveThePower = false;
        haveThePowerTimer = Time.time + haveThePowerTimeToWait;
        wiz.AddForce(new Vector2(knockbackX, knockbackY));
    }

    public void AttemptToDash()
    {
        dashDir = (int)transform.localScale.x / 2;
        isDashins = true;
        anime.SetBool("IsDashing", isDashins);
        dashTimeLeft = dashTime;
        lastDash = Time.time;
        dashPerkReset = true;
    }

    private void CheckDash()
    {
        if(dashTimeLeft > 0)
        {
            if(isDashins)
            {
                Physics2D.IgnoreLayerCollision(9, 14);
                wiz.gravityScale = 0;
                wiz.velocity = new Vector2(dashSpeed * dashDir, 0);
                dashTimeLeft -= Time.deltaTime;
            }
        }
        if(dashTimeLeft <= 0)
        {
            Physics2D.IgnoreLayerCollision(9, 14,false);
            wiz.gravityScale = 4;
            isDashins = false;
            anime.SetBool("IsDashing", isDashins);
        }
    }

    public void ChangeLightningForces(float xSpeed)
    {
            xLightning = xSpeed * transform.localScale.x / 2;
    }

    public void ChangePlatformForces(float xSpeed, float ySpeed, bool laterally, bool platformSwitch)
    {
        if (platformSwitch && isJumping)
        {
            yairSpeed = platformCoyoteY;
            airSpeed = platformCoyoteX;
        }
        if(!airLock)
        {
            horizontalLock = horizontalMove;
            airLock = true;
        }
        if(laterally)
        {
            if(horizontalLock < 0)
            {
                if (horizontalMove <= 0 && airSpeed < 0 && !isDashins)
                    xPlatform = xSpeed * 50;
                else
                {
                    xPlatform = 0;
                    airSpeed = 0;
                }
            }
            else if (horizontalLock > 0)
            {
                if (horizontalMove >= 0 && airSpeed > 0 && !isDashins)
                    xPlatform = xSpeed  * 50;
                else
                {
                    xPlatform = 0;
                    airSpeed = 0;
                }
            }
            else if(horizontalLock == 0)
            {
                if ((horizontalMove == 0 || (horizontalMove > 0 && xSpeed > 0) || (horizontalMove < 0 && xSpeed < 0)) && !isDashins)
                    xPlatform = xSpeed * 50;
                else
                {
                    xPlatform = 0;
                    airSpeed = 0;
                }
            }
            if( ySpeed - 1 >= 0)
            {
                    platPower = ySpeed - 1;
            }

        }
    }

    public void ChangeJumpCounter(float Taimou)
    {
        jumpCounter = Taimou;
        wiz.velocity = wiz.velocity * new Vector2(1, .5f);
    }

    public void ChangeHavePowerTime(float time)
    {
        haveThePowerTimer = Time.time + time;
    }

    public void EnemyDashCheck()
    {
        if (isDashins)
        {
            RaycastHit2D enemyCheck = Physics2D.BoxCast(transform.position, boxy.bounds.size, 0, Vector2.right, 0, 1 << LayerMask.NameToLayer("Enemy"), 0);
            RaycastHit2D bossCheck = Physics2D.BoxCast(transform.position, boxy.bounds.size, 0, Vector2.right, 0, 1 << LayerMask.NameToLayer("Boss"), 0);
            if (enemyCheck && dashPerkReset)
            {
                if(Inventory.instance.FindEquippedRune(Runes.Rune_of_Spectre))
                {
                    movementMana.ManaUp(Inventory.instance.GetSpectre());
                }
                if (Inventory.instance.FindEquippedRune(Runes.Rune_of_Wraith))
                {
                    helth.HealUp(Inventory.instance.GetSpectre());
                }
                dashPerkReset = false;
            }
            if (bossCheck && dashPerkReset)
            {
                if (Inventory.instance.FindEquippedRune(Runes.Rune_of_Spectre))
                {
                    movementMana.ManaUp(Inventory.instance.GetSpectre() * 2);
                }
                if (Inventory.instance.FindEquippedRune(Runes.Rune_of_Wraith))
                {
                    helth.HealUp(Inventory.instance.GetSpectre() * 2);
                }
                dashPerkReset = false;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Spikes")
        {
            StartingPoint[] points = FindObjectsOfType<StartingPoint>();
            for (int i = 0; i < points.Length; i++)
            {
                if(points[i].loadName == pointCrow)
                {
                    LoadArea loadScreen = FindObjectOfType<LoadArea>();
                    loadScreen.transition.SetTrigger("Start");
                    helth.TakeDamage(SpikeDamage,0,100,ElementType.Physical);
                    points[i].ResetPos();
                }
            }
        }
    }

    public void SetXWindSpeed(float f)
    {
        xWindSpeed = f;
    }

    public float GetXWindSpeed()
    {
        return xWindSpeed;
    }

    public void SetWindBool(bool b)
    {
        inWind = b;
    }

    public bool GetThePower()
    {
        return haveThePower;
    }

    private void DashInput()
    {
        if (Input.GetButtonDown("Dash") || Input.GetAxis("DashController") > 0)
        {
            if (Time.time >= (lastDash + dashCooldown) && movementMana.ManaDrain(20f))
                AttemptToDash();
        }
    }

    private void UpwardCamera()
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            upTimer += Time.deltaTime;
            if (upTimer > 1.5f)
            {
                camZoner.localPosition = new Vector3(.3f, 4, 0);
            }
        }
        else
        {
            upTimer = 0;
        }
    }

    private void Crouch()
    {
        if (Input.GetAxisRaw("Vertical") < 0 && IsGrounds())
        {
            inputo = 0;
            anime.SetBool("IsCrouching", true);
            colPoints[0] = new Vector3(.25f, .1f);
            colPoints[5] = new Vector3(-.25f, .1f);
            colPoints[6] = new Vector3(.25f, .1f);
            boxy.points = colPoints;
            jPower = 20;
            //boxyCrouch.enabled = true;
            //boxy.enabled = false;

            crouchTimer += Time.deltaTime;
            if (crouchTimer > 1.5f)
            {
                camZoner.localPosition = new Vector3(.3f, -4, 0);
            }
        }
        else
        {
            crouchTimer = 0;
            anime.SetBool("IsCrouching", false);
            colPoints[0] = new Vector3(.25f, .407f);
            colPoints[5] = new Vector3(-.25f, .407f);
            colPoints[6] = new Vector3(.25f, .407f);
            boxy.points = colPoints;
            //boxy.enabled = true;
            //boxyCrouch.enabled = false;
        }
    }

    private void GravitySprint()
    {
        if (Input.GetButton("Fire3"))
        {
            horizontalMove = inputo * sprintSpeed;
            if (Inventory.instance.FindEquippedRune(Runes.Rune_of_Featherweight))
                horizontalMove = inputo * sprintSpeed * 2;
            anime.speed = 1;

            if (horizontalMove != 0)
            {
                movementMana.ManaBegone(sprintMana);
                goaft.makeGoatl = true;
                if (IsGrounds())
                {
                    anime.speed = 1.5f;
                }
            }
        }
        else
        {
            horizontalMove = inputo * speed;
            goaft.makeGoatl = false;
            anime.speed = 1;
        }
    }

    private void DecreaseCoyoteTime()
    {
        if (!IsGrounds() && jumpCoyoteTime > 0)
        {
            jumpCoyoteTime -= Time.deltaTime;
        }
        else if(IsGrounds())
            jumpCoyoteTime = startCoyoteTime;
    }

    public void SetCoyoteX(float x)
    {
        platformCoyoteX = x;
    }
    public void SetCoyoteY(float y)
    {
        platformCoyoteY = y;
    }

    public void UpdatePlatformTimer()
    {
        platformTimer = platformTimerStart;
    }

    private void DecreasePlatformTimer()
    {
        platformTimer -= Time.deltaTime;
    }
}
