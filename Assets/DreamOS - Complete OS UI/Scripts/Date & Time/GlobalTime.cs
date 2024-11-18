using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Michsky.DreamOS
{
    public class GlobalTime : MonoBehaviour
    {
        // Time variables
        [Range(0.1f, 10000)] public float timeMultiplier = 10;
        [Range(1, 30)] public int currentDay = 1;
        [Range(1, 12)] public int currentMonth = 1;
        public int currentYear = 2019;
        [Range(0, 24)] public int currentHour;
        [Range(0, 60)] public int currentMinute;
        [Range(0, 60)] public float currentSecond;

        // Settings
        public bool saveAndGetValues = true;
        public bool enableAmPm = true;
        public bool enableTimedEvents = true;

        // Even
        public bool deleteEventDataAtStart;
        public List<TimedEvent> timedEvents = new List<TimedEvent>();

        public float rotationDegreesPerDay = 360;
        public float hoursPerDay = 24;
        public float minutesPerHour = 60;
        public float secondsPerMinute = 60;
        public float dayNormalizedShared;
        bool isAm;

        [System.Serializable]
        public class TimedEvent
        {
            public string eventTitle;
            public TimedEventType timedEventType;
            [Range(0, 24)] public int eventHour;
            [Range(0, 60)] public int eventMinute;
            [Range(1, 30)] public int eventDay = 1;
            [Range(1, 12)] public int eventMonth = 1;
            public int eventYear = 2019;
            public UnityEvent invokeEvents = new UnityEvent();
            [HideInInspector] public int itemID = 0;
        }

        public enum TimedEventType
        {
            ONCE,
            DAILY,
            WEEKLY,
            MONTHLY,
            YEARLY
        }

        void Start()
        {
            if (saveAndGetValues == true && PlayerPrefs.GetInt("TimeInitalized") == 1)
            {
                currentMinute = PlayerPrefs.GetInt("CurrentMinute");
                currentHour = PlayerPrefs.GetInt("CurrentHour");
                currentDay = PlayerPrefs.GetInt("CurrentDay");
                currentMonth = PlayerPrefs.GetInt("CurrentMonth");
                currentYear = PlayerPrefs.GetInt("CurrentYear");

                if (PlayerPrefs.GetInt("isAM") == 1)
                    isAm = true;
                else
                    isAm = false;

                // Debug.Log("<b>[Time Information]</b> Current minute: " + currentMinute + " - Current hour: " + currentHour);
                // Debug.Log("<b>[Date Information]</b> Current day: " + currentDay + " - Current month: " + currentMonth + " - Current year: " + currentYear);
            }

            else
                UpdateTimeData();

            if (saveAndGetValues == false && currentHour <= 12)
            {
                PlayerPrefs.SetInt("isAM", 1);
                isAm = true;
            }

            else if (saveAndGetValues == false && currentHour == 12 || currentHour >= 12)
            {
                PlayerPrefs.SetInt("isAM", 0);
                isAm = false;
            }

            for (int i = 0; i < timedEvents.Count; ++i)
            {
                if (deleteEventDataAtStart == true)
                    PlayerPrefs.DeleteKey("CompletedTimedEvent" + timedEvents[i]);
            }

            if (currentDay == 0)
                currentDay = 1;

            PlayerPrefs.SetInt("TimeInitalized", 1);
        }

        void LateUpdate()
        {
            currentSecond += Time.deltaTime * timeMultiplier;

            if (currentSecond >= 59)
            {
                currentSecond = 0;
                currentMinute += 1;
                PlayerPrefs.SetInt("CurrentMinute", currentMinute);

                if (currentMinute >= 60)
                {
                    currentHour += 1;
                    currentMinute = 0;
                    PlayerPrefs.SetInt("CurrentMinute", currentMinute);
                    PlayerPrefs.SetInt("CurrentHour", currentHour);

                    if (enableAmPm == false && currentHour == 24)
                    {
                        currentDay += 1;
                        currentHour = 0;

                        if (currentDay == 0)
                            currentDay = 1;

                        PlayerPrefs.SetInt("CurrentDay", currentDay);
                        PlayerPrefs.SetInt("CurrentHour", currentHour);
                    }

                    else if (enableAmPm == true && isAm == false && currentHour == 13)
                    {
                        PlayerPrefs.SetInt("isAM", 1);
                        isAm = true;
                        currentDay += 1;

                        if (currentDay == 0)
                            currentDay = 1;

                        currentHour = 1;
                        PlayerPrefs.SetInt("CurrentDay", currentDay);
                    }

                    else if (enableAmPm == true && isAm == true && currentHour == 13)
                    {
                        PlayerPrefs.SetInt("isAM", 0);
                        isAm = false;
                        currentHour = 1;
                    }

                    if (currentDay == 31)
                    {
                        currentDay = 1;
                        currentMonth += 1;

                        if (currentDay == 0)
                            currentDay = 1;

                        if (currentMonth == 13)
                        {
                            currentMonth = 1;
                            currentYear += 1;
                            PlayerPrefs.SetInt("CurrentYear", currentYear);
                        }

                        PlayerPrefs.SetInt("CurrentDay", currentDay);
                        PlayerPrefs.SetInt("CurrentMonth", currentMonth);
                    }
                }

                if (enableTimedEvents == true)
                    CheckForTimedEvents();
            }
        }

        public void CheckForTimedEvents()
        {
            for (int i = 0; i < timedEvents.Count; ++i)
            {
                if (PlayerPrefs.GetString("Reminder" + timedEvents[i].itemID + "Enabled") == "true"
                    && timedEvents[i].eventHour == currentHour && timedEvents[i].eventMinute == currentMinute
                    && timedEvents[i].eventDay == currentDay && timedEvents[i].eventMonth == currentMonth
                    && timedEvents[i].eventYear == currentYear)
                {
                    timedEvents[i].invokeEvents.Invoke();
                    Debug.Log("<b>[Global Time]</b> It's time for an event: <b>" + timedEvents[i].eventTitle + "</b>");

                    if (timedEvents[i].timedEventType == TimedEventType.ONCE)
                        timedEvents.Remove(timedEvents[i]);

                    else if (timedEvents[i].timedEventType == TimedEventType.DAILY)
                    {
                        timedEvents[i].eventDay = currentDay + 1;

                        if (timedEvents[i].eventDay >= 30)
                            timedEvents[i].eventDay = 0;
                    }

                    else if (timedEvents[i].timedEventType == TimedEventType.WEEKLY)
                    {
                        timedEvents[i].eventDay += 7;

                        if (timedEvents[i].eventDay >= 30)
                        {
                            timedEvents[i].eventDay -= 30;
                            timedEvents[i].eventMonth += 1;
                           
                            if (timedEvents[i].eventMonth >= 12)
                                timedEvents[i].eventYear += 1;
                        }
                    }

                    else if (timedEvents[i].timedEventType == TimedEventType.MONTHLY)
                    {
                        timedEvents[i].eventMonth += 1;

                        if (timedEvents[i].eventMonth >= 12)
                        {
                            timedEvents[i].eventMonth = 0;
                            timedEvents[i].eventYear += 1;
                        }
                    }

                    else if (timedEvents[i].timedEventType == TimedEventType.YEARLY)
                        timedEvents[i].eventYear += 1;
                }
            }
        }

        public void UpdateTimeData()
        {
            PlayerPrefs.SetFloat("CurrentSecond", currentSecond);
            PlayerPrefs.SetInt("CurrentMinute", currentMinute);
            PlayerPrefs.SetInt("CurrentHour", currentHour);
            PlayerPrefs.SetInt("CurrentMonth", currentMonth);
            PlayerPrefs.SetInt("CurrentYear", currentYear);
        }

        public void AddTimedEvent()
        {
            TimedEvent titem = new TimedEvent();
            titem.eventTitle = "New Event";
            timedEvents.Add(titem);
        }
    }
}