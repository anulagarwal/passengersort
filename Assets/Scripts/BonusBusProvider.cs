using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBusProvider : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float busSpeed;
    [SerializeField] int currentBusIndex;

    [Header("Component References")]
    [SerializeField] List<GameObject> buses;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendBus()
    {
        //Bus will come and pause in middle of the road
        //It will wait for certain seconds and then go to the end of the road
        //Based on number of passengers it will give additional coins
    }
}
