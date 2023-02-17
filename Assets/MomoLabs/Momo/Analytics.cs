using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
using Facebook.Unity;
using LionStudios.Suite.Analytics;
namespace Momo
{
    
    public class Analytics : MonoBehaviour
    {
        
        [SerializeField] int appSessionCount;
        [SerializeField] int dayNumber;
        [SerializeField] int level;
        [SerializeField] float sessionTime;
        int lastLevel; 

        bool isPaused;
        Dictionary<string, string> fields = new Dictionary<string, string>();

        public static Analytics Instance = null;

        // Start is called before the first frame update
        void Awake()
        {

            GameObject[] objs = GameObject.FindGameObjectsWithTag("Analytics");

            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }

            Application.targetFrameRate = 100;
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            Instance = this;

            DontDestroyOnLoad(this.gameObject);           
        }

        private void Start()
        {

            //Check if app opened on a new day - check when was last time opened
            //If opened on a new day, track day and level number

            appSessionCount = PlayerPrefs.GetInt("appSession", 0);
            level = PlayerPrefs.GetInt("level", 1);

            MaxSdk.SetSdkKey("TMz8cpx6TOmmFb5Krb8TvSP3p1yx_iTxJeBg0OwWbTrb5iT6RPm0vAzF5dcp6ARaCGl0TEZyMb4UQQASIewAQW");
            MaxSdk.SetUserId(SystemInfo.deviceUniqueIdentifier);
            MaxSdk.SetVerboseLogging(true);
            MaxSdk.InitializeSdk();
            //  GameAnalytics.NewDesignEvent("session", appSessionCount);
            GameAnalytics.Initialize();            
            LionAnalytics.GameStart();
            dayNumber = PlayerPrefs.GetInt("dayNumber", 0);

            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }

        }

        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }
        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }
        // Update is called once per frame
        void Update()
        {

        }


        #region App Level

        private void OnApplicationPause(bool pause)
        {
            isPaused = true;
          //  GameAnalytics.NewDesignEvent("sessionEnd", level);

            //Track session End with level number
        }

        private void OnApplicationFocus(bool focus)
        {
            if(focus && isPaused)
            {
                appSessionCount += 1;
                PlayerPrefs.SetInt("appSession", appSessionCount);
           //     GameAnalytics.NewDesignEvent("session", appSessionCount);
                isPaused = false;

                //Track session Start with level number
            }
        }

        #endregion


        #region Level Trackers

        public void StartLevel(int levelNumber)
        {
         //   TinySauce.OnGameStarted(levelNumber + "");
            level = levelNumber;
        }

        public void WinLevel()
        {
        //    TinySauce.OnGameFinished(true,0);
        }

        public void LoseLevel()
        {
         //   TinySauce.OnGameFinished(false, 0);           
        }

        public void TrackBusComplete(int id)
        {



        }

        public void TrackBonusBusNew(int id)
        {

        }

        public void TrackBonusBusComplete(int id)
        {

        }

        public void TrackUnlockSpace(int id)
        {

        }

        public void TrackDeal(int id)
        {

        }
        public void TrackSession(SessionData sd)
        {
            Dictionary<string, object> s = new Dictionary<string, object>();
            s.Add("sessionNumber", sd.sessionNumber);
            s.Add("sessionLength", sd.sessionLength);
            s.Add("sessionLevel", sd.lastLevel);
        //    TinySauce.TrackCustomEvent("session", s);


        }

        public void TrackLevel(PlayerLevelData ld)
        {
            Dictionary<string, object> l = new Dictionary<string, object>();
            l.Add("levelNumber", ld.levelNumber);
            l.Add("moves", ld.numberOfMoves);
            l.Add("time", ld.timeSpent);
         //   TinySauce.TrackCustomEvent("level", l) ;

          
        }

        public void TrackDay(DayData dd)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("dayNumber", dd.DayNumber);
            d.Add("numberOfSessions", dd.numberOfSessions);
           // TinySauce.TrackCustomEvent("level", d);

         

         //   TinySauce.TrackCustomEvent("level", d);
        }
        #endregion
        //Register daily logins
        //Register session times
        //Register last level left
        //FB/GA Login Events (Use Tiny Sauce)

    }
}
