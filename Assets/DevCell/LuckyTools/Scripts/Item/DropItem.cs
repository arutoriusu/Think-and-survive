using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    private Dictionary<int, Dictionary<string, float>> _Drops;
    [SerializeField] List<int> numsDropItems;
    [SerializeField] List<string> _Keys;
    [SerializeField] List<float> _Values;
    [SerializeField] int numItemDropOnDestroying;

    private bool eggDropped = false;
    private bool featherDropped = false;

    // Start is called before the first frame update
    void Start()
    {
        // dropChance, droppingTime, periodForDropping
        Dictionary<string, float> _Dictionary = new Dictionary<string, float>();
        _Drops = new Dictionary<int, Dictionary<string, float>>();

        int dictNum = 0;
        for (int i = 0; i < _Keys.Count; i++)
        {
            if (i + 1 % 3 == 0)
            {
                dictNum += 1;
            }
            _Dictionary[_Keys[i]] = _Values[i];
            _Drops[numsDropItems[dictNum]] = _Dictionary;
            
        }

        foreach (var dict in _Drops)
        {
            for (int i = 0; i < dict.Value.Values.Sum(); i++)
            {
                dict.Value["periodForDropping"] = Time.time;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyValuePair<int, Dictionary<string, float>> entry in _Drops)
        {
            if (entry.Value["periodForDropping"] + entry.Value["droppingTime"] <= Time.time &&
                Random.Range(0, 101) > entry.Value["dropChance"])
            {
                if (AllowSpawn())
                {
                    GameObject myItem = GameManager.CreateItem(entry.Key, transform.position);
                    GameManager.IncreaseCountObjectsInWorld(entry.Key.ToString());
                }

                entry.Value["periodForDropping"] = Time.time;

            }

            if (gameObject.GetComponent<Animal>().Health <= 0 && !eggDropped)
            {
                if (Random.Range(0, 101) > entry.Value["dropChance"])
                {
                    GameObject myItem = GameManager.CreateItem(entry.Key, transform.position);
                    eggDropped = true;
                }
            }
        }
        if (gameObject.GetComponent<Animal>().Health <= 0 && !featherDropped)
        {
            GameObject dropItem = GameManager.CreateItem(numItemDropOnDestroying, transform.position);
            featherDropped = true;
        }

    }

    private bool AllowSpawn()
    {
        if (gameObject.name.ToLower().Contains("duck"))
        {
            if (GameManager.CountEggs < 3)
            {
                return true;
            }
        }
        return false;
    }
}
