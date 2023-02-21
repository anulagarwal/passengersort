using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public int maxRows = 5;
    [SerializeField] public int maxRowsPerFreeSpot = 1;
    [SerializeField] public int characterSpeedHigh = 16;
    [SerializeField] public int characterAccelerationHigh = 65;



    [SerializeField] public int maxCharacterPerRow = 10;
    [SerializeField] public float xOffsetCharacter = 0.2f;
    [SerializeField] public float yOffsetCharacter = 0.2f;
    [SerializeField] public float xMaxDoor = 0.2f;
    [SerializeField] public float xMaxAngle = -15f;


    [SerializeField] public float minStopDistance = 0.4f;
    [SerializeField] public int level = 0;
    [SerializeField] public int coinsPerComplete = 75;

    public Bus selectedBus;
    public Bus oldBus;

    public Bus enteredBus;


    [Header("Component References")]
    [SerializeField] public Transform busEndPoint;
    [SerializeField] public Transform busStartPoint;
    [SerializeField] public Transform busSpawnStartPoint;

    [SerializeField] public List<Bus> buses;
    [SerializeField] List<BusPoint> busPoints;





    public static BusManager Instance = null;



    //Organize
    //Keep track of all slots and check which ones empty
    private void Awake()
    {
        Application.targetFrameRate = 100;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBus(Bus b)
    {
        buses.Add(b);
    }

    public void RemoveBus(Bus b)
    {
        buses.Remove(b);
    }
    public void ResetAllDoors()
    {
        foreach(Bus b in buses)
        {
            if(b.CheckAllPassengersIn())
            b.CloseDoor();
        }

    }
    public BusPoint GetBusPoint()
    {
        return busPoints.Find(x => x.GetPointType() == BusPointType.Empty);
    }

    public void RemoveCharacterFromBuses(Character c, Bus except)
    {
        foreach(Bus b in buses)
        {
            if(b!=except)
            b.RemoveCharacter(c);
        }
    }
    #region BusSelections

    public void SelectBus(Bus b)
    {
        b.UpdateCharacterSpeed(BusManager.Instance.characterSpeedHigh, BusManager.Instance.characterAccelerationHigh);
        selectedBus = b;
        oldBus = b;
        GameManager.Instance.AddMove(1, MoveType.Select);
        b.OpenDoor();
       
    }

    public void UpdateBusPoints()
    {
        foreach(BusPoint bp in busPoints)
        {
            bp.CheckForExpensive();
        }
    }
    public List<Bus> GetBuses()
    {
        return buses;
    }
    public List<Bus> GetBusesNoMax()
    {
        List<Bus> b =  buses.FindAll(x=>x.rows.Count < GetMaxRows(x.bustype));
        if (b.Count > 5)
        {
            b.RemoveRange(maxRows, b.Count - maxRows);
        }

        return b;
    }

    public void EnterBus(Bus b)
    {
        enteredBus = b;
        b.UpdateCharacterSpeed(BusManager.Instance.characterSpeedHigh, BusManager.Instance.characterAccelerationHigh);

        b.OpenDoor();


    }
    public int GetMaxRows(BusType bt)
    {
        int i = 0;
        switch (bt)
        {
            case BusType.Bus:

                i = maxRows;
                break;
            case BusType.Bonus:
                i = maxRows;
                break;
            case BusType.Spot:
                i =  maxRowsPerFreeSpot;
                break;

        }
        return i;
    }
    public void ResetSelections()
    {
      //  selectedBus.UpdateCharacterSpeed(5, 20);
      //  enteredBus.UpdateCharacterSpeed(5, 20);

        selectedBus = null;
        foreach(Bus b in buses)
        {
            if (b.rows.Count > 0)
            {
               b.DehighlightTopRows();
            }
        }
       
        enteredBus = null;
    }

    #endregion
    /* public void CheckForWin()
     {
         //Check all buses
         //Check if every individual bus and their rows are same color
         //If all same color then wind
         bool isSimilar = false;
         foreach(Bus b in buses)
         {
             if (b.rows.Count > 0)
             {
                 if (b.IsAllRowSimilar())
                 {
                     isSimilar = true;

                 }
                 else
                 {
                     isSimilar = false;
                 }

             }

         }
         if (isSimilar)
         {
             Invoke("Win", 3f);
         }

     }*/

    public void Win()
    {
        GameManager.Instance.WinLevel();
    }
}
