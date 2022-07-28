using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player: MonoBehaviour
{
    private int number;
    private string playerName;
    private GameObject itemNear;
    private GameObject objectNearZPlus;
    private GameObject objectNearZMinus;
    private GameObject objectNearXPlus;
    private GameObject objectNearXMinus;
    private GameObject itemInHands;
    private CharacterController cc;
    private SkinnedMeshRenderer[] smr;
    private Animator anim;
    private int countLifes = 5;
    private bool damagedByVelocity = false;
    private float fallMagnitude = 0;
    private float oldPositionY = 0f;
    private int tempMask = 0;
    private bool isAnyBlockPlaced = false;
    private Vector3 pixelPos = new Vector3(0, 0, 0);

    private float xVel = 0;
    private float xVelNew = 0;
    private float yVel = 0;
    private float yVelNew = 0;
    private float sliceDelay = 0;
    private float lastPunchTime;
    private float lastDamaged;
    private bool leftSide = false;
    private KeyCode leftKey;
    private KeyCode rightKey;
    private KeyCode upKey;
    private KeyCode downKey;

    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject satietyBar;
    [SerializeField] Material mat;
    [SerializeField] Material matDamaged;
    [SerializeField] GameObject sliceObject;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float resistSpeed;
    [SerializeField] int curDir = -1;
    [SerializeField] GameObject damagePopup;
    [SerializeField] GameObject benchGrid;
    [SerializeField] GameObject otherPlayer;
    [SerializeField] GameObject canvasObject;
    [SerializeField] GameObject furnaceGrid;
    [SerializeField] GameObject rightItemSocket;
    [SerializeField] GameObject leftItemSocket;
    [SerializeField] GameObject levelParent;
    [SerializeField] Texture2D levelTexture;
    [SerializeField] int health;
    [SerializeField] int satiety;
    [SerializeField] int healthMax;
    [SerializeField] int satietyMax;
    SatietyBar satietyBarComponent;
    Slider satietyBarMainParts;
    Slider satietyBarBackParts;
    private float holdKeyDownForDrop;
    private GameObject gridObject;

    public GameObject ItemNear { get => itemNear; set => itemNear = value; }
    public GameObject ObjectNearZPlus { get => objectNearZPlus; set => objectNearZPlus = value; }
    public GameObject ObjectNearZMinus { get => objectNearZMinus; set => objectNearZMinus = value; }
    public GameObject ItemInHands { get => itemInHands; set => itemInHands = value; }
    public GameObject BenchGrid { get => benchGrid; set => benchGrid = value; }
    public CharacterController Cc { get => cc; set => cc = value; }
    public SkinnedMeshRenderer[] Smr { get => smr; set => smr = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public int Health { get => health; set => health = value; }
    public int Satiety { get => satiety; set => satiety = value; }
    public GameObject DamagePopup { get => damagePopup; set => damagePopup = value; }
    public int CountLifes { get => countLifes; set => countLifes = value; }
    public int SatietyMax { get => satietyMax; set => satietyMax = value; }
    public int HealthMax { get => healthMax; set => healthMax = value; }
    public GameObject OtherPlayer { get => otherPlayer; set => otherPlayer = value; }
    public GameObject CanvasObject { get => canvasObject; set => canvasObject = value; }
    public float OldPositionY { get => oldPositionY; set => oldPositionY = value; }
    public int CurDir { get => curDir; set => curDir = value; }
    public float ResistSpeed { get => resistSpeed; set => resistSpeed = value; }
    public float JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public GameObject SliceObject { get => sliceObject; set => sliceObject = value; }
    public float XVel { get => xVel; set => xVel = value; }
    public float XVelNew { get => xVelNew; set => xVelNew = value; }
    public float YVel { get => yVel; set => yVel = value; }
    public float YVelNew { get => yVelNew; set => yVelNew = value; }
    public float SliceDelay { get => sliceDelay; set => sliceDelay = value; }
    public float LastPunchTime { get => lastPunchTime; set => lastPunchTime = value; }
    public float LastDamaged { get => lastDamaged; set => lastDamaged = value; }
    public GameObject HealthBar { get => healthBar; set => healthBar = value; }
    public GameObject SatietyBar { get => satietyBar; set => satietyBar = value; }
    public Material Mat { get => mat; set => mat = value; }
    public Material MatDamaged { get => matDamaged; set => matDamaged = value; }
    public bool LeftSide { get => leftSide; set => leftSide = value; }
    public GameObject ObjectNearXPlus { get => objectNearXPlus; set => objectNearXPlus = value; }
    public GameObject ObjectNearXMinus { get => objectNearXMinus; set => objectNearXMinus = value; }
    public string PlayerName { get => playerName; set => playerName = value; }
    public int Number { get => number; set => number = value; }
    public GameObject FurnaceGrid { get => furnaceGrid; set => furnaceGrid = value; }

    void Start()
    {
        var cameraMain = GameObject.Find("CameraMain");
        var cnvs = GameObject.Find("Canvas");
        var craftPanelPosition = GameObject.Find("CraftPanelPosition");
        if (gameObject.name == "Player1(Clone)")
        {
            number = 1;
            cameraMain.GetComponent<Cam>().player1 = gameObject;
            BenchGrid = cnvs.transform.GetChild(0).gameObject;
            FurnaceGrid = cnvs.transform.GetChild(2).gameObject;
            //otherPlayer = GameObject.Find("Player2(Clone)");
            craftPanelPosition.GetComponent<CraftGridPosition>().Player1 = gameObject;
        } else if (gameObject.name == "Player2(Clone)")
        {
            number = 2;
            cameraMain.GetComponent<Cam>().player2 = gameObject;
            BenchGrid = cnvs.transform.GetChild(1).gameObject;
            FurnaceGrid = cnvs.transform.GetChild(3).gameObject;
            //otherPlayer = GameObject.Find("Player1(Clone)");
            craftPanelPosition.GetComponent<CraftGridPosition>().Player2 = gameObject;
        }
        gameObject.transform.GetChild(2).GetComponent<Canvas>().worldCamera = cameraMain.GetComponent<Camera>();
        benchGrid.GetComponent<CraftGrid>().Player = gameObject;
        FurnaceGrid.GetComponent<CraftGrid>().Player = gameObject;

        Anim = GetComponent<Animator>();
        Cc = GetComponent<CharacterController>();
        Smr = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer mr in Smr)
        {
            mr.material = Mat;
        }
        satietyBarComponent = SatietyBar.GetComponent<SatietyBar>();
        satietyBarMainParts = satietyBarComponent.barMain.GetComponent<Slider>();
        satietyBarBackParts = satietyBarComponent.barBack.GetComponent<Slider>();
        satietyBarComponent.creator = gameObject;
        satietyBarMainParts.minValue = 0;
        satietyBarMainParts.maxValue = SatietyMax;
        satietyBarBackParts.minValue = 0;
        satietyBarBackParts.maxValue = SatietyMax;
        satietyBarMainParts.value = SatietyMax;
        satietyBarComponent.barText.GetComponent<TextMeshProUGUI>().text = Satiety.ToString();
        GetComponent<TextMesh>().text = GetComponent<TextMesh>().text.Split(' ')[0] + ' ' + Satiety.ToString();
        var satietyBarObject = Instantiate(SatietyBar, transform.position, transform.rotation, CanvasObject.transform);
        satietyBarObject.transform.GetChild(0).transform.position = new Vector3(
            satietyBarObject.transform.GetChild(0).transform.position.x,
            satietyBarObject.transform.GetChild(0).transform.position.y-10,
            satietyBarObject.transform.GetChild(0).transform.position.z);
        satietyBarObject.transform.GetChild(0).transform.localScale = new Vector3(1, 0.5f, 1);

        var healthBarComponent = HealthBar.GetComponent<HealthBar>();
        var healthBarMainParts = healthBarComponent.barMain.GetComponent<Slider>();
        var healthBarBackParts = healthBarComponent.barBack.GetComponent<Slider>();
        healthBarComponent.creator = gameObject;
        healthBarMainParts.minValue = 0;
        healthBarMainParts.maxValue = HealthMax;
        healthBarBackParts.minValue = 0;
        healthBarBackParts.maxValue = HealthMax;
        healthBarMainParts.value = HealthMax;
        healthBarComponent.barText.GetComponent<TextMeshProUGUI>().text = Health.ToString();
        GetComponent<TextMesh>().text = Health.ToString() + ' ' + GetComponent<TextMesh>().text.Split(' ')[1];
        var healthBarObject = Instantiate(HealthBar, transform.position, transform.rotation, CanvasObject.transform);
        healthBarObject.transform.GetChild(0).transform.position = new Vector3(transform.position.x, satietyBarObject.transform.position.y+30, transform.position.z);

        InitKeyboard();

        InvokeRepeating("HealthTick", 0f, 5f);
        InvokeRepeating("GetHungryTick", 0f, 60f);
    }

    public void InitKeyboard()
    {
        if (gameObject.name == "Player1(Clone)")
        {
            leftKey = KeyCode.A;
            rightKey = KeyCode.D;
            upKey = KeyCode.W;
            downKey = KeyCode.S;
        }
        else if (gameObject.name == "Player2(Clone)")
        {
            leftKey = KeyCode.LeftArrow;
            rightKey = KeyCode.RightArrow;
            upKey = KeyCode.UpArrow;
            downKey = KeyCode.DownArrow;
        }
    }

    public void Update()
    {
        OldPositionY = transform.position.y;

        if (LastDamaged + 0.2f > Time.time)
        {
            foreach (SkinnedMeshRenderer mr in Smr)
            {
                mr.material = MatDamaged;
            }
        }
        else
        {
            foreach (SkinnedMeshRenderer mr in Smr)
            {
                mr.material = Mat;
            }
            LastDamaged = 0f;
        }

        ControlCraftGrid();

        if (Input.GetKeyDown(downKey) && Time.time > LastPunchTime + GLOBAL.PUNCH_DURATION)
        {
            StartCoroutine(ReleaseItem());
            Interact();
            gridObject = GetOpenGrid();
            if (gridObject)
            {
                gridObject.GetComponent<CraftGrid>().CloseChoiceInterface();
            }

            SliceDelay = (1f / 6);
            LastPunchTime = Time.time;
            SliceObject.GetComponent<Slice>().dir = CurDir;
            if (!ItemInHands || (!IsPlayerArmed(ItemInHands) && ItemInHands.CompareTag("Eat") == false))
            {
                int bodyAnim = 4;
                if (CurDir == 1)
                {
                    bodyAnim = 8;
                }
                if (CurDir == -1)
                {
                    bodyAnim = 4;
                }
                Anim.SetInteger("Body", bodyAnim);
            }
            else if (IsPlayerArmed(ItemInHands))
            {
                ArmorAnimation();
                ItemInHands.GetComponent<Weapon>().MakeHit();
            }

            if (ItemInHands)
            {
                if (ItemInHands.gameObject.GetComponent<Eat>() != null)
                {
                    Satiety += ItemInHands.gameObject.GetComponent<Item>().ItemModel.Description.satisfyingHunger;
                    if (Satiety > 25)
                    {
                        Satiety = 25;
                    }
                    GetHungryTick();
                    Destroy(ItemInHands);
                }
            }

            TakeItem();
        }
        if (Input.GetKeyDown(upKey))
        {
            gridObject = GetOpenGrid();
            if (gridObject)
            {
                GameObject craftObjectNear = GetCraftObjectNear();
                int bias = 0;
                if (craftObjectNear.CompareTag("Bench"))
                {
                    bias = 1;
                }
                else if (craftObjectNear.CompareTag("Furnace"))
                {
                    bias = 3;
                }
                if (craftObjectNear.transform.childCount - bias != 0 &&
                    craftObjectNear.transform.childCount - bias >= gridObject.GetComponent<CraftGrid>().CurrentSlot + 1)
                {
                    
                    GameObject go = craftObjectNear.transform.GetChild(
                            gridObject.GetComponent<CraftGrid>().CurrentSlot + bias
                        ).gameObject;
                    
                    if (curDir == -1)
                    {
                        go.transform.parent = rightItemSocket.transform;
                    }
                    else if (curDir == 1)
                    {
                        go.transform.parent = leftItemSocket.transform;
                    }
                    ItemInHands = go;
                    ItemInHands.SetActive(true);
                    itemInHands.GetComponent<Positioning>().PutItemInHands();
                }
                yVel = 0;
                StartCoroutine(CloseChoiceInterfaceCoroutine());
            }
        }
        if (Input.GetKeyDown(leftKey))
        {
            gridObject = GetOpenGrid();
            if (gridObject)
            {
                gridObject.GetComponent<CraftGrid>().LeftChoice();
            }
        }
        if (Input.GetKeyDown(rightKey))
        {
            gridObject = GetOpenGrid();
            if (gridObject)
            {
                gridObject.GetComponent<CraftGrid>().RightChoice();
            }
        }
        if (Input.GetKeyUp(downKey))
        {
            holdKeyDownForDrop = Time.time;
        }
        if (SliceDelay > 0 && Time.time > LastPunchTime + SliceDelay)
        {
            if (!ItemInHands || !IsPlayerArmed(ItemInHands))
            {
                if (CurDir == 1)
                    Instantiate(SliceObject, Cc.transform.position + (Vector3.up * 8) + new Vector3(XVel / 8, 0, 0), Quaternion.Euler(0, 0, 0));
                else if (CurDir == -1)
                    Instantiate(SliceObject, Cc.transform.position + (Vector3.up * 8) + new Vector3(XVel / 8, 0, 0), Quaternion.Euler(180, -180, 0));
                SliceDelay = 0;
            }
        }
    }

    private GameObject GetOpenGrid()
    {
        if (BenchGrid.activeSelf && benchGrid.GetComponent<CraftGrid>().ChoiceInterfaceOpen)
        {
            gridObject = BenchGrid;
        }
        else if (FurnaceGrid.activeSelf && FurnaceGrid.GetComponent<CraftGrid>().ChoiceInterfaceOpen)
        {
            gridObject = FurnaceGrid;
        }
        else
        {
            gridObject = null;
        }
        return gridObject;
    }

    private void ArmorAnimation()
    {
        if (ItemInHands.GetComponent<Item>().ItemModel.Description.itemClass == "Sword")
        {
            if (CurDir == -1)
            {
                Anim.SetInteger("Body", 3);
            }
            if (CurDir == 1)
            {
                Anim.SetInteger("Body", 5);
            }
        }
        if (ItemInHands.GetComponent<Item>().ItemModel.Description.itemClass == "Picker")
        {
            if (CurDir == -1)
            {
                Anim.SetInteger("Body", 10);
            }
            if (CurDir == 1)
            {
                Anim.SetInteger("Body", 9);
            }
        }
        if (ItemInHands.GetComponent<Item>().ItemModel.Description.itemClass == "Hatchet")
        {
            if (CurDir == -1)
            {
                Anim.SetInteger("Body", 12);
            }
            if (CurDir == 1)
            {
                Anim.SetInteger("Body", 11);
            }
        }
        if (ItemInHands.GetComponent<Item>().ItemModel.Description.itemClass == "Shovel")
        {
            if (CurDir == -1)
            {
                Anim.SetInteger("Body", 14);
            }
            if (CurDir == 1)
            {
                Anim.SetInteger("Body", 13);
            }
        }
        if (ItemInHands.GetComponent<Item>().ItemModel.Description.itemClass == "Spear")
        {
            if (CurDir == -1)
            {
                Anim.SetInteger("Body", 16);
            }
            if (CurDir == 1)
            {
                Anim.SetInteger("Body", 15);
            }
        }
    }

    public void FixedUpdate()
    {
        CheckNearObjects();

        YVelNew -= GLOBAL.GRAVITY;
        YVel -= GLOBAL.GRAVITY;
        if (Cc.isGrounded)
        {
            YVelNew = 0;
            YVel = 0;
        }
        if (Cc.isGrounded && Input.GetKey(upKey))
        {
            YVelNew = 1;
            YVel = GLOBAL.GRAVITY * JumpSpeed;
        }
        if (YVelNew > YVel)
        {
            YVel += GLOBAL.GRAVITY;
        }

        if (XVel == 0)
        {
            if (!ItemInHands)
            {
                if (Time.time > LastPunchTime + GLOBAL.PUNCH_DURATION)
                {
                    Anim.SetInteger("Body", 0);
                }
            }
            else
            {
                if (IsPlayerArmed(ItemInHands))
                {
                    StopArmorAnimation(0);
                }
                else
                {
                    if (Time.time > LastPunchTime + GLOBAL.PUNCH_DURATION)
                    {
                        Anim.SetInteger("Body", 0);
                    }
                }
            }

            Anim.SetInteger("Legs", 0);
        }
        if (Input.GetKey(leftKey))
        {
            transform.rotation = Quaternion.Euler(0, 56, 0);
            if (Anim.GetInteger("Body") == 8 || Anim.GetInteger("Body") == 5)
            {
                Anim.SetInteger("Body", 1);
                LastPunchTime = 0;
            }
            if ((!ItemInHands || !IsPlayerArmed(ItemInHands)) && Time.time > LastPunchTime + GLOBAL.PUNCH_DURATION)
            {
                Anim.SetInteger("Body", 1);
            }
            else if (ItemInHands)
            {
                if (IsPlayerArmed(ItemInHands))
                {
                    StopArmorAnimation(1);
                }
            }
            else
            {
                if (!ItemInHands)
                {
                    Anim.SetInteger("Body", 4);
                }
                else if (IsPlayerArmed(ItemInHands))
                {
                    Anim.SetInteger("Body", 3);
                    //ItemInHands.GetComponentInChildren<Weapon>().IsDamaging = true;
                }

            }
            Anim.SetInteger("Legs", 1);
            CurDir = -1;
            XVelNew = -MoveSpeed;
            XVel -= ResistSpeed;
            if (XVel <= XVelNew) XVel = XVelNew;

            if (!LeftSide)
            {
                LeftSide = true;
                ChangeHand();
            }
        }
        else if (!Input.GetKey(leftKey) && (CurDir == -1 || XVelNew > 0))
        {
            if (XVel < 0 && XVel < -ResistSpeed) XVel += ResistSpeed; //ТОРМОЗ
            if (XVel < 0 && XVel >= -ResistSpeed) XVel = 0;
        }
        if (Input.GetKey(rightKey))
        {
            transform.rotation = Quaternion.Euler(0, -56, 0);
            if (Anim.GetInteger("Body") == 4 || Anim.GetInteger("Body") == 3)
            {
                Anim.SetInteger("Body", 2);
                LastPunchTime = 0;
            }
            if ((!ItemInHands || (!IsPlayerArmed(ItemInHands))) && Time.time > LastPunchTime + GLOBAL.PUNCH_DURATION)
            {
                Anim.SetInteger("Body", 2);
            }
            else if (ItemInHands)
            {
                if (IsPlayerArmed(ItemInHands))
                {
                    StopArmorAnimation(2);
                }
            }
            else
            {

                if (!ItemInHands)
                {
                    Anim.SetInteger("Body", 8);
                }
                else if (IsPlayerArmed(ItemInHands))
                {
                    Anim.SetInteger("Body", 5);
                    //ItemInHands.GetComponentInChildren<Weapon>().IsDamaging = true;
                }

            }
            Anim.SetInteger("Legs", 2);
            CurDir = 1;
            XVelNew = MoveSpeed;
            XVel += ResistSpeed;
            if (XVel >= XVelNew) XVel = XVelNew;


            if (LeftSide)
            {
                LeftSide = false;
                ChangeHand();
            }
        }
        else if (!Input.GetKey(rightKey) && (CurDir == 1 || XVelNew < 0))
        {
            if (XVel > 0 && XVel > ResistSpeed) XVel -= ResistSpeed; //ТОРМОЗ
            if (XVel > 0 && XVel <= ResistSpeed) XVel = 0;
        }

        // If choice interface in craft grid is open
        gridObject = GetOpenGrid();
        if (gridObject)
        {
            XVel = 0;
            YVel = 0;
        }

        //IF SOME POSITION WRONG
        if (Cc.transform.position.y < -512)
        {
            CountLifes -= 1;
            Cc.transform.position = OtherPlayer.transform.position + (Vector3.up * 16);
        }
        else
        {
            Cc.Move(new Vector3(XVel * Time.fixedDeltaTime, YVel * Time.fixedDeltaTime, 0));
        }

        if (Cc.transform.position.z != 0) Cc.transform.position = new Vector3(Cc.transform.position.x, Cc.transform.position.y, 0);

        CheckHealthLife();

        {
            //BUILDING
            if (GLOBAL.SELECTED_BLOCK)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Vector3 position = new Vector3((Mathf.RoundToInt(Cc.transform.position.x / 16) * 16), (Mathf.RoundToInt(Cc.transform.position.y / 16) * 16), 32);
                    Instantiate(GLOBAL.SELECTED_BLOCK, position, Quaternion.Euler(-90, 180, 0), levelParent.transform);
                    pixelPos = position; tempMask += 4;
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    Vector3 position = new Vector3((Mathf.RoundToInt(Cc.transform.position.x / 16) * 16), (Mathf.RoundToInt(Cc.transform.position.y / 16) * 16), 16);
                    Instantiate(GLOBAL.SELECTED_BLOCK, position, Quaternion.Euler(-90, 180, 0), levelParent.transform);
                    pixelPos = position; tempMask += 8;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    Vector3 position = new Vector3((Mathf.RoundToInt(Cc.transform.position.x / 16) * 16), (Mathf.RoundToInt(Cc.transform.position.y / 16) * 16), 0);
                    Instantiate(GLOBAL.SELECTED_BLOCK, position, Quaternion.Euler(-90, 180, 0), levelParent.transform);
                    pixelPos = position; tempMask += 16;
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    Vector3 position = new Vector3((Mathf.RoundToInt(Cc.transform.position.x / 16) * 16), (Mathf.RoundToInt(Cc.transform.position.y / 16) * 16), -16);
                    Instantiate(GLOBAL.SELECTED_BLOCK, position, Quaternion.Euler(-90, 180, 0), levelParent.transform);
                    pixelPos = position; tempMask += 32;
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    Vector3 position = new Vector3((Mathf.RoundToInt(Cc.transform.position.x / 16) * 16), (Mathf.RoundToInt(Cc.transform.position.y / 16) * 16), -32);
                    Instantiate(GLOBAL.SELECTED_BLOCK, position, Quaternion.Euler(-90, 180, 0), levelParent.transform);
                    pixelPos = position; tempMask += 64;
                }
                if (Input.GetKeyDown(KeyCode.Equals))
                {
                    //Debug.Log("PLACED BLOCKS SAVED");
                    isAnyBlockPlaced = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    //Debug.Log("BLOCKS ROW DELETED");
                    isAnyBlockPlaced = true; tempMask = 0;
                }
                if (isAnyBlockPlaced)
                {
                    //string str = "Some block was placed.";
                    foreach (ColorToPrefab colorMapping in levelParent.GetComponent<LevelGenerator>().colorMappings)
                    {
                        if (colorMapping.prefab == GLOBAL.SELECTED_BLOCK)
                        {
                            //str += " Placed block contains in ColorMappings.\n";
                            Color maskedColor, mappingColor;
                            if (tempMask == 0)
                            {
                                pixelPos = new Vector3((Mathf.RoundToInt(Cc.transform.position.x / 16) * 16), (Mathf.RoundToInt(Cc.transform.position.y / 16) * 16), -32);
                                mappingColor = new Color(1f, 0f, 0f, 0f);
                                maskedColor = new Color(1f, 0f, 0f, 0f);
                            }
                            else
                            {
                                mappingColor = colorMapping.color;
                                maskedColor = new Color((float)tempMask / 255, 0, 0, 1);
                            }
                            levelTexture.SetPixel((int)(pixelPos.x / 16) * 2, (int)(pixelPos.y / 16) * 2, mappingColor);
                            levelTexture.SetPixel((int)(pixelPos.x / 16) * 2 + 1, (int)(pixelPos.y / 16) * 2, mappingColor);
                            levelTexture.SetPixel((int)(pixelPos.x / 16) * 2, (int)(pixelPos.y / 16) * 2 + 1, mappingColor);
                            levelTexture.SetPixel((int)(pixelPos.x / 16) * 2 + 1, (int)(pixelPos.y / 16) * 2 + 1, maskedColor);
                            levelTexture.Apply(false);
                            string path = Application.dataPath + "/../" + AssetDatabase.GetAssetPath(levelTexture);
                            File.WriteAllBytes(path, levelTexture.EncodeToPNG());
                            //str += " Pixel-1 (" + ((int)(pixelPos.x / 16) * 2).ToString() + ", " + ((int)(pixelPos.y / 16) * 2).ToString() + "). Color-1 (" + colorMapping.color.ToString() + ").\n";
                            //str += " Pixel-2 (" + ((int)(pixelPos.x / 16) * 2 + 1).ToString() + ", " + ((int)(pixelPos.y / 16) * 2).ToString() + "). Color-2 (" + colorMapping.color.ToString() + ").\n";
                            //str += " Pixel-3 (" + ((int)(pixelPos.x / 16) * 2).ToString() + ", " + ((int)(pixelPos.y / 16) * 2 + 1).ToString() + "). Color-3 (" + colorMapping.color.ToString() + ").\n";
                            //str += " Pixel-4 (" + ((int)(pixelPos.x / 16) * 2 + 1).ToString() + ", " + ((int)(pixelPos.y / 16) * 2 + 1).ToString() + "). Color-4 (" + maskedColor.ToString() + ").\n";
                        }
                    }
                    //Debug.Log(str);
                    isAnyBlockPlaced = false;
                    pixelPos = new Vector3(0, 0, 0);
                }
            }
        }
    }

    private void StopArmorAnimation(int v)
    {
        float duration = GLOBAL.ITEM_CLASSES[ItemInHands.GetComponent<Item>().ItemModel.Description.itemClass];
        if (Time.time > LastPunchTime + duration)
        {
            Anim.SetInteger("Body", v);
        }
    }

    public void ControlCraftGrid()
    {
        GameObject craftObjectNear = GetCraftObjectNear();
        
        if (craftObjectNear != null)
        {
            if ((!craftObjectNear.GetComponent<Interactable>().IsUsing ||
                craftObjectNear.GetComponent<Interactable>().lastInteractPlayer == gameObject))
            {
                if (craftObjectNear.CompareTag("Bench") == true)
                {
                    BenchGrid.GetComponent<CraftGrid>().Open();
                    if (FurnaceGrid.activeSelf)
                    {
                        FurnaceGrid.GetComponent<CraftGrid>().Close();
                    }
                }
                else if (craftObjectNear.CompareTag("Furnace") == true)
                {
                    FurnaceGrid.GetComponent<CraftGrid>().Open();
                    if (BenchGrid.activeSelf)
                    {
                        BenchGrid.GetComponent<CraftGrid>().Close();
                    }
                }

                craftObjectNear.GetComponent<Interactable>().IsUsing = true;
                craftObjectNear.GetComponent<Interactable>().lastInteractPlayer = gameObject;
            }
        }
        else
        {
            if (BenchGrid.activeSelf)
            {
                BenchGrid.GetComponent<CraftGrid>().Close();
            }
            else if (FurnaceGrid.activeSelf)
            {
                FurnaceGrid.GetComponent<CraftGrid>().Close();
            }

        }
    }

    private GameObject GetCraftObjectNear()
    {
        GameObject craftObjectNear = null;
        if (ObjectNearZPlus && (ObjectNearZPlus.CompareTag("Bench") == true || ObjectNearZPlus.CompareTag("Furnace") == true))
        {
            craftObjectNear = ObjectNearZPlus;
        }
        if (ObjectNearZMinus && (ObjectNearZMinus.CompareTag("Bench") == true || ObjectNearZMinus.CompareTag("Furnace") == true))
        {
            craftObjectNear = ObjectNearZMinus;
        }
        return craftObjectNear;
    }

    public void HealthTick()
    {
        if (Satiety >= 20 && Health > 0 && Health < HealthMax)
        {
            Health += 1;
        }
        GetComponent<TextMesh>().text = Health.ToString() + ' ' + GetComponent<TextMesh>().text.Split(' ')[1];
    }

    public void GetHungryTick()
    {
        if (Satiety > 0)
        {
            Satiety -= 1;
        }
        if (Satiety >= 20)
        {
            GetComponent<TextMesh>().text = GetComponent<TextMesh>().text.Split(' ')[0] + " 20";
            return;
        }
        GetComponent<TextMesh>().text = GetComponent<TextMesh>().text.Split(' ')[0] + " " + Satiety.ToString();
    }

    public static bool IsPlayerArmed(GameObject itemInHands)
    {
        if (itemInHands.GetComponent<Item>() && 
            (itemInHands.GetComponent<Item>().ItemModel.Description.type == "Weapon" ||
                itemInHands.GetComponent<Item>().ItemModel.Description.type == "WeaponAndCraft"))
        {
            return true;
        }
        return false;
    }

    public void CheckHealthLife()
    {
        if (OtherPlayer)
        {
            if (Health <= 0)
            {
                DropItemInHands();
                CountLifes -= 1;
                Health = HealthMax;
                GetComponent<TextMesh>().text = Health.ToString() + ' ' + GetComponent<TextMesh>().text.Split(' ')[1];
                Cc.enabled = false;
                Cc.transform.position = OtherPlayer.transform.position + (Vector3.up * 16);
                Cc.enabled = true;
            }
        }

        FallDamage();

        if (CountLifes == 0)
        {
            Destroy(gameObject);
        }
    }

    private void FallDamage()
    {
        if (Cc.velocity.magnitude >= 160 && Cc.velocity.magnitude > fallMagnitude && Math.Abs(transform.position.y - OldPositionY) > 3)
        {
            fallMagnitude = Cc.velocity.magnitude;
        }

        if (Cc.isGrounded && fallMagnitude > 0)
        {
            LastDamaged = Time.time;
            Health -= 3;
            GetComponent<TextMesh>().text = Health.ToString() + ' ' + GetComponent<TextMesh>().text.Split(' ')[1];
            DamagePopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "3";
            Instantiate(DamagePopup, gameObject.transform.position + Vector3.up * 25f, Quaternion.Euler(0, 0, 0), CanvasObject.transform);
            fallMagnitude = 0;
        }

        if (Math.Abs(transform.position.y - OldPositionY) > 10 && fallMagnitude > 300)
        {
            CountLifes -= 1;
            Cc.transform.position = OtherPlayer.transform.position + (Vector3.up * 16);
            fallMagnitude = 0;
            damagedByVelocity = true;
        }
        if (damagedByVelocity && Cc.isGrounded)
        {
            damagedByVelocity = false;
        }
    }

    private void DropItemInHands()
    {
        if (ItemInHands)
        {
            ItemInHands.transform.parent = GameManager.ItemsWorldStatic.transform;
            ItemInHands.GetComponent<Pickable>().Owner = null;
            itemInHands.transform.position = gameObject.transform.position;
            ItemInHands.GetComponent<Positioning>().DropItemFromHands();
            ItemInHands = null;
        }
    }

    public void CheckNearObjects()
    {
        RaycastToFind("plus"); //z
        RaycastToFind("minus"); // z
        RaycastToFind("XPlus");
        RaycastToFind("XMinus");
    }

    private void RaycastToFind(string direction)
    {
        int rayOriginDistance = 1;
        Vector3 dirVector = Vector3.forward;
        GameObject objectNear = objectNearZPlus;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y, transform.position.z + rayOriginDistance * dirVector.z);
        if (direction == "minus")
        {
            dirVector = Vector3.back;
            objectNear = objectNearZMinus;
            origin = new Vector3(transform.position.x, transform.position.y, transform.position.z + rayOriginDistance * dirVector.z);
        } else if (direction == "XPlus")
        {
            dirVector = Vector3.right;
            objectNear = objectNearXPlus;
            origin = new Vector3(transform.position.x + rayOriginDistance * dirVector.x, transform.position.y, transform.position.z + 6);
        } else if (direction == "XMinus")
        {
            dirVector = Vector3.left;
            objectNear = ObjectNearXMinus;
            origin = new Vector3(transform.position.x + rayOriginDistance * dirVector.x, transform.position.y, transform.position.z + 6);
        }
        Ray ray = new Ray(origin, dirVector);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 24);
        if (hit.collider)
        {
            if (objectNear && objectNear.GetComponent<Interactable>() && objectNear != hit.collider.gameObject)
            {
                objectNear.GetComponent<Interactable>().ClearInteractableObject(gameObject, gameObject.name);
            }
            if (hit.collider.gameObject.GetComponent<Interactable>() &&
                (!hit.collider.gameObject.GetComponent<Interactable>().IsUsing ||
                    hit.collider.gameObject.GetComponent<Interactable>().lastInteractPlayer == gameObject))
            {
                hit.collider.gameObject.GetComponent<Interactable>().SetInteractableObject(gameObject);
            }
            if (direction == "minus")
            {
                objectNearZMinus = hit.collider.gameObject;
            }
            else if (direction == "plus")
            {
                objectNearZPlus = hit.collider.gameObject;
            }
            else if (direction == "XPlus")
            {
                objectNearXPlus = hit.collider.gameObject;
            }
            else if (direction == "XMinus")
            {
                objectNearXMinus = hit.collider.gameObject;
            }
        }
        else
        {
            if (objectNear && objectNear.GetComponent<Interactable>())
            {
                objectNear.GetComponent<Interactable>().ClearInteractableObject(gameObject, gameObject.name);
            }
            if (direction == "minus")
            {
                objectNearZMinus = null;
            }
            else if (direction == "plus")
            {
                objectNearZPlus = null;
            }
            else if (direction == "XPlus")
            {
                objectNearXPlus = null;
            }
            else if (direction == "XMinus")
            {
                objectNearXMinus = null;
            }
        }
    }

    public void Interact()
    {
        if (ItemInHands && ItemInHands.CompareTag("WeaponAndCraft"))
        {
            ItemInHands.GetComponent<Weapon>().ObtainItem();
        }
        else
        {
            InteractWithTree();
            InteractWithCraftMechanism();
            InteractWithDoor();
        }
    }

    private void InteractWithDoor()
    {
        GameObject objectNear = null;
        if (ObjectNearXPlus && ObjectNearXPlus.CompareTag("Door"))
        {
            objectNear = ObjectNearXPlus;
        }
        else if (ObjectNearXMinus && ObjectNearXMinus.CompareTag("Door"))
        {
            objectNear = ObjectNearXMinus;
        }
        
        if (objectNear)
        {
            if (objectNear.GetComponent<Door>().Locked && itemInHands && itemInHands.CompareTag("Key"))
            {
                if (objectNear.GetComponent<Door>().Interact(curDir * -1, true))
                {
                    itemInHands.GetComponent<Key>().Use();
                    return;
                }
            }
            
            objectNear.GetComponent<Door>().Interact(curDir * -1, false);
        }
    }

    private void InteractWithCraftMechanism()
    {
        GameObject objectNear = GetCraftObjectNear();

        if (objectNear)
        {
            if (!objectNear.GetComponent<Interactable>().IsUsing || objectNear.GetComponent<Interactable>().lastInteractPlayer == gameObject)
            {
                //подбор результата
                if (objectNear.transform.GetChild(0).childCount > 0)
                {
                    DropItemInHands();
                    // у верстака есть объект результат, а у результата объекты анимации
                    ItemInHands = objectNear.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
                    GameManager.DeleteItemEffect(ItemInHands);
                    GameManager.DeleteAnimator(ItemInHands);
                    ItemInHands.transform.position = new Vector3(0, 0, 0);
                    ItemInHands.transform.rotation = new Quaternion(0, 0, 0, 0);
                    ItemInHands.GetComponent<Positioning>().OnGroundOrHand = true;
                    ItemInHands.GetComponent<Positioning>().InHands = true;
                    itemInHands.GetComponent<Pickable>().Owner = gameObject;
                    itemInHands.GetComponent<Positioning>().PutItemInHands();
                    ChangeHand();
                }
                else
                {
                    // кладем предмет из руки в верстак
                    if (itemInHands)
                    {
                        if (objectNear.GetComponent<Interactable>().PutItemInObject(ItemInHands))
                        {
                            ItemInHands.GetComponent<Positioning>().OnGroundOrHand = false;
                            ItemInHands.GetComponent<Positioning>().InHands = false;
                            itemInHands.GetComponent<Pickable>().Owner = null;
                            ItemInHands.gameObject.SetActive(false);
                            bool done = false;
                            if (objectNear.CompareTag("Bench"))
                            {
                                done = objectNear.GetComponent<CraftMechanism>().TryUseRecipe();
                            }
                            MakeCraftGrid(done, objectNear);
                            itemInHands = null;
                        }
                    }
                    else
                    {
                        StartCoroutine(OpenChoiceInterface(objectNear));
                    }
                }
            }
        }
    }

    private void MakeCraftGrid(bool done, GameObject objectNear)
    {
        if (done == true)
        {
            if (objectNear.CompareTag("Bench"))
            {
                benchGrid.GetComponent<CraftGrid>().CleanSlots();
            }
            else if (objectNear.CompareTag("Furnace"))
            {
                FurnaceGrid.GetComponent<CraftGrid>().CleanSlots();
            }
        }
        else
        {
            if (objectNear.CompareTag("Bench"))
            {
                benchGrid.GetComponent<CraftGrid>().SetItemToSlot(
                    objectNear.transform.GetChild(objectNear.transform.childCount - 1).name, objectNear.transform.childCount - 2
                );
            }
            else if (objectNear.CompareTag("Furnace"))
            {
                int itemNum = int.Parse(itemInHands.name.Replace("Item", "").Replace("(Clone)", ""));
                if (CraftRecipes.Resource.Contains(itemNum))
                {
                    FurnaceGrid.GetComponent<CraftGrid>().SetItemToSlot(
                        itemInHands.name, 0
                    );
                }
                else if (CraftRecipes.Meltable.Contains(itemNum))
                {
                    furnaceGrid.GetComponent<CraftGrid>().SetItemToSlot(
                        itemInHands.name, 1
                    );
                    
                }

            }
        }
    }

    private void InteractWithTree()
    {
        if (ObjectNearZPlus && ObjectNearZPlus.CompareTag("Tree"))
        {
            ObjectNearZPlus.GetComponent<RegularTree>().PunchesForStickSpawn += 1;
        }
        else if (ObjectNearZMinus && ObjectNearZMinus.CompareTag("Tree"))
        {
            ObjectNearZMinus.GetComponent<RegularTree>().PunchesForStickSpawn += 1;
        }
    }

    public IEnumerator OpenChoiceInterface(GameObject objectNear)
    {
        yield return new WaitForSeconds(1f);
        if (Input.GetKey(downKey))
        {
            if (objectNear.CompareTag("Bench"))
            {
                BenchGrid.GetComponent<CraftGrid>().OpenCraftGridChoiceInterface();
            } else if (objectNear.CompareTag("Furnace"))
            {
                FurnaceGrid.GetComponent<CraftGrid>().OpenCraftGridChoiceInterface();
            }
        }
    }

    private IEnumerator ReleaseItem()
    {
        yield return new WaitForSeconds(1f);
        if (Input.GetKey(downKey) && holdKeyDownForDrop + 1f < Time.time)
        {
            DropItemInHands();
        }
        holdKeyDownForDrop = 0f;
    }

    private IEnumerator CloseChoiceInterfaceCoroutine()
    {
        // delay for jumping on choosing item
        yield return new WaitForSeconds(0.15f);
        gridObject.GetComponent<CraftGrid>().CloseChoiceInterface();
        gridObject.GetComponent<CraftGrid>().Close();
        gridObject.GetComponent<CraftGrid>().Open();
    }

    public void TakeItem()
    {
        if (ItemNear && ItemNear.GetComponent<Pickable>().Owner != otherPlayer)
        {
            if (ItemInHands)
            {
                ItemInHands.transform.parent = null;
                ItemInHands.GetComponent<Positioning>().OnGroundOrHand = false;
                ItemInHands.GetComponent<Positioning>().InHands = false;
                ItemInHands.GetComponent<Pickable>().Owner = null;
            }
            GameManager.DeleteItemEffect(ItemNear);
            GameManager.DeleteAnimator(ItemNear);
            
            ItemInHands = ItemNear;
            ItemInHands.GetComponent<Positioning>().PutItemInHands();
            ItemNear = null;
            ItemInHands.GetComponent<Pickable>().Owner = gameObject;
            ChangeHand();
        }
    }

    public void ChangeItemPosition()
    {
        if (LeftSide)
        {
            ItemInHands.transform.parent = rightItemSocket.transform;
        }
        else
        {
            ItemInHands.transform.parent = leftItemSocket.transform;
        }
        
        ItemInHands.transform.localPosition = new Vector3(0,0,0);
        ItemInHands.transform.rotation = ItemInHands.transform.parent.rotation;
    }

    public void ChangeHand()
    {
        if (!ItemInHands)
        {
            return;
        }

        // если мы шли и сменили направление движения,
        // то возвращать тело в исходное положение
        if (GetComponent<Animator>().GetInteger("Body") == 2 || GetComponent<Animator>().GetInteger("Body") == 1)
        {
            GetComponent<Animator>().SetInteger("Body", 0);
        }
        //

        ChangeItemPosition();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("EnemyBullet") == true)   
        {
            int dirFactor = col.gameObject.GetComponent<Claws>().dir;
            XVel = 2 * MoveSpeed * dirFactor;
            Health -= 3;
            if (Health < 0)
            {
                Health = 0;
            }
            CheckHealthLife();
            GetComponent<TextMesh>().text = Health.ToString() + ' ' + GetComponent<TextMesh>().text.Split(' ')[1]; //ВАЖНО ЧТОБ НЕ ПРОВЕРЯТЬ В ХЕЛЗБАРЕ ВСЕ ТИПЫ СУЩЕСТВ
            LastDamaged = Time.time;
            DamagePopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "3";
            Instantiate(DamagePopup, gameObject.transform.position + Vector3.up * 25f, Quaternion.Euler(0, 0, 0), CanvasObject.transform);
        }
    }
}
