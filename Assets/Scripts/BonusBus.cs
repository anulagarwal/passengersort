using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class BonusBus : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public List<Row> rows;
    [SerializeField] public List<Transform> positions;
    [SerializeField] public int currentIndex;
    [SerializeField] public Vector3 rotation;
    [SerializeField] public BusPoint busPoint;
    [SerializeField] public int characters;
    [SerializeField] public float waitTime;


    [Header("Component References")]
    [SerializeField] Transform door;
    [SerializeField] NavMeshAgent busAgent;
    [SerializeField] List<NavMeshObstacle> walls;
    [SerializeField] List<CharacterColor> acceptedColors;
    [SerializeField] public List<Character> charactersList;
    [SerializeField] SkinnedMeshRenderer bus;



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

    public void SendBack()
    {
        PackBus();
        GetComponent<DealBusMovementHandler>().ReverseList();
        GetComponent<DealBusMovementHandler>().MoveToWaypoint();

    }

    public void SendToBonusPath(List<Transform> wp)
    {
       // GetComponent<BonusBusMovementHandler>().UpdateWayPoints(wp);
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
    public void UpdateBusWall()
    {
        if (charactersList.Count > ((BusManager.Instance.maxRows * BusManager.Instance.maxCharacterPerRow) / 2))
        {
            float f = ((float)charactersList.Count - (float)((BusManager.Instance.maxRows * BusManager.Instance.maxCharacterPerRow) / 2)) / (float)(BusManager.Instance.maxRows * BusManager.Instance.maxCharacterPerRow);
            bus.SetBlendShapeWeight(0, f * 250);
            //   door.transform.position = Vector3.Lerp(door.transform.position, targetDoor.position, 0.2f);
            // wallLeft.position = Vector3.Lerp(origPosLeft, targetLeftWall.position, f * 250);
            // wallRight.position = Vector3.Lerp(origPosLeft, targetLeftWall.position,  f * 250);
        }
        else
        {
            //    wallRight.DOMove(origPosRight, 0.2f);
            //    wallLeft.DOMove(origPosLeft, 0.2f);
            //  door.transform.position = origDoor;

            bus.SetBlendShapeWeight(0, 0);
        }
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
        CloseDoor();
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
        foreach (Row r in rows)
        {
            foreach (Character c in r.characters)
            {
                c.UpdateAgent(BusManager.Instance.characterSpeedHigh, BusManager.Instance.characterAccelerationHigh);
            }
        }
        OpenDoor();
    }
    #endregion

    public void SellCharacters()
    {
        //Sell add coins based on total number of characters
        int c = 0;
        foreach(Row r in rows)
        {

        }
        
            
        Destroy(gameObject);
    }


    public void SetIndication()
    {

    }
    #region RowManagement

    public void AddRow(Row r)
    {
        rows.Add(r);
        ResetRows();
    }

    public void AddCharacter(Character c)
    {
        bool isInBus = false;
        foreach (Row r in rows)
        {
            if (r.characters.Find(x => x == c) != null)
            {
                isInBus = true;
            }
        }

        if (isInBus && charactersList.Find(x => x == c) == null)
        {
            charactersList.Add(c);
            characters++;
            UpdateBusWall();
        }
    }

    public void RemoveCharacter(Character c)
    {
        bool isInBus = false;
        foreach (Row r in rows)
        {
            if (r.characters.Find(x => x == c) != null)
            {
                isInBus = true;
            }
        }

        if (!isInBus && charactersList.Find(x => x == c) != null)
        {
            charactersList.Remove(c);
            characters--;
            UpdateBusWall();
        }
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

    public Vector3 GetTopwRowCharacterPos(int val, Row r)
    {

        if (val >= 10)
        {
            val = val % 10;
        }
       
            if (rows.Count > 0)
            {
                return positions[rows.Count - 1].transform.GetChild(rows.Count - 1 - rows.FindIndex(x => x == r)).GetChild(Mathf.FloorToInt((float)val / (float)2)).position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
            }
            else
            {
                return positions[0].transform.GetChild(0).position;
            }
       
    }

    public Transform GetTopRow()
    {
        if (rows.Count > 0)
        {
            return positions[rows.Count - 1].transform.GetChild(rows.Count - 1);
        }
        else
        {
            return positions[0].transform.GetChild(0);
        }
    }

    public void ResetRows()
    {
        int i = rows.Count;
        foreach (Row r in rows)
        {

            r.MoveCharactersTo(positions[i - 1].transform.GetChild((positions[i - 1].childCount - 1) - rows.FindIndex(x => x == r)).position, transform);
        }
    }

    public Vector3 GetRowPos(int maxRow, int index)
    {
        return positions[maxRow].transform.GetChild(maxRow - index).position;
    }

    public List<Vector3> GetRowsPos()
    {
        List<Vector3> v = new List<Vector3>();

        foreach (Transform t in positions[rows.Count - 1].transform.GetComponentsInChildren<Transform>())
        {
            v.Add(t.position);
        }



        return v;
    }
    #endregion
}
