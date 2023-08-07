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
        float amount = base.GetMoney();

        if (store_money - amount >= 0)
        {
            store_money -= amount;
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
