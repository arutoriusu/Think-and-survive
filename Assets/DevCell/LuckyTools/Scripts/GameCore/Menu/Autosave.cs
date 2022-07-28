using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autosave : MonoBehaviour
{
    private int timeForAutosave = 20;
    private float lastSave;

    [SerializeField] GameObject saveLoad;

    private void Start()
    {
        lastSave = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastSave + timeForAutosave < Time.time)
        {
            saveLoad.GetComponent<SaveLoad>().AutoSaveGame();
            lastSave = Time.time;
        }
    }
}
