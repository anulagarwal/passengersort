using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusManager : MonoBehaviour
{

    [SerializeField] List<Bus> buses;
    public Bus selectedBus;
    public Bus enteredBus;



    public static BusManager Instance = null;


    
    private void Awake()
    {
        Application.targetFrameRate = 100;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectBus(Bus b)
    {
        selectedBus = b;
    }


    public void EnterBus(Bus b)
    {
        enteredBus = b;
    }
    public void ResetSelections()
    {
        selectedBus = null;
        enteredBus = null;
    }
}
