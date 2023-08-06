using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public List<GameObject> peopleList;
    public Vector3 scale;
    public float interval;

    float lastTime;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (lastTime + interval < Time.time)
        {
            var deadPersons = FindObjectsOfType<Person>().ToList();

            deadPersons = deadPersons.Where(x => x.personStatus == PersonStatus.Dead).ToList();

            lastTime = Time.time;

            if (deadPersons.Count > 0)
            {
                if (peopleList.Count > 0)
                {
                    int newrnd = Random.Range(0, peopleList.Count);
                    var newObj = Instantiate(peopleList[newrnd], transform.position , new Quaternion());

                    newObj.transform.localScale = scale;
                }

                int rnd = Random.Range(0, deadPersons.Count);
                deadPersons[rnd].gameObject.SetActive(false);
            }
        }
    }
}
