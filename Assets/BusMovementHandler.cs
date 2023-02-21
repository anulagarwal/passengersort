using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using DG.Tweening;
using System.Threading.Tasks;
public class BusMovementHandler : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float speed;
    [SerializeField] int index = 0;
    [SerializeField] public bool isGoingToLot = false;




    [Header("Component References")]
    [SerializeField] Waypoint wp;
    [SerializeField] List<Waypoint> wps;
    [SerializeField] BusPoint b;
    [SerializeField] NavMeshAgent n;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(n.enabled && !isGoingToLot)
        {
            if (n.remainingDistance <= 0.7f)
            {
                n.enabled = false;
                print("yes");

                BusManager.Instance.RemoveBus(GetComponent<Bus>());
                BusManager.Instance.GetComponent<BusProvider>().SendBusTo(transform.position);
                Destroy(gameObject);
            }
        }

        if(n.enabled && isGoingToLot)
        {
            if (n.remainingDistance <= 0.5f)
            {
                // n.Stop();
                n.Stop();
                n.enabled = false;
                MoveToBusPoint();
               // n.enabled = false;
            }
        }
    }
    public void MoveToTarget(Vector3 p)
    {
        n.enabled =true;
        n.SetDestination(p);
    }
    public void UpdateBusPoint(BusPoint bp)
    {
        isGoingToLot = true;
        b = bp;
    }
  

    public void MoveToBusPoint()
    {
        
        Vector3 v = Vector3.zero;
        if (b.transform.position.x > transform.position.x)
        {
            v = new Vector3(0, 90, 0);
        }

        if (b.transform.position.x < transform.position.x)
        {
            v = new Vector3(0, -90, 0);
        }

        if (b.transform.position.z > transform.position.z)
        {
            v = new Vector3(0, 0, 0);
        }

        if (b.transform.position.z < transform.position.z)
        {
            v = new Vector3(0, 180, 0);
        }
        transform.DORotate(b.rotation, 1f).OnComplete(() =>
        {           
            GetComponent<Bus>().state = BusState.Idle;
            GetComponent<Bus>().UnPackBus();
        });

        transform.DOMove(b.transform.position, 1f);
    }

    public void UpdateWayPoints(List<Waypoint> w, BusPoint bp)
    {
        wps = w;
        b = bp;
    }

  
}
