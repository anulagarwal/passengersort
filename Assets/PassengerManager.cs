using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassengerObjects
{
    public CharacterColor color;
    public GameObject obj;
    public Material skinOriginal;
    public Material bodyOriginal;
    public Material bodyHighlight;
    public Material skinHighlight;
    public Color col;

}

public class PassengerManager : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] List<PassengerObjects> passengers;
    [SerializeField] public List<float> radius;


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


    public Material GetOriginalSkin(CharacterColor col)
    {
        return passengers.Find(x => x.color == col).skinOriginal;
    }
    public Material GetHighlightSkin(CharacterColor col)
    {
        return passengers.Find(x => x.color == col).skinHighlight;
    }

    public Material GetOriginalBody(CharacterColor col)
    {
        return passengers.Find(x => x.color == col).bodyOriginal;
    }
    public Material GetHighlightBody(CharacterColor col)
    {
        return passengers.Find(x => x.color == col).bodyHighlight;
    }

    public Color GetColor(CharacterColor col)
    {
        return passengers.Find(x => x.color == col).col;
    }
}
