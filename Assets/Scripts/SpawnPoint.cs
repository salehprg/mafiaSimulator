using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public List<GameObject> peopleList;

    public float delay;
    float waitTime;


    Vector3 scale;


    void Awake()
    {

    }

    private void Start()
    {
        scale = GameManagerr.instance.scale;
        waitTime = Time.time + delay;
    }


    // Update is called once per frame
    void Update()
    {
        if (waitTime < Time.time)
        {
            int rnd = Random.Range(0, peopleList.Count);

            if (peopleList.Count > 0)
            {
                var newObj = Instantiate(peopleList[rnd], transform.position, new Quaternion());

                newObj.transform.localScale = scale;

                waitTime = Time.time + delay;
            }
        }
    }
}
