using UnityEngine;

public class Slice : MonoBehaviour
{
    public int dir = -1;

    void Start()
    {
        Destroy(gameObject, GLOBAL.SLICE_DURATION);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tree") == true)
        {
            collision.gameObject.GetComponent<RegularTree>().PunchesForStickSpawn += 1;
        }
        if (collision.gameObject.name == "BlockGravel(Clone)" ||
            collision.gameObject.name == "BlockWeb(Clone)")
        {
            collision.gameObject.GetComponent<Ground>().Hits += 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tree") == true)
        {
            other.gameObject.GetComponent<RegularTree>().PunchesForStickSpawn += 1;
        }
        if (other.gameObject.name == "BlockGravel(Clone)" ||
            other.gameObject.name == "BlockWeb(Clone)")
        {
            other.gameObject.GetComponent<Ground>().Hits += 1;
        }
    }
}
