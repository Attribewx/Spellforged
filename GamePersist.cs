using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePersist : MonoBehaviour
{
    public static GamePersist instance;
    SaveData _saveData = new SaveData();
    public Inventory inventorys;
    public EnemyManager beatBoxin;
    public LoadArea loadArea;
    public Movement movement;
    public Attacks attack;
    public GameObject mainChar;
    public Health healths;
    public HealthBar[] barsMan;
    public HealthBar heathBar;
    public InventoryController invController;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        invController = FindObjectOfType<InventoryController>();
        inventorys = FindObjectOfType<Inventory>();
        beatBoxin = FindObjectOfType<EnemyManager>();
        loadArea = FindObjectOfType<LoadArea>();
        movement = FindObjectOfType<Movement>();
        attack = FindObjectOfType<Attacks>();
        mainChar = FindObjectOfType<Movement>().gameObject;
        healths = mainChar.GetComponent<Health>();
        barsMan = FindObjectsOfType<HealthBar>();
        for (int i = 0; i < barsMan.Length; i++)
        {
            if(barsMan[i].gameObject.name == "HealthBar")
            {
                heathBar = barsMan[i];
            }
        }
    }

    public void Load()
    {
        Debug.Log("we are loadin");
        using (StreamReader streamReader = new StreamReader($"SaveGame.json"))
        {
            var json = streamReader.ReadToEnd();
            _saveData = JsonUtility.FromJson<SaveData>(json);
            inventorys.items = _saveData.inventoryData;
            inventorys.equipment = _saveData.inventoryEquips;
            beatBoxin.deadBoies = _saveData.deadBoies;
            movement.pointCrow = _saveData.levelName;
            attack.allTehUnloks = _saveData.waponUnbocks;
            attack.magicNumber = _saveData.spellNumber;
            healths.helf = _saveData.health;
            beatBoxin.deadBaus = _saveData.bossyBoies;
        }

        if (attack.magicNumber == attack.lightSwordNumba)
        {
            attack.anime.SetBool("IsSword", true);
            attack.anime.Play("WizardIdleSword");
            attack.anime.SetBool("IsFiring", false);
            Debug.Log(attack.Magicals.name);
            attack.Magicals.SetActive(false);
        }
        movement.enabled = true;
        attack.enabled = true;
        heathBar.SetHealthStart(healths.helf);
        invController.loadRunes();
        loadArea.LoadsSpecLevel(_saveData.levelNombre);
    }

    public void Save()
    {
        Debug.Log("we are savin");
        _saveData.inventoryData = inventorys.items;
        _saveData.inventoryEquips = inventorys.equipment;
        _saveData.deadBoies = beatBoxin.deadBoies;
        _saveData.levelName = movement.pointCrow;
        _saveData.levelNombre = SceneManager.GetActiveScene().name;
        _saveData.waponUnbocks = attack.allTehUnloks;
        _saveData.spellNumber = attack.magicNumber;
        _saveData.health = healths.helf;
        _saveData.bossyBoies = beatBoxin.deadBaus;

        var json = JsonUtility.ToJson(_saveData);

        using (StreamWriter streamWriter = new StreamWriter($"SaveGame.json"))
        {
            streamWriter.Write(json);
        }
        
    }

    public void UpdateReferences()
    {
        loadArea = FindObjectOfType<LoadArea>();
    }
}
