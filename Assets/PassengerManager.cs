using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassengerObjects
{
    public CharacterColor color;
    public GameObject obj;
}
public class PassengerManager : MonoBehaviour
{

    [Header("Component References")]
    [SerializeField] List<PassengerObjects> passengers;

    public static PassengerManager Instance = null;


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

    public GameObject GetPassenger(CharacterColor col)
    {
       return passengers.Find(x => x.color == col).obj;            
    }
}
