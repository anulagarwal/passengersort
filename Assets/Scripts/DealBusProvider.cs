using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

using DG.Tweening;
public class DealBusProvider : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float busSpeed;
    [SerializeField] int currentBusIndex;

    [Header("Component References")]
    [SerializeField] List<GameObject> buses;
    [SerializeField] List<Transform> waypoints;

    public static DealBusProvider Instance = null;

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

    public async void DealBus()
    {
        BonusBusProvider.Instance.StopBusTimer();
        GameObject g = Instantiate(buses[0], waypoints[0].position, Quaternion.identity);
        await Task.Delay(500);

        g.GetComponent<DealBus>().PackBus();
        g.GetComponent<DealBus>().SendToDeal(waypoints);
        currentBusIndex++;

        
        GameManager.Instance.AddMove(1);

        Momo.Analytics.Instance.TrackDeal(currentBusIndex);

    }
}
