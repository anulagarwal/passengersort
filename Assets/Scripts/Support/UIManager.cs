using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;
public class UIManager : MonoBehaviour
{
    #region Properties
    public static UIManager Instance = null;

    [Header("Components Reference")]

    [SerializeField] private GameObject PointText;
    [SerializeField] private GameObject AwesomeText;
    [SerializeField] private GameObject JoyStick;  

    [Header("UI Panel")]
    [SerializeField] private GameObject mainMenuUIPanel = null;
    [SerializeField] private GameObject gameplayUIPanel = null;
    [SerializeField] private GameObject gameOverWinUIPanel = null;
    [SerializeField] private GameObject gameOverLoseUIPanel = null;
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private Text mainLevelText = null;
    [SerializeField] private Text inGameLevelText = null;
    [SerializeField] private Text winLevelText = null;
    [SerializeField] private Text loseLevelText = null;
    [SerializeField] private Text debugText = null;


    [Header("Reward/Coins")]
    [SerializeField] List<Text> allCurrentCoins = null;
    [SerializeField] Transform coinBarPos = null;
    [SerializeField] List<Transform> coins = null;


    [Header("Post Level")]
    [SerializeField] Button multiplyReward;
    [SerializeField] Text multiplyText;
    [SerializeField] Text levelReward;


    [Header("Daily")]
    [SerializeField] Button dailyReward;
    [SerializeField] Text dailyText;



    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        //SwitchControls(Controls.Touch);
    }
    #endregion

    #region Getter And Setter

    #endregion

    #region Public Core Functions
    public void SwitchUIPanel(UIPanelState state)
    {
        switch (state)
        {
            case UIPanelState.MainMenu:
                mainMenuUIPanel.SetActive(true);
                gameplayUIPanel.SetActive(false);
                gameOverWinUIPanel.SetActive(false);
                gameOverLoseUIPanel.SetActive(false);
                break;
            case UIPanelState.Gameplay:
                mainMenuUIPanel.SetActive(false);
                gameplayUIPanel.SetActive(true);
                gameOverWinUIPanel.SetActive(false);
                gameOverLoseUIPanel.SetActive(false);
                break;
            case UIPanelState.GameWin:
                mainMenuUIPanel.SetActive(false);
                gameplayUIPanel.SetActive(false);
                gameOverWinUIPanel.SetActive(true);
                gameOverLoseUIPanel.SetActive(false);
                break;
            case UIPanelState.GameLose:
                mainMenuUIPanel.SetActive(false);
                gameplayUIPanel.SetActive(false);
                gameOverWinUIPanel.SetActive(false);
                gameOverLoseUIPanel.SetActive(true);
                break;
        }
    }

    public void UpdateScore(int value)
    {
        scoreText.text = "" + value;
    }

    public void UpdateDebugText(string s)
    {
        debugText.text = s;
    }
    public void UpdateLevel(int level)
    {
        mainLevelText.text = "LEVEL " + level;
        inGameLevelText.text = "LEVEL " + level;
        winLevelText.text = "LEVEL " + level;
        loseLevelText.text = "LEVEL " + level;

    }

    public void UpdateCurrentCoins(int v)
    {
        foreach(Text t in allCurrentCoins)
        {
            t.text = v + "";
        }
    }

    public void UpdateLevelReward(int v)
    {
        levelReward.text ="+"+ v + "";
    }

    #region Give Rewards

    #endregion

    #region OnClickUIButtons    

    public void OnClickPlayButton()
    {
        GameManager.Instance.StartLevel();
    }

    public void OnClickChangeButton()
    {
        GameManager.Instance.ChangeLevel();
    }

    public void OnClickMove()
    {
        GameManager.Instance.AddMove(1);
    }

    public void OnClickWin()
    {
        GameManager.Instance.WinLevel();
    }

    public void OnClickDeal()
    {
        DealBusProvider.Instance.DealBus();
    }
    #endregion


    public void SpawnPointText(Vector3 point)
    {
        Instantiate(PointText, point, Quaternion.identity);
    }

    public void SpawnAwesomeText(Vector3 point, string s)
    {
        GameObject g = Instantiate(AwesomeText, new Vector3(point.x, 2, point.z), Quaternion.identity);
        g.GetComponentInChildren<TextMeshPro>().text = s;
    }


    #endregion

    #region Coin Pool

    public async void SendPoolTo(bool add, Vector3 worldPos)
    {
        if (add)
        {
            foreach (Transform c in coins)
            {
                c.gameObject.SetActive(true);
                c.transform.position = new Vector3(Camera.main.WorldToScreenPoint(worldPos).x + Random.Range(-50, 50), Camera.main.WorldToScreenPoint(worldPos).y + Random.Range(-50, 50));
                c.transform.localScale = Vector3.one;
                await Task.Delay(50);              
            }
        }

        else
        {
            foreach (Transform c in coins)
            {
                c.gameObject.SetActive(true);
                c.transform.position = new Vector3(coinBarPos.position.x + Random.Range(-50, 50), coinBarPos.position.y + Random.Range(-50, 50));
                c.transform.localScale = Vector3.one;
                await Task.Delay(50);

            }
        }

        if (add)
        {
            foreach (Transform c in coins)
            {
                await Task.Delay(50);
                c.transform.DOScale(Vector3.one * 0.5f, 0.5f);
                c.transform.DOMove(coinBarPos.position, 1.5f).OnComplete(() => {
                    c.gameObject.SetActive(false);
                });
            }
        }

        else
        {
            foreach (Transform c in coins)
            {
                
                await Task.Delay(50);

                c.transform.DOScale(Vector3.one * 0.75f, 0.5f);
                c.transform.DOMove(Camera.main.WorldToScreenPoint(worldPos), 1f).OnComplete(() => {
                    c.gameObject.SetActive(false);
                });
            }
        }
    }


    #endregion

    #region Button Click events


    #endregion
}




