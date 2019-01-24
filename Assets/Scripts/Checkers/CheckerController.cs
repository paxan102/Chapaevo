using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckerController : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnEndOfStep = new UnityEvent();
    [HideInInspector] public UnityEvent OnLoseRound = new UnityEvent();

    public void SetEnemyCheckersPool(List<Checker> enemyCheckersPool)
    {
        this.enemyCheckersPool = enemyCheckersPool;
        currentEnemiesCount = enemyCheckersPool.Count;
        typeOfEnemyCheckers = enemyCheckersPool[0].GetTypeOfCheckers();
    }

    public virtual void Init(List<Checker> checkersPool, UIManager UI, TypeOfPlayer typeOfPlayer)
    {
        this.UI = UI;
        alliesCheckersPool = checkersPool;
        currentAlliesCount = checkersPool.Count;
        this.typeOfPlayer = typeOfPlayer;
        foreach (var checker in checkersPool)
        {
            checker.DisableArrow();
            checker.SetMaterials(checker.GetComponent<MeshRenderer>().materials);
            ChangeCheckersColor(checker, TypeOfMaterial.BASE_MATERIAL);
            checker.gameObject.SetActive(true);
        }
    }

    public int GetAlliesCounter()
    {
        return currentAlliesCount;
    }

    public void ResetCounters()
    {
        currentAlliesCount = 8;
        currentEnemiesCount = 8;
    }

    public virtual void Enable()
    {
        if (CheckersCount(alliesCheckersPool) == 0)
        {
            OnLoseRound.Invoke();
            return;
        }

        currentAlliesCount = CheckersCount(alliesCheckersPool);
        currentEnemiesCount = CheckersCount(enemyCheckersPool);
        gameObject.SetActive(true);
    }

    public virtual void Disable()
    {        
        gameObject.SetActive(false);
        CheckEvents();
    }
    
    #region private

    protected const int BASE_MATERIAL = 0;
    protected const int SELECTED_MATERIAL = 0;
    protected const float MAX_MOUSE_DRAG = 35;
    protected const float CHECKER_HEIGHT = 0.9f;

    protected List<Checker> alliesCheckersPool;
    protected List<Checker> enemyCheckersPool;
    protected int currentEnemiesCount;
    protected int currentAlliesCount;
    protected UIManager UI;
    protected TypeOfPlayer typeOfPlayer;
    protected TypeOfCheckers typeOfEnemyCheckers;
    bool isAllCheckerStopped = false;

    void CheckEvents()
    {
        isAllCheckerStopped = true;

        foreach (var checker in alliesCheckersPool)
        {
            if (checker.gameObject.activeSelf)
                if (checker.GetVelocity().magnitude > 0)
                {
                    isAllCheckerStopped = false;
                    break;
                }
        }

        foreach (var checker in enemyCheckersPool)
        {
            if (checker.gameObject.activeSelf)
                if (checker.GetVelocity().magnitude > 0)
                {
                    isAllCheckerStopped = false;
                    break;
                }
        }

        if (!isAllCheckerStopped)
        {
            Invoke("CheckEvents", 0.1f);
            return;
        }

        UI.SetStrengthScale(typeOfPlayer, 0);

        int countOfAllies = CheckersCount(alliesCheckersPool);
        int countOfEnemies = CheckersCount(enemyCheckersPool);

        UI.SetAliveCheckers(typeOfPlayer, countOfAllies, countOfEnemies);

        if (CheckersCount(alliesCheckersPool) > 0)
        {
            if (countOfAllies < currentAlliesCount)
            {
                currentAlliesCount = countOfAllies;
                OnEndOfStep.Invoke();
                return;
            }

            if (countOfEnemies == 0)
            {
                OnEndOfStep.Invoke();
                return;
            }

            if (countOfEnemies < currentEnemiesCount)
            {
                currentEnemiesCount = countOfEnemies;
                Enable();
                return;
            }

            OnEndOfStep.Invoke();
        }   
        else
            OnLoseRound.Invoke();
    }

    int CheckersCount(List<Checker> checkersPool)
    {
        int result = 0;

        foreach (var checker in checkersPool)
            if (checker.gameObject.activeSelf)
                result++;

        return result;
    }

    protected virtual void HandleSelectChecker(Checker checker)
    {
        ChangeCheckersColor(checker, TypeOfMaterial.SELECTED_MATERIAL);
    }
     
    protected void ChangeCheckersColor(Checker checker, TypeOfMaterial typeOfMaterial)
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
