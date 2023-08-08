using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPerson
{
    List<ITargetable> GetMyTargets();
    ITargetable FindNewTarget();
    Person GetActivePerson();
    Wallet GetWallet();
    Movement GetMovement();

    void FindAllTargets();
    void SetNewPosition(Transform newTransfor);
    void SetTarget(ITargetable targetable);
    void CatchByPolice();
    float GetDeadTime();
    void FinishJob();
    void KeepInJail(float _prisonTime);

    void SetActivePerson(IPerson person);
    void OnIdle();
    void FinishJobPlayAnim(ITargetable target);
    void StatusChanged(PersonStatus pers);
    void Heal();
    void Kill();
    void GoToWaitingRoom();

    abstract void ReachTarget(ITargetable target);
    abstract void DoingJob(ITargetable target);
}
