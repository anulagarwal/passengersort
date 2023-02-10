using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Character : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] public CharacterState state;

    [SerializeField] public bool isMoving;
    [SerializeField] bool moved;

    [Header("Component References")]
    [SerializeField] public Bus b;




    // Start is called before the first frame update
    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        UpdateState(CharacterState.Idle);

    }


    private void Update()
    {
        if (state == CharacterState.Moving && agent.enabled)
        {
            if (agent.remainingDistance <= BusManager.Instance.minStopDistance && !isMoving)
            {
                //isMoving = false;
               // UpdateState(CharacterState.Idle);
              //  b.CheckForPassengers();
            }

            if(b.GetRowsPos().Find(x=> Vector3.Distance(x, agent.transform.position) < BusManager.Instance.minStopDistance) != null && !isMoving)
            {
                UpdateState(CharacterState.Idle);
                b.CheckForPassengers();
            }
        }
    }
    public void MoveTo(Vector3 pos, Transform bus)
    {
        agent.SetDestination(pos);
        transform.SetParent(bus);
        b = bus.GetComponent<Bus>();
        UpdateState(CharacterState.Moving);
        isMoving = true;
    }

    public void DisableAgent()
    {
        agent.enabled = false;
    }

    public void EnableAgent()
    {
        agent.enabled = true;
    }

    public void UpdateState(CharacterState cs)
    {
        state = cs;
    }
    public void EnterBus(Bus bu)
    {
       // b = bu;
        isMoving = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bus")
        {
            if (other.gameObject.GetComponent<EnterTrigger>().b == b)
            {
              //  isMoving = false;
            }
        }
    }


}
