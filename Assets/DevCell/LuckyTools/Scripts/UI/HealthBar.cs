using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public GameObject creator;
    public GameObject barMain;
    public GameObject barBack;
    public GameObject barText;
    public Camera cam;

    void Start()
    {
        cam = GameObject.Find("CameraMain").GetComponent<Camera>();
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    void Update()
    {
        if (creator)
        {
            int textVal;
            int.TryParse(creator.GetComponent<TextMesh>().text.Split(' ')[0], out textVal);
            barMain.GetComponent<Slider>().value = textVal;
            barBack.GetComponent<Slider>().value = textVal;
            barText.GetComponent<TextMeshProUGUI>().text = textVal.ToString();

            if (creator.CompareTag("Player") == true)
            {
                transform.position = creator.transform.position + new Vector3(0, -21, 0);
            } else if (creator.CompareTag("Enemy") == true)
            {
                transform.position = creator.transform.position + new Vector3(0, 20, 0);
            }


        }
        else
        {
            Destroy(gameObject);
        }
    }
}
