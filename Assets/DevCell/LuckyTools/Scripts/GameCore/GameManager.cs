using GameCore;
using GameCore.Item;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // make spawning separate object
    
    public static HashSet<Vector3> spiderWebCoordinatesAlreadySpawn;

    public Player GetPlayerData(int num)
    {
        if (num == 1)
        {
            return Player1.GetComponent<Player>();
        }
        else if (num == 2)
        {
            return Player2.GetComponent<Player>();
        }
        return null;
    }

    private int duckSpawningTime = 15;
    private float timeForDuckSpawning;
    private int duckSpawningChance = 40;
    private static int countDucks = 0;

    private int spiderWebSpawningTime = 15;
    private float timeForSpiderWebSpawning;
    private int spiderWebSpawningChance = 40;
    private static int countSpiderWeb = 0;

    private int frogSpawningTime = 15;
    private float timeForFrogSpawning;
    private int frogSpawningChance = 40;
    private static int countFrogs = 0;

    private int zombieSpawningTime = 15;
    private float timeForZombieSpawning;
    private int zombieSpawningChance = 40;
    private static int countZombie = 0;

    private Vector3[] grassCoordinates;
    private int numGrass;

    private Vector3[] spiderWebCoordinatesForSpawn;
    private int numSpiderWeb;

    private Vector3[] zombieSpawnCoordinates;
    private int numZombieSpawn;

    private static int countEggs = 0;

    [SerializeField] GameObject duck;
    [SerializeField] GameObject spiderWeb;
    [SerializeField] GameObject frog;
    [SerializeField] GameObject zombie;

    [SerializeField] GameObject newLevelGeneratorScript;
    [SerializeField] GameObject itemsGeneratorScript;
    [SerializeField] GameObject itemAnimatorPrefab;
    [SerializeField] GameObject itemEffectPrefab;
    [SerializeField] GameObject furnaceSmokePrefab;
    [SerializeField] GameObject player1Prefab;
    [SerializeField] GameObject player2Prefab;
    [SerializeField] GameObject hit;
    [SerializeField] GameObject itemsWorld;
    [SerializeField] GameObject creaturesWorld;
    [SerializeField] GameObject dirLight;

    private static GameObject itemAnimatorPrefabStatic;
    private static GameObject itemEffectPrefabStatic;
    private static GameObject furnaceSmokePrefabStatic;
    private static GameObject hitPrefabStatic;
    private static GameObject newLevelGeneratorScriptStatic;
    private static GameObject itemsWorldStatic;
    private GameObject player1;
    private GameObject player2;

    public GameObject ItemsGeneratorScript { get => itemsGeneratorScript; set => itemsGeneratorScript = value; }
    public static int CountDucks { get => countDucks; set => countDucks = value; }
    public static int CountSpiderWeb { get => countSpiderWeb; set => countSpiderWeb = value; }
    public static int CountFrogs { get => countFrogs; set => countFrogs = value; }
    public static int CountZombie { get => countZombie; set => countZombie = value; }
    public static int CountEggs { get => countEggs; set => countEggs = value; }
    public static GameObject FurnaceSmokePrefabStatic { get => furnaceSmokePrefabStatic; set => furnaceSmokePrefabStatic = value; }
    public static GameObject HitPrefabStatic { get => hitPrefabStatic; set => hitPrefabStatic = value; }
    public static GameObject NewLevelGeneratorScriptStatic { get => newLevelGeneratorScriptStatic; set => newLevelGeneratorScriptStatic = value; }
    public GameObject Player1Prefab { get => player1Prefab; set => player1Prefab = value; }
    public GameObject Player2Prefab { get => player2Prefab; set => player2Prefab = value; }
    public GameObject Player1 { get => player1; set => player1 = value; }
    public GameObject Player2 { get => player2; set => player2 = value; }
    public GameObject Zombie { get => zombie; set => zombie = value; }
    public GameObject CreaturesWorld { get => creaturesWorld; set => creaturesWorld = value; }
    public GameObject ItemsWorld { get => itemsWorld; set => itemsWorld = value; }
    public static GameObject ItemsWorldStatic { get => itemsWorldStatic; set => itemsWorldStatic = value; }
    public GameObject Duck { get => duck; set => duck = value; }
    public GameObject Frog { get => frog; set => frog = value; }
    public GameObject DirLight { get => dirLight; set => dirLight = value; }
    public GameObject SpiderWeb { get => spiderWeb; set => spiderWeb = value; }

    private void Start()
    {
        spiderWebCoordinatesAlreadySpawn = new HashSet<Vector3>();

        itemAnimatorPrefabStatic = itemAnimatorPrefab;
        itemEffectPrefabStatic = itemEffectPrefab;
        furnaceSmokePrefabStatic = furnaceSmokePrefab;
        hitPrefabStatic = hit;
        newLevelGeneratorScriptStatic = newLevelGeneratorScript;
        ItemsWorldStatic = itemsWorld;

        player1 = Instantiate(Player1Prefab,
            NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().MostLeftBlockPosition + new Vector3(0, 10, 0),
            Player1Prefab.transform.rotation);
        player2 = Instantiate(Player2Prefab,
            NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().MostLeftBlockPosition + new Vector3(10, 10, 0),
            Player2Prefab.transform.rotation);
        player1.GetComponent<Player>().OtherPlayer = GameObject.Find("Player2(Clone)");
        player2.GetComponent<Player>().OtherPlayer = GameObject.Find("Player1(Clone)");

        numGrass = newLevelGeneratorScript.GetComponent<NewLevelGenerator>().NumGrass;
        grassCoordinates = newLevelGeneratorScript.GetComponent<NewLevelGenerator>().GrassCoordinates;

        numSpiderWeb = newLevelGeneratorScript.GetComponent<NewLevelGenerator>().NumSpiderWeb;
        spiderWebCoordinatesForSpawn = newLevelGeneratorScript.GetComponent<NewLevelGenerator>().SpiderWebCoordinates;

        numZombieSpawn = newLevelGeneratorScript.GetComponent<NewLevelGenerator>().NumZombieSpawn;
        zombieSpawnCoordinates = newLevelGeneratorScript.GetComponent<NewLevelGenerator>().ZombieSpawnCoordinates;

        timeForDuckSpawning = Time.time;
        timeForSpiderWebSpawning = Time.time;
        timeForFrogSpawning = Time.time;
        InvokeRepeating("Spawning", 0f, 5f);
    }

    void Spawning()
    {
        if (SpawnObject("duck", timeForDuckSpawning,
                    duckSpawningTime,
                    CountDucks,
                    duckSpawningChance,
                    numGrass,
                    Duck,
                    grassCoordinates,
                    5))
        {
            timeForDuckSpawning = Time.time;
        }

        if (SpawnObject("spiderweb", timeForSpiderWebSpawning,
                    spiderWebSpawningTime,
                    CountSpiderWeb,
                    spiderWebSpawningChance,
                    numSpiderWeb,
                    SpiderWeb,
                    spiderWebCoordinatesForSpawn,
                    5))
        {
            timeForSpiderWebSpawning = Time.time;
        }

        if (SpawnObject("frog", timeForFrogSpawning,
                    frogSpawningTime,
                    CountFrogs,
                    frogSpawningChance,
                    numGrass,
                    Frog,
                    grassCoordinates,
                    5))
        {
            timeForFrogSpawning = Time.time;
        }

        int maxZombie = 5;
        if (global::DirLight.IsDay)
        {
            maxZombie = 2;
        }
        if (SpawnObject("zombie", timeForZombieSpawning,
                    zombieSpawningTime,
                    CountZombie,
                    zombieSpawningChance,
                    numZombieSpawn,
                    Zombie,
                    zombieSpawnCoordinates,
                    maxZombie))
        {
            timeForZombieSpawning = Time.time;
        }
    }

    public static void RemoveZombie(GameObject zombie)
    {
        Destroy(zombie);
        DecreaseCountObjectsInWorld(zombie.name);
    }

    public static void DecreaseCountObjectsInWorld(string name)
    {
        switch (name.Replace("(Clone)", "").Replace("Item","").ToLower())
        {
            case "duck":
                CountDucks -= 1;
                break;
            case "spiderweb":
                CountSpiderWeb -= 1;
                break;
            case "frog":
                CountFrogs -= 1;
                break;
            case "26":
                CountEggs -= 1;
                break;
            case "zombie":
                CountZombie -= 1;
                break;
        }
    }

    public static void IncreaseCountObjectsInWorld(string name)
    {
        switch (name.Replace("(Clone)", "").Replace("Item", "").ToLower())
        {
            case "duck":
                CountDucks += 1;
                break;
            case "spiderweb":
                CountSpiderWeb += 1;
                break;
            case "frog":
                CountFrogs += 1;
                break;
            case "26":
                CountEggs += 1;
                break;
            case "zombie":
                CountZombie += 1;
                break;
        }
    }

    bool SpawnObject(string objectName,
                     float timeForObjectSpawning, 
                     float objectSpawningTime, 
                     int countObject, 
                     int objectSpawningChance,
                     int numTargetBlock,
                     GameObject objectSpawn,
                     Vector3[] targetBlockCoordinates,
                     int maxObjectCount
                     )
    {
        if (timeForObjectSpawning + objectSpawningTime <= Time.time &&
            countObject < maxObjectCount &&
            Random.Range(0, 101) > objectSpawningChance)
        {
            Quaternion q = Quaternion.Euler(0, 0, 0);

            // specific actions for spiderweb
            int randomTargetBlockPosition = Random.Range(0, numTargetBlock);
            if (objectName == "spiderweb")
            {
                if (spiderWebCoordinatesAlreadySpawn.Contains(targetBlockCoordinates[randomTargetBlockPosition]))
                {
                    return false;
                }
            }
            //

            var myObjectSpawn = Instantiate(objectSpawn, targetBlockCoordinates[randomTargetBlockPosition], q);
            myObjectSpawn.transform.parent = creaturesWorld.transform;
            if (myObjectSpawn.GetComponent<Animal>())
            {
                myObjectSpawn.GetComponent<Animal>().Type = objectName;
            }

            // specific actions for spiderweb
            if (objectName == "spiderweb")
            {
                myObjectSpawn.transform.parent = newLevelGeneratorScript.transform;
                myObjectSpawn.AddComponent<Prefab>().PrefabName = "BlockWeb";
                myObjectSpawn.AddComponent<BoxCollider>().isTrigger = true;
                myObjectSpawn.AddComponent<Ground>();
                spiderWebCoordinatesAlreadySpawn.Add(targetBlockCoordinates[randomTargetBlockPosition]);
            }
            //

            myObjectSpawn.SetActive(true);
            IncreaseCountObjectsInWorld(objectName);

            return true;
        }
        return false;
    }

    public static void DeleteAnimator(GameObject itemNear)
    {
        if (itemNear.transform.parent && 
            itemNear.transform.parent.name == "ItemAnimator(Clone)")
        {
            var itemParent = itemNear.transform.parent.gameObject;
            itemNear.transform.parent = null;
            Destroy(itemParent);
        }
    }

    public static void AddItemEffect(GameObject item)
    {
        var itemEffect = Instantiate(
            itemEffectPrefabStatic
        );
        itemEffect.transform.SetParent(item.transform.GetChild(0).parent, false);
        itemEffect.transform.rotation = new Quaternion(0, 0, 0, 0);
        item.GetComponent<Positioning>().EffectAdded = true;
    }

    public static void DeleteItemEffect(GameObject item)
    {
        if (item.transform.GetChild(item.transform.childCount-1).name.Contains("ItemEffect"))
        {
            Destroy(item.transform.GetChild(item.transform.childCount - 1).gameObject);
        }
        item.GetComponent<Positioning>().EffectAdded = false;
    }

    public static void InitItemWithAnimator(GameObject item, GameObject itemAnimatorPrefab)
    {
        item.AddComponent<Positioning>();
        item.AddComponent<Pickable>();
        item.transform.parent = itemAnimatorPrefab.transform;
        item.SetActive(true);
    }

    public static GameObject CreateItem(int itemIndex, Vector3 itemPosition)
    {
        var item = Instantiate(
                    ItemsGenerator.Items[itemIndex],
                    new Vector3(itemPosition.x, itemPosition.y, itemPosition.z),
                    ItemsGenerator.Items[itemIndex].transform.rotation
                    );
        IncreaseCountObjectsInWorld(item.name);
        var itemAnimator = Instantiate(
            itemAnimatorPrefabStatic
        );
        InitItemWithAnimator(item, itemAnimator);
        itemAnimator.transform.parent = ItemsWorldStatic.transform;
        item.SetActive(true);

        if (GetItemModel(itemIndex) != null)
        {
            item.AddComponent<Item>();
            item.GetComponent<Item>().ItemModel = GetItemModel(itemIndex);
            // TODO: use interface
            if (GetItemModel(itemIndex).Description.type == "Weapon" ||
                GetItemModel(itemIndex).Description.type == "WeaponAndCraft")
            {
                item.tag = "WeaponAndCraft";
                item.AddComponent<Weapon>();
            }
            if (GetItemModel(itemIndex).Description.type == "CraftMaterial")
            {
                item.tag = "CraftMaterial";
                item.AddComponent<CraftMaterial>();
            }
            else if (GetItemModel(itemIndex).Description.type == "Eat")
            {
                item.tag = "Eat";
                item.AddComponent<Eat>();
            }
            else if (GetItemModel(itemIndex).Description.type == "Key")
            {
                item.tag = "Key";
                item.AddComponent<Key>();
            }
        }
        return item;
    }

    public static ItemModel GetItemModel(int numItem)
    {
         return ItemsGenerator.Factory.GetItemModel(numItem);
    }
}
