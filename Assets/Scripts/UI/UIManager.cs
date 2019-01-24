using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnClickRestart = new UnityEvent();
    [HideInInspector] public UnityEvent OnClickFirstButton = new UnityEvent();
    [HideInInspector] public UnityEvent OnClickSecondButton = new UnityEvent();
    [HideInInspector] public UnityEvent OnClickFirstPlayer = new UnityEvent();
    [HideInInspector] public UnityEvent OnClickSecondPlayer = new UnityEvent();

    [SerializeField] MonoBehaviour firstPlayerStrength;
    [SerializeField] MonoBehaviour secondPlayerStrength;

    [SerializeField] public Text firstPlayerYourMove;
    [SerializeField] public Text secondPlayerYourMove;

    [SerializeField] public Text firstPlayerWins;
    [SerializeField] public Text secondPlayerWins;

    [SerializeField] public Text firstPlayerCheckersAlive;
    [SerializeField] public Text secondPlayerCheckersAlive;

    [SerializeField] public MonoBehaviour winScreen;
    [SerializeField] public Text winText;
    [SerializeField] public Button winButton;

    [SerializeField] public MonoBehaviour firstScreen;
    [SerializeField] public Button firstButton;

    [SerializeField] public MonoBehaviour secondScreen;
    [SerializeField] public Button secondButton;

    [SerializeField] public MonoBehaviour chooseScreen;
    [SerializeField] public Button firstPlayerButton;
    [SerializeField] public Button secondPlayerButton;


    public void Init()
    {
        firstPlayerYourMove.gameObject.SetActive(false);
        secondPlayerYourMove.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(false);
        secondScreen.gameObject.SetActive(false);
        chooseScreen.gameObject.SetActive(false);

        firstButton.onClick.AddListener(HandleFirstButtonClick);
        secondButton.onClick.AddListener(HandleSecondButtonClick);

        firstPlayerButton.onClick.AddListener(HandleFirstPlayerButtonClick);
        secondPlayerButton.onClick.AddListener(HandleSecondPlayerButtonClick);
    }
    
    public void WinScreenOpen(TypeOfPlayer typeOfPlayer)
    {
        string winString;
        if (typeOfPlayer == TypeOfPlayer.FIRST)
            winString = FIRST_PLAYER_WIN;
        else
            winString = SECOND_PLAYER_WIN;

        winText.text = winString;
        winScreen.gameObject.SetActive(true);

        winButton.onClick.AddListener(HandleWinButtonClick);
    }

    public void HandleWinButtonClick()
    {
        winButton.onClick.RemoveListener(HandleWinButtonClick);
        winScreen.gameObject.SetActive(false);
        OnClickRestart.Invoke();
    }

    public void HandleFirstButtonClick()
    {
        firstButton.onClick.RemoveListener(HandleFirstButtonClick);
        firstScreen.gameObject.SetActive(false);
        secondScreen.gameObject.SetActive(true);
        OnClickFirstButton.Invoke();
    }

    public void HandleSecondButtonClick()
    {
        secondButton.onClick.RemoveListener(HandleSecondButtonClick);
        secondScreen.gameObject.SetActive(false);
        chooseScreen.gameObject.SetActive(true);
        OnClickSecondButton.Invoke();
    }

    public void HandleFirstPlayerButtonClick()
    {
        chooseScreen.gameObject.SetActive(false);
        OnClickFirstPlayer.Invoke();
    }

    public void HandleSecondPlayerButtonClick()
    {
        chooseScreen.gameObject.SetActive(false);
        OnClickSecondPlayer.Invoke();
    }

    public void SetAliveCheckers(TypeOfPlayer typeOfPlayer, int first, int second)
    {
        if (typeOfPlayer == TypeOfPlayer.FIRST)
        {
            firstPlayerCheckersAlive.text = first.ToString();
            secondPlayerCheckersAlive.text = second.ToString();
        }
        else
        {
            firstPlayerCheckersAlive.text = second.ToString();
            secondPlayerCheckersAlive.text = first.ToString();
        }
    }

    public void ResetUI()
    {
        firstPlayerYourMove.gameObject.SetActive(false);
        secondPlayerYourMove.gameObject.SetActive(false);
        firstPlayerCheckersAlive.text = "8";
        secondPlayerCheckersAlive.text = "8";
    }
    
    public void SetStrengthScale(TypeOfPlayer typeOfPlayer, float percent)
    {
        if (typeOfPlayer == TypeOfPlayer.FIRST)
            firstPlayerStrength.transform.localScale = new Vector3(percent, 1, 1);
        else
            secondPlayerStrength.transform.localScale = new Vector3(percent, 1, 1);
    }

    private const string FIRST_PLAYER_WIN = "1 PLAYER WIN";
    private const string SECOND_PLAYER_WIN = "2 PLAYER WIN";
}
