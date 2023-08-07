using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
    public GameObject coinAnim;

    public override void Destroy()
    {
        Destroy(this.gameObject);
        print("House Destroyerd !");
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
