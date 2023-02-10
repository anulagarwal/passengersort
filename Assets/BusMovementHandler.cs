using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
public class BusMovementHandler : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float speed;
    [SerializeField] int index = 0;
    [SerializeField] bool isGoingToLot = false;




    [Header("Component References")]
    [SerializeField] Waypoint wp;
    [SerializeField] List<Waypoint> wps;
    [SerializeField] BusPoint b;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public async void MoveToWaypoint()
    {
        wp = wps[index];
        Vector3 v = Vector3.zero;
        if(wp.transform.position.x > transform.position.x)
        {
            v = new Vector3(0, 90, 0);
        }

        if(wp.transform.position.x < transform.position.x)
        {
            v = new Vector3(0, -90, 0);
        }

        if (wp.transform.position.y > transform.position.y)
        {
            v = new Vector3(0, 0, 0);
        }

        if (wp.transform.position.y < transform.position.y)
        {
            v = new Vector3(0, 180, 0);
        }

        transform.DORotate(v, 0.5f);
        if (v != Vector3.zero) 
        await Task.Delay(500);
        transform.DOMove(wps[index].transform.position, speed).SetSpeedBased(true).OnComplete(()=> {
           
            if ((!wp.isEnd && !isGoingToLot))
            {
                index++;
                MoveToWaypoint();
            }
            else if (index!= wps.Count-1 && isGoingToLot)
            {
                index++;
                MoveToWaypoint();
            }
            else if(index == wps.Count-1 && isGoingToLot)
            {
                MoveToBusPoint();
            }
            else if(wp.isEnd && !isGoingToLot)
            {
                BusManager.Instance.RemoveBus(GetComponent<Bus>());
                BusManager.Instance.GetComponent<BusProvider>().SendBusTo(transform.position);
                Destroy(gameObject);

                //Give coins
            }
        });
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
        transform.DORotate(v, 0.5f);

        transform.DOMove(b.transform.position, speed).SetSpeedBased(true).OnComplete(() => {

            GetComponent<Bus>().UnPackBus();
            transform.DORotate(b.rotation, 0.5f);
        });
    }

    public void UpdateWayPoint(Waypoint w)
    {
        wp = w;
    }

    public void UpdateWayPoints(List<Waypoint> w, BusPoint bp)
    {
        wps = w;
        b = bp;
    }

    public void ReverseList()
    {
        wps.Reverse();
        isGoingToLot = true;
    }
}
