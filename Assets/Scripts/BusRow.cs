using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BusRows", order = 1)]
public class BusRow : ScriptableObject
{
    public List<Row> rows;
}
