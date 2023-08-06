using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building
{
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
}
