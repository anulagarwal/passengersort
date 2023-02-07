using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public int maxRows = 5;
    public Bus selectedBus;
    public Bus enteredBus;


    [Header("Component References")]
    [SerializeField] public Transform busEndPoint;
    [SerializeField] public Transform busStartPoint;
    [SerializeField] List<Bus> buses;
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

    #region BusSelections

    public void SelectBus(Bus b)
    {
        ResetAllDoors();

        selectedBus = b;
        b.OpenDoor();

    }


    public void EnterBus(Bus b)
    {
        enteredBus = b;
        b.OpenDoor();
    }

    public void ResetSelections()
    {
        selectedBus = null;
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
