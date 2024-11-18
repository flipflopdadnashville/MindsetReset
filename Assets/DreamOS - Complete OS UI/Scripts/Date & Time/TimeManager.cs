using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.DreamOS; // namespace

public class TimeManager : MonoBehaviour 
{
    public GlobalTime timeManager;

    private void Update() {
        DateTime now = DateTime.Now;
        timeManager.currentSecond = now.Second; // Change second (0-60)
        timeManager.currentMinute = now.Minute; // Change minute (0-60)
        timeManager.currentHour = now.Hour; // Change hour (0-24)
        timeManager.currentDay = now.Day; // Change day (1-30)
        timeManager.currentMonth = now.Month; // Change month (1-12)
        timeManager.currentYear = now.Year; // Change year
        SetTime(); 
    }


    void SetTime()
    {
    //    timeManager.currentSecond = 30f; // Change second (0-60)
    //    timeManager.currentMinute = 25; // Change minute (0-60)
    //    timeManager.currentHour = 10; // Change hour (0-24)
    //    timeManager.currentDay = 15; // Change day (1-30)
    //    timeManager.currentMonth = 6; // Change month (1-12)
    //    timeManager.currentYear = 2020; // Change year
    timeManager.UpdateTimeData(); // Update the stored data
    timeManager.enableAmPm = true; // Enable AM/PM label
    }
}