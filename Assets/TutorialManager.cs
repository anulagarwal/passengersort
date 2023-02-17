using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] int maxSteps;
    [SerializeField] public int currentIndex;
    [SerializeField] string stepBaseName;
    [SerializeField] bool isOver = false;
    [SerializeField] List<MoveType> moves;
    [SerializeField] List<float> interval;
    [SerializeField] List<string> texts;
    [SerializeField] bool tutorial;



    [Header("Component References")]
    [SerializeField] Animator anim;
    [SerializeField] Transform finger;

    public static TutorialManager Instance = null;


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
        
        if (PlayerPrefs.GetInt("tutorial", 0) == 1 || !tutorial)
        {
            UIManager.Instance.ActiveMask(false);

            DisableT();
            Destroy(this);
        }
        else{
            UIManager.Instance.ActiveMask(true);
            UpdateText(currentIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isOver)
        UIManager.Instance.UpdateMaskPosition(finger.position);
    }

    public void PlayStep(MoveType m)
    {
        
        finger.gameObject.SetActive(false);
        UIManager.Instance.ActiveMask(false);
        if (m == moves[currentIndex-1])
        {            
            Invoke("NextStep", interval[currentIndex-1]);
        }
    }
    public void UpdateText(int index)
    {
        UIManager.Instance.UpdateTutorialText(texts[index-2]);
    }

    public MoveType GetMoveType()
    {        
       return moves[currentIndex-1];        
    }
    public void NextStep()
    {
        if (!isOver)
        {
            if (currentIndex > maxSteps)
            {
                DisableT();
            }
            else
            {
                UIManager.Instance.ActiveMask(true);

                finger.gameObject.SetActive(true);
                currentIndex++;
                UpdateText(currentIndex);

                anim.Play(stepBaseName + (currentIndex-1));
            }

            if (currentIndex >= maxSteps)
            {
                Invoke("DisableT", interval[currentIndex-1]);
            }

        }
    }
    public void DisableT()
    {
        isOver = true;
        UIManager.Instance.ActiveMask(false);
        PlayerPrefs.SetInt("tutorial", 1);
        anim.StopPlayback();
        anim.gameObject.SetActive(false);
        BusManager.Instance.GetComponent<BonusBusProvider>().enabled = true;
        Destroy(this, 3);
    }
    
}
