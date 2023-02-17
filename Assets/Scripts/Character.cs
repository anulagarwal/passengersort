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
    [SerializeField] public float remainDistance;

    [SerializeField] bool moved;

    [Header("Component References")]
    [SerializeField] public Bus b;
    [SerializeField] public MeshRenderer mesh;
    Material[] materials;




    // Start is called before the first frame update
    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        UpdateState(CharacterState.Idle);
       

    }

    public void UpdateBodyMat(Material m)
    {
        materials = mesh.materials;
        materials[1] = m;
        mesh.materials = materials;
    }

    public void UpdateSkinMat(Material m)
    {
        materials = mesh.materials;
        materials[2] = m;
        mesh.materials = materials;
    }

    public void UpdateRadius(float f)
    {
        agent.radius = f;
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

            if (b.bustype == BusType.Bus)
            {
                if (agent.remainingDistance< BusManager.Instance.minStopDistance && !isMoving)
                {
                    UpdateState(CharacterState.Idle);
                    BusManager.Instance.RemoveCharacterFromBuses(this, b);
                    b.AddCharacter(this);
                   // agent.isStopped = true;
                    UpdateAgent(5, 25);
                    b.CheckForPassengers();
                }
            }
            else
            {
                if (b.GetRowsPos().Find(x => Vector3.Distance(x, agent.transform.position) < BusManager.Instance.minStopDistance) != null && !isMoving)
                {
                    UpdateState(CharacterState.Idle);
                    BusManager.Instance.RemoveCharacterFromBuses(this, b);
                }
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

    public void UpdateAgent(float speed, float acc)
    {
        agent.acceleration = acc;
        agent.speed = speed;
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
        b = bu;
        isMoving = false;
    }   

}
