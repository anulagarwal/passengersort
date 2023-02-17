using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public CharacterColor GetColorOnIndex(int i)
    {
        switch (i)
        {
            case 0:
                return CharacterColor.Red;

            case 1:

                return CharacterColor.Green;


            case 2:

                return CharacterColor.Blue;


            case 3:

                return CharacterColor.Yellow;
        }

        return CharacterColor.Red;
    }


      public void Spawn(BusPoint bp)
        {
         GetComponent<Bus>().busPoint = bp;
        foreach (Row r in rows)
         {
             for (int i = 0; i < BusManager.Instance.maxCharacterPerRow; i++)
             {
                 GameObject g = Instantiate(PassengerManager.Instance.GetPassenger(r.color), GetComponent<Bus>().GetRowPos(rows.Count - 1, rows.IndexOf(r)) + new Vector3(Random.Range(-0.02f, 0.02f), 0, Random.Range(-0.02f, 0.02f)), Quaternion.identity);
                 r.AddCharacter(g.GetComponent<Character>());
                g.GetComponent<Character>().EnterBus(GetComponent<Bus>());

                g.transform.SetParent(transform);
             }
             GetComponent<Bus>().AddRow(r);


         }
         foreach (Row ro in GetComponent<Bus>().rows)
         {
             foreach (Character c in ro.characters)
             {
                 GetComponent<Bus>().AddCharacter(c);
                bp.GetComponent<BusIndicator>().DisableBar();

            }
        }
        //GetComponent<Bus>().rows = rows;
        GetComponent<Bus>().PackBus();
        //GetComponent<Bus>().ResetRows();
        Destroy(this);

        /*  List<int> colorRows = new List<int>();
          colorRows.Add(0);
          colorRows.Add(0);
          colorRows.Add(0);
          colorRows.Add(0);

          foreach (Bus b in BusManager.Instance.buses)
          {
              if (b.rows.Count > 0)
              {
                  switch (b.rows[b.rows.Count - 1].color)
                  {
                      case CharacterColor.Red:
                          colorRows[0]++;
                          break;

                      case CharacterColor.Green:
                          colorRows[1]++;
                          break;

                      case CharacterColor.Blue:
                          colorRows[2]++;
                          break;

                      case CharacterColor.Yellow:
                          colorRows[3]++;
                          break;
                  }
              }
          }

          colorRows.Sort();
          rows.Clear();
          print(colorRows[0]);
          for (int z = 0; z < colorRows.FindAll(x => x != 0).Count; z++)
          {
              Row r = new Row();
              CharacterColor c = GetColorOnIndex(colorRows.FindAll(x => x != 0).Count - z - 1);
              r.SetRow(c);
              for (int i = 0; i < BusManager.Instance.maxCharacterPerRow; i++)
              {
                  GameObject g = Instantiate(PassengerManager.Instance.GetPassenger(r.color), GetComponent<Bus>().GetRowPos(rows.Count - 1, rows.IndexOf(r)) + new Vector3(Random.Range(-0.01f, 0.01f), 0, Random.Range(-0.01f, 0.01f)), Quaternion.identity);
                  r.AddCharacter(g.GetComponent<Character>());
                  g.transform.SetParent(transform);
              }
              rows.Add(r);
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
        */
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
                    GameObject g = Instantiate(PassengerManager.Instance.GetPassenger(r.color), GetComponent<Bus>().GetRowPos(rows.Count - 1, rows.IndexOf(r)) + new Vector3(Random.Range(-0.03f, 0.03f), 0, Random.Range(-0.03f, 0.03f)), Quaternion.identity);
                    r.AddCharacter(g.GetComponent<Character>());
                    g.GetComponent<Character>().EnterBus(GetComponent<Bus>());
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
            GetComponent<Bus>().UnPackBus();

            GetComponent<Bus>().ResetRows();


            
            //First check which level
            //Then check how many top row of each color
            //Rank by highest number & put that in bottom
            //Lowest on top
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
                    GameObject g = Instantiate(PassengerManager.Instance.GetPassenger(r.color), GetComponent<DealBus>().GetRowPos(rows.Count - 1, rows.IndexOf(r)) + new Vector3(Random.Range(-0.015f, 0.015f), 0, Random.Range(-0.015f, 0.015f)), Quaternion.identity);
                    r.AddCharacter(g.GetComponent<Character>());
                    g.transform.SetParent(transform);
                }
            }
        }
        Destroy(this);
    }
}
