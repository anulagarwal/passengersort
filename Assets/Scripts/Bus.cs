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
    [SerializeField] public bool isPickedUp;
    [SerializeField] public Vector3 rotation;
    [SerializeField] public BusPoint busPoint;
    [SerializeField] public int characters;
    [SerializeField] public BusType bustype;
    [SerializeField] public List<CharacterColor> preferredColors;



    [SerializeField] public BusState state;



    [Header("Component References")]
    [SerializeField] GameObject confetti;
    [SerializeField]  NavMeshAgent busAgent;
    [SerializeField] List<NavMeshObstacle> walls;

    [Header("Bus Wall & Door expand")]
    [SerializeField] Transform wallLeft;
    [SerializeField] Transform wallRight;
    [SerializeField] Vector3 origPosLeft;
    [SerializeField] Vector3 origPosRight;
    [SerializeField] Vector3 origDoor;
    [SerializeField] float maxXDoor =2;

    [SerializeField] GameObject doorWall;




    [SerializeField] Transform targetLeftWall;
    [SerializeField] Transform targetRightWall;
    [SerializeField] Transform targetDoor;



    [SerializeField] Transform door;

    [SerializeField] public List<Character> charactersList;

    [SerializeField] SkinnedMeshRenderer bus;






    #region Mono events

    private void Awake()
    {
        //        origPosRight = targetRightWall.position;
        //        origPosLeft = targetLeftWall.position;
        if (bustype == BusType.Bus || bustype == BusType.Bonus)
        {
            origDoor = door.transform.position;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        busAgent = GetComponent<NavMeshAgent>();
        foreach(Row r in rows)
        {

        }
        if (bustype == BusType.Bus)
        {           
            doorWall.GetComponent<NavMeshObstacle>().enabled = false;
            doorWall.SetActive(false);
        }

        maxXDoor = BusManager.Instance.xMaxDoor;

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
        if (state != BusState.Moving)
        {
            if (BusManager.Instance.selectedBus == null && bustype != BusType.Bonus)
            {
                BusManager.Instance.SelectBus(this);
                rowsCount = rows.Count;
                HighlightTopRow();
            }
            else if (BusManager.Instance.selectedBus == this)
            {
                BusManager.Instance.ResetSelections();
            }
            else if (BusManager.Instance.selectedBus != null && BusManager.Instance.enteredBus == null && BusManager.Instance.selectedBus != this)
            {
                BusManager.Instance.EnterBus(this);
                BusManager.Instance.selectedBus.MoveRowTo(this);
                GameManager.Instance.AddMove(1, MoveType.Move);

                BusManager.Instance.ResetSelections();
            }
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
            doorWall.SetActive(false);
            doorWall.GetComponent<NavMeshObstacle>().enabled = false;

            door.transform.DOLocalRotate(new Vector3(door.transform.localRotation.eulerAngles.x, door.transform.localRotation.eulerAngles.y, 120f), 0.5f, RotateMode.Fast).OnComplete(() => {
                
            });
        }

        if(bustype == BusType.Bonus)
        {
            door.transform.DOLocalRotate(new Vector3(door.transform.localRotation.eulerAngles.x, door.transform.localRotation.eulerAngles.y, 120f), 0.5f, RotateMode.Fast).OnComplete(() => {

            });
        }
    }
    public void CloseDoor()
    {
        if (bustype == BusType.Bus )
        {
            door.transform.DOLocalRotate(new Vector3(door.transform.localRotation.eulerAngles.x, door.transform.localRotation.eulerAngles.y, 0), 0.5f, RotateMode.Fast);
        }

        if(bustype == BusType.Bonus)
        {
            door.transform.DOLocalRotate(new Vector3(door.transform.localRotation.eulerAngles.x, door.transform.localRotation.eulerAngles.y, 0), 0.5f, RotateMode.Fast);
        }
    }

    #endregion

    #region Passenger Transfers

    public void MoveRowTo(Bus b)
    {
        if (rows.Count > 0)
        {
            if (b.rows.Count > 0 && b.rows.Count < BusManager.Instance.GetMaxRows(b.bustype))
            {
                if (b.preferredColors.Count == 0 || b.preferredColors.FindAll(x => x == rows[rows.Count - 1].color).Count > 0)
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
            else if (b.rows.Count < BusManager.Instance.GetMaxRows(b.bustype))
            {
                if (b.preferredColors.Count == 0 || b.preferredColors.FindAll(x => x == rows[rows.Count - 1].color).Count > 0)
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
            }
            if (bustype == BusType.Bus)
            {
                //busPoint.GetComponent<BusIndicator>().ColorBars(this);
            }
            //BusManager.Instance.CheckForWin();
        }
    }
    public bool CheckAllPassengersIn()
    {

        if (charactersList.Count == BusManager.Instance.maxCharacterPerRow * rows.Count)

            return true;

        else

            return false;
    }

    public async void CheckForPassengers()
    {
        if (!isComplete)
        {

            if (CheckAllPassengersIn())
            {
                //BusManager.Instance.ResetAllDoors();
               
                //BusManager.Instance.ResetAllDoors();
                if (IsAllRowSimilar() && charactersList.Count == BusManager.Instance.maxCharacterPerRow * BusManager.Instance.maxRows)
                {
                    isComplete = true;
                    if (bustype == BusType.Bus)
                    {
                        doorWall.SetActive(true);
                        doorWall.GetComponent<NavMeshObstacle>().enabled = true;
                    }
                    await Task.Delay(3500);
                    PackBus();

                    await Task.Delay(1500);

                    busPoint.CompleteBus();
                    SendToTravel();
                }
            }
        }
    }

    #endregion

    #region Bus movement
    public async void PackBus()
    {
        foreach (Row r in rows)
        {
            r.DisableAgents();
        }
        foreach (NavMeshObstacle o in walls)
        {
            o.enabled = false;
        }
        await Task.Delay(500);
        CloseDoor();
        if (bustype == BusType.Bus)
        {
            doorWall.SetActive(false);
            doorWall.GetComponent<NavMeshObstacle>().enabled = false;
        }


        state = BusState.Moving;
        if(busPoint!=null)
        busPoint.GetComponent<BusIndicator>().DisableBar();
    }

    public void UnPackBus()
    {
        origDoor = door.transform.position;
        foreach (Row r in rows)
        {
            r.EnableAgents();
        }
        foreach (NavMeshObstacle o in walls)
        {
            o.enabled = true;
        }
        state = BusState.Idle;
        
        if (busPoint!=null)
            busPoint.GetComponent<BusIndicator>().ColorBars(this);

        if (bustype == BusType.Bus)
        {
            doorWall.SetActive(false);
            doorWall.GetComponent<NavMeshObstacle>().enabled = false;
        }


        OpenDoor();

    }
    public void UpdateCharacterSpeed(float speed, float acc)
    {
        foreach (Character c in charactersList)
        {
            c.UpdateAgent(speed, acc);
        }
    }
    public void SendToTravel()
    {
        isGoingToLot = false;
        List<Waypoint> w = new List<Waypoint>();

      
        // GetComponent<BusMovementHandler>().UpdateWayPoints(w, busPoint);
        // GetComponent<BusMovementHandler>().MoveToWaypoint();
        GetComponent<BusMovementHandler>().isGoingToLot = false;
        GetComponent<BusMovementHandler>().MoveToTarget(BusManager.Instance.busStartPoint.position);
        busPoint.Reset();

        UpdateState(BusState.Moving);
        //Now move after this
    }


    public void SendToParkingLot(Vector3 pos, BusPoint bp)
    {
        isGoingToLot = true;
        busPoint = bp;

        PackBus();
        List<Waypoint> w = new List<Waypoint>();

        //  busAgent.enabled = true;
        //  busAgent.SetDestination(bp.transform.position);

        GetComponent<BusMovementHandler>().UpdateWayPoints(w, bp);
        //  GetComponent<BusMovementHandler>().ReverseList();
        //  GetComponent<BusMovementHandler>().MoveToWaypoint();
        GetComponent<BusMovementHandler>().UpdateBusPoint(bp);
        GetComponent<BusMovementHandler>().MoveToTarget(bp.transform.position);
        UpdateState(BusState.Moving);
        isGoingToLot = true;
        rotation = bp.rotation;
        bp.Occupy(this);

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
    }

    public void UpdateBusWall()
    {
        if (charactersList.Count > ((BusManager.Instance.maxRows * BusManager.Instance.maxCharacterPerRow) / 2))
        {
            float f = ((float)charactersList.Count - (float)((BusManager.Instance.maxRows * BusManager.Instance.maxCharacterPerRow) / 2)) / (float)(BusManager.Instance.maxRows * BusManager.Instance.maxCharacterPerRow);
            bus.SetBlendShapeWeight(0, f * 250);
            door.transform.position = new Vector3(origDoor.x + (maxXDoor * ((float)charactersList.Count/(float)((float)BusManager.Instance.maxCharacterPerRow * (float)BusManager.Instance.maxRows))), origDoor.y, origDoor.z);
            door.transform.localRotation = Quaternion.Euler(new Vector3(door.transform.localRotation.eulerAngles.x, BusManager.Instance.xMaxAngle * ((float)charactersList.Count / (float)((float)BusManager.Instance.maxCharacterPerRow * (float)BusManager.Instance.maxRows)), door.transform.localRotation.eulerAngles.z));
           // wallLeft.position = Vector3.Lerp(origPosLeft, targetLeftWall.position, f * 250);
           // wallRight.position = Vector3.Lerp(origPosLeft, targetLeftWall.position,  f * 250);
        }
        else
        {
            //    wallRight.DOMove(origPosRight, 0.2f);
            //    wallLeft.DOMove(origPosLeft, 0.2f);
           // door.transform.rotation = Quaternion.Euler(Vector3.zero);
            door.transform.position = origDoor;

            bus.SetBlendShapeWeight(0, 0);
        }
    }
    public void AddCharacter(Character c)
    {
        bool isInBus = false;
        foreach (Row r in rows)
        {
            if (r.characters.Find(x => x == c) != null)
            {
                isInBus = true;
            }
        }

        if (isInBus && charactersList.Find(x=>x==c) == null)
        {
            charactersList.Add(c);
            c.UpdateAgent(5, 10);
            if(bustype == BusType.Bonus)
            {
                UIManager.Instance.SpawnAwesomeText(c.GetComponentInChildren<NavMeshAgent>().transform.position, "$1");
                CoinManager.Instance.AddCoins(1);
            }
            characters++;
            if (isPickedUp)
            {
                if(charactersList.Count == rows.Count * BusManager.Instance.maxCharacterPerRow)
                {
                    GetComponent<BonusBusMovementHandler>().MoveToSpawn();
                    PackBus();
                }
            }
            UpdateBusWall();
            if(busPoint!=null)
            busPoint.GetComponent<BusIndicator>().ColorBars(this);
        }
    }

    public void RemoveCharacter(Character c)
    {
        bool isInBus = false;
        foreach(Row r in rows)
        {
            if (r.characters.Find(x => x == c)!=null)
            {
                isInBus = true;
            }
        }

        if (!isInBus && charactersList.Find(x => x == c) != null)
        {
            charactersList.Remove(c);
            characters--;
            UpdateBusWall();
            if (busPoint != null)
                busPoint.GetComponent<BusIndicator>().ColorBars(this);
        }
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
      
        if (val >= 10)
        {
            val = val % 10;
        }
        if (bustype == BusType.Bus)
        {
            if (rows.Count > 0)
            {
                return positions[rows.Count - 1].transform.GetChild(rows.Count - 1 - rows.FindIndex(x => x == r)).GetChild(Mathf.FloorToInt((float)val / (float)2)).position + new Vector3(Random.Range(-0.1f,0.1f),0,Random.Range(-0.1f,0.1f));
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
