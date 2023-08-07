using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Wallet))]
public abstract class Person : MonoBehaviour, IPerson, ITargetable
{
    public Sprite icon;

    [SerializeField]
    protected float idleTime = 1;
    protected float prisonTime = 0;
    [SerializeField]
    protected float health = 100;
    protected float Health
    {
        set
        {
            health = value;
            if (health <= 0)
            {
                deadTime = Time.time;
                health = 0;
                personStatus = PersonStatus.Dead;
                target?.SetActivePerson(null);
                movement?.SetGoal(null, 0);
            }
            else
            {
                if (personStatus == PersonStatus.Dead)
                {
                    personStatus = PersonStatus.Idle;
                }
            }
        }
        get { return health; }
    }
    public PersonStatus personStatus;

    [SerializeField]
    protected Transform center;
    [SerializeField]
    protected List<BuildingTypes> buildingTypes;
    [SerializeField]
    protected List<PersonTypes> personTypes;
    [SerializeField]
    protected Person activePerson;
    private ITargetable target;

    protected float waitTime;
    protected float deadTime;

    protected GameObject waitingroom;
    protected List<ITargetable> targets;

    public Movement movement;
    protected Wallet wallet;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        wallet = GetComponent<Wallet>();

        OnAwake();
    }
    private void Start()
    {
        prisonTime = 0;

        targets = new List<ITargetable>();
        personStatus = PersonStatus.Idle;

        var rooms = GameObject.FindGameObjectsWithTag("room");
        int indx = Random.Range(0, rooms.Length);

        waitingroom = rooms[indx];

        FindAllTargets();

        OnStart();
    }

    public virtual void OnAwake() { }
    public virtual void OnStart() { }

    void UpdateStatus()
    {
        if (health > 0 && target == null)
        {
            personStatus = PersonStatus.Idle;
        }

        if (activePerson?.health <= 0)
        {
            SetActivePerson(null);
        }

        if (health > 0)
        {
            if (personStatus == PersonStatus.Dead)
            {
                personStatus = PersonStatus.Idle;
            }
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
                    OnIdle();

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
                }
                else
                {
                    GoToWaitingRoom();
                }
                break;

            case PersonStatus.Working:
                DoingJob(target);
                break;

            case PersonStatus.GoingToWork:
                if (movement.IsReached())
                {
                    personStatus = PersonStatus.Working;
                    ReachTarget(target);
                }
                else
                {
                    if (target != null)
                        SetNewPosition(((MonoBehaviour)target).transform);
                    else
                    {
                        print("Delte");
                    }
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
        this.Health = 0;
    }

    public void Heal()
    {
        this.Health = 100;
        personStatus = PersonStatus.Idle;
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
    public virtual void OnIdle() { }

    public virtual ITargetable FindNewTarget()
    {
        FindAllTargets();
        var _targets = GetMyTargets();

        if (_targets == null)
            return null;

        int indx = Random.Range(0, _targets.Count);
        if (_targets.Count == 0)
            return null;

        var temp = _targets[indx];

        if (temp.GetActivePerson() != null && temp.GetActivePerson() != this)
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

        FinishJobPlayAnim(target);
    }

    public void GoToWaitingRoom()
    {
        personStatus = PersonStatus.Idle;
        if (waitingroom != null)
            movement.SetGoal(waitingroom.transform, 1);
    }

    public Wallet GetWallet()
    {
        return wallet;
    }

    public float GetDeadTime()
    {
        return deadTime;
    }

    public float GetWaitingTime()
    {
        return waitTime;
    }

    private void OnDestroy()
    {
        GetActivePerson()?.SetActivePerson(null);
        GetActivePerson()?.GoToWaitingRoom();
        SetActivePerson(null);
    }

    public abstract void ReachTarget(ITargetable target);
    public abstract void DoingJob(ITargetable target);
    public virtual void FinishJobPlayAnim(ITargetable target) { }

    public void SetTarget(ITargetable targetable)
    {
        target = targetable;
    }
}


