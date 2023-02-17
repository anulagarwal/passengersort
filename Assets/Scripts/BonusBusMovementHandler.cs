using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UI;
public class BonusBusMovementHandler : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float speed;
    [SerializeField] float waitTime;

    [SerializeField] int index = 0;
    [SerializeField] public bool isGoingToPickup = false;




    [Header("Component References")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] Bus bus;

    [SerializeField] BusPoint b;
    [SerializeField] NavMeshAgent n;
    [SerializeField] GameObject timerObj;
    [SerializeField] List<Image> characterImages;
    [SerializeField] Image timer;




    // Start is called before the first frame update
    void Start()
    {
        bus = GetComponent<Bus>();
        timerObj.SetActive(false);

        for(int i=0;i< bus.preferredColors.Count; i++)
        {
            characterImages[i].gameObject.SetActive(true);
            characterImages[i].sprite = PassengerManager.Instance.GetIcon(bus.preferredColors[i]);
        }
        UIManager.Instance.UpdatePowerupButton(false, PowerupManager.Instance.GetPowerupCost(PowerupType.Deal));
    }

    // Update is called once per frame
    void Update()
    {
        if (n.enabled && !isGoingToPickup)
        {
            if (n.remainingDistance <= 0.7f)
            {
                n.enabled = false;
                CoinManager.Instance.AddCoins(GetComponent<Bus>().charactersList.Count * 2, transform.position);
                UIManager.Instance.UpdatePowerupButton(true, PowerupManager.Instance.GetPowerupCost(PowerupType.Deal));
                BonusBusProvider.Instance.isTimeActive= false;
                BonusBusProvider.Instance.StartBusTimer();
                //Track bus complete
                Destroy(gameObject);
            }
        }

        if (n.enabled && isGoingToPickup)
        {
            if (n.remainingDistance <= 0.5f)
            {
                n.Stop();
                n.enabled = false;
                GetComponent<Bus>().state = BusState.Idle;
                GetComponent<Bus>().UnPackBus();
                timerObj.SetActive(true);
                timer.DOFillAmount(1, waitTime).OnComplete(() => {

                    if (GetComponent<Bus>().rows.Count * BusManager.Instance.maxCharacterPerRow == GetComponent<Bus>().charactersList.Count)
                    {
                        GetComponent<Bus>().PackBus();
                        MoveToSpawn();
                    }
                    else
                    {
                        GetComponent<Bus>().isPickedUp = true;
                    }

                }); 
                //Wait x seconds
                //Unpack and wait to fill
                //After x seconds, packup and go back to spawn point
            }
        }
    }
    public void MoveToTarget(Vector3 p)
    {
        n.enabled = true;
        n.SetDestination(p);
        isGoingToPickup = true;
    }

    public async void MoveToSpawn()
    {
        await Task.Delay(2500);
        isGoingToPickup = false;
        timerObj.SetActive(false);
        n.enabled = true;
        n.SetDestination(spawnPoint.position);
    }

    public void UpdateSpawnPoint(Transform p)
    {
        spawnPoint = p;
    }
    public void UpdateBusPoint(BusPoint bp)
    {
        isGoingToPickup = true;
        b = bp;
    }
   
  

  
}
