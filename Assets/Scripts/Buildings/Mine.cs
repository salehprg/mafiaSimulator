using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building
{
    public GameObject coinAnim;
    public override void Destroy()
    {
        print("Mine destroyed !");
    }

    public override float GetMoney()
    {
        float amount = payment;

        if (store_money - amount >= 0)
        {
            store_money -= amount;

            PlayAnimation();
            CheckForDestroy();
            return amount;
        }

        return 0;
    }

    public override void PlayAnimation()
    {
        if (activePerson != null)
        {
            var tmp = Instantiate(coinAnim, activePerson.transform.position, new Quaternion());
            tmp.transform.SetParent(activePerson.transform);
            Destroy(tmp, 1.8f);
        }
    }
}
