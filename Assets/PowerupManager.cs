using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Powerup
{
    public List<int> cost;
    public PowerupType powerup;
}
public class PowerupManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] List<Powerup> powerup;

    public static PowerupManager Instance = null;


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


    public int GetPowerupCost(PowerupType pt)
    {
        return powerup.Find(x => x.powerup == pt).cost[0];
    }
}
