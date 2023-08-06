using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Wallet))]
public abstract class Person : MonoBehaviour, IPerson, ITargetable
{
    public float idleTime = 1;
    public float prisonTime = 0;
    public float health = 100;
    public PersonStatus personStatus;

    public Transform center;
    public List<BuildingTypes> buildingTypes;
    public List<PersonTypes> personTypes;
    public ITargetable target;
    public Person activePerson;

    protected float waitTime;

    protected GameObject waitingroom;
    protected List<ITargetable> targets;

    public Movement movement;
    public Wallet wallet;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        wallet = GetComponent<Wallet>();
    }
    public virtual void Start()
    {
        prisonTime = 0;

        targets = new List<ITargetable>();
        personStatus = PersonStatus.Idle;

        var rooms = GameObject.FindGameObjectsWithTag("room");
        int indx = Random.Range(0, rooms.Length);

        waitingroom = rooms[indx];

        FindAllTargets();
    }

    void UpdateStatus()
    {
        if (activePerson?.health <= 0)
        {
            SetActivePerson(null);
        }

        if (health <= 0)
        {
            personStatus = PersonStatus.Dead;
        }
        else
        {
            if (personStatus == PersonStatus.Dead)
                personStatus = PersonStatus.Idle;
        }
    }

    public virtual void Update()
    {
        UpdateStatus();

        switch (personStatus)
        {
            case PersonStatus.Dead:
                SetActivePerson(null);
                return;

            case PersonStatus.Idle:
                if (waitTime < Time.time)
                {
                    var _target = FindNewTarget();
                    if (_target != null)
                    {
                        target = _target;
                        target.SetActivePerson(this);
                        movement.SetGoal(((MonoBehaviour)target).transform);
                        personStatus = PersonStatus.GoingToWork;
                    }
                    else
                    {
                        GoToWaitingRoom();
                    }
                }else{
                    GoToWaitingRoom();
                }
                break;

            case PersonStatus.Working:
                DoingJob();
                break;

            case PersonStatus.GoingToWork:
                if (movement.reachGoal)
                {
                    personStatus = PersonStatus.Working;
                    ReachTarget(target);
                }
                break;

            case PersonStatus.Catched:
                break;

            case PersonStatus.Prison:
                if (prisonTime < Time.time)
                {
                    personStatus = PersonStatus.Idle;
                }
                return;
        }

    }

    public void Kill()
    {
        this.health = 0;
    }

    public virtual List<ITargetable> GetMyTargets()
    {
        try
        {
            var _targets = targets;
            if (target != null && _targets != null)
                _targets = _targets.Where(x => x != target).ToList();

            return _targets;
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
            throw;
        }

    }

    public void FindAllTargets()
    {
        targets = new List<ITargetable>();

        foreach (var type in buildingTypes)
        {
            switch (type)
            {
                case BuildingTypes.House:
                    targets.AddRange(FindObjectsOfType<House>().ToList());
                    break;

                case BuildingTypes.Mine:
                    targets.AddRange(FindObjectsOfType<Mine>().ToList());
                    break;
            }
        }

        foreach (var type in personTypes)
        {
            switch (type)
            {
                case PersonTypes.All:
                    targets.AddRange(FindObjectsOfType<Person>().ToList());
                    break;
                case PersonTypes.Worker:
                    targets.AddRange(FindObjectsOfType<Worker>().ToList());
                    break;
                case PersonTypes.Investor:
                    targets.AddRange(FindObjectsOfType<Investor>().ToList());
                    break;
                case PersonTypes.Thief:
                    targets.AddRange(FindObjectsOfType<Thief>().ToList());
                    break;
                case PersonTypes.BriberThief:
                    targets.AddRange(FindObjectsOfType<BriberThief>().ToList());
                    break;
                case PersonTypes.Miner:
                    targets.AddRange(FindObjectsOfType<Miner>().ToList());
                    break;
                case PersonTypes.Assassin:
                    targets.AddRange(FindObjectsOfType<Assassin>().ToList());
                    break;
            }
        }
    }

    public virtual ITargetable FindNewTarget()
    {
        var _targets = GetMyTargets();

        int indx = Random.Range(0, _targets.Count);
        if (_targets.Count == 0)
            return null;

        var temp = _targets[indx];

        if (temp.GetActivePerson() != null)
            return null;

        return temp;
    }

    public Person GetActivePerson()
    {
        return activePerson;
    }

    public void SetActivePerson(IPerson person)
    {
        activePerson = (Person)person;
    }
    public void SetNewPosition(Transform newTransfor)
    {
        movement.SetGoal(newTransfor);
    }
    public void CatchByPolice()
    {
        personStatus = PersonStatus.Catched;
    }
    public void KeepInJail(float _prisonTime)
    {
        prisonTime = Time.time + _prisonTime;
        personStatus = PersonStatus.Prison;
    }
    public void FinishJob()
    {
        target?.SetActivePerson(null);
        waitTime = Time.time + idleTime;

        personStatus = PersonStatus.Idle;
    }

    public void GoToWaitingRoom()
    {
        personStatus = PersonStatus.Idle;
        if (waitingroom != null)
            movement.SetGoal(waitingroom.transform, 1);
    }
    public abstract void ReachTarget(ITargetable target);

    public abstract void DoingJob();
}
