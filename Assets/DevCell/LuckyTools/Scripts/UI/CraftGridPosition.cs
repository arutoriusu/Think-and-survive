using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftGridPosition : MonoBehaviour
{
    [SerializeField] GameObject player1BenchPanel;
    [SerializeField] GameObject player2BenchPanel;
    [SerializeField] GameObject player1FurnacePanel;
    [SerializeField] GameObject player2FurnacePanel;

    private GameObject player1;
    private GameObject player2;

    private bool panelsChanged = false;
    private float player1panelX;
    private float player2panelX;

    public GameObject Player1 { get => player1; set => player1 = value; }
    public GameObject Player2 { get => player2; set => player2 = value; }

    private void FixedUpdate()
    {
        if (!Player1 || !Player2)
        {
            return;
        }
        if (Player1.transform.position.x < Player2.transform.position.x && panelsChanged)
        {
            panelsChanged = false;
            ChangingPanelPositions(player1BenchPanel, player2BenchPanel);
            ChangingPanelPositions(player1FurnacePanel, player2FurnacePanel);
        }
        else if (Player1.transform.position.x > Player2.transform.position.x && !panelsChanged)
        {
            panelsChanged = true;
            ChangingPanelPositions(player1BenchPanel, player2BenchPanel);
            ChangingPanelPositions(player1FurnacePanel, player2FurnacePanel);
        }
    }

    private void ChangingPanelPositions(GameObject panel1, GameObject panel2)
    {
        float temp = panel1.transform.position.x;
        panel1.transform.position = new Vector3(
            panel2.transform.position.x,
            panel1.transform.position.y,
            panel1.transform.position.z);
        panel2.transform.position = new Vector3(
            temp,
            panel2.transform.position.y,
            panel2.transform.position.z);
    }
}
