using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Bus : MonoBehaviour
{
    [SerializeField] public List<NavMeshAgent> agents;
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void MoveCharactersTo(Bus b)
    {
        foreach (NavMeshAgent a in agents)
        {
            a.SetDestination(b.transform.position);
            b.AddCharacter(a);
        }
        agents.Clear();
    }

    public void AddCharacter(NavMeshAgent n)
    {
        agents.Add(n);        
    }
    public void OnMouseUp()
    {
        if (BusManager.Instance.selectedBus == null)
        {
            BusManager.Instance.SelectBus(this);
        }
        else if (BusManager.Instance.selectedBus != null && BusManager.Instance.enteredBus == null)
        {
            BusManager.Instance.selectedBus.MoveCharactersTo(this);
            BusManager.Instance.ResetSelections();
        }
    }
}
