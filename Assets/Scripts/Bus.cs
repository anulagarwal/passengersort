using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
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
    [SerializeField] public int characters;
    [SerializeField] public BusType bustype;




    [SerializeField] public BusState state;



    [Header("Component References")]
    [SerializeField] Transform door;
    [SerializeField] GameObject confetti;
    [SerializeField]  NavMeshAgent busAgent;
    [SerializeField] List<NavMeshObstacle> walls;
    [SerializeField] SkinnedMeshRenderer bus;






    #region Mono events
    // Start is called before the first frame update
    void Start()
    {
        busAgent = GetComponent<NavMeshAgent>();
        foreach(Row r in rows)
        {

        }

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
            rowsCount = rows.Count;
            HighlightTopRow();
         }
        else if(BusManager.Instance.selectedBus == this)
        {
            BusManager.Instance.ResetSelections();
            CloseDoor();
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
    int rowsCount = 0;
    public void HighlightTopRow()
    {
        rowsCount--;
        if (rows.Count > 0)
        {
            rows[rowsCount].HighlightCharacters();
            if (rowsCount > 0)
            {
                if (rows[rowsCount].color == rows[rowsCount - 1].color)
                {
                    HighlightTopRow();
                }
            }
        }
        rowsCount = rows.Count;
        if (bustype == BusType.Bus)
        {
            busPoint.GetComponent<BusIndicator>().EnableBar(true);
        }

    }


    public void DehighlightTopRows()
    {
        foreach(Row r in rows)
        {
            r.DehighlightCharacters();
        }

       // GetComponent<BusIndicator>().EnableBar(false);
    }

    public void OpenDoor()
    {
        if (bustype == BusType.Bus)
        {
            door.transform.DOLocalRotate(new Vector3(0, 0, 120f), 0.5f, RotateMode.Fast);
            door.gameObject.isStatic = true;
        }
    }
    public void CloseDoor()
    {
        if (bustype == BusType.Bus)
        {
            door.transform.DOLocalRotate(new Vector3(0, 0f, 0), 0.5f, RotateMode.Fast);
            door.gameObject.isStatic = false;
        }
    }

    #endregion

    #region Passenger Transfers

    public void MoveRowTo(Bus b)
    {
        if (rows.Count > 0)
        {
            if (b.rows.Count > 0 && b.rows.Count<BusManager.Instance.GetMaxRows(b.bustype))
            {
                if(b.rows[b.rows.Count - 1].color == rows[rows.Count - 1].color)
                {
                    CharacterColor c = rows[rows.Count - 1].color;
                    b.AddRow(rows[rows.Count - 1]);

                    rows[rows.Count - 1].MoveCharactersTo(b.GetTopRowPos(), b.transform);
                    rows.RemoveAt(rows.Count - 1);
                    ResetRows();
                    b.ResetRows();
                    if (rows.Count > 0 && rows[rows.Count - 1].color == c && b.rows.Count < BusManager.Instance.GetMaxRows(b.bustype))
                    {
                        MoveRowTo(b);
                    }
                }
            }
            else if(b.rows.Count < BusManager.Instance.GetMaxRows(b.bustype))
            {
                CharacterColor c = rows[rows.Count - 1].color;
                b.AddRow(rows[rows.Count - 1]);

                rows[rows.Count - 1].MoveCharactersTo(b.GetTopRowPos(), b.transform);
                rows.RemoveAt(rows.Count - 1);
                ResetRows();

                if (rows.Count > 0 && rows[rows.Count - 1].color == c && b.rows.Count < BusManager.Instance.GetMaxRows(b.bustype))
                {
                    MoveRowTo(b);
                }
            }
            if (bustype == BusType.Bus)
            {
                busPoint.GetComponent<BusIndicator>().ColorBars(this);
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

    public async void CheckForPassengers()
    {
        if (!isComplete)
        {
            
            if (CheckAllPassengersIn())
            {
                //BusManager.Instance.ResetAllDoors();
                CloseDoor();
                if(BusManager.Instance.oldBus!=null)
                BusManager.Instance.oldBus.CloseDoor();
                //BusManager.Instance.ResetAllDoors();
                await Task.Delay(2000);
                if (IsAllRowSimilar())
                {
                    isComplete = true;
                    CoinManager.Instance.AddCoins(50, transform.position);
                    busPoint.CompleteBus();                  
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
        busPoint.GetComponent<BusIndicator>().ColorBars(this);
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
     //   r.AddBus(this);
        ResetRows();
        rowsCount = rows.Count;
        DehighlightTopRows();
        if (bustype == BusType.Bus && busPoint!=null)
        {
            busPoint.GetComponent<BusIndicator>().ColorBars(this);            
        }

        foreach(Row x in rows)
        {
            //x.UpdateCharacterRadius(PassengerManager.Instance.radius[rows.Count - 1]);
        }
    }

    public void UpdateBusWall()
    {
        if (characters > (BusManager.Instance.maxRows * BusManager.Instance.maxCharacterPerRow) / 2)
        {
            float f = ((float)characters - (float)((BusManager.Instance.maxRows * BusManager.Instance.maxCharacterPerRow) / 2)) / (float)(BusManager.Instance.maxRows * BusManager.Instance.maxCharacterPerRow);
            bus.SetBlendShapeWeight(0, f * 100);
        }
        else
        {
            bus.SetBlendShapeWeight(0, 0);
        }
    }
    public void AddCharacter()
    {
        characters++;
        UpdateBusWall();
    }

    public void RemoveCharacter()
    {
        characters--;
        UpdateBusWall();
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

    public Vector3 GetTopwRowCharacterPos(int val, Row r)
    {
        if (bustype == BusType.Bus)
        {
            if (rows.Count > 0)
            {
                return positions[rows.Count - 1].transform.GetChild(rows.Count - 1 - rows.FindIndex(x => x == r)).GetChild(Mathf.FloorToInt((float)val / (float)2)).position;
            }
            else
            {
                return positions[0].transform.GetChild(0).position;
            }
        }
        else
        {
            return positions[rows.Count - 1].transform.GetChild(rows.Count - 1 - rows.FindIndex(x => x == r)).position;
        }
    }

    public Transform GetTopRow()
    {
        if (rows.Count > 0)
        {
            return positions[rows.Count - 1].transform.GetChild(rows.Count - 1);
        }
        else
        {
            return positions[0].transform.GetChild(0);
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

    public List<Vector3> GetRowsPos()
    {
        List<Vector3> v = new List<Vector3>();
        
            foreach (Transform t in positions[rows.Count - 1].transform.GetComponentsInChildren<Transform>())
            {
                v.Add(t.position);
            }
        
        

        return v;
    }
    #endregion


    #region Check conditions

    public bool IsAllRowSimilar()
    {
        bool similar = false;

        if (rows.Count == BusManager.Instance.maxRows)
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
