using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryController : MonoBehaviour
{

    private GameObject player;
    private Attacks attacks;
    public GameObject menu;
    public Text[] butts;
    public RectTransform slides;
    private Vector3 starto;
    private CanvasGroup canvasG;
    private Image[] elements;
    private Button[] elementToggles;
    [SerializeField] private Material matter;
    [SerializeField] private GameObject elementsHolder;
    [SerializeField] private float runeDist = 50;
    [SerializeField] private float equipHeight = 80;
    [SerializeField] private float equipXOffset = -100;

    private float pageNum;
    [SerializeField]private float leanTime = .5f;
    private float leanTimer;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        attacks = player.GetComponent<Attacks>();
        starto = slides.position;
        canvasG = menu.GetComponent<CanvasGroup>();
        elements = elementsHolder.GetComponentsInChildren<Image>();
        elementToggles = elementsHolder.GetComponentsInChildren<Button>();
    }

    void Update()
    {

        if(SceneManager.GetActiveScene().buildIndex != 0)
        {           //NOT THE MAIN MENU\\
            if(menu.activeInHierarchy)
                InventoryViewer();

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!menu.activeInHierarchy)
                {
                    menu.SetActive(true);
                    canvasG.alpha = 0;
                    canvasG.LeanAlpha(1, .3f);
                    attacks.SetAttack(false);
                }
                else
                {
                    canvasG.LeanAlpha(0, .3f).setOnComplete(Enabler);
                    attacks.SetAttack(true);
                }
            }
            if (menu.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.E) && leanTimer < Time.time)
                {
                    ChangeTheUIPage(-1920);
                    leanTimer = Time.time + leanTime;
                }
                if (Input.GetKeyDown(KeyCode.Q) && leanTimer < Time.time)
                {
                    ChangeTheUIPage(1920);
                    leanTimer = Time.time + leanTime;
                }
            }
        }
    }

    public void SetMaxMana()
    {
        attacks.curMana = attacks.maxMana;
    }

    public void InventoryViewer()
    {
        for (int i = 0; i < Inventory.instance.items.Length; i++)
        {
            for (int a = 0; a < butts.Length; a++)
            {
                string butter = butts[a].text;
                if (butter == Inventory.instance.items[i].ToString())
                {
                    butts[a].gameObject.SetActive(true);
                }
            }
        }
        if(Inventory.instance.items.Length <= 0)
        {
            for (int i = 0; i < butts.Length; i++)
            {
                butts[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < attacks.allTehUnloks.Length; i++)
            if (attacks.GetUnlocks(i))
            {
                elementToggles[i].gameObject.SetActive(true);
            }
            else
            {
                elementToggles[i].gameObject.SetActive(false);
            }
    }

    public void UnlockAllElements()
    {
        for (int i = 0; i < attacks.allTehUnloks.Length; i++)
        {
            attacks.allTehUnloks[i] = true;
        }
    }

    public void ChangeTheUIPage(float xPos)
    {
        if(pageNum == 0 && xPos > 0)
        {
            slides.transform.LeanMoveLocalX(xPos, leanTime);
            pageNum--;
        }
        else if(pageNum == 1 && xPos > 0)
        {
            slides.transform.LeanMoveLocalX(0, leanTime);
            pageNum--;
        }
        else if (pageNum == -1 && xPos < 0)
        {
            slides.transform.LeanMoveLocalX(0, leanTime);
            pageNum++;
        }
        else if (pageNum == 0 && xPos < 0)
        {
            slides.transform.LeanMoveLocalX(xPos, leanTime);
            pageNum++;
        }
    }

    public void Enabler()
    {
        menu.SetActive(false);
    }

    public void loadRunes()
    {
        for (int i = 0; i < Inventory.instance.equipment.Length; i++)
        {
            for (int g = 0; g < butts.Length; g++)
            {
                if(Inventory.instance.equipment[i].ToString() == butts[g].text)
                {
                    RectTransform parentObj = butts[g].transform.parent.GetComponent<RectTransform>();
                    butts[g].transform.LeanMoveLocal(-parentObj.localPosition / 3 + new Vector3(equipXOffset + runeDist * i, equipHeight, 0), 0);
                }
            }
        }
    }

    public void EquipRunes(int rune)
    {
        Inventory.instance.EquipRune(rune);

        //for (int i = 0; i < Inventory.instance.equipment.Length; i++)
        //{
        //    for (int z = 0; z < butts.Length; z++)
        //    {
        //        if(Inventory.instance.equipment[i] == rune && butts[z].text == Inventory.instance.equipment[i].ToString())
        //        {
        //            butts[z].gameObject.LeanMove(new Vector3(100 + i * 50, 225, 0), .2f);
        //            return;
        //        }
        //        else if(Inventory.instance.equipment[i])
        //    }
        //}

        ////WORKS BUT TRY TO IMPROVE\\\\

        for (int i = 0; i < Inventory.instance.items.Length; i++)
        {
            for (int a = 0; a < butts.Length; a++)
            {
                string butter = butts[a].text;
                if (butter == Inventory.instance.items[i].ToString() && butter == rune.ToString())
                {
                    for (int c = 0; c < Inventory.instance.equipment.Length; c++)
                    {
                        if (Inventory.instance.items[i].ToString() == Inventory.instance.equipment[c].ToString())
                        {
                            for (int d = 0; d < Inventory.instance.equipment.Length; d++)
                            {
                                    Debug.Log("Equipment at slot " + d + " is: " + Inventory.instance.equipment[d]);
                                if (Inventory.instance.equipment[d] == rune)
                                {
                                    RectTransform parentObj = butts[a].transform.parent.GetComponent<RectTransform>();
                                    butts[a].transform.LeanMoveLocal(-parentObj.localPosition/3 + new Vector3(equipXOffset + runeDist * c, equipHeight,0), .2f);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            butts[a].transform.LeanMoveLocal(Vector3.zero, .2f);
                        }
                    }
                }
            }
        }
    }

    public void EquipElements(int i)
    {
        bool b = attacks.SetElements(i);
        if(b == false)
        {
            elements[i + 1].material = matter;
        }
        else
        {
            elements[i + 1].material = null;
        }
    }
}
