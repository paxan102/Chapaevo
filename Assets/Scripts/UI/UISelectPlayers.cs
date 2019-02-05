using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UISelectPlayers : MonoBehaviour {

    [HideInInspector] public UnityEvent OnClickFirstPlayerButton = new UnityEvent();
    [HideInInspector] public UnityEvent OnClickSecondPlayerButton = new UnityEvent();

    [SerializeField] Button firstPlayerButton;
    [SerializeField] Button secondPlayerButton;

    public void Init()
    {
        firstPlayerButton.onClick.AddListener(HandleClickFirstPlayerButton);
        secondPlayerButton.onClick.AddListener(HandleClickSecondPlayerButton);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    #region private

    void HandleClickFirstPlayerButton()
    {
        OnClickFirstPlayerButton.Invoke();
    }

    void HandleClickSecondPlayerButton()
    {
        OnClickSecondPlayerButton.Invoke();
    }

    #endregion
}
