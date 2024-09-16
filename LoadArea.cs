using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadArea : MonoBehaviour
{
    public Animator transition;
    public string levelNamo;
    public float transTime = 1f;
    AsyncOperation asyncOperation;
    AsyncOperation asyncOperationTwo;

    public string appointedKnight;
    private Attacks attacks;
    private Movement plato;
    private GameObject[] platos;
    private Health invul;
    private Rigidbody2D rigi;
    private BossHealthBar bHB;
    public bool addsForcesUp;
    public bool hasEntered;

    List<AsyncOperation> scenesLoad = new List<AsyncOperation>();

    void Start()
    {
        bHB = FindObjectOfType<BossHealthBar>();
        attacks = GameObject.FindGameObjectWithTag("Player").GetComponent<Attacks>();
        plato = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        platos = GameObject.FindGameObjectsWithTag("Player");
        invul = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        rigi = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        GamePersist.instance.UpdateReferences();
        if(SceneManager.GetActiveScene().name != "Main Menu")
        {
            plato.enabled = true;
            attacks.enabled = true;
            StartCoroutine(LateStart());
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
            if(addsForcesUp)
            {
                hasEntered = true;
                rigi.velocity = Vector2.up * 10;
            }        
    }

    void OnTriggerEnter2D(Collider2D colio)
    {
        if(colio.tag == "Player")
        {
            plato.jumpKeyDown = false;
            plato.pointCrow = appointedKnight;
            Debug.Log(appointedKnight);
            plato.enabled = false;
            attacks = FindObjectOfType<Attacks>();
            attacks.enabled = false;
            LoadsLevel();
        }
    }

    public void NewGame()
    {
        attacks = FindObjectOfType<Attacks>();
        attacks.enabled = true;
        plato.enabled = true;
        plato.pointCrow = "Left TS";
        StartCoroutine(LoadLevel3());
    }

    public void LoadsLevel()
    {
        StartCoroutine(LoadLevel3());
        //StartCoroutine(LoadLevel(levelNamo));
        //MySceneManager.LoadScene(levelNamo,this);
    }

    public void LoadsSpecLevel(string specLevel)
    {
        if(specLevel != null)
        {
            StartCoroutine(LoadSpecLevel3(specLevel));
        }
        else
        {
            plato.pointCrow = "Left TS";
            StartCoroutine(LoadLevel3());
        }
    }

    IEnumerator LoadLevel(string levelNoma)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transTime);


    }

    IEnumerator LoadLevel2()
    {
        invul.isLoading = true;
        invul.loadTime = Time.time + invul.loadTimer;
        string currentScene = SceneManager.GetActiveScene().name;
        asyncOperation = SceneManager.LoadSceneAsync(levelNamo, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = true;
        yield return asyncOperation;
        SceneManager.UnloadSceneAsync(currentScene);
    }

    IEnumerator LoadSpecLevel3(string levelCasa)
    {
        invul.isLoading = true;
        invul.loadTime = Time.time + invul.loadTimer;
        string currentScene = SceneManager.GetActiveScene().name;
        asyncOperation = SceneManager.LoadSceneAsync(levelCasa, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = true;
        yield return asyncOperation;
        asyncOperationTwo = SceneManager.UnloadSceneAsync(currentScene);
    }

    IEnumerator LoadLevel3()
    {
        transition.SetTrigger("Start");
        if (!bHB)
            bHB = FindObjectOfType<BossHealthBar>();
        if(bHB)
        bHB.UninitializeBar();
        yield return new WaitForSeconds(transTime);
        invul.isLoading = true;
        invul.loadTime = Time.time + invul.loadTimer;
        string currentScene = SceneManager.GetActiveScene().name;
        asyncOperation = SceneManager.LoadSceneAsync(levelNamo, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = true;
        yield return asyncOperation;
        asyncOperationTwo = SceneManager.UnloadSceneAsync(currentScene);
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(.1f);
        foreach (var item in platos)
        {
            if (item)
            {
                Movement move = item.GetComponent<Movement>();
                move.enabled = true;
            }
        }
    }    
}
