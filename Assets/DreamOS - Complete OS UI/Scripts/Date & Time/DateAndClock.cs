using UnityEngine;
using TMPro;

namespace Michsky.DreamOS
{
    [AddComponentMenu("DreamOS/Date & Time/Date and Clock")]
    public class DateAndClock : MonoBehaviour
    {
        [Header("Resources")]
        public GlobalTime globalTimeScript;

        [Header("Settings")]
        public bool enableAmPmLabel;
        public bool addSeconds;
        public ObjectType objectType;
        [HideInInspector] public DateFormat dateFormat;

        [Header("Analog Clock")]
        [HideInInspector] public Transform clockHourHand;
        [HideInInspector] public Transform clockMinuteHand;
        [HideInInspector] public Transform clockSecondHand;

        [Header("Digital Clock")]
        [HideInInspector] public TextMeshProUGUI digitalClockText;

        [Header("Digital Date")]
        [HideInInspector] public TextMeshProUGUI digitalDateText;

        public enum ObjectType
        {
            ANALOG_CLOCK,
            DIGITAL_CLOCK,
            DIGITAL_DATE,
        }

        public enum DateFormat
        {
            DD_MM_YYYY,
            MM_DD_YYYY,
            YYYY_MM_DD
        }

        void Start()
        {
            if (globalTimeScript == null)
                globalTimeScript = GameObject.Find("Date & Time").GetComponent<GlobalTime>();
           
            if (objectType == ObjectType.DIGITAL_CLOCK && digitalClockText == null)
                digitalClockText = gameObject.GetComponent<TextMeshProUGUI>();
            else if (objectType == ObjectType.DIGITAL_DATE && digitalDateText == null)
                digitalDateText = gameObject.GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            if (objectType == ObjectType.ANALOG_CLOCK)
                AnalogClock();
            else if (objectType == ObjectType.DIGITAL_CLOCK)
                DigitalClock();
            else if (objectType == ObjectType.DIGITAL_DATE)
                DigitalDate();
        }

        public void AnalogClock()
        {
            clockHourHand.localRotation = Quaternion.Euler(0, 0, globalTimeScript.currentHour * -15 * 2);
            clockMinuteHand.localRotation = Quaternion.Euler(0, 0, globalTimeScript.currentMinute * -6);
            
            if(addSeconds == true)
            {
                clockSecondHand.localRotation = Quaternion.Euler(0, 0, globalTimeScript.currentSecond * -6);
            }
        }

        public void DigitalClock()
        {
            if (globalTimeScript.enableAmPm == false)
            {
                if (globalTimeScript.currentMinute.ToString().Length == 1)
                    digitalClockText.text = globalTimeScript.currentHour + ":" + "0" + globalTimeScript.currentMinute;
                else
                    digitalClockText.text = globalTimeScript.currentHour + ":" + globalTimeScript.currentMinute;
            }

            else
            {
                if (globalTimeScript.currentMinute.ToString().Length == 1)
                    digitalClockText.text = globalTimeScript.currentHour + ":" + "0" + globalTimeScript.currentMinute;
                else
                    digitalClockText.text = globalTimeScript.currentHour + ":" + globalTimeScript.currentMinute;

                if (addSeconds == true)
                    digitalClockText.text = digitalClockText.text + ":" + globalTimeScript.currentSecond.ToString("00");

                if (globalTimeScript.enableAmPm == true && PlayerPrefs.GetInt("isAM") == 1 && enableAmPmLabel == true)
                    digitalClockText.text = digitalClockText.text + " AM";
                else if (globalTimeScript.enableAmPm == true && PlayerPrefs.GetInt("isAM") == 0 && enableAmPmLabel == true)
                    digitalClockText.text = digitalClockText.text + " PM";

            }
        }

        public void DigitalDate()
        {
            if (dateFormat == DateFormat.DD_MM_YYYY)
                digitalDateText.text = globalTimeScript.currentDay + "."
                    + globalTimeScript.currentMonth + "."
                    + globalTimeScript.currentYear;
            
            else if (dateFormat == DateFormat.MM_DD_YYYY)
                digitalDateText.text = globalTimeScript.currentMonth + "."
                    + globalTimeScript.currentDay 
                    + "." + globalTimeScript.currentYear;
            
            else if (dateFormat == DateFormat.YYYY_MM_DD)
                digitalDateText.text = globalTimeScript.currentYear + "-"
                    + globalTimeScript.currentMonth + "-"
                    + globalTimeScript.currentDay;
        }
    }
}