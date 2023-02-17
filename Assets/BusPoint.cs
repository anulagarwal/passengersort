using UnityEngine;
using DG.Tweening;
using TMPro; 
public class BusPoint : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] Bus occupiedBy;
    [SerializeField] BusPointType type;
    [SerializeField] int cost = 50;

    [SerializeField] public Vector3 rotation;

    [Header("Component References")]
    [SerializeField] public Transform waypointStart;
    [SerializeField] public Transform lockImg;
    [SerializeField] public GameObject locked;
    [SerializeField] public TextMeshPro lockText;
    [SerializeField] public GameObject confetti;
    [SerializeField] public Color lockedColor;
    [SerializeField] public Color lockedExpensiveColor;
    [SerializeField] public SpriteRenderer spriteBase;
    [SerializeField] public SpriteRenderer buttonBase;


    private void Start()
    {
        UpdateState(type);
    }

    private void OnMouseDown()
    {
        if(type == BusPointType.Locked)
        {
            if (CoinManager.Instance.SubtractCoins(cost, transform.position))
            {
                Invoke("Unlock", 2f);
            }
        }
    }


    public void Reset()
    {
        occupiedBy = null;
        CoinManager.Instance.AddCoins(BusManager.Instance.coinsPerComplete, transform.position);
        
        UpdateState(BusPointType.Empty);
    }

    public void Occupy(Bus b)
    {
        occupiedBy = b;
        UpdateState(BusPointType.Occupied);

    }
    public BusPointType GetPointType()
    {
        return type;
    }

    public void UpdateState(BusPointType bpt)
    {
        type = bpt;
        switch (bpt)
        {
            case BusPointType.Locked:
                GetComponent<BoxCollider>().enabled = true;
                lockText.text = "" + cost;
                CheckForExpensive();
                break;

            case BusPointType.Empty:
                GetComponent<BoxCollider>().enabled = false;
                locked.SetActive(false);
                break;

            case BusPointType.Occupied:
                locked.SetActive(false);
                GetComponent<BoxCollider>().enabled = false;

                break;
        }
    }

    public void CheckForExpensive()
    {
        if (CoinManager.Instance.CheckForCoins(cost))
        {
            ColorIn();
            if (PlayerPrefs.GetInt("tutorial", 0) == 0 && TutorialManager.Instance.GetMoveType()== MoveType.Unlock)
            {
                TutorialManager.Instance.PlayStep(MoveType.Unlock);
            }
        }
        else
        {
            GreyOut();
        }
    }
    public void GreyOut()
    {
        spriteBase.color = Color.gray;
        buttonBase.color = Color.gray;
    }

    public void ColorIn()
    {
        spriteBase.color = new Color(spriteBase.color.r, spriteBase.color.g, spriteBase.color.b, 255);
        buttonBase.color = new Color(buttonBase.color.r, buttonBase.color.g, buttonBase.color.b, 255);
    }

    public void Unlock()
    {
        //Disable Box Collider
        locked.SetActive(false);
        UpdateState(BusPointType.Empty);
        if (PlayerPrefs.GetInt("tutorial", 0) == 0)
        {
            TutorialManager.Instance.PlayStep(MoveType.Select);
        }
        BusManager.Instance.GetComponent<BusProvider>().SendEmptyBus(this);
        BusManager.Instance.level++;
        GameManager.Instance.AddMove(1);

        CompleteBus();
    }
    public int GetCost()
    {
        return cost;
    }

    public void CompleteBus()
    {
        confetti.SetActive(true);
        Invoke("DisableConfetti",2.5f);
    }

    public void DisableConfetti()
    {
        confetti.SetActive(false);
    }

}
