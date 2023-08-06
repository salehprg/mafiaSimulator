using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
    public override void Destroy()
    {
        Destroy(this.gameObject);
        print("House Destroyerd !");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
