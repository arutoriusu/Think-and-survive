using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject buttonsList;
    [SerializeField] GameObject savesObject;
    [SerializeField] GameObject titleObject;
    [SerializeField] GameObject saveLoad;
    [SerializeField] GameObject newSave;

    private void Open()
    {
        titleObject.GetComponent<TextMeshProUGUI>().text = "Menu";
        buttonsList.SetActive(true);
        savesObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!transform.GetChild(0).gameObject.activeSelf)
            {
                Open();
                transform.GetChild(0).gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                Close();
            }
            
        }
    }

    public void Close()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        DeleteSavesObjects();
        Time.timeScale = 1;
    }

    private void DeleteSavesObjects()
    {
        for (int i = 0; i < savesObject.transform.childCount; i++)
        {
            if (savesObject.transform.GetChild(i).gameObject != newSave)
            {
                Destroy(savesObject.transform.GetChild(i).gameObject);
            }
        }
    }

    public void OpenSavesToSave()
    {
        newSave.SetActive(true);
        buttonsList.SetActive(false);
        savesObject.SetActive(true);
        titleObject.GetComponent<TextMeshProUGUI>().text = "Save";
        LoadSavesNames();
    }

    public void OpenSavesToLoad()
    {
        newSave.SetActive(false);
        buttonsList.SetActive(false);
        savesObject.SetActive(true);
        titleObject.GetComponent<TextMeshProUGUI>().text = "Load";
        LoadSavesNames();
    }

    private void LoadSavesNames()
    {
        foreach (string item in Directory.GetDirectories(Application.persistentDataPath))
        {
            int indexSlash = item.LastIndexOf("/");
            int indexSlash2 = item.LastIndexOf("\\");
            string nameFile = item.Substring(Math.Max(indexSlash, indexSlash2) + 1);
            var saveObject = Instantiate(newSave);
            saveObject.transform.SetParent(savesObject.transform);
            saveObject.transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0);
            saveObject.transform.localScale = new Vector3(1, 1, 1);
            saveObject.SetActive(true);
            saveObject.GetComponentInChildren<TextMeshProUGUI>().text = nameFile;
        }
    }

    public void SaveOrLoad(GameObject save)
    {
        if (titleObject.GetComponent<TextMeshProUGUI>().text == "Save")
        {
            string nameFile = DateTime.Now.ToString().Replace(":", "_").Replace(" ", "_");
            if (save != newSave)
            {
                nameFile = save.GetComponentInChildren<TextMeshProUGUI>().text.Substring(
                            0,
                            save.GetComponentInChildren<TextMeshProUGUI>().text.IndexOf(" ")
                           )
                    + " " + nameFile;
                DeleteSave(save.GetComponentInChildren<TextMeshProUGUI>().text);
            }
            else
            {
                int countSaves = PlayerPrefs.GetInt("countSaves") + 1;
                nameFile = countSaves + " " + nameFile;
                PlayerPrefs.SetInt("countSaves", countSaves);
                /*var saveObject = Instantiate(save);
                saveObject.transform.SetParent(savesObject.transform);*/
            }
            saveLoad.GetComponent<SaveLoad>().SaveGame(nameFile);
            DeleteSavesObjects();
            LoadSavesNames();
        } else if (titleObject.GetComponent<TextMeshProUGUI>().text == "Load")
        {
            saveLoad.GetComponent<SaveLoad>().LoadGame(save.GetComponentInChildren<TextMeshProUGUI>().text);
        }
    }

    private void DeleteSave(string text)
    {
        Directory.Delete(Application.persistentDataPath
                + "/" + text, true);
    }

    public void MainMenu()
    {
        DeleteSavesObjects();
        titleObject.GetComponent<TextMeshProUGUI>().text = "Menu";
        buttonsList.SetActive(true);
        savesObject.SetActive(false);
    }
}
