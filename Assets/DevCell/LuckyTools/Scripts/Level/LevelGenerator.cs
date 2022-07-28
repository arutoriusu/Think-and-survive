using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelGenerator : MonoBehaviour
{
	public Texture2D level;
	public ColorToPrefab[] colorMappings;
	public GrassToPlace[] grassMappings;

	void Start()
	{
		GenerateLevel();
	}

	void GenerateLevel()
	{

		for (int x = 0; x < level.width/2; x++)
		{
			for (int y = 0; y < level.height/2; y++)
			{
				GenerateTile(x, y);
			}
		}
	}

	void GenerateTile(int x, int y)
	{
		//Random.InitState(256753673);
		Color pixelColor = level.GetPixel(x * 2, y * 2);
		Color pixel2Color = level.GetPixel(x * 2 + 1, y * 2 + 1);

		if (pixelColor.a == 0)
		{
			return;
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

				Vector3 position;
				if ((depthMask & 256) != 0)
				{
					position = new Vector3((x * 16), (y * 16), -32);
					GameObject preloaded = GameObject.Find("Preloaded/" + colorMapping.prefab.name + "x12");
					if (!preloaded) preloaded = colorMapping.prefab;
					preloaded.transform.localScale = new Vector3(100, 100, 100);
					var ins = Instantiate(preloaded, position, Quaternion.Euler(-90, 180, 0), transform);
					ins.AddComponent<BoxCollider>();
					ins.isStatic = true;

					if (colorMapping.prefab.name.Equals("BlockGrass"))
					{
						for (int j = 0; j < 12; j++)
						{
							bool placed = false;
							foreach (GrassToPlace curGrass in grassMappings)
							{
								
								for (int i = 0; i < grassMappings.Length; i++)
								{
									if (curGrass.index == i)
									{
										float chance = curGrass.weight / (grassMappings.Length + 1);
										if (Random.Range(0f, 100f) <= chance)
										{
											position = new Vector3((x * 16), (y * 16) + 16, -32 + (j * 16));
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
				}
				if ((depthMask & 128) != 0)
				{
					position = new Vector3((x * 16), (y * 16), -32);
					GameObject preloaded = GameObject.Find("Preloaded/" + colorMapping.prefab.name + "x5");
					if (!preloaded) preloaded = colorMapping.prefab;
					preloaded.transform.localScale = new Vector3(100, 100, 100);
					var ins = Instantiate(preloaded, position, Quaternion.Euler(-90, 180, 0), transform);
					ins.AddComponent<BoxCollider>();
					ins.isStatic = true;

					if (colorMapping.prefab.name.Equals("BlockGrass"))
					{
						for (int j = 0; j < 5; j++)
						{
							bool placed = false;
							foreach (GrassToPlace curGrass in grassMappings)
							{

								for (int i = 0; i < grassMappings.Length; i++)
								{
									if (curGrass.index == i)
									{
										float chance = curGrass.weight / (grassMappings.Length + 1);
										if (Random.Range(0f, 100f) <= chance)
										{
											position = new Vector3((x * 16), (y * 16) + 16, -32 + (j * 16));
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
				}
				if ((depthMask & 2) != 0)
				{
					position = new Vector3((x * 16), (y * 16), 48);
					GameObject preloaded = GameObject.Find("Preloaded/" + colorMapping.prefab.name + "x5");
					if (!preloaded) preloaded = colorMapping.prefab;
					preloaded.transform.localScale = new Vector3(100, 100, 100);
					var ins = Instantiate(preloaded, position, Quaternion.Euler(-90, 180, 0), transform);
					ins.AddComponent<BoxCollider>();
					ins.isStatic = true;
					//Debug.Log("Preloaded/" + colorMapping.prefab.name + "x5");

					if (colorMapping.prefab.name.Equals("BlockGrass"))
					{
						for (int j = 0; j < 5; j++)
						{
							bool placed = false;
							foreach (GrassToPlace curGrass in grassMappings)
							{

								for (int i = 0; i < grassMappings.Length; i++)
								{
									if (curGrass.index == i)
									{
										float chance = curGrass.weight / (grassMappings.Length + 1);
										if (Random.Range(0f, 100f) <= chance)
										{
											position = new Vector3((x * 16), (y * 16) + 16, 48 + (j * 16));
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
				}
				if ((depthMask & 4) != 0)
				{
					position = new Vector3((x * 16), (y * 16), 32);
					colorMapping.prefab.transform.localScale = new Vector3(100, 100, 100);
					var ins = Instantiate(colorMapping.prefab, position, Quaternion.Euler(-90, 180, 0), transform);
					ins.AddComponent<BoxCollider>();
					ins.isStatic = true;

					if (colorMapping.prefab.name.Equals("BlockGrass"))
					{
						for (int j = 0; j < 1; j++)
						{
							bool placed = false;
							foreach (GrassToPlace curGrass in grassMappings)
							{

								for (int i = 0; i < grassMappings.Length; i++)
								{
									if (curGrass.index == i)
									{
										float chance = curGrass.weight / (grassMappings.Length + 1);
										if (Random.Range(0f, 100f) <= chance)
										{
											position = new Vector3((x * 16), (y * 16) + 16, 32 + (j * 16));
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
				}
				if ((depthMask & 8) != 0)
				{
					position = new Vector3((x * 16), (y * 16), 16);
					colorMapping.prefab.transform.localScale = new Vector3(100, 100, 100);
					var ins = Instantiate(colorMapping.prefab, position, Quaternion.Euler(-90, 180, 0), transform);
					ins.AddComponent<BoxCollider>();
					ins.isStatic = true;

					if (colorMapping.prefab.name.Equals("BlockGrass"))
					{
						for (int j = 0; j < 1; j++)
						{
							bool placed = false;
							foreach (GrassToPlace curGrass in grassMappings)
							{

								for (int i = 0; i < grassMappings.Length; i++)
								{
									if (curGrass.index == i)
									{
										float chance = curGrass.weight / (grassMappings.Length + 1);
										if (Random.Range(0f, 100f) <= chance)
										{
											position = new Vector3((x * 16), (y * 16) + 16, 16 + (j * 16));
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
				}
				if ((depthMask & 16) != 0)
				{
					position = new Vector3((x * 16), (y * 16), 0);
					colorMapping.prefab.transform.localScale = new Vector3(100, 100, 100);
					var ins = Instantiate(colorMapping.prefab, position, Quaternion.Euler(-90, 180, 0), transform);
					ins.AddComponent<BoxCollider>();
					ins.isStatic = true;

					if (colorMapping.prefab.name.Equals("BlockGrass"))
					{
						for (int j = 0; j < 1; j++)
						{
							bool placed = false;
							foreach (GrassToPlace curGrass in grassMappings)
							{

								for (int i = 0; i < grassMappings.Length; i++)
								{
									if (curGrass.index == i)
									{
										float chance = curGrass.weight / (grassMappings.Length + 1);
										if (Random.Range(0f, 100f) <= chance)
										{
											position = new Vector3((x * 16), (y * 16) + 16, 0 + (j * 16));
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
				}
				if ((depthMask & 32) != 0)
				{
					position = new Vector3((x * 16), (y * 16), -16);
					colorMapping.prefab.transform.localScale = new Vector3(100, 100, 100);
					var ins = Instantiate(colorMapping.prefab, position, Quaternion.Euler(-90, 180, 0), transform);
					ins.AddComponent<BoxCollider>();
					ins.isStatic = true;

					if (colorMapping.prefab.name.Equals("BlockGrass"))
					{
						for (int j = 0; j < 1; j++)
						{
							bool placed = false;
							foreach (GrassToPlace curGrass in grassMappings)
							{

								for (int i = 0; i < grassMappings.Length; i++)
								{
									if (curGrass.index == i)
									{
										float chance = curGrass.weight / (grassMappings.Length + 1);
										if (Random.Range(0f, 100f) <= chance)
										{
											position = new Vector3((x * 16), (y * 16) + 16, -16 + (j * 16));
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
				}
				if ((depthMask & 64) != 0)
				{
					position = new Vector3((x * 16), (y * 16), -32);
					colorMapping.prefab.transform.localScale = new Vector3(100, 100, 100);
					var ins = Instantiate(colorMapping.prefab, position, Quaternion.Euler(-90, 180, 0), transform);
					ins.AddComponent<BoxCollider>();
					ins.isStatic = true;

					if (colorMapping.prefab.name.Equals("BlockGrass"))
					{
						for (int j = 0; j < 1; j++)
						{
							bool placed = false;
							foreach (GrassToPlace curGrass in grassMappings)
							{

								for (int i = 0; i < grassMappings.Length; i++)
								{
									if (curGrass.index == i)
									{
										float chance = curGrass.weight / (grassMappings.Length + 1);
										if (Random.Range(0f, 100f) <= chance)
										{
											position = new Vector3((x * 16), (y * 16) + 16, -32 + (j * 16));
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
				}
			}
		}
	}

	public GameObject textObject;
	private GameObject selectedObject;
	int selectedIndex = 0;

	void Update()
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
}