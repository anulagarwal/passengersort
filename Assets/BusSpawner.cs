using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusSpawner : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public bool IsDealBus;

    [Header("Component References")]
    [SerializeField] public List<Row> rows;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Spawn()
    {
        if (!IsDealBus)
        {
            foreach (Row r in rows)
            {
                for (int i = 0; i < r.maxCharacters; i++)
                {
                    GameObject g = Instantiate(PassengerManager.Instance.GetPassenger(r.color), GetComponent<Bus>().GetRowPos(rows.Count - 1, rows.IndexOf(r)) + new Vector3(Random.Range(-0.01f, 0.01f), 0, Random.Range(-0.01f, 0.01f)), Quaternion.identity);
                    r.AddCharacter(g.GetComponent<Character>());
                    g.transform.SetParent(transform);
                }
                GetComponent<Bus>().AddRow(r);
            }

            //GetComponent<Bus>().rows = rows;
            GetComponent<Bus>().ResetRows();
        }

        else
        {
            foreach (Row r in rows)
            {
                for (int i = 0; i < r.maxCharacters; i++)
                {
                    GameObject g = Instantiate(PassengerManager.Instance.GetPassenger(r.color), GetComponent<DealBus>().GetRowPos(rows.Count - 1, rows.IndexOf(r)) + new Vector3(Random.Range(-0.015f, 0.015f), 0, Random.Range(-0.015f, 0.015f)), Quaternion.identity);

                    r.AddCharacter(g.GetComponent<Character>());
                    g.transform.SetParent(transform);
                }
                GetComponent<DealBus>().AddRow(r);
            }

            //GetComponent<Bus>().rows = rows;
            GetComponent<DealBus>().ResetRows();

        }
        Destroy(this);
    }
}
