using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, ITakeDamage
{
    public int maxHealth = 500;
    public float helf = 5;
    public ElementType weakness;
    public float weaknessMulti = 2;
    public bool isDead = false;
    public bool isOff = false;
    public Animator death;
    public bool isInvulnerable = false;
    public float invisibleTime = 1f;
    private float timeLeftForInvinsibilty;
    public bool isHurting;
    Material matte;
    public float alphaChange = 3f;
    private bool wasAdded = false;
    public string[] names;
    private string[] bosses;

    public HealthBar healthBar;
    public SpriteRenderer drPeps;

    public bool isLoading;
    public float loadTimer = .2f;
    public float loadTime;
    public bool wantsColorDeath;

    public Animator death2;
    private Inventory inv;
    [SerializeField] private bool isBoss = false;
    [SerializeField] private float bossKillHealthBarDelay = 1;
    private BossHealthBar bHB;
    private bool forgivenessRune = false;

    // Start is called before the first frame update
    void Start()
    {
        if(isBoss)
        {
            bHB = FindObjectOfType<BossHealthBar>();
        }
        names = GameObject.FindGameObjectWithTag("Enemy Manager").GetComponent<EnemyManager>().deadBoies;
        bosses = GameObject.FindGameObjectWithTag("Enemy Manager").GetComponent<EnemyManager>().deadBaus;
        if(death == null)
        death = transform.GetComponent<Animator>();
        matte = GetComponent<SpriteRenderer>().material;
        foreach (string nombre in names)
        {
            if(nombre == gameObject.name)
            {
                gameObject.SetActive(false);
            }
        }

        foreach (string nombre in bosses)
        {
            if (nombre == gameObject.name)
            {
                gameObject.SetActive(false);
            }
        }

        if (gameObject.tag == "Player")
        {
            helf = maxHealth;
            if(healthBar != null)
            {
                healthBar.SetMaxHealth(maxHealth);
                healthBar.SetHealth(helf);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(helf <= 0)
        {
            if(isBoss)
            {
                StartCoroutine(TurnBossBarOff());
            }

            SelfDestruct();

            if (wantsColorDeath)
            {

                if(matte.color.r > 0)
                {
                    matte.SetColor("_Color", new Color(matte.color.r - Time.deltaTime * alphaChange, matte.color.g, matte.color.b, 0));
                }
                if (matte.color.g > 0)
                {
                    matte.SetColor("_Color", new Color(matte.color.r, matte.color.g - Time.deltaTime * alphaChange, matte.color.b, 0));
                }
                if (matte.color.b > 0)
                {
                    matte.SetColor("_Color", new Color(matte.color.r, matte.color.g, matte.color.b - Time.deltaTime * alphaChange, 0));
                }
            }

            if(gameObject.tag == "Enemy" && !wasAdded)
            {
                FindObjectOfType<EnemyManager>().AddDeadBoi(gameObject);
                wasAdded = true;
            }

            if (gameObject.tag == "Boss" && !wasAdded)
            {
                FindObjectOfType<EnemyManager>().AddDeadBoiss(gameObject);
                wasAdded = true;
            }

        }

        if(isOff)
        {
            gameObject.SetActive(false);
        }

        if(gameObject.tag == "Player")
        { 
            death.SetBool("isHurt", isHurting);
        }

        //Sets the invincibility time at the end of the frames
        if(isHurting)
        {
            timeLeftForInvinsibilty = Time.time + invisibleTime;
        }

        if (gameObject.tag == "Player")
        {
            if (isInvulnerable)
            {
                Physics2D.IgnoreLayerCollision(9, 14, true);
            }
            else
            {
                Physics2D.IgnoreLayerCollision(9, 14, false);
            }
        }

        if(timeLeftForInvinsibilty < Time.time)
        {
            isInvulnerable = false;
        }

        //temp fix for transition pain
        if(loadTime < Time.time)
        {
            isLoading = false;
        }
    }



    public void TakeDamage(float damage, float knockX, float knockY, ElementType elementDamage)
    {
        if(!isInvulnerable && !isLoading && elementDamage == weakness && elementDamage != ElementType.Physical)
        {
            if (Inventory.instance.FindEquippedRune(Runes.Rune_of_Forgiveness) && transform.tag == "Player" && helf - damage <= 0 && !forgivenessRune)
            {
                helf = 1;
                forgivenessRune = true;
            }
            else
            {
                helf -= damage * weaknessMulti;
            }
        }
        else if(!isInvulnerable && !isLoading)
        {
            if (Inventory.instance.FindEquippedRune(Runes.Rune_of_Forgiveness) && transform.tag == "Player" && helf - damage <= 0 && !forgivenessRune)
            {
                helf = 1;
                forgivenessRune = true;
            }
            else
            {
                helf -= damage;
            }
        }
        if(transform.tag == "Player" && !isInvulnerable && !isLoading)
        {
            isInvulnerable = true;
            isHurting = true;
            gameObject.GetComponent<Movement>().AddKnockback(knockX, knockY);
            healthBar.SetHealth(helf);
            StartCoroutine(Clyde(invisibleTime));
            gameObject.GetComponent<Movement>().airSpeed = 0;
            gameObject.GetComponent<Movement>().jumpKeyDown = false;
        }
        if(isBoss)
        {
            if (!bHB)
                bHB = FindObjectOfType<BossHealthBar>();
            if (elementDamage == weakness && elementDamage != ElementType.Physical)
                bHB.setBar(damage * weaknessMulti);
            else

                bHB.setBar(damage);
        }
    }

    public void TakeSoftDamage(float dam)
    {
        if (!isInvulnerable && !isLoading)
        {
            helf -= dam;
            healthBar.SetHealth(helf);
        }
    }

    public void HealUp(float heal)
    {
        helf += heal;
        if(helf > maxHealth)
        {
            helf = maxHealth;
        }
        healthBar.SetHealth(helf);
    }

    void SelfDestruct()
    {
        isDead = true;
        if (death != null)
        {
            death.SetBool("IsDead", true);
            if(death2)
            {
                death2.SetBool("IsDead", true);
            }
        }
    }

    IEnumerator Clyde(float inviz)
    {
        float timeToFade = inviz / 8;
        for (int i = 0; i < 4; i++)
        {
            drPeps.color = Vector4.Lerp(drPeps.color, Color.black, 1f);
            yield return new WaitForSeconds(timeToFade);
            drPeps.color = Vector4.Lerp(drPeps.color, Color.white, 1f);
            yield return new WaitForSeconds(timeToFade);
        }
    }

    public void IncreaseMaxHealth(int increaseAMT)
    {
        maxHealth += increaseAMT;
        healthBar.SetMaxHealth(maxHealth);
    }

    public IEnumerator TurnBossBarOff()
    {
        yield return new WaitForSeconds(bossKillHealthBarDelay);
        bHB.UninitializeBar();
    }
}
