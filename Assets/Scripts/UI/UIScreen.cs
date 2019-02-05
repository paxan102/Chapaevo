using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIScreen : MonoBehaviour {

    [HideInInspector] public UnityEvent OnClickButton = new UnityEvent();

    [SerializeField] Button button;

    public void Init()
    {
        button.onClick.AddListener(HandleOnClickButton);
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

    void HandleOnClickButton()
    {
        OnClickButton.Invoke();
    }

    #endregion
}
