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

    public CharacterColor GetAlternateColorBasedOnLevel( CharacterColor col)
    {
        int level = BusManager.Instance.level;
        float f = Random.Range(0, 100);

        CharacterColor c = CharacterColor.Red;
        if (f < 20)
        {
            c = col;
        }
        else
        {
            if (level < 1)
            {
                if (col == CharacterColor.Blue)
                {
                    c = CharacterColor.Red;
                }
                if (col == CharacterColor.Red)
                {
                    c = CharacterColor.Blue;
                }
            }
            else if (level >= 1)
            {
                if (col == CharacterColor.Blue)
                {
                    c = CharacterColor.Red;
                }
                if (col == CharacterColor.Red)
                {
                    c = CharacterColor.Yellow;
                }
                if (col == CharacterColor.Yellow)
                {
                    c = CharacterColor.Blue;
                }
            }
        }

        return c;
    }

    public CharacterColor GetRandomColorBasedOnLevel()
    {
        int level = BusManager.Instance.level;
        CharacterColor c = CharacterColor.Red;
        int random = 0;
        if (level < 1)
        {
            random = Random.Range(0, 2);

            if (random == 0)
            {
                c = CharacterColor.Red;
            }
            if (random == 1)
            {
                c = CharacterColor.Blue;
            }
        }
        else if (level >= 1)
        {
            random = Random.Range(0, 3);
            if (random == 0)
            {
                c = CharacterColor.Red;
            }
            if (random == 1)
            {
                c = CharacterColor.Blue;
            }
            if (random == 2)
            {
                c = CharacterColor.Yellow;
            }
        }

        return c;
    }
}
