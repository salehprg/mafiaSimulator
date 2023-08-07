using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManagerr : MonoBehaviour
{

    public static GameManagerr instance;

    public Dictionary<PersonStatus, GameObject> status;
    public List<GameObject> peopleList;
    public Vector3 scale;
    public float delay;
    public float interval;

    float waitTime;

    [SerializeField]
    protected List<StatusSpriteKeys> StatusSpriteList = new List<StatusSpriteKeys>();

    public Dictionary<PersonStatus, GameObject> statusSprites = new Dictionary<PersonStatus, GameObject>();

    public GameObject[] prisons;
    public GameObject[] policeStations;

    public GameManagerr()
    {
        instance = this;
    }

    void Awake()
    {
        foreach (var kvp in StatusSpriteList)
        {
            statusSprites[kvp.status] = kvp.sprite;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (waitTime < Time.time)
        {
            var deadPersons = FindObjectsOfType<Person>().ToList();

            deadPersons = deadPersons.Where(x => x.personStatus == PersonStatus.Dead).ToList();

            foreach (var deadPrsn in deadPersons)
            {
                if (Time.time - deadPrsn.GetDeadTime() > delay)
                {
                    if (peopleList.Count > 0)
                    {
                        int newrnd = Random.Range(0, peopleList.Count);
                        var newObj = Instantiate(peopleList[newrnd], transform.position, new Quaternion());

                        newObj.transform.localScale = scale;
                    }
                    
                    Destroy(deadPrsn.gameObject);
                    waitTime = Time.time + interval;

                    return;
                }
            }
        }
    }
}
