using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BriberThief : Thief
{
    public float coolDownTime = 0;
    public float briberAmount = 0;

    float briberTime = 0;
    bool setCoolDown = false;

    public override void OnUpdate()
    {
        if (personStatus == PersonStatus.Catched)
        {
            if (!setCoolDown)
            {
                setCoolDown = true;
                briberTime = Time.time + coolDownTime;
            }

            if (briberTime < Time.time)
            {
                var police = (Police)GetActivePerson();
                float amount = wallet.WithdrawBalance(briberAmount);

                if(police == null){
                    setCoolDown = false;
                    FinishJob();
                }
                
                if (police.Bribe(this , amount))
                {
                    setCoolDown = false;
                    FinishJob();
                }
                else
                {
                    wallet.DepositBalance(amount);
                }
            }
        }
    }
}
