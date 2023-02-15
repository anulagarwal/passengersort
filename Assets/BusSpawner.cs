using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusSpawner : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public bool IsDealBus;
    [SerializeField] public bool IsSpawnBus;


    [Header("Component References")]
    [SerializeField] public List<Row> rows;
    // Start is called before the first frame update
    void Start()
    {
        if (!IsSpawnBus)
        {
            Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void Spawn(BusPoint bp)
    {
        GetComponent<Bus>().busPoint = bp;

        foreach (Row r in rows)
        {
            for (int i = 0; i < BusManager.Instance.maxCharacterPerRow; i++)
            {
                GameObject g = Instantiate(PassengerManager.Instance.GetPassenger(r.color), GetComponent<Bus>().GetRowPos(rows.Count - 1, rows.IndexOf(r)) + new Vector3(Random.Range(-0.01f, 0.01f), 0, Random.Range(-0.01f, 0.01f)), Quaternion.identity);
                r.AddCharacter(g.GetComponent<Character>());
                g.transform.SetParent(transform);
            }
            GetComponent<Bus>().AddRow(r);

            
        }
        foreach (Row ro in GetComponent<Bus>().rows)
        {
            foreach (Character c in ro.characters)
            {
                GetComponent<Bus>().AddCharacter(c);
            }
        }
        //GetComponent<Bus>().rows = rows;
        GetComponent<Bus>().ResetRows();
    }


    void Spawn()
    {
        if (!IsDealBus)
        {
            List<GameObject> addedChar = new List<GameObject>();
            foreach (Row r in rows)
            {
                for (int i = 0; i < BusManager.Instance.maxCharacterPerRow; i++)
                {
                    GameObject g = Instantiate(PassengerManager.Instance.GetPassenger(r.color), GetComponent<Bus>().GetRowPos(rows.Count - 1, rows.IndexOf(r)) + new Vector3(Random.Range(-0.01f, 0.01f), 0, Random.Range(-0.01f, 0.01f)), Quaternion.identity);
                    r.AddCharacter(g.GetComponent<Character>());                   
                    g.transform.SetParent(transform);
                }
                GetComponent<Bus>().AddRow(r);
            }
            foreach (Row ro in GetComponent<Bus>().rows)
            {
                foreach (Character c in ro.characters)
                {
                    GetComponent<Bus>().AddCharacter(c);
                }
            }
            //GetComponent<Bus>().rows = rows;
            GetComponent<Bus>().ResetRows();
        }

        else
        {
            //Get buses with less than max rows
            List<Bus> busesNoMax = BusManager.Instance.GetBusesNoMax();
            rows.Clear();
            foreach (Bus b in busesNoMax)
            {
               
                Row r = new Row();
                if (b.rows.Count > 0)
                {
                    r.SetRow(PassengerManager.Instance.GetAlternateColorBasedOnLevel(b.rows[b.rows.Count - 1].color));
                }
                else if(b.rows.Count == 0)
                {
                    r.SetRow(PassengerManager.Instance.GetRandomColorBasedOnLevel());
                }
                rows.Add(r);
                GetComponent<DealBus>().AddRow(r);
                if (rows.Count >= 4)
                {
                    break;
                }
            }

            foreach (Row r in rows)
            {
                for (int i = 0; i < BusManager.Instance.maxCharacterPerRow; i++)
                {
                    GameObject g = Instantiate(PassengerManager.Instance.GetPassenger(r.color), GetComponent<DealBus>().GetRowPos(rows.Count - 1, rows.IndexOf(r)) + new Vector3(Random.Range(-0.01f, 0.01f), 0, Random.Range(-0.01f, 0.01f)), Quaternion.identity);
                    r.AddCharacter(g.GetComponent<Character>());
                    g.transform.SetParent(transform);
                }
            }
        }
        Destroy(this);
    }
}
