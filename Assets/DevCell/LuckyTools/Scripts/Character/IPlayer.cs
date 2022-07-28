using UnityEngine;

public interface IPlayer
{
    public GameObject ObjectNearZPlus { get; set; }
    public GameObject ObjectNearZMinus { get; set; }
    public GameObject ItemInHands { get; set; }
    public GameObject BenchGrid { get; set; }

    void ChangeHand();
}