using UnityEngine;

public class Claws : MonoBehaviour
{
    public int dir = -1;

    void Start()
    {
        Destroy(gameObject, GLOBAL.CLAWS_DURATION);
    }
}
