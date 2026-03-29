using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class save_slot : MonoBehaviour
{
    public int slotNumber;

    [Header("ui reference")]
    public TextMeshProUGUI statusText;   
    public GameObject nameInputPanel;    
    public TMP_InputField nameInputField;
    public Button deleteButton;          

    void Start()
    {
        RefreshSlotUI();
    }

    public void RefreshSlotUI()
    {
        string path = Application.persistentDataPath + "/save_" + slotNumber + ".json";

        if (File.Exists(path))
        {
            //nacteni jmena ze souboru pro zobrazeni v menu
            string json = File.ReadAllText(path);
            data bonus = JsonUtility.FromJson<data>(json);

            statusText.text = bonus.saveName;
            deleteButton.gameObject.SetActive(true);
        }
        else
        {
            statusText.text = "EMPTY SLOT";
            deleteButton.gameObject.SetActive(false);
        }
    }

    public void OnSlotSelected()
    {
        string path = Application.persistentDataPath + "/save_" + slotNumber + ".json";

        if (File.Exists(path))
        {
            //pokud save existuje nacite ho
            save_manager.instance.LoadGame(slotNumber);
            StartGame();
        }
        else
        {
            nameInputPanel.SetActive(true);

            Button confirmBtn = nameInputPanel.transform.Find("confirm").GetComponent<Button>();
            //smaze stare listenery aby se neukladalo do vic slotu najednou
            confirmBtn.onClick.RemoveAllListeners();
            confirmBtn.onClick.AddListener(ConfirmNewGame);

            nameInputField.text = "";
        }
    }

    public void ConfirmNewGame()
    {
        string name = nameInputField.text;
        if (string.IsNullOrEmpty(name)) return;

        save_manager.instance.ResetManagers();

        //vytvoreni novych dat pro vybrany slot
        save_manager.instance.currentSlot = slotNumber;
        save_manager.instance.currentSave = new data();
        save_manager.instance.currentSave.saveName = name;

        //ulozeni a start hry
        save_manager.instance.SaveGame();
        nameInputPanel.SetActive(false);
        StartGame();
    }

    public void DeleteSlot()
    {
        //smazani souboru
        save_manager.instance.DeleteSave(slotNumber);
        RefreshSlotUI();
    }

    void StartGame()
    {
        //nacte zakladni scenu a prida ui
        SceneManager.LoadScene("base");
        if (modifier_ui.instance == null)
        {
            SceneManager.LoadScene("GlobalUI", LoadSceneMode.Additive);
        }

        //zapnuti hudu pokud existuje manager
        if (manager.instance != null) manager.instance.HUDVisible(true);
    }
}