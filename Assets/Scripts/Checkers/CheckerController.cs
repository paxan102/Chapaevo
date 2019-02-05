using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckerController : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnEndOfStep = new UnityEvent();

    public virtual void SetupPlayer(List<Checker> checkersPool, UIPlayer uiPlayer)
    {
        this.uiPlayer = uiPlayer;
        allyCheckersPool = checkersPool;

        foreach (var checker in checkersPool)
        {
            checker.DisableArrow();
            checker.SetMaterials(checker.GetComponent<MeshRenderer>().materials);
            ChangeCheckerColor(checker, TypeOfMaterial.BASE_MATERIAL);
        }
    }

    public virtual void Enable()
    {
        gameObject.SetActive(true);
    }

    public virtual void Disable()
    {        
        gameObject.SetActive(false);
        OnEndOfStep.Invoke();
    }
    
    #region private

    protected const int BASE_MATERIAL = 0;
    protected const int SELECTED_MATERIAL = 0;
    protected const float MAX_MOUSE_DRAG = 35;
    protected const float CHECKER_HEIGHT = 0.9f;

    protected List<Checker> allyCheckersPool;
    protected UIPlayer uiPlayer;

    protected virtual void HandleSelectChecker(Checker checker)
    {
        ChangeCheckerColor(checker, TypeOfMaterial.SELECTED_MATERIAL);
    }
     
    protected void ChangeCheckerColor(Checker checker, TypeOfMaterial typeOfMaterial)
    {
        MeshRenderer meshRenderer = checker.GetComponent<MeshRenderer>();
        Material[] materials = checker.GetMaterials();

        if (typeOfMaterial == TypeOfMaterial.BASE_MATERIAL)
        {
            meshRenderer.materials = materials;
        }
        else
        {
            meshRenderer.material = materials[(int)typeOfMaterial];
        }
    }

    #endregion
    
    public enum TypeOfMaterial
    {
        BASE_MATERIAL,
        SELECTED_MATERIAL
    } 
}
