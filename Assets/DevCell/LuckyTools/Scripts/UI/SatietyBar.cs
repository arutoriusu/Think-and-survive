using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SatietyBar : MonoBehaviour
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
            int.TryParse(creator.GetComponent<TextMesh>().text.Split(' ')[1], out textVal);
            barMain.GetComponent<Slider>().value = textVal;
            barBack.GetComponent<Slider>().value = textVal;
            barText.GetComponent<TextMeshProUGUI>().text = "";
            transform.position = creator.transform.position + new Vector3(0, 20, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
