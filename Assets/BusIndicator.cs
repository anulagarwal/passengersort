using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
public class BusIndicator : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] float delayTime;
    [SerializeField] float startTime;



    [Header("Component References")]
    [SerializeField] List<SpriteRenderer> bars;
    [SerializeField] GameObject barParent;
    [SerializeField] List<Image> barFills;


    // Start is called before the first frame update
    void Start()
    {
        //EnableBar(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (barParent.activeSelf)
        {
            if(startTime + delayTime < Time.time)
            {
                DisableBar();
            }
        }
    }


    public void ColorBars(Bus b)
    {

        int x = 5 - b.rows.Count;
         if (b.rows.Count > 0)
         {
             for (int i = 0; i < b.rows.Count; i++)
             {
                barFills[i].color = PassengerManager.Instance.GetColor(b.rows[i].color);
                float f = ((float)b.charactersList.Count - ((float)(i)* BusManager.Instance.maxCharacterPerRow))/(float)BusManager.Instance.maxCharacterPerRow;
                if (f > 0.9f)
                {
                    f = 1;
                }
                barFills[i].DOFillAmount(f, 0.5f);
             }

         }
        if (x > 0)
        {
            for (int i = Mathf.CeilToInt((float)b.charactersList.Count / (float)BusManager.Instance.maxCharacterPerRow); i < 5; i++)
            {
                barFills[i].color = Color.white;
            }
        }
        if (b.charactersList.Count > 0)
        {
            for (int i = 0; i <= Mathf.FloorToInt(b.charactersList.Count / BusManager.Instance.maxCharacterPerRow); i++)
            {
               // barFills[i].color = PassengerManager.Instance.GetColor(b.rows[i].color);
                float f = ((float)b.charactersList.Count - ((float)(i) * BusManager.Instance.maxCharacterPerRow)) / (float)BusManager.Instance.maxCharacterPerRow;
                if (f<0.15f)
                {
                    f = 0;
                }
                barFills[i].DOFillAmount(f, 0.5f);
            }
        }

         

        
       

        
           /* print(Mathf.FloorToInt(b.charactersList.Count / BusManager.Instance.maxCharacterPerRow));
            for(int i = 0; i< Mathf.FloorToInt(b.charactersList.Count / BusManager.Instance.maxCharacterPerRow); i++)
            {
                barFills[i].color = PassengerManager.Instance.GetColor(b.rows[i].color);
                float f = ((float)b.charactersList.Count / (float)BusManager.Instance.maxCharacterPerRow);
                barFills[i].DOFillAmount(f, 0.5f);
            }
            if (b.charactersList.Count % BusManager.Instance.maxCharacterPerRow > 0) 
            {
                barFills[Mathf.FloorToInt(b.charactersList.Count / BusManager.Instance.maxCharacterPerRow)].color = PassengerManager.Instance.GetColor(b.rows[Mathf.FloorToInt(b.charactersList.Count / BusManager.Instance.maxCharacterPerRow) - 1].color);
                float f = ((float)b.charactersList.Count % (float)BusManager.Instance.maxCharacterPerRow) / (float)BusManager.Instance.maxCharacterPerRow;
                if (f > 0.9f)
                {
                    barFills[Mathf.FloorToInt(b.charactersList.Count / BusManager.Instance.maxCharacterPerRow)].DOFillAmount(1, 0.5f);
                }
                else
                {
                    barFills[Mathf.FloorToInt(b.charactersList.Count / BusManager.Instance.maxCharacterPerRow)].DOFillAmount(f, 0.5f);
                }
            }
        */
        barParent.SetActive(true);
        startTime = Time.time;
    }

    public void EnableBar(bool active)
    {
        if (!active)
        {
            Invoke("DisableBar", 8f);
        }
        else
        {
            barParent.SetActive(active);
           // barParent.transform.DOScale(Vector3.one, 0.5f);

        }
    }

    public void DisableBar()
    {
       barParent.SetActive(false);
      //  barParent.transform.DOScale(Vector3.zero, 0.5f);
    }
}
