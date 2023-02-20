using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DealBusMovementHandler : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float speed;
    [SerializeField] int index = 0;
    [SerializeField] public bool isGoingToDeal = true;

    [Header("Component References")]
    [SerializeField] Transform wp;
    [SerializeField] List<Transform> wps;

    private void Start()
    {
        isGoingToDeal = true;
    }

    public void MoveToWaypoint()
    {
        wp = wps[index];
        Vector3 v = Vector3.zero;
        if (wp.transform.position.x > transform.position.x)
        {
            v = new Vector3(0, 90, 0);
        }

        if (wp.transform.position.x < transform.position.x)
        {
            v = new Vector3(0, -90, 0);
        }

        if (wp.transform.position.z > transform.position.z)
        {
            v = new Vector3(0, 0, 0);
        }

        if (wp.transform.position.z < transform.position.z)
        {
            v = new Vector3(0, 180, 0);
        }

        transform.DORotate(v, 0.5f);
        transform.DOMove(wps[index].position, speed).SetSpeedBased(true).OnComplete(() => {

            if ((index != wps.Count - 1 && !isGoingToDeal))
            {
                index++;
                MoveToWaypoint();
            }
            else if (index != wps.Count - 1 && isGoingToDeal)
            {
                index++;
                MoveToWaypoint();
            }
            else if (index == wps.Count - 1 && isGoingToDeal)
            {
                //Deal from here then wait
                GetComponent<DealBus>().Deal();
            }
            else if (index == wps.Count - 1 && !isGoingToDeal)
            {
                BonusBusProvider.Instance.StartBusTimer();
                Destroy(gameObject);
                //Give coins
            }
        });
    }

    public void UpdateWayPoint(Transform w)
    {
        wp = w;
    }

    public void UpdateWayPoints(List<Transform> w)
    {
        wps.Add( w[0]);
        wps.Add(w[1]);

    }

    public void ReverseList()
    {
        wps.Reverse();
        isGoingToDeal = false;
        index = 0;
    }
}
