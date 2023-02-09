using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro; 
public class BusPoint : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] Bus occupiedBy;
    [SerializeField] BusPointType type;
    [SerializeField] int cost = 50;

    [SerializeField] public Vector3 rotation;

    [Header("Component References")]
    [SerializeField] public Transform waypointStart;
    [SerializeField] public Transform lockImg;
    [SerializeField] public GameObject locked;
    [SerializeField] public TextMeshPro lockText;


    private void Start()
    {
        UpdateState(type);
    }

    private void OnMouseDown()
    {
        if(type == BusPointType.Locked)
        {
            if (CoinManager.Instance.SubtractCoins(cost, transform.position))
            {
                Unlock();
                UpdateState(BusPointType.Empty);
            }
        }
    }


    public void Reset()
    {
        occupiedBy = null;
        UpdateState(BusPointType.Empty);
    }

    public void Occupy(Bus b)
    {
        occupiedBy = b;
        UpdateState(BusPointType.Occupied);

    }
    public BusPointType GetPointType()
    {
        return type;
    }

    public void UpdateState(BusPointType bpt)
    {
        type = bpt;
        switch (bpt)
        {
            case BusPointType.Locked:
                GetComponent<BoxCollider>().enabled = true;
                lockText.text = "" + cost;
                break;

            case BusPointType.Empty:
                GetComponent<BoxCollider>().enabled = false;

                break;

            case BusPointType.Occupied:

                break;
        }
    }

    public void Unlock()
    {
        //Disable Box Collider
        locked.SetActive(false);
        BusManager.Instance.GetComponent<BusProvider>().SendEmptyBus(this);
        //fly coins from ui - add to coinmanager only this functionality
    }
    public int GetCost()
    {
        return cost;
    }
}
