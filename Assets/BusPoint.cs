using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusPoint : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] Bus occupiedBy;
    [SerializeField] BusPointType type;
    [SerializeField] public Vector3 rotation;

    [SerializeField] public Transform waypointStart;





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
    }
}
