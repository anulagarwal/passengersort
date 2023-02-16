using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class DealBus : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] public List<Row> rows;
    [SerializeField] public List<Transform> positions;
    [SerializeField] public int currentIndex;
    [SerializeField] public Vector3 rotation;
    [SerializeField] public BusPoint busPoint;


    [Header("Component References")]
    [SerializeField] Transform door;
    [SerializeField] NavMeshAgent busAgent;
    [SerializeField] List<NavMeshObstacle> walls;


    #region Effects
    public void OpenDoor()
    {
        door.transform.DOLocalRotate(new Vector3(0, 0, 120), 0.5f, RotateMode.Fast);
    }
    public void CloseDoor()
    {
        door.transform.DOLocalRotate(new Vector3(0, 0f, 0), 0.5f, RotateMode.Fast);
    }

    #endregion

    #region Passenger Transfers

   public void Deal()
    {
        //Disable deal mechanic when no more space
        UnPackBus();
        for(int i = rows.Count-1; i>=0; i--)
        {
            //Send each row to buses from bus manager
            Bus b = BusManager.Instance.GetBusesNoMax()[GetIndex(i, BusManager.Instance.buses.FindAll(x => x.rows.Count < BusManager.Instance.maxRows).Count)];
            b.AddRow(rows[rows.Count - 1]);

            rows[rows.Count - 1].MoveCharactersTo(b.GetTopRowPos(), b.transform);
            rows.RemoveAt(rows.Count - 1);
            ResetRows();
            b.ResetRows();

        }
        Invoke("SendBack", 5f);
    }

    public void SendBack()
    {
        PackBus();
        GetComponent<DealBusMovementHandler>().ReverseList();
        GetComponent<DealBusMovementHandler>().MoveToWaypoint();

    }

    public void SendToDeal(List<Transform> wp)
    {
        GetComponent<DealBusMovementHandler>().UpdateWayPoints(wp);
        GetComponent<DealBusMovementHandler>().MoveToWaypoint();
    }

    public int GetIndex(int indexVal, int max)
    {
        int i = indexVal;
        if (indexVal >= max)
        {
            i = 0;
        }
        return i;
    }

    #endregion

    #region Bus movement
    public void PackBus()
    {
        foreach (Row r in rows)
        {
            r.DisableAgents();
        }
        foreach (NavMeshObstacle o in walls)
        {
            o.enabled = false;
        }
    }

    public void UnPackBus()
    {
        foreach (Row r in rows)
        {
            r.EnableAgents();
        }
        foreach (NavMeshObstacle o in walls)
        {
            o.enabled = true;
        }
        OpenDoor();
    }     
    #endregion

    #region RowManagement

    public void AddRow(Row r)
    {
        rows.Add(r);
        ResetRows();
    }

    public Vector3 GetTopRowPos()
    {
        if (rows.Count > 0)
        {
            return positions[rows.Count - 1].transform.GetChild(rows.Count - 1).position;
        }
        else
        {
            return positions[0].transform.GetChild(0).position;
        }
    }

    public void ResetRows()
    {
        int i = rows.Count;
        foreach (Row r in rows)
        {
            r.MoveDealCharactersTo(positions[i - 1].transform.GetChild((positions[i - 1].childCount - 1) - rows.FindIndex(x => x == r)).position, transform);
        }
    }
    public Vector3 GetTopwRowCharacterPos(int val, Row r)
    {

        if (val >= 10)
        {
            val = val % 10;
        }
        
            if (rows.Count > 0)
            {
                return positions[rows.Count - 1].transform.GetChild(rows.Count - 1 - rows.FindIndex(x => x == r)).GetChild(Mathf.FloorToInt((float)val / (float)2)).position;
            }
            else
            {
                return positions[0].transform.GetChild(0).position;
            }
      
    }
    public Vector3 GetRowPos(int maxRow, int index)
    {
        return positions[maxRow].transform.GetChild(maxRow - index).position;
    }
    #endregion


  
}
