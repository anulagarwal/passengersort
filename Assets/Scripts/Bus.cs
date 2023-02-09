using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using DG.Tweening;
public class Bus : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] public List<Row> rows;
    [SerializeField] public List<Transform> positions;
    [SerializeField] public int currentIndex;
    [SerializeField] public bool isComplete;
    [SerializeField] public bool isGoingToLot;
    [SerializeField] public Vector3 rotation;
    [SerializeField] public BusPoint busPoint;



    [SerializeField] public BusState state;



    [Header("Component References")]
    [SerializeField] Transform door;
    [SerializeField] GameObject confetti;
    [SerializeField]  NavMeshAgent busAgent;
    [SerializeField] List<NavMeshObstacle> walls;





    #region Mono events
    // Start is called before the first frame update
    void Start()
    {
        busAgent = GetComponent<NavMeshAgent>();
        foreach(Row r in rows)
        {

        }

        print(Camera.main.WorldToScreenPoint(transform.position));
    }

    // Update is called once per frame
    void Update()
    {
       if(state == BusState.Moving)
        {
            
        }
    }

    #endregion
    #region Mouse events
    public void OnMouseUp()
    {
        if (BusManager.Instance.selectedBus == null)
         {
             BusManager.Instance.SelectBus(this);
            
         }
         else if (BusManager.Instance.selectedBus != null && BusManager.Instance.enteredBus == null && BusManager.Instance.selectedBus != this)
         {
             BusManager.Instance.EnterBus(this);
             BusManager.Instance.selectedBus.MoveRowTo(this);
             BusManager.Instance.ResetSelections();
         } 
       // SendToTravel();
      
    }

    public void OnMouseDown()
    {
        SoundManager.Instance.Play(Sound.Pop);
    }
    #endregion

    public void Spawn()
    {

    }

    #region Effects
    public void OpenDoor()
    {
        door.transform.DOLocalRotate(new Vector3(0, 0, 120f), 0.5f, RotateMode.Fast);
        door.gameObject.isStatic = true;
    }
    public void CloseDoor()
    {
        door.transform.DOLocalRotate(new Vector3(0, 0f, 0), 0.5f, RotateMode.Fast);
        door.gameObject.isStatic = false;

    }

    #endregion

    #region Passenger Transfers

    public void MoveRowTo(Bus b)
    {
        if (rows.Count > 0)
        {
            if (b.rows.Count > 0 && b.rows.Count<BusManager.Instance.maxRows)
            {
                if(b.rows[b.rows.Count - 1].color == rows[rows.Count - 1].color)
                {
                    CharacterColor c = rows[rows.Count - 1].color;
                    rows[rows.Count - 1].MoveCharactersTo(b.GetTopRowPos(), b.transform);
                    b.AddRow(rows[rows.Count - 1]);
                    rows.RemoveAt(rows.Count - 1);
                    ResetRows();

                    if (rows.Count > 0 && rows[rows.Count - 1].color == c && b.rows.Count < BusManager.Instance.maxRows)
                    {
                        MoveRowTo(b);
                    }

                }
            }
            else if(b.rows.Count < BusManager.Instance.maxRows)
            {
                CharacterColor c = rows[rows.Count - 1].color;
                rows[rows.Count - 1].MoveCharactersTo(b.GetTopRowPos(), b.transform);
                b.AddRow(rows[rows.Count - 1]);
                rows.RemoveAt(rows.Count - 1);
                ResetRows();

                if (rows.Count > 0 && rows[rows.Count - 1].color == c && b.rows.Count < BusManager.Instance.maxRows)
                {
                    MoveRowTo(b);
                }

            }

            //BusManager.Instance.CheckForWin();
        }
    }
    public bool CheckAllPassengersIn()
    {
        bool stop = false;
        foreach (Row r in rows)
        {
            //If all characters in
            if (r.IsStopped())
            {
                stop = true;
            }
            else
            {
                stop = false;
            }
        }

        return stop;

    }

    public void CheckForPassengers()
    {
        if (!isComplete)
        {
            
            if (CheckAllPassengersIn())
            {
                //BusManager.Instance.ResetAllDoors();
                CloseDoor();

                if (IsAllRowSimilar())
                {
                    isComplete = true;
                    CoinManager.Instance.AddCoins(50, transform.position);
                    confetti.SetActive(true);                    
                    SendToTravel();                    
                }
            }
        }
    }
    #endregion

    #region Bus movement
    public void PackBus()
    {
        foreach (Row r in rows)
        {
            r.DisableAgents();
        }
        foreach (NavMeshObstacle o in walls)
        {
            o.enabled = false;
        }
    }

    public void UnPackBus()
    {
        foreach (Row r in rows)
        {
            r.EnableAgents();
        }
        foreach (NavMeshObstacle o in walls)
        {
            o.enabled = true;
        }
    }
    
    public void SendToTravel()
    {
        isGoingToLot = false;
        PackBus();
        List<Waypoint> w = new List<Waypoint>();

        Waypoint wp = busPoint.waypointStart.GetComponent<Waypoint>();
        w.Add(wp);

        while (!wp.isEnd)
        {
            wp = wp.nextPoint.GetComponent<Waypoint>();
            w.Add(wp);
        }
        GetComponent<BusMovementHandler>().UpdateWayPoints(w, busPoint);
        GetComponent<BusMovementHandler>().MoveToWaypoint();
        busPoint.Reset();

        UpdateState(BusState.Moving);
        //Now move after this
    }


    public void SendToParkingLot(Vector3 pos, BusPoint bp)
    {
        isGoingToLot = true;
        PackBus();
        List<Waypoint> w = new List<Waypoint>();

        Waypoint wp = bp.waypointStart.GetComponent<Waypoint>();
        w.Add(wp);

        while (!wp.isEnd)
        {
            wp = wp.nextPoint.GetComponent<Waypoint>();
            w.Add(wp);
        }
        //  busAgent.enabled = true;
        //  busAgent.SetDestination(bp.transform.position);

        GetComponent<BusMovementHandler>().UpdateWayPoints(w, bp);
        GetComponent<BusMovementHandler>().ReverseList();
        GetComponent<BusMovementHandler>().MoveToWaypoint();
        UpdateState(BusState.Moving);
        isGoingToLot = true;
        rotation = bp.rotation;
        bp.Occupy(this);
        busPoint = bp;

    }

    #endregion


    public void UpdateState(BusState s)
    {
        state = s;
    }

    #region RowManagement

    public void AddRow(Row r)
    {
        rows.Add(r);
        ResetRows();


    }

    public Vector3 GetTopRowPos()
    {
        if (rows.Count > 0)
        {
            return positions[rows.Count - 1].transform.GetChild(rows.Count - 1).position;
        }
        else
        {
            return positions[0].transform.GetChild(0).position;
        }
    }

    public void ResetRows()
    {
        int i = rows.Count;
        foreach(Row r in rows)
        {
            r.MoveCharactersTo(positions[i - 1].transform.GetChild((positions[i - 1].childCount-1) - rows.FindIndex(x=>x==r)).position, transform);
        }
    }

    public Vector3 GetRowPos(int maxRow, int index)
    {
        return positions[maxRow].transform.GetChild(maxRow - index).position;
    }
    #endregion


    #region Check conditions

    public bool IsAllRowSimilar()
    {
        bool similar = false;

        if (rows.Count == 3)
        {
            foreach(Row r in rows)
            {
                if(r.color == rows[0].color)
                {
                    similar = true;
                }
                else
                {
                    similar = false;
                    return similar;
                }
            }
        }       

        return similar;
    }

    #endregion
}
