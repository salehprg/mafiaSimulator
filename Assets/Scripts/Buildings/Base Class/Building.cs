using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, IBuilding, ITargetable
{
    public float health;
    public float store_money;
    public float payment;
    public float waitTime;
    public Person activePerson;


    public virtual float GetMoney()
    {
        CheckForDestroy();
        return payment;
    }

    public void SetActivePerson(IPerson person)
    {
        activePerson = (Person)person;
    }

    public virtual void CheckForDestroy(){
        if(store_money <= 0){
            Destroy();
        }
    }
    public abstract void Destroy();

    public Person GetActivePerson()
    {
        return activePerson;
    }
}
