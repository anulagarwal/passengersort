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
    [SerializeField] List<GameObject> buses;
    [SerializeField] List<GameObject> unlockBuses;

    [SerializeField] public Transform busStartPoint;

    [SerializeField] public GameObject bus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Bus

    public async void SendBusTo(Vector3 pos)
    {
        //Spawn a bus and send it to target location
        //After bus reaches target location, it will be activated
        GameObject g = Instantiate(buses[GetBusIndex(currentBusIndex)], BusManager.Instance.busSpawnStartPoint.position, Quaternion.identity);        
        g.GetComponent<BusSpawner>().Spawn(BusManager.Instance.GetBusPoint());
        await Task.Delay(500);
        g.GetComponent<Bus>().SendToParkingLot(pos, BusManager.Instance.GetBusPoint());
        BusManager.Instance.AddBus(g.GetComponent<Bus>());
        Momo.Analytics.Instance.TrackBusComplete(currentBusIndex);
        currentBusIndex++;

        if (currentBusIndex % 10 == 0)
        {
            GameManager.Instance.RequestReview();
        }
    }


    public int GetBusIndex(int index)
    {

        if (index >= buses.Count)
        {
            index = 0;
        }
        return index;
    }


    public async void SendEmptyBus(BusPoint bp)
    {
        GameObject g = Instantiate(unlockBuses[currentUnlockIndex], BusManager.Instance.busStartPoint.position, Quaternion.identity);
        g.GetComponent<BusSpawner>().Spawn(BusManager.Instance.GetBusPoint());

        await Task.Delay(1500);
        g.GetComponent<Bus>().PackBus();

        g.GetComponent<Bus>().SendToParkingLot(bp.transform.position, bp);
        BusManager.Instance.AddBus(g.GetComponent<Bus>());
        Momo.Analytics.Instance.TrackUnlockSpace(currentUnlockIndex);
        currentUnlockIndex++;

    }
    #endregion
}
