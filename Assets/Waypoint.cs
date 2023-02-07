using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] public Transform nextPoint;
    [SerializeField] public bool isStart;
    [SerializeField] public bool isEnd;

}
