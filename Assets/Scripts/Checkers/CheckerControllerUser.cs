using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerControllerUser : CheckerController
{
    public override void Init(List<Checker> checkersPool, UIManager UI, TypeOfPlayer typeOfPlayer)
    {
        base.Init(checkersPool, UI, typeOfPlayer);

        foreach (var checker in checkersPool)
        {
            checker.OnSelected.AddListener(HandleSelectChecker);
        }
    }

    public override void Enable()
    {
        base.Enable();

        foreach (var checker in alliesCheckersPool)
            checker.SetIsCanSelect(true);
    }

    public override void Disable()
    {
        DisableSelect();

        foreach (var checker in alliesCheckersPool)
            checker.SetIsCanSelect(false);

        base.Disable();
    }

    #region private

    const int LEFT_MOUSE = 0;    

    bool isCheckerSelected = false;
    bool isInUnselectedRadius = false;
    bool isShooting = false;
    Checker selectedChecker;

    protected override void HandleSelectChecker(Checker checker)
    {
        if (isCheckerSelected)
        {
            foreach (var ally in alliesCheckersPool)
            {
                if (ally != checker)
                {
                    ally.SetIsCanSelect(true);
                }
            }

            isCheckerSelected = false;
            checker.UnfreezeRotation();
            DisableSelect();
            return;
        }

        base.HandleSelectChecker(checker);

        foreach(var ally in alliesCheckersPool)
        {
            if(ally != checker)
            {
                ally.SetIsCanSelect(false);
            }
        }

        isCheckerSelected = true;
        selectedChecker = checker;
        checker.FreezeRotation();
        checker.EnableArrow();
        checker.OnMouseExitFromChecker.AddListener(HandleExitFromChecker);
    }

    void HandleExitFromChecker()
    {
        isInUnselectedRadius = true;
        selectedChecker.OnMouseEnterInChecker.AddListener(HandleEnterInChecker);
    }

    void HandleEnterInChecker()
    {
        isInUnselectedRadius = false;
    }

    void DisableSelect()
    {
        if (selectedChecker)
        {
            selectedChecker.DisableArrow();
            ChangeCheckersColor(selectedChecker, TypeOfMaterial.BASE_MATERIAL);
            selectedChecker.OnMouseEnterInChecker.RemoveAllListeners();
            selectedChecker.OnMouseExitFromChecker.RemoveAllListeners();
            selectedChecker = null;
        }

        isShooting = false;
        isInUnselectedRadius = false;
        isCheckerSelected = false;
    }

    void Update()
    {
        if (!isCheckerSelected)
            return;

        if (isShooting)
            return;

        Vector3 currentMousePointInWorld = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
        currentMousePointInWorld.y = CHECKER_HEIGHT;

        Vector3 direction = selectedChecker.transform.position - currentMousePointInWorld;
        Vector3 rotation = Quaternion.LookRotation(direction).eulerAngles;
        
        selectedChecker.GetArrowTransform().rotation = Quaternion.Euler(rotation.x + 90, rotation.y, rotation.z - 90);

        float procent;
        if (direction.magnitude >= MAX_MOUSE_DRAG)
            procent = 1;
        else
            procent = direction.magnitude / MAX_MOUSE_DRAG;

        UI.SetStrengthScale(typeOfPlayer, procent);

        if (isInUnselectedRadius)
        {
            if (Input.GetMouseButtonUp(LEFT_MOUSE))
            {
                isShooting = true;
                selectedChecker.CheckerUnselected();
                selectedChecker.Move(-direction, procent);
                Invoke("Disable", 2f);
            }
        }
    }

    #endregion
}
