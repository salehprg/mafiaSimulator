
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

    public List<Building> buildings;
    public List<Person> persons;

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

    public void AddBuilding(Building building){
        buildings.Add(building);
    }
    public void RemoveBuilding(Building building){
        buildings.Remove(building);
    }

    public void AddPerson(Person person){
        persons.Add(person);
    }
    public void RemovePerson(Person person){
        persons.Remove(person);
    }

    public IEnumerable<Person> GetPerson(System.Type person){
        return persons.Where(x => x.GetType() == person || x.GetType().BaseType == person);
    }
    public IEnumerable<Building> GetBuildings(System.Type building){

        return buildings.Where(x => x.GetType() == building);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime < Time.time)
        {
            var deadPersons = persons.Where(x => x.personStatus == PersonStatus.Dead);

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
