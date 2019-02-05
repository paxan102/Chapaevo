using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerControllerAI : CheckerController
{
    [SerializeField] float minForceProcent; //0-1;
    [SerializeField] float maxForceProcent;

    public void SetupEnemyCheckers(List<Checker> enemyCheckersPool)
    {
        this.enemyCheckersPool = enemyCheckersPool;
    }

    Checker GetRandomEnableChecker()
    {
        List<Checker> enableCheckers = new List<Checker>();
        foreach (var checker in allyCheckersPool)
        {
            if (checker.gameObject.activeSelf == true)
                enableCheckers.Add(checker);
        }
        return enableCheckers[Random.Range(0, enableCheckers.Count)];
    }

    public override void Enable()
    {
        base.Enable();

        rotateAngle = 45;

        selectedChecker = GetRandomEnableChecker();
        ChangeCheckerColor(selectedChecker, TypeOfMaterial.SELECTED_MATERIAL);

        currentForceProcent = Random.Range(minForceProcent, maxForceProcent);

        foreach (var enemyChecker in enemyCheckersPool)
        {
            if (enemyChecker.gameObject.activeSelf)
            {
                directionToNearestChecker = enemyChecker.transform.position - selectedChecker.transform.position;
                break;
            }
        }

        foreach (var enemyChecker in enemyCheckersPool)
        {
            if (enemyChecker.gameObject.activeSelf)
            {
                Vector3 newDirection =  enemyChecker.transform.position - selectedChecker.transform.position;

                RaycastHit hit = new RaycastHit();
                Physics.Raycast(selectedChecker.transform.position, newDirection, out hit, 100);


                Checker hitChecker = hit.collider.GetComponent<Checker>();

                if (hitChecker && hitChecker == enemyChecker)
                    if (newDirection.magnitude < directionToNearestChecker.magnitude)
                        directionToNearestChecker = newDirection;
            }
        }

        float distanceWeaking = directionToNearestChecker.magnitude / MAX_MAGNITUDE;

        if (distanceWeaking < WEAKNESS)
            distanceWeaking = WEAKNESS;

        if (distanceWeaking > 1)
            distanceWeaking = 1;

        currentForceProcent *= distanceWeaking;

        selectedChecker.CheckerSelected();

        lookRotation = Quaternion.LookRotation(-directionToNearestChecker).eulerAngles;

        Invoke("ShakeArrowRight", ANIMATION_TIME);
    }

    #region private

    const float ANIMATION_TIME = 0.02f;
    const float MAX_MAGNITUDE = 70;
    const float WEAKNESS = 0.85f;

    Vector3 directionToNearestChecker;
    Checker selectedChecker;
    float currentForceProcent;
    int rotateAngle;
    Vector3 lookRotation;
    List<Checker> enemyCheckersPool;

    void ShakeArrowRight()
    {
        rotateAngle--;
        selectedChecker.GetArrowTransform().rotation = Quaternion.Euler(lookRotation.x + 90, lookRotation.y, lookRotation.z - 90 + rotateAngle);

        if(rotateAngle < -44)
            Invoke("ShakeArrowLeft", ANIMATION_TIME);
        else
            Invoke("ShakeArrowRight", ANIMATION_TIME);
    }

    void ShakeArrowLeft()
    {
        rotateAngle++;
        selectedChecker.GetArrowTransform().rotation = Quaternion.Euler(lookRotation.x + 90, lookRotation.y, lookRotation.z - 90 + rotateAngle);

        if (rotateAngle == 0)
            Invoke("ChangeUIStrength", 0.2f);
        else
            Invoke("ShakeArrowLeft", ANIMATION_TIME);
    }
    
    void ChangeUIStrength()
    {
        uiPlayer.SetStrengthScale(currentForceProcent);
        Invoke("UnselectChecker", 0.1f);
    }

    void UnselectChecker()
    {
        selectedChecker.CheckerUnselected();
        ChangeCheckerColor(selectedChecker, TypeOfMaterial.BASE_MATERIAL);
        Invoke("Shoot", 0.1f);
    }

    void Shoot()
    {
        selectedChecker.Move(directionToNearestChecker, currentForceProcent);
        Disable();
    }

    #endregion
}
