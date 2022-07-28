using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Zombie : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;
    public GameObject clawsObject;
    public float moveSpeed;
    public float jumpSpeed;
    public float resistSpeed;
    public int curDir = -1;
    public int health;
    public int healthMax;
    private Animator anim;
    private CharacterController cc;
    private float xVel = 0;
    private float xVelNew = 0;
    private float yVel = 0;
    private float yVelNew = 0;
    private float clawsDelay = 0;
    private float lastPunchTime;
    public GameObject canvasObject;
    public GameObject healthBar;
    public Material mat;
    public Material matDamaged;
    private int rnd;
    public float activeRange;
    public float aggroRange;
    private float lastDamaged;
    private SkinnedMeshRenderer[] smr;
    public GameObject damagePopup;
    private GameObject curTarPlayer = null;
    private float oldPositionX;

    public float XVel { get => xVel; set => xVel = value; }
    public float YVel { get => yVel; set => yVel = value; }
    public GameObject Player1 { get => player1; set => player1 = value; }
    public GameObject Player2 { get => player2; set => player2 = value; }

    void Start()
    {
        var cameraMain = GameObject.Find("CameraMain");
        if (!Player1)
        {
            Player1 = GameObject.Find("Player1(Clone)");
        }
        if (!Player2)
        {
            Player2 = GameObject.Find("Player2(Clone)");
        }
        
        gameObject.transform.GetComponentInChildren<Canvas>().worldCamera = cameraMain.GetComponent<Camera>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        smr = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer mr in smr)
        {
            mr.material = mat;
        }
        var healthBarComponent = healthBar.GetComponent<HealthBar>();
        var healthBarMainParts = healthBarComponent.barMain.GetComponent<Slider>();
        var healthBarBackParts = healthBarComponent.barBack.GetComponent<Slider>();
        healthBarComponent.creator = gameObject;
        healthBarMainParts.minValue = 0;
        healthBarMainParts.maxValue = healthMax;
        healthBarBackParts.minValue = 0;
        healthBarBackParts.maxValue = healthMax;
        healthBarMainParts.value = healthMax;
        healthBarComponent.barText.GetComponent<TextMeshProUGUI>().text = health.ToString();
        GetComponent<TextMesh>().text = health.ToString();
        Instantiate(healthBar, transform.position, transform.rotation, canvasObject.transform);
        InvokeRepeating("SlowUpdate", 0f, 1f);
        InvokeRepeating("HealthTick", 0f, 5f);
    }

    void SlowUpdate()
    {
        rnd = Random.Range(0, 4); //0-ПРЫЖОК 1-ВЛЕВО 2-ВПРАВО 3-УДАР
    }

    void HealthTick()
    {
        if (health > 0 && health < healthMax)
        {
            health += 1;
            GetComponent<TextMesh>().text = health.ToString();
        }
    }

    void FixedUpdate()
    {
        oldPositionX = transform.position.x;

        if (!Player1 && !Player2) return;
        float dist1 = 9999, dist2 = 9999, minTarDist;
        if (Player1) dist1 = Mathf.Abs(transform.position.x - Player1.transform.position.x);
        if (Player2) dist2 = Mathf.Abs(transform.position.x - Player2.transform.position.x);
        minTarDist = Mathf.Min(dist1, dist2);
        if (minTarDist > activeRange)
        {
            if (GameManager.CountZombie > 2 && DirLight.IsDay)
            {
                GameManager.RemoveZombie(gameObject);
            }
            return;
        }
        if (minTarDist < aggroRange)
        {
            if (dist1 < dist2)
            {
                curTarPlayer = Player1;
            }
            else if (dist1 > dist2)
            {
                curTarPlayer = Player2;
            }

            if (!Player1)
            {
                curTarPlayer = Player2;
            }
            else if (!Player2)
            {
                curTarPlayer = Player1;
            }
            if (!curTarPlayer)
            {
                return;
            }

            //if (!curTarPlayer) return; //ПОПРАВИТЬ ЧТОБ ЗОМБИ ПЕРЕКЛЮЧАЛСЯ НА ИГРОКА 2 ИЛИ 1
            int tmpRnd;
            if ((transform.position.x - curTarPlayer.transform.position.x) > 10)
            {
                transform.rotation = Quaternion.Euler(0, 56, 0);
                curDir = -1;
                if (rnd == 2)
                {
                    tmpRnd = Random.Range(0, 2);
                    if (tmpRnd == 0)
                        rnd = 1;
                    else
                        rnd = 3;
                }
            } else if ((transform.position.x - curTarPlayer.transform.position.x) <= -10)
            {
                transform.rotation = Quaternion.Euler(0, -56, 0);
                curDir = 1;
                if (rnd == 1)
                {
                    tmpRnd = Random.Range(0, 2);
                    if (tmpRnd == 0)
                        rnd = 2;
                    else
                        rnd = 3;
                }
            }
        }
        else
        {
            if(rnd == 3) rnd = Random.Range(0, 3);
        }
        string curTarPlayerName = "none";
        if (curTarPlayer)
        {
            curTarPlayerName = curTarPlayer.name;
        }

        //Debug.Log("rnd = " + rnd.ToString() + ", minTarDist = " + minTarDist.ToString() + ", curTarPlayer = " + curTarPlayerName);

        yVelNew -= GLOBAL.GRAVITY;
        YVel -= GLOBAL.GRAVITY;
        if (cc.isGrounded)
        {
            yVelNew = 0;
            YVel = 0;
        }
        if (cc.isGrounded && rnd == 0)
        {
            if (Random.Range(0, 100) < 5)
            {
                yVelNew = 1;
                YVel = GLOBAL.GRAVITY * jumpSpeed;
            }
        }
        if (yVelNew > YVel)
        {
            YVel += GLOBAL.GRAVITY;
        }

        if (XVel == 0)
        {
            if (Time.time > lastPunchTime + GLOBAL.PUNCH_DURATION) anim.SetInteger("Body", 0);
            anim.SetInteger("Legs", 0);
        }
        if (rnd == 1)
        {
            transform.rotation = Quaternion.Euler(0, 56, 0);
            if (anim.GetInteger("Body") == 8)
            {
                anim.SetInteger("Body", 1);
                lastPunchTime = 0;
            }
            if (Time.time > lastPunchTime + GLOBAL.PUNCH_DURATION)
            {

                anim.SetInteger("Body", 1);
            }
            else
            {
                anim.SetInteger("Body", 4);
            }
            anim.SetInteger("Legs", 1);
            curDir = -1;
            xVelNew = -moveSpeed;
            XVel -= resistSpeed;
            if (XVel <= xVelNew) XVel = xVelNew;
        }
        else if (rnd != 1 && (curDir == -1 || xVelNew > 0))
        {
            if (XVel < 0 && XVel < -resistSpeed) XVel += resistSpeed; //ТОРМОЗ
            if (XVel < 0 && XVel >= -resistSpeed) XVel = 0;
        }
        if (rnd == 2)
        {
            transform.rotation = Quaternion.Euler(0, -56, 0);
            if (anim.GetInteger("Body") == 4)
            {
                anim.SetInteger("Body", 2);
                lastPunchTime = 0;
            }
            if (Time.time > lastPunchTime + GLOBAL.PUNCH_DURATION)
            {
                anim.SetInteger("Body", 2);
            }
            else
            {
                anim.SetInteger("Body", 8);
            }
            anim.SetInteger("Legs", 2);
            curDir = 1;
            xVelNew = moveSpeed;
            XVel += resistSpeed;
            if (XVel >= xVelNew) XVel = xVelNew;
        }
        else if (rnd != 2 && (curDir == 1 || xVelNew < 0))
        {
            if (XVel > 0 && XVel > resistSpeed) XVel -= resistSpeed; //ТОРМОЗ
            if (XVel > 0 && XVel <= resistSpeed) XVel = 0;
        }

        //IF SOME POSITION WRONG
        if (cc.transform.position.y < -512)
        {
            GameManager.DecreaseCountObjectsInWorld(gameObject.name);
            Destroy(gameObject);
        }
        else
            cc.Move(new Vector3(XVel * Time.fixedDeltaTime, YVel * Time.fixedDeltaTime, 0));
        if (cc.transform.position.z != 0) cc.transform.position = new Vector3(cc.transform.position.x, cc.transform.position.y, 0);

        //УДАР
        if (rnd == 3 && Time.time > lastPunchTime + GLOBAL.PUNCH_DURATION)
        {
            if (curDir == -1)
            {
                clawsDelay = GLOBAL.CLAWS_DURATION / 2; lastPunchTime = Time.time;
                anim.SetInteger("Body", 4);
                clawsObject.GetComponent<Claws>().dir = -1;
            }
            if (curDir == 1)
            {
                clawsDelay = GLOBAL.CLAWS_DURATION / 2; lastPunchTime = Time.time;
                anim.SetInteger("Body", 8);
                clawsObject.GetComponent<Claws>().dir = 1;
            }
        }
        if (clawsDelay > 0 && Time.time > lastPunchTime + clawsDelay)
        {
            if (curDir == 1)
                Instantiate(clawsObject, cc.transform.position + (Vector3.up * 8) + new Vector3(XVel / 8, 0, 0), Quaternion.Euler(0,-90,0));
            else if (curDir == -1)
                Instantiate(clawsObject, cc.transform.position + (Vector3.up * 8) + new Vector3(XVel / 8, 0, 0), Quaternion.Euler(0, 90, -90));
            clawsDelay = 0;
        }
    }


    void Update()
    {
        if (lastDamaged + 0.2f > Time.time)
        {
            foreach (SkinnedMeshRenderer mr in smr)
            {
                mr.material = matDamaged;
            }
        }
        else
        {
            foreach (SkinnedMeshRenderer mr in smr)
            {
                mr.material = mat;
            }
            lastDamaged = 0;
        }

        if (health <= 0)
        {
            Death();
        }

        if (transform.position.x == oldPositionX)
        {
            anim.SetInteger("Legs", 0);
            anim.SetInteger("Body", 0);
        }
        else
        {
            if (rnd == 2)
            {
                anim.SetInteger("Legs", 2);
                anim.SetInteger("Body", 2);
            }
            else if (rnd == 1)
            {
                anim.SetInteger("Legs", 1);
                anim.SetInteger("Body", 1);
            }
                
        }
    }

    private void Death()
    {
        GameObject bone = GameManager.CreateItem(236, transform.position);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Zombie trigger with " + col.gameObject.name);
        if (col.gameObject.CompareTag("OurBullet") == true)
        {
            //Debug.Log("Zombie trigger with OurBullet");
            int dirFactor = col.gameObject.GetComponentInParent<Slice>().dir;
            XVel = 2 * moveSpeed * dirFactor;
            GetDamage(1);
            lastDamaged = Time.time;
            damagePopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "1";
            Instantiate(damagePopup, gameObject.transform.position + Vector3.up * 25f, Quaternion.Euler(0, 0, 0), canvasObject.transform);
        } else if (col.gameObject.CompareTag("Hit") == true)
        {
            col.gameObject.GetComponent<Hit>().Weapon.GetComponent<Weapon>().DecreaseDurability(1);
            int dirFactor = col.gameObject.GetComponent<Hit>().Weapon.GetComponent<Weapon>().dir;
            XVel = 3 * moveSpeed * dirFactor;
            int dmg = col.gameObject.GetComponent<Hit>().Weapon.GetComponent<Item>().ItemModel.Description.damage;
            GetDamage(dmg);
            damagePopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = dmg.ToString();
            Instantiate(
                damagePopup, 
                gameObject.transform.position + Vector3.up * 25f, 
                Quaternion.Euler(0, 0, 0), 
                canvasObject.transform
            );
            //col.gameObject.transform.parent.GetComponentInChildren<Weapon>().IsDamaging = false;
        }
    }

    public void GetDamage(int damage)
    {
        health -= damage;
        if (health < 0) {
            health = 0;
            GameManager.DecreaseCountObjectsInWorld(gameObject.name);
        }
        GetComponent<TextMesh>().text = health.ToString();
    }
}