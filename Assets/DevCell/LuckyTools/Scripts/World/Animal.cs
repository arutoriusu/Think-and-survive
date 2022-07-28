using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    private string type;
    private int rnd = 1;
    public float resistSpeed = 2;
    private float xVel = 0;
    private float xVelNew = 0;
    private int moveSpeed = 30;
    private float yVel = 0;
    private float yVelNew = 0;
    private int jumpSpeed = 15;
    private int health = 4;
    private float oldPositionX;
    private float lastDamaged;

    private Animator anim;
    private CharacterController cc;
    [SerializeField] GameObject canvasObject;
    [SerializeField] GameObject damagePopup;

    public int Health { get => health; set => health = value; }
    public float XVel { get => xVel; set => xVel = value; }
    public float YVel { get => yVel; set => yVel = value; }
    public int Rnd { get => rnd; set => rnd = value; }
    public int MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public string Type { get => type; set => type = value; }

    // Start is called before the first frame update
    void Start()
    {
        canvasObject = GameObject.Find("Canvas");
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        InvokeRepeating("SlowUpdate", 0f, 1f);
    }

    void SlowUpdate()
    {
        Rnd = Random.Range(-1, 2);
    }

    private void FixedUpdate()
    {
        oldPositionX = transform.position.x;

        if (Rnd == 1)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);
            xVelNew = -MoveSpeed;
            XVel -= resistSpeed;
            if (XVel <= xVelNew) XVel = xVelNew;
        }
        else if (Rnd != 1 && xVelNew > 0)
        {
            if (XVel < 0 && XVel < -resistSpeed) XVel += resistSpeed; //рнплнг
            if (XVel < 0 && XVel >= -resistSpeed) XVel = 0;
        }
        if (Rnd == -1)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
            //transform.rotation = Quaternion.Euler(0, -56, 0);
            xVelNew = MoveSpeed;
            XVel += resistSpeed;
            if (XVel >= xVelNew) XVel = xVelNew;
        }
        else if (Rnd != -1 && xVelNew < 0)
        {
            if (XVel > 0 && XVel > resistSpeed) XVel -= resistSpeed; //рнплнг
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

        yVelNew -= GLOBAL.GRAVITY;
        YVel -= GLOBAL.GRAVITY;
        if (cc.isGrounded)
        {
            yVelNew = 0;
            YVel = 0;
        }
        if (cc.isGrounded && Rnd == 0)
        {
            yVelNew = 1;
            YVel = GLOBAL.GRAVITY * jumpSpeed;
        }
        if (yVelNew > YVel)
        {
            YVel += GLOBAL.GRAVITY;
        }
        if (cc.transform.position.z != 0) cc.transform.position = new Vector3(cc.transform.position.x, cc.transform.position.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (lastDamaged + 0.2f > Time.time)
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(false);
            lastDamaged = 0;
        }

        if (Rnd == 0)
        {
            anim.SetInteger("Index", 0);
        }
        else if (Rnd == -1)
        {
            anim.SetInteger("Index", 2);
            if (XVel != 4)
            {
                cc.Move(new Vector3(MoveSpeed * Time.fixedDeltaTime, YVel * Time.fixedDeltaTime, 0));
            }
        } 
        else if (Rnd == 1)
        {
            anim.SetInteger("Index", 2);
            if (XVel != -4)
            {
                cc.Move(new Vector3(-MoveSpeed * Time.fixedDeltaTime, YVel * Time.fixedDeltaTime, 0));
            }
        }

        if (transform.position.x == oldPositionX)
        {
            if (anim.GetInteger("Index") == 2)
            {
                anim.SetInteger("Index", 0);
            }
        }
        else
        {
            anim.SetInteger("Index", 2);
        }

        if (Health <= 0)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OurBullet") == true)
        {
            int dirFactor = other.gameObject.GetComponentInParent<Slice>().dir;
            XVel = 2 * MoveSpeed * dirFactor;
            GetDamage(1);
            lastDamaged = Time.time;
            damagePopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "1";
            Instantiate(damagePopup, gameObject.transform.position + Vector3.up * 25f, Quaternion.Euler(0, 0, 0), canvasObject.transform);
        }
        else if (other.gameObject.CompareTag("Hit") == true)
        {
            other.gameObject.GetComponent<Hit>().Weapon.GetComponent<Weapon>().DecreaseDurability(1);
            int dirFactor = other.gameObject.GetComponent<Hit>().Weapon.GetComponent<Weapon>().dir;
            XVel = 3 * MoveSpeed * dirFactor;
            int dmg = other.gameObject.GetComponent<Hit>().Weapon.GetComponent<Item>().ItemModel.Description.damage;
            GetDamage(dmg);
            lastDamaged = Time.time;
            damagePopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = dmg.ToString();
            Instantiate(
                damagePopup, 
                gameObject.transform.position + Vector3.up * 25f,
                Quaternion.Euler(0, 0, 0), 
                canvasObject.transform
            );
        } else if (other.gameObject.CompareTag("EnemyBullet") == true)
        {
            int dirFactor = other.gameObject.GetComponent<Claws>().dir;
            XVel = 3 * MoveSpeed * dirFactor;
            GetDamage(3);
            lastDamaged = Time.time;
            damagePopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "3";
            Instantiate(
                damagePopup, 
                gameObject.transform.position + Vector3.up * 25f, 
                Quaternion.Euler(0, 0, 0), 
                canvasObject.transform
            );
        }


    }

    public void GetDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;// GetComponent<TextMesh>().text = health.ToString();
            GameManager.DecreaseCountObjectsInWorld(gameObject.name);
        }
    }
}
