using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Attacks : MonoBehaviour
{
    public Movement movementChecks;
    public Health healthy;
    public List<GameObject> allTehNoods;
    public List<Material> allTehMatts;
    public bool[] allTehUnloks;
    private bool[] allTehEquips;
    public GameObject grayNoodle;
    public GameObject Magicals;
    public Animator anime;
    public Animator animeMagic;
    public Transform magicBoneZone;
    public Transform magicBoneZoneTop;
    public Transform magicBoneZoneCrouched;
    public Transform magicBoneZoneBottom;
    public Transform magicBoneZoneHigher;
    public Transform magicBoneZoneLightning;
    public Transform magicBoneZoneBehind;
    public float spood;
    public bool isFires = false;
    public bool lookingUp = false;
    public bool lookingDown = false;
    public Vector2 flippedX = new Vector2(-2, 2);

    public float timeToNextFire = 0f;
    public float fireRate = .2f;

    public int magicNumber = 0;
    public SpriteRenderer prettyColors;
    public SpriteRenderer spellColors;
    public Light2D lightColor;
    public Light2D lightColor2;
    public HealthBar manaBar;
    public float maxMana = 100f;
    public float curMana = 100f;
    public float manaCost = 10;
    public float manaRegen = 5;

    private GameObject lightnignSpawn;
    private GameObject hitSpawn;
    public GameObject hitEffectLight;
    public GameObject rTape;
    public GameObject lTape;
    public float marksman;
    public float lightDam;
    public bool lightCharge = true;

    public int lightSwordNumba = 6;
    public float lightSwordDam = 7;
    public float swordMaster = 1.25f;
    private bool canAttack = true;

    void Awake()
    {
        allTehUnloks = new bool[allTehNoods.Capacity];
        allTehUnloks[0] = true;
        allTehEquips = new bool[allTehNoods.Capacity];
        for (int i = 0; i < allTehEquips.Length; i++)
        {
            allTehEquips[i] = true;
        }
    }

    void Start()
    {
        prettyColors = GetComponent<SpriteRenderer>();
        spellColors = Magicals.GetComponent<SpriteRenderer>();
        healthy = GetComponent<Health>();
        if(manaBar != null)
        {
            manaBar.SetMaxHealth((int) maxMana);
        }
    }

    void Update()
    {

        if(curMana < maxMana)
        {
            curMana += manaRegen * Time.deltaTime;
            if(curMana > maxMana)
            {
                curMana = maxMana;
            }
        }
        if(manaBar != null)
        {
            manaBar.SetHealth((int)curMana);
        }

        if (Input.GetButtonDown("Fire2") && magicNumber + 1 < allTehNoods.Capacity)
        {
            magicNumber++;


            for (int i = magicNumber; i < allTehUnloks.Length; i++)
            {
                if (allTehUnloks[i] != true)
                {
                    magicNumber++;
                    if (magicNumber == allTehNoods.Capacity)
                    {
                        magicNumber = 0;
                    }
                }
                else if (allTehUnloks[i] == true && allTehEquips[i] == false)
                {
                    magicNumber++;
                    if (magicNumber == allTehNoods.Capacity)
                    {
                        magicNumber = 0;
                    }
                }
                else
                    break;
            }


            if(magicNumber == lightSwordNumba)
            {
                SwordEffects(true);
            }
            else
            {
                SwordEffects(false);
            }
        }else if(Input.GetButtonDown("Fire2"))
        {
            magicNumber = 0;
            if (magicNumber == lightSwordNumba)
            {
                SwordEffects(true);
            }
            else
            {
                SwordEffects(false);
            }
        }
        grayNoodle = allTehNoods[magicNumber];

        if(Input.GetAxisRaw("Vertical") > 0)
        {
            lookingUp = true;
            anime.SetBool("LookingUp", true);
        }
        else if(Input.GetAxisRaw("Vertical") < 0)
        {
            lookingDown = true;
            anime.SetBool("LookingUp", false);
        }
        else
        {
            lookingUp = false;
            anime.SetBool("LookingUp", false);
            lookingDown = false;
        }


        if (curMana <= 1)
        {
            lightCharge = false;
        }


        if (lightCharge == false)
        {
            if(curMana >= 10)
            {
                lightCharge = true;
            }
        }

        if(Input.GetButtonDown("Fire1") && timeToNextFire < Time.time &&  grayNoodle.name != "Eceltro" && !movementChecks.isDashins && canAttack)
        {
            if (grayNoodle.name == "Gravity Slamity")
            {
                if(movementChecks.IsGrounds())
                {
                    curMana -= manaCost;
                    timeToNextFire = Time.time + fireRate;
                    anime.SetTrigger("Slammy");
                }
            }else if (grayNoodle.name == "Dark Org")
            {
                if (DarkOrg.IsOrbSpace())
                {
                    curMana -= manaCost;
                    timeToNextFire = Time.time + fireRate;
                    ShootDarkOrg();
                }
            }else if(grayNoodle.name == "Ghoaft")
            {
                curMana -= manaCost;
                timeToNextFire = Time.time + fireRate;
                anime.SetTrigger("SwordSwong");
            }
            else
            {
                DeductMana();
                timeToNextFire = Time.time + fireRate;
                ShootGaryNoodle();
            }
        }

        if(Input.GetButton("Fire1") && curMana >= manaCost * Time.deltaTime && grayNoodle.name == "Eceltro" && lightCharge && !movementChecks.isDashins && canAttack)
        {
            curMana -= manaCost * Time.deltaTime;
            Lightning();
            if(!FindObjectOfType<AudioManager>().IsPlaying("LIGHT"))
            FindObjectOfType<AudioManager>().Play("LIGHT");
        }
        else
        {
            FindObjectOfType<AudioManager>().Stop("LIGHT");
            if (lightnignSpawn)
                Destroy(lightnignSpawn);
            lightnignSpawn = null;
            if (hitSpawn)
                Destroy(hitSpawn);
            hitSpawn = null;
            anime.SetBool("WizardLightning", false);
            movementChecks.ChangeLightningForces(0);
            if(grayNoodle.name == "Eceltro")
            {
                Magicals.SetActive(true);
            }
        }

        if (!isFires && isFires != anime.GetBool("IsFiring") && !anime.GetBool("IsSword"))
        {
            anime.SetBool("IsFiring", isFires);
            Magicals.SetActive(true);
        }

        switch (grayNoodle.name)
        {
            case "Wizard Blast":
                fireRate = .2f;
                ChangeMaterial("Wizard Blast");
                animeMagic.SetInteger("Magic Number", 0);
                lightColor.color = new Color(1, 1, 1);
                lightColor2.color = new Color(1, 1, 1);
                manaCost = 7f;
                spood = 750;
                break;
            case "Flame Ball":
                fireRate = .5f;
                ChangeMaterial("Flame Ball");
                animeMagic.SetInteger("Magic Number", 1);
                lightColor.color = new Color(1, .5f, .5f);
                lightColor2.color = new Color(1, .5f, .5f);
                manaCost = 20f;
                spood = 750;
                break;
            case "Eceltro":
                fireRate = .5f;
                ChangeMaterial("Lightning Blue");
                animeMagic.SetInteger("Magic Number", 2);
                lightColor.color = new Color(.2f, 1f, 1f);
                lightColor2.color = new Color(.2f, 1f, 1f);
                manaCost = 40f;
                spood = 750;
                break;
            case "Water Speer":
                fireRate = .3f;
                ChangeMaterial("Water World");
                animeMagic.SetInteger("Magic Number", 3);
                lightColor.color = new Color(.5f, .5f, 1f);
                lightColor2.color = new Color(.5f, .5f, 1f);
                manaCost = 15f;
                spood = 750;
                break;
            case "Dark Org":
                fireRate = .2f;
                ChangeMaterial("Dark Magician");
                animeMagic.SetInteger("Magic Number", 4);
                lightColor.color = new Color(1f, .4f, 1);
                lightColor2.color = new Color(1f, .4f, 1);
                manaCost = 20f;
                spood = 750;
                break;
            case "Gravity Slamity":
                fireRate = .4f;
                ChangeMaterial("Gravity Maybe");
                animeMagic.SetInteger("Magic Number", 5);
                lightColor.color = new Color(1f, .6f, .6f);
                lightColor2.color = new Color(1f, .6f, .6f);
                manaCost = 10f;
                spood = 750;
                break;
            case "Ghoaft":
                fireRate = .2f;
                ChangeMaterial("Sunlight Slash");
                animeMagic.SetInteger("Magic Number", 6);
                lightColor.color = new Color(1f, 1f, .5f);
                lightColor2.color = new Color(1f, 1f, .5f);
                manaCost = 10f;
                spood = 750;
                break;
            case "IceArrow":
                fireRate = .35f;
                ChangeMaterial("Wizard Blast");
                animeMagic.SetInteger("Magic Number", 7);
                lightColor.color = new Color(.3f, 1f, 1f);
                lightColor2.color = new Color(.3f, 1f, 1f);
                manaCost = 12f;
                spood = 1300;
                break;

        }
    }

    void Lightning()
    {
        movementChecks.ChangeLightningForces(-100);
        Magicals.SetActive(false);
        isFires = true;
        anime.SetBool("WizardLightning", true);
        if (lightnignSpawn == null)
        {
            lightnignSpawn = Instantiate(grayNoodle, magicBoneZoneLightning.position, Quaternion.identity);
            if (transform.localScale.x < 0)
                lightnignSpawn.transform.localScale = flippedX;
            lightnignSpawn.transform.SetParent(transform);
        }

        if (transform.localScale.x > 0)
        {
            RaycastHit2D hitBoi = Physics2D.BoxCast(magicBoneZoneHigher.position, new Vector2(.1f, .4f), 0, Vector2.right, marksman, (1 << LayerMask.NameToLayer("Enemy") | (1 << LayerMask.NameToLayer("Ground") | (1 << LayerMask.NameToLayer("PuzzleKeys")))));
            //RaycastHit2D hitBoi = Physics2D.BoxCast(magicBoneZoneHigher.position, Vector2.right, marksman, (1 << LayerMask.NameToLayer("Enemy") | (1 << LayerMask.NameToLayer("Ground"))));
            if (hitBoi)
            {
                Instantiate(rTape, hitBoi.point, Quaternion.identity);
                if (hitSpawn == null)
                    hitSpawn = Instantiate(hitEffectLight, hitBoi.point, Quaternion.identity);
                else
                    hitSpawn.transform.position = hitBoi.point;

                ITakeDamage interaction1 = hitBoi.collider.GetComponent<ITakeDamage>();
                interaction1?.TakeDamage(lightDam * Time.deltaTime, 0, 0, ElementType.Electric);
            }
            else
            {
                Destroy(hitSpawn);
                hitSpawn = null;
            }
        }
        else
        {
            RaycastHit2D hitBoi = Physics2D.BoxCast(magicBoneZoneHigher.position, new Vector2(.1f, .5f), 0, Vector2.left, marksman, (1 << LayerMask.NameToLayer("Enemy") | (1 << LayerMask.NameToLayer("Ground") | (1 << LayerMask.NameToLayer("PuzzleKeys")))));
            //RaycastHit2D hitBoi = Physics2D.Raycast(transform.position, Vector2.left, marksman, (1 << LayerMask.NameToLayer("Enemy") | (1 << LayerMask.NameToLayer("Ground"))));
            if (hitBoi)
            {
                Instantiate(lTape, hitBoi.point, Quaternion.identity);
                if (hitSpawn == null)
                {
                    hitSpawn = Instantiate(hitEffectLight, hitBoi.point, Quaternion.identity);
                    hitSpawn.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                { 
                    hitSpawn.transform.position = hitBoi.point;
                    hitSpawn.GetComponent<SpriteRenderer>().flipX = true;
                }
                ITakeDamage interaction1 = hitBoi.collider.GetComponent<ITakeDamage>();
                interaction1?.TakeDamage(lightDam * Time.deltaTime, 0, 0, ElementType.Electric);
                //Debug.Log(hitBoi.collider.name);
                //Debug.Log(hitBoi.point);
            }
            else
            {
                Destroy(hitSpawn);
                hitSpawn = null;
            }
        }
    }

    void Slam()
    {
        Instantiate(grayNoodle,gameObject.transform.position, Quaternion.identity);
    }

    void LightSwordSlash()
    {
        movementChecks.inputoLock = movementChecks.inputo;
        if (transform.localScale.x > 0)
        {
            RaycastHit2D hitBoi = Physics2D.BoxCast(magicBoneZone.position, new Vector2(1f, 1f), 0, Vector2.right, swordMaster, (1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("PuzzleKeys")));
            ITakeDamage interaction1 = hitBoi.collider.GetComponent<ITakeDamage>();
            interaction1?.TakeDamage(lightDam, 0, 0, ElementType.Light);
        }
        else
        {
            RaycastHit2D hitBoi = Physics2D.BoxCast(magicBoneZone.position, new Vector2(1f, 1f), 0, Vector2.left, swordMaster, (1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("PuzzleKeys")));
            ITakeDamage interaction1 = hitBoi.collider.GetComponent<ITakeDamage>();
            interaction1?.TakeDamage(lightDam, 0, 0, ElementType.Light);
        }
    }

    void ShootGaryNoodle()
    {
        FindObjectOfType<AudioManager>().Play2("BWER");
        Magicals.SetActive(false);
        isFires = true;
        anime.SetBool("IsFiring", true);

        if (!lookingUp && !lookingDown)
        {
            GameObject garyNoodle = Instantiate(grayNoodle, magicBoneZone.position, Quaternion.identity);
            garyNoodle.GetComponent<SpriteRenderer>().material = prettyColors.material;

            if (transform.localScale.x > 0)
                garyNoodle.GetComponent<Rigidbody2D>().AddForce(Vector2.right * spood);
            else
            {
                garyNoodle.transform.localScale = flippedX;
                garyNoodle.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -spood);
            }
            Destroy(garyNoodle, 5f);
        }else if(lookingUp)
        {
            GameObject garyNoodle = Instantiate(grayNoodle, magicBoneZoneTop.position, Quaternion.Euler(0,0,90));
            garyNoodle.GetComponent<SpriteRenderer>().material = prettyColors.material;
            garyNoodle.GetComponent<Rigidbody2D>().AddForce(Vector2.up * spood);
        }else if(lookingDown)
        {
            if (movementChecks.IsGrounds())
            {
                GameObject garyNoodle = Instantiate(grayNoodle, magicBoneZoneCrouched.position, Quaternion.identity);
                garyNoodle.GetComponent<SpriteRenderer>().material = prettyColors.material;

                if (transform.localScale.x > 0)
                    garyNoodle.GetComponent<Rigidbody2D>().AddForce(Vector2.right * spood);
                else
                {
                    garyNoodle.transform.localScale = flippedX;
                    garyNoodle.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -spood);
                }
                Destroy(garyNoodle, 5f);
            }
            else
            {
                GameObject garyNoodle = Instantiate(grayNoodle, magicBoneZoneBottom.position, Quaternion.Euler(0, 0, -90));
                garyNoodle.GetComponent<SpriteRenderer>().material = prettyColors.material;
                garyNoodle.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * spood);
            }

        }
    }

    void ShootDarkOrg()
    {

        int orbPos = DarkOrg.NullOrbCheck();

        lookingUp = true;
        anime.SetBool("LookingUp", true);

        Magicals.SetActive(false);
        isFires = true;
        anime.SetBool("IsFiring", true);


        switch (orbPos)
        {
            case 0:
                    Instantiate(grayNoodle, magicBoneZoneTop.position, Quaternion.Euler(0, 0, 90));
                break;
            case 1:
                if (transform.localScale.x == 2)
                {
                    Instantiate(grayNoodle, magicBoneZoneBehind.position, Quaternion.Euler(0, 0, 90));
                }
                else
                {
                    Instantiate(grayNoodle, magicBoneZone.position, Quaternion.Euler(0, 0, 90));
                }
                break;
            case 2:
                if (transform.localScale.x == 2)
                {
                    Instantiate(grayNoodle, magicBoneZone.position, Quaternion.Euler(0, 0, 90));
                }
                else
                {
                    Instantiate(grayNoodle, magicBoneZoneBehind.position, Quaternion.Euler(0, 0, 90));
                }
                break;
            case 3:
                if (transform.localScale.x == 2)
                {
                    Instantiate(grayNoodle, magicBoneZoneBehind.position, Quaternion.Euler(0, 0, 90));
                }
                else
                {
                    Instantiate(grayNoodle, magicBoneZone.position, Quaternion.Euler(0, 0, 90));
                }
                break;
            case 4:
                if (transform.localScale.x == 2)
                {
                    Instantiate(grayNoodle, magicBoneZone.position, Quaternion.Euler(0, 0, 90));
                }
                else
                {
                    Instantiate(grayNoodle, magicBoneZoneBehind.position, Quaternion.Euler(0, 0, 90));
                }
                break;
            case -1:
                Debug.Log("Failure");
                break;
        }
        


    }

    public void AddANood(int noodNum)
    {
        allTehUnloks[noodNum] = true;
    }
    
    void ChangeMaterial(string material)
    {
        foreach (Material matteName in allTehMatts)
        {
            if (matteName.name == material)
            {
                prettyColors.material = matteName;
                spellColors.material = matteName;
            }
        }

    }//brotato

    public void ManaBegone(float manCost)
    {
        curMana -= manCost * Time.deltaTime;
    }

    public void turnOffMagic()
    {
        Magicals.SetActive(false);
    }

    public bool ManaDrain(float manoCost)
    {
        if(manoCost < curMana)
        {
            curMana -= manoCost;
            return true;
        }
        else
        {
            return false;
        }
    }

    void DeductMana()
    {
        if(curMana - manaCost > 0)
        {
            curMana -= manaCost;
        }else
        {
            if (curMana > 0)
            {
                healthy.TakeSoftDamage((curMana - manaCost) * -1);
                curMana -= manaCost;
            }
            else
            {
                healthy.TakeSoftDamage(manaCost);
                curMana -= manaCost/7;
            }
        }
    }

    public void IncreaseMaxMana(int IncreaseAMT)
    {
        maxMana += IncreaseAMT;
        if(manaBar)
        {
            manaBar.SetMaxHealth(maxMana);
            manaBar.SetHealth(maxMana);
        }
    }

    public void ManaUp(int IncreaseAMT)
    {
        curMana += IncreaseAMT;
        if (curMana > maxMana)
            curMana = maxMana;
    }

    public void SetAttack(bool boo)
    {
        canAttack = boo;
    }

    public bool SetElements(int i)
    {
        allTehEquips[i] = !allTehEquips[i];
        return allTehEquips[i];
    }

    public bool GetElement(int i)
    {
        return allTehEquips[i];
    }

    public bool GetUnlocks(int i)
    {
        return allTehUnloks[i];
    }
   
    public void SwordEffects(bool on)
    {
        if(on == true)
        {
            anime.SetBool("IsSword", true);
            Magicals.SetActive(false);
        }
        else
        {
            anime.SetBool("IsSword", false);
            Magicals.SetActive(true);
        }
    }
}
