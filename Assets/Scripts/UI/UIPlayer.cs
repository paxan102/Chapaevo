using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIPlayer : MonoBehaviour {

    [SerializeField] Image playerStrength;
    [SerializeField] Text playerYourMove;
    [SerializeField] Text playerCheckersAlive;
    [SerializeField] Text playerWins;

    public void Init()
    {
        ResetPlayer();
    }

    public void SetPlayerWins(int count)
    {
        playerWins.text = count.ToString();
    }

    public void SetStrengthScale(float percent)
    {
        playerStrength.transform.localScale = new Vector3(percent, 1, 1);
    }

    public void SetCountOfAliveCheckers(int count)
    {
        playerCheckersAlive.text = count.ToString();
    }

    public void ResetPlayer()
    {
        playerYourMove.gameObject.SetActive(false);
        playerCheckersAlive.text = "8";
    }

    public void SetYourMove(bool set)
    {
        playerYourMove.gameObject.SetActive(set);
    }
}
