using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    private BinaryFormatter bf = new BinaryFormatter();
    private FileStream file;
    SurrogateSelector surrogateSelector = new SurrogateSelector();
    Vector3SerializationSurrogate vector3SS = new Vector3SerializationSurrogate();

    private void Start()
    {
        surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3SS);
        bf.SurrogateSelector = surrogateSelector;
    }

    public void AutoSaveGame()
    {
        string name = "autosave ";
        SaveGame(name + DateTime.Now.ToString().Replace(":", "_").Replace(" ", "_"));
    }

    public void SaveGame(string nameFile)
    {
        string dirPath = Application.persistentDataPath + "/" + nameFile + "/";
        Directory.CreateDirectory(dirPath);
        file = File.Create(dirPath + nameFile + ".data");

        GameData gameData = new GameData();
        PlayersData playersData = SavePlayerData(gameManager.GetPlayerData(1), gameManager.GetPlayerData(2));
        ItemsData itemsData = SaveItemsData();
        CreaturesData creaturesData = SaveCreaturesData();
        DirLightData dirLightData = SaveDirLightData();
        BlocksData blocksData = SaveBlocksData();
        SaveLevelData(dirPath);

        gameData.creaturesData = creaturesData;
        gameData.playersData = playersData;
        gameData.itemsData = itemsData;
        gameData.dirLightData = dirLightData;
        gameData.blocksData = blocksData;

        bf.Serialize(file, gameData);
        file.Close();
    }

    public void LoadGame(string fileName)
    {
        try
        {
            file = File.Open(Application.persistentDataPath
                + "/" + fileName + "/" + fileName + ".data", FileMode.Open);
        }
        catch
        {
            Debug.LogError("Can't load save file");
            return;
        }

        GameData gameData = (GameData)bf.Deserialize(file);

        CreatePlayers();
        LoadPlayerData(gameData.playersData.player1);
        LoadPlayerData(gameData.playersData.player2);
        LoadCreaturesData(gameData);
        LoadDirLightData(gameData);
        LoadItemsData(gameData);
        LoadLevelData(fileName);
        LoadBlocksData(gameData);
        file.Close();
    }

    private void LoadDirLightData(GameData gameData)
    {
        gameManager.DirLight.GetComponent<DirLight>().Angle = gameData.dirLightData.angle;
        gameManager.DirLight.GetComponent<DirLight>().RotationInfo = gameData.dirLightData.rotation;
        gameManager.DirLight.GetComponent<DirLight>().R = gameData.dirLightData.r;
        gameManager.DirLight.GetComponent<DirLight>().G = gameData.dirLightData.g;
        gameManager.DirLight.GetComponent<DirLight>().B = gameData.dirLightData.b;
        DirLight.IsDay = gameData.dirLightData.isDay;
    }

    private PlayersData SavePlayerData(Player playerObject1, Player playerObject2)
    {
        PlayerData playerData1 = GetPlayerData(playerObject1);
        PlayerData playerData2 = GetPlayerData(playerObject2);
        PlayersData playersData = new PlayersData(playerData1, playerData2);
        return playersData;
    }

    private PlayerData GetPlayerData(Player playerObject)
    {
        PlayerData data = new PlayerData
        {
            playerName = playerObject.PlayerName
        };
        if (playerObject.ItemInHands)
        {
            data.numItemInHands = playerObject.ItemInHands.GetComponent<Item>().ItemModel.Description.id;
        }
        else
        {
            data.numItemInHands = -1;
        }

        data.number = playerObject.Number;
        data.position = playerObject.transform.position;
        data.countLifes = playerObject.CountLifes;
        data.curDir = playerObject.CurDir;
        data.health = playerObject.Health;
        data.satiety = playerObject.Satiety;

        return data;
    }

    private void SaveLevelData(string dirPath)
    {
        Texture2D level1 = new Texture2D(
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().level1.width,
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().level1.height,
            TextureFormat.ARGB32,
            false
        );
        Texture2D level2 = new Texture2D(
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().level2.width,
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().level2.height,
            TextureFormat.ARGB32,
            false
        );
        Texture2D level3 = new Texture2D(
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().level3.width,
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().level3.height,
            TextureFormat.ARGB32,
            false
        );
        Texture2D level4 = new Texture2D(
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().level4.width,
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().level4.height,
            TextureFormat.ARGB32,
            false
        );
        Texture2D level5 = new Texture2D(
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().level5.width,
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().level5.height,
            TextureFormat.ARGB32,
            false
        );
        Texture2D level6 = new Texture2D(
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().level6.width,
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().level6.height,
            TextureFormat.ARGB32,
            false
        );
        SetLevelParameters(level1);
        SetLevelParameters(level2);
        SetLevelParameters(level3);
        SetLevelParameters(level4);
        SetLevelParameters(level5);
        SetLevelParameters(level6);

        Texture2D level = null;
        for (int i = 0; i < GameManager.NewLevelGeneratorScriptStatic.transform.childCount; i++)
        {
            switch (GameManager.NewLevelGeneratorScriptStatic.transform.GetChild(i).transform.position.z)
            {
                case 32:
                    level = level1;
                    break;
                case 16:
                    level = level2;
                    break;
                case 0:
                    level = level3;
                    break;
                case -16:
                    level = level4;
                    break;
                case -32:
                    level = level5;
                    break;
                case 48:
                    level = level6;
                    break;
            }
            string prefabName = "";
            if (GameManager.NewLevelGeneratorScriptStatic.transform.GetChild(i).GetComponent<Prefab>())
            {
                prefabName = GameManager.NewLevelGeneratorScriptStatic.transform.GetChild(i).GetComponent<Prefab>().PrefabName;
            }
            if (level)
            {
                foreach (ColorToPrefab colorMapping in GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().colorMappings)
                {
                    
                    if (colorMapping.prefab.name == prefabName)
                    {
                        if (prefabName == "BlockWeb")
                        {
                            continue;
                        }
                        level.SetPixel(
                                (Mathf.RoundToInt(
                                    GameManager.NewLevelGeneratorScriptStatic.transform.GetChild(i).transform.position.x / 8)
                                    ),
                                (Mathf.RoundToInt(
                                    GameManager.NewLevelGeneratorScriptStatic.transform.GetChild(i).transform.position.y / 8)
                                    ),
                                colorMapping.color
                            );

                        level.SetPixel(
                            (Mathf.RoundToInt(
                                GameManager.NewLevelGeneratorScriptStatic.transform.GetChild(i).transform.position.x / 8)
                                ),
                            (Mathf.RoundToInt(
                                GameManager.NewLevelGeneratorScriptStatic.transform.GetChild(i).transform.position.y / 8 + 1)
                                ),
                            colorMapping.color
                        );

                        level.SetPixel(
                            (Mathf.RoundToInt(
                                GameManager.NewLevelGeneratorScriptStatic.transform.GetChild(i).transform.position.x / 8 + 1)
                                ),
                            (Mathf.RoundToInt(
                                GameManager.NewLevelGeneratorScriptStatic.transform.GetChild(i).transform.position.y / 8)
                                ),
                            colorMapping.color
                        );

                        level.SetPixel(
                            (Mathf.RoundToInt(
                                GameManager.NewLevelGeneratorScriptStatic.transform.GetChild(i).transform.position.x / 8 + 1)
                                ),
                            (Mathf.RoundToInt(
                                GameManager.NewLevelGeneratorScriptStatic.transform.GetChild(i).transform.position.y / 8 + 1)
                                ),
                            colorMapping.color
                        );
                    }
                }
            }
        }
        level1.filterMode = FilterMode.Point;
        level2.filterMode = FilterMode.Point;
        level3.filterMode = FilterMode.Point;
        level4.filterMode = FilterMode.Point;
        level5.filterMode = FilterMode.Point;
        level6.filterMode = FilterMode.Point;

        level1.Apply();
        level2.Apply();
        level3.Apply();
        level4.Apply();
        level5.Apply();
        level6.Apply();

        SaveLevelImage(level1, "level1", dirPath);
        SaveLevelImage(level2, "level2", dirPath);
        SaveLevelImage(level3, "level3", dirPath);
        SaveLevelImage(level4, "level4", dirPath);
        SaveLevelImage(level5, "level5", dirPath);
        SaveLevelImage(level6, "level6", dirPath);
    }

    private void SetLevelParameters(Texture2D level)
    {
        level.alphaIsTransparency = true;
        level.filterMode = FilterMode.Point;

        for (int i = 0; i < level.width; i++)
        {
            for (int j = 0; j < level.height; j++)
            {
                level.SetPixel(i, j, Color.clear);
            }
        }
    }

    private BlocksData SaveBlocksData()
    {
        BlocksData blocksData = new BlocksData();
        for (int i = 0; i < GameManager.NewLevelGeneratorScriptStatic.transform.childCount; i++)
        {
            if (GameManager.NewLevelGeneratorScriptStatic.transform.GetChild(i)
                            .GetComponent<Prefab>() &&
                GameManager.NewLevelGeneratorScriptStatic.transform.GetChild(i)
                            .GetComponent<Prefab>().PrefabName == "BlockWeb")
            {
                blocksData.spiderWebs.Add(GameManager.NewLevelGeneratorScriptStatic.transform
                    .GetChild(i).position);
            }

        }
        return blocksData;
    }

    private void SaveLevelImage(Texture2D level, string name, string dirPath)
    {
        byte[] bytes = level.EncodeToPNG();
        
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllBytes(dirPath + name + ".png", bytes);
    }

    private ItemsData SaveItemsData()
    {
        ItemsData itemsData = new ItemsData();
        if (gameManager.ItemsWorld.transform.childCount > 0)
        {
            for (int i = 0; i < gameManager.ItemsWorld.transform.childCount; i++)
            {
                if (gameManager.ItemsWorld.transform.GetChild(i).GetComponent<Weapon>())
                {
                    WeaponData weaponData = new WeaponData
                    {
                        itemNum =
                        gameManager.ItemsWorld.transform.GetChild(i).GetComponent<Item>().ItemModel.Description.id,
                        position =
                        gameManager.ItemsWorld.transform.GetChild(i).position,
                        durablitiy =
                        gameManager.ItemsWorld.transform.GetChild(i).GetComponent<Weapon>().Durablitiy
                    };
                    itemsData.weaponsData.Add(weaponData);
                }
                else
                {
                    ItemData itemData = new ItemData
                    {
                        itemNum =
                            gameManager.ItemsWorld.transform.GetChild(i).GetComponentInChildren<Item>().ItemModel.Description.id,
                        position =
                            gameManager.ItemsWorld.transform.GetComponentInChildren<Item>().gameObject.transform.position
                    };
                    itemsData.itemsData.Add(itemData);
                }
            }
        }
        return itemsData;
    }

    private CreaturesData SaveCreaturesData()
    {
        CreaturesData creaturesData = new CreaturesData();
        if (gameManager.CreaturesWorld.transform.childCount > 0)
        {
            for (int i = 0; i < gameManager.CreaturesWorld.transform.childCount; i++)
            {
                if (gameManager.CreaturesWorld.transform.GetChild(i).GetComponent<Zombie>())
                {
                    CreatureData creatureData = new CreatureData
                    {
                        position = gameManager.CreaturesWorld.transform.GetChild(i).position,
                        health = gameManager.CreaturesWorld.transform.GetChild(i).GetComponent<Zombie>().health,
                        type = "zombie"
                    };
                    creaturesData.enemiesData.Add(creatureData);
                } else if (gameManager.CreaturesWorld.transform.GetChild(i).GetComponent<Animal>())
                {
                    CreatureData creatureData = new CreatureData
                    {
                        position = gameManager.CreaturesWorld.transform.GetChild(i).position,
                        health = gameManager.CreaturesWorld.transform.GetChild(i).GetComponent<Animal>().Health,
                        type = gameManager.CreaturesWorld.transform.GetChild(i).GetComponent<Animal>().Type
                    };
                    creaturesData.enemiesData.Add(creatureData);
                }
            }
        }
        return creaturesData;
    }

    private DirLightData SaveDirLightData()
    {
        DirLightData dirLightData = new DirLightData()
        {
            isDay = DirLight.IsDay,
            angle = gameManager.DirLight.GetComponent<DirLight>().Angle,
            rotation = gameManager.DirLight.GetComponent<DirLight>().RotationInfo,
            r = gameManager.DirLight.GetComponent<DirLight>().R,
            g = gameManager.DirLight.GetComponent<DirLight>().G,
            b = gameManager.DirLight.GetComponent<DirLight>().B
        };
        return dirLightData;
    }

    private void CreatePlayers()
    {
        Destroy(gameManager.Player1);
        gameManager.Player1 = Instantiate(gameManager.Player1Prefab,
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().MostLeftBlockPosition + new Vector3(0, 10, 0),
            gameManager.Player1Prefab.transform.rotation);
        

        Destroy(gameManager.Player2);
        gameManager.Player2 = Instantiate(gameManager.Player2Prefab,
            GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().MostLeftBlockPosition + new Vector3(10, 10, 0),
            gameManager.Player2Prefab.transform.rotation);
        gameManager.Player1.GetComponent<Player>().OtherPlayer = gameManager.Player2;
        gameManager.Player2.GetComponent<Player>().OtherPlayer = gameManager.Player1;
    }

    private void LoadPlayerData(PlayerData playerData)
    {
        GameObject player = null;
        if (playerData.number == 1)
        {
            player = gameManager.Player1;
        }
        else if (playerData.number == 2)
        {
            player = gameManager.Player2;
        }
        player.GetComponent<Player>().CountLifes = playerData.countLifes;
        player.GetComponent<Player>().Satiety = playerData.satiety;
        player.GetComponent<Player>().Health = playerData.health;
        player.GetComponent<Player>().CurDir = playerData.curDir;
        player.GetComponent<Player>().PlayerName = playerData.playerName;
        player.SetActive(false);
        player.GetComponent<Player>().GetComponent<CharacterController>().transform.position = playerData.position;
        player.SetActive(true);
        if (playerData.numItemInHands != -1)
        {
            player.GetComponent<Player>().ItemInHands = GameManager.CreateItem(playerData.numItemInHands, new Vector3(0, 0, 0));
            player.GetComponent<Player>().ItemInHands.GetComponent<Positioning>().PutItemInHands();
            player.GetComponent<Player>().ChangeHand();
            player.GetComponent<Player>().ItemInHands.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    private void LoadCreaturesData(GameData gameData)
    {
        foreach (Transform child in gameManager.CreaturesWorld.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var elem in gameData.creaturesData.enemiesData)
        {
            if (elem.type == "zombie")
            {
                var enemy = Instantiate(gameManager.Zombie, elem.position, gameManager.Zombie.transform.rotation);
                    enemy.GetComponent<Zombie>().GetComponent<CharacterController>().transform.position = elem.position;
                    enemy.GetComponent<Zombie>().health = elem.health;
                
                enemy.transform.parent = gameManager.CreaturesWorld.transform;
                if (!enemy.GetComponent<Zombie>().Player1)
                {
                    enemy.GetComponent<Zombie>().Player1 = gameManager.Player1;
                }
                if (!enemy.GetComponent<Zombie>().Player2)
                {
                    enemy.GetComponent<Zombie>().Player2 = gameManager.Player2;
                }
            }
            else
            {
                GameObject animalPrefab = null;
                if (elem.type == "duck")
                {
                    animalPrefab = gameManager.Duck;
                }
                else if (elem.type == "frog")
                {
                    animalPrefab = gameManager.Frog;
                }
                var animal = Instantiate(animalPrefab, elem.position, animalPrefab.transform.rotation);
                animal.GetComponent<Animal>().GetComponent<CharacterController>().transform.position = elem.position;
                animal.GetComponent<Animal>().Health = elem.health;
                animal.transform.parent = gameManager.CreaturesWorld.transform;
            }

        }
    }

    private void LoadItemsData(GameData gameData)
    {
        foreach (Transform child in gameManager.ItemsWorld.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var elem in gameData.itemsData.weaponsData)
        {
            var item = GameManager.CreateItem(elem.itemNum, elem.position);
            item.GetComponent<Weapon>().Durablitiy = elem.durablitiy;
            item.transform.position = elem.position;
            item.transform.parent = GameManager.ItemsWorldStatic.transform;
        }
        foreach (var elem in gameData.itemsData.itemsData)
        {
            var item = GameManager.CreateItem(elem.itemNum, elem.position);
            item.transform.position = elem.position;
            item.transform.parent = GameManager.ItemsWorldStatic.transform;
        }
    }

    private void LoadBlocksData(GameData gameData)
    {
        if (gameData.blocksData.spiderWebs != null)
        {
            foreach (var position in gameData.blocksData.spiderWebs)
            {
                var web = Instantiate(gameManager.SpiderWeb, position, gameManager.SpiderWeb.transform.rotation);
                web.transform.parent = GameManager.NewLevelGeneratorScriptStatic.transform;
                web.AddComponent<Prefab>().PrefabName = "BlockWeb";
                web.AddComponent<BoxCollider>().isTrigger = true;
                web.AddComponent<Ground>();
                GameManager.spiderWebCoordinatesAlreadySpawn.Add(position);
            }
        }
    }

    private void LoadLevelData(string fileName)
    {
        string fullPath = Application.persistentDataPath
            + "/" + fileName + "/level1.png";
        var rawData = System.IO.File.ReadAllBytes(fullPath);
        Texture2D level1 = new Texture2D(1, 1);
        level1.LoadImage(rawData);

        fullPath = Application.persistentDataPath
            + "/" + fileName + "/level2.png";
        rawData = System.IO.File.ReadAllBytes(fullPath);
        Texture2D level2 = new Texture2D(1, 1);
        level2.LoadImage(rawData);

        fullPath = Application.persistentDataPath
            + "/" + fileName + "/level3.png";
        rawData = System.IO.File.ReadAllBytes(fullPath);
        Texture2D level3 = new Texture2D(1, 1);
        level3.LoadImage(rawData);

        fullPath = Application.persistentDataPath
            + "/" + fileName + "/level4.png";
        rawData = System.IO.File.ReadAllBytes(fullPath);
        Texture2D level4 = new Texture2D(1, 1);
        level4.LoadImage(rawData);

        fullPath = Application.persistentDataPath
            + "/" + fileName + "/level5.png";
        rawData = System.IO.File.ReadAllBytes(fullPath);
        Texture2D level5 = new Texture2D(1, 1);
        level5.LoadImage(rawData);

        fullPath = Application.persistentDataPath
            + "/" + fileName + "/level6.png";
        rawData = System.IO.File.ReadAllBytes(fullPath);
        Texture2D level6 = new Texture2D(1, 1);
        level6.LoadImage(rawData);

        level1.filterMode = FilterMode.Point;
        level2.filterMode = FilterMode.Point;
        level3.filterMode = FilterMode.Point;
        level4.filterMode = FilterMode.Point;
        level5.filterMode = FilterMode.Point;
        level6.filterMode = FilterMode.Point;
        GameManager.NewLevelGeneratorScriptStatic.GetComponent<NewLevelGenerator>().NewGameLevel(level1, level2, level3, level4, level5, level6);
    }
}
