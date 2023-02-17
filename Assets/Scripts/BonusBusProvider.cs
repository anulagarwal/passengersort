using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class BonusBusProvider : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float busSpeed;
    [SerializeField] int currentBusIndex;
    [SerializeField] float timeDelay = 10f;
    [SerializeField] public bool isTimeActive = false;



    [Header("Component References")]
    [SerializeField] List<GameObject> buses;
    [SerializeField] Transform spawnPos;
    [SerializeField] Transform targetPos;



    public static BonusBusProvider Instance = null;

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
        StartBusTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBusTimer()
    {
        if (!isTimeActive)
        {
            UIManager.Instance.ResetBusTimer(timeDelay);
            isTimeActive = true;
        }
    }

    public void StopBusTimer()
    {
        if (isTimeActive)
        {
            UIManager.Instance.StopTimer();
            isTimeActive = false;
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

    public async void SendBonusBus()
    {
        GameObject g = Instantiate(buses[GetBusIndex(currentBusIndex)], spawnPos.position, Quaternion.identity);
        await Task.Delay(500);
        // g.GetComponent<Bus>().SendToParkingLot(spawnPos, BusManager.Instance.GetBusPoint());
        g.GetComponent<BonusBusMovementHandler>().MoveToTarget(targetPos.position);
        g.GetComponent<BonusBusMovementHandler>().UpdateSpawnPoint(spawnPos);
        Momo.Analytics.Instance.TrackBonusBusNew(currentBusIndex);
        currentBusIndex++;
    }

    public void BonusBusComplete()
    {
        Momo.Analytics.Instance.TrackBonusBusComplete(currentBusIndex-1);
    }
}
