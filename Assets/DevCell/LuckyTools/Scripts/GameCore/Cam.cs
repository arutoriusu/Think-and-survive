using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public bool isTwoPlayers;

    void Update()
    {
        if (player1 && player2)
        {
            if (isTwoPlayers)
                transform.position = new Vector3((player1.transform.position.x + player2.transform.position.x) / 2, 40 + (player1.transform.position.y + player2.transform.position.y) / 2, transform.position.z);
            else
                transform.position = new Vector3(player1.transform.position.x, 40 + player1.transform.position.y, transform.position.z);
        }
        if (player1 && !player2)
        {
            transform.position = new Vector3(player1.transform.position.x, 40 + player1.transform.position.y, transform.position.z);
        }
        if (!player1 && player2)
        {
            transform.position = new Vector3(player2.transform.position.x, 40 + player2.transform.position.y, transform.position.z);
        }
        /*if (!player1 && !player2)
        {
            Debug.Break();
        }*/
    }
}