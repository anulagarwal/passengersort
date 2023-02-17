using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] int startCoins;
    [SerializeField] int currentCoins;

    [Header("Rewards")]
    [SerializeField] int levelReward;



    public static CoinManager Instance = null;


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
        startCoins = PlayerPrefs.GetInt("coins", startCoins);
        currentCoins = startCoins;
        UpdateEconomyElements();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Level Rewards

    public void AddToLevelReward(int v)
    {
        levelReward += v;
        UIManager.Instance.UpdateLevelReward(levelReward);
    }

    public void MultiplyLevelReward(int v)
    {
        levelReward *= v;
        UIManager.Instance.UpdateLevelReward(levelReward);
    }

    public bool CheckForCoins(int v)
    {
        if (v >= currentCoins)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    #endregion

    #region Coin Getter Setter
    public void AddCoins(int v, Vector3 worldPos)
    {
        currentCoins += v;
        PlayerPrefs.SetInt("coins", currentCoins);
        UIManager.Instance.UpdateCurrentCoins(currentCoins);
        UIManager.Instance.SendPoolTo(true, worldPos);
        UpdateEconomyElements();

    }


    public bool SubtractCoins(int v, Vector3 worldPos)
    {
        if (currentCoins - v > 0)
        {
            currentCoins -= v;
            PlayerPrefs.SetInt("coins", currentCoins);
            UIManager.Instance.SendPoolTo(false, worldPos);
            UpdateEconomyElements();
            return true;
        }
        else
        {
            return false;
        }

    }
    public bool SubtractCoins(int v)
    {
        if (currentCoins - v > 0)
        {
            currentCoins -= v;
            PlayerPrefs.SetInt("coins", currentCoins);
            UpdateEconomyElements();
            return true;
        }
        else
        {
            return false;
        }

    }


    public void UpdateEconomyElements()
    {
        UIManager.Instance.UpdateCurrentCoins(currentCoins);
        BusManager.Instance.UpdateBusPoints();
        if (currentCoins >= PowerupManager.Instance.GetPowerupCost(PowerupType.Deal))
        {
            UIManager.Instance.UpdatePowerupButton(true, PowerupManager.Instance.GetPowerupCost(PowerupType.Deal));
            if ( PlayerPrefs.GetInt("tutorial",0)==0&& TutorialManager.Instance.GetMoveType() == MoveType.Deal)
            {
                TutorialManager.Instance.PlayStep(MoveType.Deal);
            }
        }
        else
        {
            UIManager.Instance.UpdatePowerupButton(false, PowerupManager.Instance.GetPowerupCost(PowerupType.Deal));
        }
    }
    #endregion

}
