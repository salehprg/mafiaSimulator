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

    private void Start() {
        GameManagerr.instance.AddBuilding(this);
    }

    public virtual float GetMoney()
    {
        PlayAnimation();
        CheckForDestroy();
        return payment;
    }

    public void SetActivePerson(IPerson person)
    {
        activePerson = (Person)person;
    }

    public virtual void CheckForDestroy(){
        if(store_money <= 0){
            Destroy(this.gameObject);
        }
    }

    public Person GetActivePerson()
    {
        return activePerson;
    }

    private void OnDestroy() {
        GameManagerr.instance.RemoveBuilding(this);
    }

    public abstract void Destroy();
    public abstract void PlayAnimation();

}
