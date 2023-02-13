using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BusIndicator : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] float delayTime;
    [SerializeField] float startTime;



    [Header("Component References")]
    [SerializeField] List<SpriteRenderer> bars;
    [SerializeField] GameObject barParent;

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
                bars[i].color = PassengerManager.Instance.GetColor(b.rows[i].color);
            }
        }

        if (x > 0)
        {
            for (int i = b.rows.Count; i < 5; i++)
            {
                bars[i].color = Color.white;
            }
        }
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
