using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPerson
{
    void Kill();
    List<ITargetable> GetMyTargets();
    void FindAllTargets();
    ITargetable FindNewTarget();
    void SetNewPosition(Transform newTransfor);
    void SetTarget(ITargetable targetable);
    void CatchByPolice();
    float GetDeadTime();
    float GetWaitingTime();
    Wallet GetWallet();

}
