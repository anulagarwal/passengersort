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
    [SerializeField] List<PreferredColors> preferredColors;




    [Header("Component References")]
    [SerializeField] GameObject bus;
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
        UIManager.Instance.UpdatePowerupButton(true, PowerupManager.Instance.GetPowerupCost(PowerupType.Deal));
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
        UIManager.Instance.UpdatePowerupButton(true, PowerupManager.Instance.GetPowerupCost(PowerupType.Deal)) ;

    }

    public void StopBusTimer()
    {
        if (isTimeActive)
        {
            UIManager.Instance.StopTimer();
            isTimeActive = false;
        }
        UIManager.Instance.UpdatePowerupButton(false, PowerupManager.Instance.GetPowerupCost(PowerupType.Deal));

    }


    public async void SendBonusBus()
    {
        GameObject g = Instantiate(bus, spawnPos.position, Quaternion.identity);
        g.GetComponent<Bus>().preferredColors = preferredColors[currentBusIndex].colors;
        await Task.Delay(500);
        // g.GetComponent<Bus>().SendToParkingLot(spawnPos, BusManager.Instance.GetBusPoint());
        g.GetComponent<BonusBusMovementHandler>().MoveToTarget(targetPos.position);
        g.GetComponent<BonusBusMovementHandler>().UpdateSpawnPoint(spawnPos);
        currentBusIndex++;
        if(currentBusIndex>= preferredColors.Count)
        {
            currentBusIndex = 0;
        }
        Momo.Analytics.Instance.TrackBonusBusNew(currentBusIndex);

    }

    public void BonusBusComplete()
    {
        Momo.Analytics.Instance.TrackBonusBusComplete(currentBusIndex-1);
    }
}
