using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class BusProvider : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float busSpeed;
    [SerializeField] int currentBusIndex;
    [SerializeField] int currentUnlockIndex;


    [Header("Component References")]
    [SerializeField] GameObject bus;

    [SerializeField] List<BusRow> busRows;
    [SerializeField] List<BusRow> unlockBusRows;    
    [SerializeField] public Transform busStartPoint;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Bus

    //This is for filled buses
    public async void SendBusTo(Vector3 pos)
    {
        //Spawn a bus and send it to target location
        //After bus reaches target location, it will be activated
        GameObject g = Instantiate(bus, busStartPoint.position, Quaternion.identity);
        g.GetComponent<BusSpawner>().UpdateRows(busRows[currentBusIndex]);
        g.GetComponent<BusSpawner>().Spawn(BusManager.Instance.GetBusPoint());
        await Task.Delay(500);
        g.GetComponent<Bus>().SendToParkingLot(pos, BusManager.Instance.GetBusPoint());
        BusManager.Instance.AddBus(g.GetComponent<Bus>());
        
        currentBusIndex++;

        if (currentBusIndex % 10 == 0)
        {
            GameManager.Instance.RequestReview();
        }
        if(currentBusIndex >= busRows.Count)
        {
            currentBusIndex = 0;
        }
        Momo.Analytics.Instance.TrackBusComplete(currentBusIndex);
    }


  

   
    //This is for unlocked slots
    public async void SendEmptyBus(BusPoint bp)
    {
        GameObject g = Instantiate(bus, busStartPoint.position, Quaternion.identity);
        g.GetComponent<BusSpawner>().UpdateRows(unlockBusRows[currentUnlockIndex]);
        g.GetComponent<BusSpawner>().Spawn(BusManager.Instance.GetBusPoint());
        await Task.Delay(1500);
        g.GetComponent<Bus>().PackBus();
        g.GetComponent<Bus>().SendToParkingLot(bp.transform.position, bp);
        BusManager.Instance.AddBus(g.GetComponent<Bus>());
        currentUnlockIndex++;
        if(currentUnlockIndex>= unlockBusRows.Count)
        {
            currentUnlockIndex = 0;
        }
        Momo.Analytics.Instance.TrackUnlockSpace(currentUnlockIndex);

    }
    #endregion
}
