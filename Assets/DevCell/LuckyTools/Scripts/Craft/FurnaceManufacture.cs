using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceManufacture : MonoBehaviour
{
    int readiness;
    GameObject smoke;
    public int Readiness { get => readiness; set => readiness = value; }

    private void Start()
    {
        Readiness = 0;
        InvokeRepeating("FurnaceTick", 0f, 2f); 
    }

    public void FurnaceTick()
    {
        if (transform.GetChild(2).childCount > 0)
        {
            if (!smoke)
            {
                smoke = Instantiate(GameManager.FurnaceSmokePrefabStatic);
                smoke.transform.SetParent(gameObject.transform, false);
            }
            Readiness += 1;
            if (transform.GetChild(2).GetChild(0).GetComponent<Meltable>().Value > 0)
            {
                transform.GetChild(2).GetChild(0).GetComponent<Meltable>().Value -= 1;
            }
            else
            {
                Destroy(transform.GetChild(2).GetChild(0));
            }
        }
        else
        {
            Destroy(smoke);
            smoke = null;
        }

        if (Readiness == 20)
        {
            bool done = gameObject.GetComponent<CraftMechanism>().TryUseRecipe();
            if (done)
            {
                Readiness = 0;
            }
        }
    }

}
