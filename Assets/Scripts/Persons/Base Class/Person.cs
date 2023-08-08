using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEngine.Events;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Wallet))]
public abstract class Person : MonoBehaviour, IPerson, ITargetable
{

    public Sprite icon;

    [SerializeField]
    protected float idleTime = 1;

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

    PersonStatus Person_status;
    public PersonStatus personStatus
    {
        get { return Person_status; }
        set
        {
            if (Person_status != value)
            {
                StatusChanged(value);
                showPersonInfo.ShowStatus(value);
            }
            Person_status = value;
        }
    }

    [SerializeField]
    protected List<BuildingType> buildingTypes;
    [SerializeField]
    protected List<PersonType> personTypes;
    [SerializeField]
    protected Person activePerson;

    protected float waitTime;
    protected float deadTime;
    protected GameObject waitingroom;
    protected List<ITargetable> targets = new List<ITargetable>();
    protected Movement movement;
    protected Wallet wallet;

    ShowPersonInfo showPersonInfo;
    float prisonTime = 0;
    ITargetable target;

    int frameSkip = 0;
    int iter = 0;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        wallet = GetComponent<Wallet>();
        showPersonInfo = GetComponentInChildren<ShowPersonInfo>();

        GameManagerr.instance.AddPerson(this);

        OnAwake();
    }
    private void Start()
    {
        prisonTime = 0;

        personStatus = PersonStatus.Idle;

        var rooms = GameObject.FindGameObjectsWithTag("room");
        int indx = Random.Range(0, rooms.Length);

        waitingroom = rooms[indx];

        showPersonInfo.ShowStatus(Person_status);

        frameSkip = Random.Range(1, 10);
        FindAllTargets();
        OnStart();
    }


    void UpdateStatus()
    {
        if (health > 0 && target == null && personStatus != PersonStatus.Idle && personStatus != PersonStatus.Catched)
        {
            personStatus = PersonStatus.Idle;
        }

        if (personStatus == PersonStatus.Catched && activePerson == null)
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

    public void Update()
    {
        try{
        iter++;

        if (iter % frameSkip == 0)
            return;
            
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

                    waitTime = Time.time + idleTime;
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
                    try
                    {
                        if (target != null)
                            SetNewPosition(((MonoBehaviour)target).transform);
                    }
                    catch (System.Exception)
                    {
                        personStatus = PersonStatus.Idle;
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

        OnUpdate();
        }
        catch(System.Exception) {}
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
                case BuildingType.House:
                    targets.AddRange(GameManagerr.instance.GetBuildings(typeof(House)));
                    break;

                case BuildingType.Mine:
                    var result = GameManagerr.instance.GetBuildings(typeof(Mine));
                    targets.AddRange(result);
                    break;
            }
        }

        foreach (var type in personTypes)
        {
            switch (type)
            {
                case PersonType.All:
                    targets.AddRange(GameManagerr.instance.GetPerson(typeof(Person)));
                    break;
                case PersonType.Worker:
                    targets.AddRange(GameManagerr.instance.GetPerson(typeof(Worker)));
                    break;
                case PersonType.Investor:
                    targets.AddRange(GameManagerr.instance.GetPerson(typeof(Investor)));
                    break;
                case PersonType.Thief:
                    targets.AddRange(GameManagerr.instance.GetPerson(typeof(Thief)));
                    break;
                case PersonType.BriberThief:
                    targets.AddRange(GameManagerr.instance.GetPerson(typeof(BriberThief)));
                    break;
                case PersonType.Miner:
                    targets.AddRange(GameManagerr.instance.GetPerson(typeof(Miner)));
                    break;
                case PersonType.Assassin:
                    targets.AddRange(GameManagerr.instance.GetPerson(typeof(Assassin)));
                    break;
            }
        }
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
        personStatus = PersonStatus.Idle;

        FinishJobPlayAnim(target);
    }

    public void GoToWaitingRoom()
    {
        personStatus = PersonStatus.Idle;
        if (waitingroom != null)
            SetNewPosition(waitingroom.transform);
    }

    private void OnDestroy()
    {
        GameManagerr.instance.RemovePerson(this);
        GetActivePerson()?.SetTarget(null);
        GetActivePerson()?.SetNewPosition(null);
        GetActivePerson()?.GoToWaitingRoom();
        SetActivePerson(null);
    }

    public Wallet GetWallet()
    {
        return wallet;
    }
    public float GetDeadTime()
    {
        return deadTime;
    }
    public void SetTarget(ITargetable targetable)
    {
        target = targetable;
    }

    public Movement GetMovement()
    {
        return movement;
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


    public abstract void ReachTarget(ITargetable target);
    public abstract void DoingJob(ITargetable target);

    public virtual void OnAwake() { }
    public virtual void OnStart() { }
    public virtual void OnUpdate() { }

    public virtual void OnIdle() { }
    public virtual void FinishJobPlayAnim(ITargetable target) { }
    public virtual void StatusChanged(PersonStatus pers) { }
    public virtual ITargetable FindNewTarget()
    {
        FindAllTargets();
        var _targets = GetMyTargets();

        if (!_targets.Any())
            return null;

        int indx = Random.Range(0, _targets.Count);
        if (_targets.Count == 0)
            return null;

        var temp = _targets[indx];

        if (temp.GetActivePerson() != null && temp.GetActivePerson() != this)
            return null;

        return temp;
    }

}


