using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusProvider : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float busSpeed;
    [SerializeField] int currentBusIndex;

    [Header("Component References")]
    [SerializeField] List<GameObject> buses;
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

    public void SendBusTo(Vector3 pos)
    {
        //Spawn a bus and send it to target location
        //After bus reaches target location, it will be activated
        GameObject g = Instantiate(bus, BusManager.Instance.busStartPoint.position, Quaternion.identity);
        g.GetComponent<Bus>().SendToParkingLot(pos, BusManager.Instance.GetBusPoint());

        BusManager.Instance.AddBus(g.GetComponent<Bus>());
    }

    #endregion
}
