using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] int maxSteps;
    [SerializeField] int currentIndex;
    [SerializeField] string stepBaseName;
    [SerializeField] bool isOver;



    [Header("Component References")]
    [SerializeField] Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayStep()
    {
        if (!isOver)
        {
            anim.Play(stepBaseName + currentIndex);
            currentIndex++;
            if (currentIndex >= maxSteps)
            {
                isOver = true;
            }
        }
    }
}
