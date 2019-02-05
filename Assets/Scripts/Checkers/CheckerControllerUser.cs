using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerControllerUser : CheckerController
{
    public override void SetupPlayer(List<Checker> checkersPool, UIPlayer uiPlayer)
    {
        if (allyCheckersPool != null)
        {
            foreach (var checker in allyCheckersPool)
            {
                checker.OnSelected.RemoveListener(HandleSelectChecker);
            }
        }

        base.SetupPlayer(checkersPool, uiPlayer);

        foreach (var checker in checkersPool)
        {
            checker.OnSelected.AddListener(HandleSelectChecker);
        }
    }

    public override void Enable()
    {
        base.Enable();

        foreach (var checker in allyCheckersPool)
            checker.SetIsPlayerCanSelect(true);
    }

    public override void Disable()
    {
        DisableSelect();

        foreach (var checker in allyCheckersPool)
            checker.SetIsPlayerCanSelect(false);

        base.Disable();
    }

    #region private

    const int LEFT_MOUSE = 0;    

    bool isCheckerSelected = false;
    bool isInUnselectedRadius = false;
    Checker selectedChecker;

    protected override void HandleSelectChecker(Checker checker)
    {
        if (isCheckerSelected)
        {
            foreach (var ally in allyCheckersPool)
            {
                if (ally != checker)
                {
                    ally.SetIsPlayerCanSelect(true);
                }
            }

            isCheckerSelected = false;
            checker.UnfreezeRotation();
            DisableSelect();
            return;
        }

        base.HandleSelectChecker(checker);

        foreach(var ally in allyCheckersPool)
        {
            if(ally != checker)
            {
                ally.SetIsPlayerCanSelect(false);
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
            ChangeCheckerColor(selectedChecker, TypeOfMaterial.BASE_MATERIAL);
            selectedChecker.OnMouseEnterInChecker.RemoveAllListeners();
            selectedChecker.OnMouseExitFromChecker.RemoveAllListeners();
            selectedChecker = null;
        }
        
        isInUnselectedRadius = false;
        isCheckerSelected = false;
    }

    void Update()
    {
        if (!isCheckerSelected)
            return;

        Vector3 currentMousePointInWorld = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
        currentMousePointInWorld.y = CHECKER_HEIGHT;

        Vector3 direction = selectedChecker.transform.position - currentMousePointInWorld;
        Vector3 rotation = Quaternion.LookRotation(direction).eulerAngles;
        
        selectedChecker.GetArrowTransform().rotation = Quaternion.Euler(rotation.x + 90, rotation.y, rotation.z - 90);

        float percent;
        if (direction.magnitude >= MAX_MOUSE_DRAG)
            percent = 1;
        else
            percent = direction.magnitude / MAX_MOUSE_DRAG;

        uiPlayer.SetStrengthScale(percent);

        if (isInUnselectedRadius)
        {
            if (Input.GetMouseButtonUp(LEFT_MOUSE))
            {
                selectedChecker.CheckerUnselected();
                ChangeCheckerColor(selectedChecker, TypeOfMaterial.BASE_MATERIAL);
                selectedChecker.Move(-direction, percent);
                Disable();
            }
        }
    }

    #endregion
}
