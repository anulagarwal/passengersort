using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Character : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] public CharacterState state;

    [SerializeField] bool isMoving;
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
            if (agent.remainingDistance <= agent.stoppingDistance/2)
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
}
