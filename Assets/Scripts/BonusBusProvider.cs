using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class BonusBusProvider : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float busSpeed;
    [SerializeField] int currentBusIndex;

    [Header("Component References")]
    [SerializeField] List<GameObject> buses;
    [SerializeField] Transform spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetBusIndex(int index)
    {

        if (index >= buses.Count)
        {
            index = 0;
        }
        return index;
    }

    public async void SendBonusBus()
    {
        GameObject g = Instantiate(buses[GetBusIndex(currentBusIndex)], spawnPos.position, Quaternion.identity);
        await Task.Delay(500);
       // g.GetComponent<Bus>().SendToParkingLot(spawnPos, BusManager.Instance.GetBusPoint());
        BusManager.Instance.AddBus(g.GetComponent<Bus>());
        Momo.Analytics.Instance.TrackBonusBusNew(currentBusIndex);
        currentBusIndex++;
    }

    public void BonusBusComplete()
    {
        Momo.Analytics.Instance.TrackBonusBusComplete(currentBusIndex-1);
    }
}
