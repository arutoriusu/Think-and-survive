using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private int hits = 0;

    public int Hits { get => hits; set => hits = value; }

    private void Start()
    {
        if (GetComponent<Prefab>().PrefabName == "BlockWeb")
        {
            transform.rotation = Quaternion.Euler(0, 90, 90);
        }
    }

    private void FixedUpdate()
    {
        if (Hits >= 10)
        {
            if (gameObject.name == "BlockGravel(Clone)")
            {
                GameManager.CreateItem(246, new Vector3(transform.position.x, transform.position.y+5, 0));
            }
            else if (gameObject.name == "BlockWeb(Clone)")
            {
                GameManager.CreateItem(248, new Vector3(transform.position.x, transform.position.y-10, 0));
                GameManager.spiderWebCoordinatesAlreadySpawn.Remove(transform.position);
            }
            else if (gameObject.name == "BlockOreIron(Clone)")
            {
                GameManager.CreateItem(97, new Vector3(transform.position.x, transform.position.y + 5, 0));
            }
            hits = 0;

            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (gameObject.name == "BlockWeb(Clone)" && 
            (other.CompareTag("Player") == true || 
            other.CompareTag("Animal") == true ||
            other.CompareTag("Enemy") == true))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (other.GetComponent<Player>().CurDir > 0)
                {
                    if (other.GetComponent<Player>().XVel > 4)
                    {
                        other.GetComponent<Player>().XVel = 4;
                    }
                } else if (other.GetComponent<Player>().CurDir < 0)
                {
                    if (other.GetComponent<Player>().XVel < -4)
                    {
                        other.GetComponent<Player>().XVel = -4;
                    }
                }
                other.GetComponent<Player>().YVel = 0;
            }
            if (other.gameObject.CompareTag("Animal") == true)
            {
                if (other.GetComponent<Animal>().Rnd < 0)
                {
                    if (other.GetComponent<Animal>().XVel > 4)
                    {
                        other.GetComponent<Animal>().XVel = 4;
                    }
                }
                else if (other.GetComponent<Animal>().Rnd > 0)
                {
                    if (other.GetComponent<Animal>().XVel < -4)
                    {
                        other.GetComponent<Animal>().XVel = -4;
                    }
                }
                other.GetComponent<Animal>().YVel = 0;
            }
            if (other.gameObject.CompareTag("Enemy") == true)
            {
                if (other.GetComponent<Zombie>().curDir > 0)
                {
                    if (other.GetComponent<Zombie>().XVel > 4)
                    {
                        other.GetComponent<Zombie>().XVel = 4;
                    }
                }
                else if (other.GetComponent<Zombie>().curDir < 0)
                {
                    if (other.GetComponent<Zombie>().XVel < -4)
                    {
                        other.GetComponent<Zombie>().XVel = -4;
                    }
                }
                other.GetComponent<Zombie>().YVel = 0;
            }
        }
    }
}
