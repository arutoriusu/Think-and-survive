using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NewLevelGenerator : MonoBehaviour
{
	public Texture2D level1;
	public Texture2D level2;
	public Texture2D level3;
	public Texture2D level4;
	public Texture2D level5;
	public Texture2D level6;
	public ColorToPrefab[] colorMappings;
	public GrassToPlace[] grassMappings;
	public GameObject[] trees;
	public GameObject[] cactuses;
	public GameObject deadTree;
	public GameObject textObject;
	public GameObject sky;
	public GameObject mountains;
	public GameObject gameManager;
	
	private int selectedIndex = 0;

	private System.Random random;

	private GameObject selectedObject;

	private int numGrass = 0;
	private Vector3[] grassCoordinates;

	private int numSpiderWeb = 0;
	private Vector3[] spiderWebCoordinates;

	private int numZombieSpawn = 0;
	private Vector3[] zombieSpawnCoordinates;

	private HashSet<Vector3> doorsCoordinates;

	private Vector3 mostLeftBlockPosition;
	private int mostRightBlockX;
	int yMaxForSpawn;

	public Vector3[] GrassCoordinates { get => grassCoordinates; set => grassCoordinates = value; }
    public int NumGrass { get => numGrass; set => numGrass = value; }
    public Vector3[] SpiderWebCoordinates { get => spiderWebCoordinates; set => spiderWebCoordinates = value; }
    public int NumSpiderWeb { get => numSpiderWeb; set => numSpiderWeb = value; }
    public int NumZombieSpawn { get => numZombieSpawn; set => numZombieSpawn = value; }
    public Vector3[] ZombieSpawnCoordinates { get => zombieSpawnCoordinates; set => zombieSpawnCoordinates = value; }
    public Vector3 MostLeftBlockPosition { get => mostLeftBlockPosition; set => mostLeftBlockPosition = value; }
    public HashSet<Vector3> DoorsCoordinates { get => doorsCoordinates; set => doorsCoordinates = value; }

    private Dictionary<float, float> maxBlockY = new Dictionary<float, float>();

	void Start()
	{
		NewGameLevel(level1, level2, level3, level4, level5, level6);
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            if (selectedIndex > 0 && selectedIndex <= colorMappings.Length - 1)
                selectedObject = colorMappings[--selectedIndex].prefab;
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (selectedIndex >= 0 && selectedIndex < colorMappings.Length - 1)
                selectedObject = colorMappings[++selectedIndex].prefab;
        }
        if (selectedObject)
        {
            textObject.GetComponent<Text>().text = selectedObject.name;
            GLOBAL.SELECTED_BLOCK = selectedObject;
        }
        else textObject.GetComponent<Text>().text = "Нет";
    }

    public void NewGameLevel(
		Texture2D level1, 
		Texture2D level2, 
		Texture2D level3, 
		Texture2D level4,
		Texture2D level5,
		Texture2D level6)
	{
		int maxSize = level3.width * level3.height;
		GrassCoordinates = new Vector3[maxSize];
		SpiderWebCoordinates = new Vector3[maxSize];
		ZombieSpawnCoordinates = new Vector3[maxSize];
		DoorsCoordinates = new HashSet<Vector3>();

		foreach (Transform child in gameObject.transform)
		{
			Destroy(child.gameObject);
		}
		LevelGenerator(1, level1, 32);
		LevelGenerator(2, level2, 16);

		random = new System.Random(3);
		yMaxForSpawn = 0;

		LevelGenerator(3, level3, 0);
		LevelGenerator(4, level4, -16);
		LevelGenerator(5, level5, -32);
		LevelGenerator(6, level6, 48);

		/*for (int x = 0; x < level3.width / 2; x++)
		{
			for (int y = 0; y < level3.height / 2; y++)
			{
				Color pixelColor = level3.GetPixel(x * 2, y * 2);
				Color pixel2Color = level3.GetPixel(x * 2 + 1, y * 2 + 1);
				GenerateTile(x, y, 0, pixelColor, pixel2Color);
			}
		}*/
		MakeBackground(sky, (int)sky.transform.position.x, 3100);
		MakeBackground(mountains, (int)mountains.transform.position.x, 7700);
		gameManager.SetActive(true);
	}

	void LevelGenerator(int seed, Texture2D level, int layer)
    {
		random = new System.Random(seed);
		for (int x = 0; x < level.width / 2; x++)
		{
			for (int y = 0; y < level.height / 2; y++)
			{
				Color pixelColor = level.GetPixel(x * 2, y * 2);
				Color pixel2Color = level.GetPixel(x * 2 + 1, y * 2 + 1);
				GenerateTile(x, y, layer, pixelColor, pixel2Color);
			}
		}
	}

    void GenerateTile(int x, int y, int blocksLayer, Color pixelColor, Color pixel2Color)
	{

		if (pixelColor.a == 0)
		{
			return;
		}

		if (blocksLayer == 0 && x == 6 && y > yMaxForSpawn)
        {
			mostLeftBlockPosition = new Vector3(x * 16, y * 16, 0);
			yMaxForSpawn = y;
		}

		if (mostRightBlockX < x * 16)
		{
			mostRightBlockX = x * 16;
		}

		foreach (ColorToPrefab colorMapping in colorMappings)
		{
			if (colorMapping.color.r == pixelColor.r && colorMapping.color.g == pixelColor.g && colorMapping.color.b == pixelColor.b)
			{
				int depthMask;
				if (pixelColor.r == pixel2Color.r && pixelColor.g == pixel2Color.g && pixelColor.b == pixel2Color.b)
				{
					depthMask = 256;
				}
				else
				{
					depthMask = (int)(pixel2Color.r * 256);
				}

				//if ((depthMask & 64) != 0)
				{
					Vector3 position = new Vector3((x * 16), (y * 16), blocksLayer);
					GameObject ins = null;

					if (blocksLayer == 48)
					{
						GameObject preloaded = GameObject.Find("Preloaded/" + colorMapping.prefab.name + "x5");
						if (!preloaded) preloaded = colorMapping.prefab;
						//preloaded.transform.localScale = new Vector3(100, 100, 100);
						ins = Instantiate(preloaded, position, Quaternion.Euler(-90, 180, 0), transform);
					} else if (blocksLayer == 0)
					{
						if (colorMapping.prefab.name == "BlockGrass")
						{
							GrassCoordinates[NumGrass] = new Vector3(position.x, position.y + 16, position.z);
							NumGrass += 1;
						} else if (colorMapping.prefab.name == "BlockStone")
						{
							bool createWeb = false;
                            for (int i = 2; i < 10; i+=2)
                            {
								if (level3.GetPixel(x * 2, y * 2 - i).a != 0)
                                {
									createWeb = true;
									break;
                                }
							}

							if (level3.GetPixel(x*2, y*2-1).a == 0 && createWeb)
                            {
								spiderWebCoordinates[numSpiderWeb] = new Vector3(position.x-8, position.y-7, position.z);
								numSpiderWeb += 1;
							}
						}
						else if (colorMapping.prefab.name == "BlockWeb")
                        {
							spiderWebCoordinates[numSpiderWeb] = new Vector3(position.x, position.y, position.z);
							numSpiderWeb += 1;
						}
						else if (colorMapping.prefab.name == "BlockZombieSpawner")
						{
							zombieSpawnCoordinates[NumZombieSpawn] = position;
							NumZombieSpawn += 1;
						}
					}

					ins = Instantiate(colorMapping.prefab, position, Quaternion.Euler(-90, 180, 0), transform);
					ins.isStatic = true;

					if (ins)
					{
						ins.AddComponent<Prefab>().PrefabName = colorMapping.prefab.name;
					}

					if (colorMapping.prefab.name == "BlockGrassx5"
						|| colorMapping.prefab.name == "BlockGrass"
						|| colorMapping.prefab.name == "BlockSand"
						|| colorMapping.prefab.name == "BlockStone"
						|| colorMapping.prefab.name == "BlockStonex5"
						|| colorMapping.prefab.name == "BlockWood0"
						|| colorMapping.prefab.name == "BlockWood0x5"
						|| colorMapping.prefab.name == "BlockDirt"
						|| colorMapping.prefab.name == "BlockGravel"
						|| colorMapping.prefab.name == "BlockGravelx5"
						|| colorMapping.prefab.name == "BlockOreIron"
						)
					{
						ins.transform.tag = "Ground";
						ins.AddComponent<BoxCollider>();
						ins.AddComponent<Ground>();
						try
						{
							if (maxBlockY[position.x] < position.y)
							{
								maxBlockY[position.x] = position.y;
							}
						}
						catch (KeyNotFoundException)
						{
							maxBlockY[position.x] = 0;
						}
					} else if (colorMapping.prefab.name == "DoorWood")
					{
						if (!DoorsCoordinates.Contains(position))
                        {
							
							DoorsCoordinates.Add(position);
							ins.AddComponent<BoxCollider>();
							ins.AddComponent<Door>();
							ins.tag = "Door";
							ins.transform.localPosition = new Vector3(
								ins.transform.localPosition.x, 
								ins.transform.localPosition.y, 
								6.5f);
						}
                        else
                        {
							Destroy(ins);
                        }
					}

					if (colorMapping.prefab.name.Equals("BlockBench") || colorMapping.prefab.name.Equals("BlockFurnace"))
					{
						if (colorMapping.prefab.name.Equals("BlockBench"))
						{
							ins.transform.tag = "Bench";
						} else if (colorMapping.prefab.name.Equals("BlockFurnace"))
						{
							ins.transform.tag = "Furnace";
							ins.AddComponent<FurnaceManufacture>();
						}

						var bc = ins.AddComponent<BoxCollider>();

						ins.gameObject.AddComponent<Interactable>();
						ins.gameObject.AddComponent<CraftMechanism>();
						GameObject resultBench = new GameObject();
						resultBench.name = "Result";
						resultBench.transform.parent = ins.transform;

						if (colorMapping.prefab.name.Equals("BlockFurnace"))
                        {
							GameObject resource = new GameObject();
							resource.name = "Resource";
							resource.transform.parent = ins.transform;

							GameObject smelter = new GameObject();
							smelter.name = "Smelter";
							smelter.transform.parent = ins.transform;
						}
					}

					if (colorMapping.prefab.name.Equals("BlockChair"))
                    {
						var bc = ins.AddComponent<BoxCollider>();
						bc.size = new Vector3(0.12f, 0.12f, 0.1f);
						bc.center = new Vector3(0, 0, 0.05f);
					}
					

					if (colorMapping.prefab.name.Equals("BlockGrass"))
					{
						int jj = 1;
						// спавн деревьев на 6 слое
						if (blocksLayer == 48)
                        {
							jj = 5;
							int randomNumber = random.Next(0, 100);
							if (randomNumber > 60)
							{
								int insTreeRotation = random.Next(0, 360);
								int tree = random.Next(0, 2);
								GameObject insTree = Instantiate(trees[tree], new Vector3((x * 16), (y * 16) + 16, blocksLayer + random.Next(0, 64)), Quaternion.Euler(-90, 180, 0), transform);
								insTree.transform.rotation = Quaternion.Euler(0, insTreeRotation, ins.transform.rotation.z);
								insTree.SetActive(true);
							}
						}
						// спавн деревьев на 4 слое
						if (blocksLayer == 16)
						{
							jj = 5;
							int randomNumber = random.Next(0, 100);
							if (randomNumber > 90)
							{
								int insTreeRotation = random.Next(0, 360);
								int tree = random.Next(0, 2);
								GameObject insTree = Instantiate(trees[tree], new Vector3((x * 16), (y * 16) + 16, blocksLayer), Quaternion.Euler(-90, 180, 0), transform);
								insTree.transform.rotation = Quaternion.Euler(0, insTreeRotation, ins.transform.rotation.z);
								insTree.SetActive(true);
							}
						}
						
						for (int j = 0; j < jj; j++)
						{
							bool placed = false;
							foreach (GrassToPlace curGrass in grassMappings)
							{

								for (int i = 0; i < grassMappings.Length; i++)
								{
									if (curGrass.index == i)
									{
										float chance = curGrass.weight / (grassMappings.Length + 1);
										int randomNumber = random.Next(0, 100);
										if (randomNumber <= chance)
										{
											position = new Vector3((x * 16), (y * 16) + 16, blocksLayer + (j * 16));
											ins = Instantiate(curGrass.grass, position, Quaternion.Euler(-90, 180, 0), transform);
											ins.isStatic = true;
											placed = true;
											break;
										}
									}
								}
								if (placed) break;
							}
						}
					}

					if (colorMapping.prefab.name.Equals("BlockSand"))
                    {
						if (blocksLayer == 48)
						{
							if (random.Next(0, 3) != 0)
							{
								int cactus = random.Next(0, 2);
								int cactusRotation = random.Next(0, 360);
								GameObject insTree = Instantiate(cactuses[cactus], new Vector3((x * 16), (y * 16) + 16, blocksLayer + random.Next(0, 64)), Quaternion.Euler(-90, 180, 0), transform);
								insTree.transform.rotation = Quaternion.Euler(0, cactusRotation, ins.transform.rotation.z);
								insTree.SetActive(true);
							}
						}
					}

					if (colorMapping.prefab.name.Equals("BlockDirt"))
					{
						{
							if (blocksLayer == 48)
							{
								int randomNumber = random.Next(0, 100);
								if (randomNumber > 65)
								{
									int insTreeRotation = random.Next(0, 360);
									GameObject insTree = Instantiate(deadTree, new Vector3((x * 16), (y * 16) + 16, blocksLayer + random.Next(0, 64)), Quaternion.Euler(-90, 180, 0), transform);
									insTree.transform.rotation = Quaternion.Euler(0, insTreeRotation, ins.transform.rotation.z);
									insTree.SetActive(true);
									try
									{
										if (maxBlockY[insTree.transform.position.x] >= (y * 16) + 15)
										{
											Destroy(insTree);
										}
									}
									catch (KeyNotFoundException)
									{

									}
								}

							}
							// спавн мертвых деревьев на 4 слое
							if (blocksLayer == 16)
							{
								int randomNumber = random.Next(0, 100);
								if (randomNumber > 65)
								{
									int insTreeRotation = random.Next(0, 360);
									GameObject insTree = Instantiate(deadTree, new Vector3((x * 16), (y * 16) + 16, blocksLayer), Quaternion.Euler(-90, 180, 0), transform);
									insTree.transform.rotation = Quaternion.Euler(0, insTreeRotation, ins.transform.rotation.z);
									insTree.SetActive(true);
                                    try
                                    {
										if (maxBlockY[insTree.transform.position.x] >= (y * 16) + 15)
										{
											Destroy(insTree);
										}
									} catch (KeyNotFoundException)
                                    {

                                    }
                                }
							}
						}
					}				
				}
			}
		}
	}

	void MakeBackground(GameObject myObject, int startedPosition, int lengthMyObject)
    {
		int objectLastPositionX = startedPosition;
		Vector3 objectPosition = myObject.transform.position;
		do
		{
			var ins = Instantiate(myObject, objectPosition, myObject.transform.rotation);
			ins.SetActive(true);
			objectLastPositionX = objectLastPositionX + lengthMyObject;
			objectPosition = new Vector3(objectLastPositionX, objectPosition.y, objectPosition.z);
		} while (mostRightBlockX + lengthMyObject / 2 > objectLastPositionX);
	}
}
