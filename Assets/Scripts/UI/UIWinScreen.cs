using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIWinScreen : MonoBehaviour {
    
    [HideInInspector] public UnityEvent OnClickRestart = new UnityEvent();

    [SerializeField] Text winText;
    [SerializeField] Button restartButton;

    public void Init()
    {
        restartButton.onClick.AddListener(HandleClickRestartButton);
    }

    public void WinScreenOpen(int player)
    {
        string winString;
        if (player == FIRST_PLAYER)
            winString = FIRST_PLAYER_WIN;
        else
            winString = SECOND_PLAYER_WIN;

        winText.text = winString;
        gameObject.SetActive(true);
    }

    #region private

    const int FIRST_PLAYER = 0;
    const int SECOND_PLAYER = 1;
    private const string FIRST_PLAYER_WIN = "1 PLAYER WIN";
    private const string SECOND_PLAYER_WIN = "2 PLAYER WIN";

    void HandleClickRestartButton()
    {
        OnClickRestart.Invoke();
    }

    #endregion
}
