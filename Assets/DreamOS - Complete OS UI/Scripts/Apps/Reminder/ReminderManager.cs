using UnityEngine;
using TMPro;

namespace Michsky.DreamOS
{
    public class ReminderManager : MonoBehaviour
    {
        // Resources
        public GlobalTime globalTime;
        public TMP_InputField eventTitleObject;
        public TextMeshProUGUI eventHourObject;
        public TextMeshProUGUI eventMinuteObject;
        public HorizontalSelector hourSelector;
        public HorizontalSelector minuteSelector;
        public NotificationCreator reminderNotification;
        public GameObject reminder1;
        public GameObject reminder2;
        public GameObject reminder3;
        public GameObject reminder4;

        // Settings
        public string notificationTitle = "Heads up!";

        ReminderEventType reminderEventType;

        public enum ReminderEventType
        {
            ONCE,
            DAILY,
            WEEKLY,
            MONTHLY,
            YEARLY
        }

        void Start()
        {
            InitializeReminders();

            // If hour or minute selector is null, don't go further
            if (hourSelector == null || minuteSelector == null)
                return;

            // Add hour selector items
            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                    hourSelector.CreateNewItem("0" + i.ToString());
                else
                    hourSelector.CreateNewItem(i.ToString());
            }

            // Add minute selector items
            for (int i = 0; i < 60; i++)
            {
                if (i < 10)
                    minuteSelector.CreateNewItem("0" + i.ToString());
                else
                    minuteSelector.CreateNewItem(i.ToString());
            }
        }

        public void InitializeReminders()
        {
            // If reminder1 is assigned
            if (reminder1 != null)
            {
                // Get the component
                ReminderItem ritemObj = reminder1.GetComponent<ReminderItem>();

                // Check the data and set it deafults if it doesn't have any data
                if (!PlayerPrefs.HasKey("Reminder1" + "Title"))
                    ritemObj.SetDefaults();

                // Change the name and title
                reminder1.name = PlayerPrefs.GetString("Reminder1" + "Title");
                TextMeshProUGUI titleObj = reminder1.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                titleObj.text = reminder1.name;

                // Change reminder date
                TextMeshProUGUI dateObj = reminder1.transform.Find("Date").GetComponent<TextMeshProUGUI>();
                dateObj.text = PlayerPrefs.GetString("Reminder1" + "Hour")
                    + ":" + PlayerPrefs.GetString("Reminder1" + "Minute")
                    + "\n" + PlayerPrefs.GetString("Reminder1" + "Type");

                // Add reminder to global time as a timed event      
                GlobalTime.TimedEvent ritem = new GlobalTime.TimedEvent();
                ritem.eventTitle = reminder1.name;
                ritem.eventHour = int.Parse(PlayerPrefs.GetString("Reminder1" + "Hour"));
                ritem.eventMinute = int.Parse(PlayerPrefs.GetString("Reminder1" + "Minute"));
                ritem.eventDay = globalTime.currentDay;
                ritem.eventMonth = globalTime.currentMonth;
                ritem.eventYear = globalTime.currentYear;
                ritem.itemID = 1;

                // Fetch the event type data
                if (PlayerPrefs.GetString("Reminder1" + "Type") == "Once")
                    ritem.timedEventType = GlobalTime.TimedEventType.ONCE;
                else if (PlayerPrefs.GetString("Reminder1" + "Type") == "Daily")
                    ritem.timedEventType = GlobalTime.TimedEventType.DAILY;
                else if (PlayerPrefs.GetString("Reminder1" + "Type") == "Weekly")
                    ritem.timedEventType = GlobalTime.TimedEventType.WEEKLY;
                else if (PlayerPrefs.GetString("Reminder1" + "Type") == "Monthly")
                    ritem.timedEventType = GlobalTime.TimedEventType.MONTHLY;
                else if (PlayerPrefs.GetString("Reminder1" + "Type") == "Yearly")
                    ritem.timedEventType = GlobalTime.TimedEventType.YEARLY;

                // Add events to timed event
                ritem.invokeEvents.AddListener(delegate
                {
                    reminderNotification.notificationTitle = notificationTitle;
                    reminderNotification.popupDescription = ritem.eventTitle;
                    reminderNotification.CreateOnlyPopup();

                    if (ritem.timedEventType == GlobalTime.TimedEventType.ONCE)
                        ritemObj.DisableReminder();
                });

                // Add the item to global time events and update the switch
                globalTime.timedEvents.Add(ritem);
                ritemObj.switchManager.UpdateUI();
            }

            // If reminder2 is assigned
            if (reminder2 != null)
            {
                // Get the component
                ReminderItem ritemObj = reminder2.GetComponent<ReminderItem>();

                // Check the data and set it deafults if it doesn't have any data
                if (!PlayerPrefs.HasKey("Reminder2" + "Title"))
                    ritemObj.SetDefaults();

                // Change the name and title
                reminder2.name = PlayerPrefs.GetString("Reminder2" + "Title");
                TextMeshProUGUI titleObj = reminder2.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                titleObj.text = reminder2.name;

                // Change reminder date
                TextMeshProUGUI dateObj = reminder2.transform.Find("Date").GetComponent<TextMeshProUGUI>();
                dateObj.text = PlayerPrefs.GetString("Reminder2" + "Hour")
                    + ":" + PlayerPrefs.GetString("Reminder2" + "Minute")
                    + "\n" + PlayerPrefs.GetString("Reminder2" + "Type");

                // Add reminder to global time as a timed event
                GlobalTime.TimedEvent ritem = new GlobalTime.TimedEvent();
                ritem.eventTitle = reminder2.name;
                ritem.eventHour = int.Parse(PlayerPrefs.GetString("Reminder2" + "Hour"));
                ritem.eventMinute = int.Parse(PlayerPrefs.GetString("Reminder2" + "Minute"));
                ritem.eventDay = globalTime.currentDay;
                ritem.eventMonth = globalTime.currentMonth;
                ritem.eventYear = globalTime.currentYear;
                ritem.itemID = 2;

                // Fetch the event type data
                if (PlayerPrefs.GetString("Reminder2" + "Type") == "Once")
                    ritem.timedEventType = GlobalTime.TimedEventType.ONCE;
                else if (PlayerPrefs.GetString("Reminder2" + "Type") == "Daily")
                    ritem.timedEventType = GlobalTime.TimedEventType.DAILY;
                else if (PlayerPrefs.GetString("Reminder2" + "Type") == "Weekly")
                    ritem.timedEventType = GlobalTime.TimedEventType.WEEKLY;
                else if (PlayerPrefs.GetString("Reminder2" + "Type") == "Monthly")
                    ritem.timedEventType = GlobalTime.TimedEventType.MONTHLY;
                else if (PlayerPrefs.GetString("Reminder2" + "Type") == "Yearly")
                    ritem.timedEventType = GlobalTime.TimedEventType.YEARLY;

                // Add events to timed event
                ritem.invokeEvents.AddListener(delegate
                {
                    reminderNotification.notificationTitle = notificationTitle;
                    reminderNotification.popupDescription = ritem.eventTitle;
                    reminderNotification.CreateOnlyPopup();
                });

                // Add the item to global time events and update the switch
                globalTime.timedEvents.Add(ritem);
                ritemObj.switchManager.UpdateUI();
            }

            // If reminder3 is assigned
            if (reminder3 != null)
            {
                // Get the component
                ReminderItem ritemObj = reminder3.GetComponent<ReminderItem>();

                // Check the data and set it deafults if it doesn't have any data
                if (!PlayerPrefs.HasKey("Reminder3" + "Title"))
                    ritemObj.SetDefaults();

                // Change the name and title
                reminder3.name = PlayerPrefs.GetString("Reminder3" + "Title");
                TextMeshProUGUI titleObj = reminder3.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                titleObj.text = reminder3.name;

                // Change reminder date
                TextMeshProUGUI dateObj = reminder3.transform.Find("Date").GetComponent<TextMeshProUGUI>();
                dateObj.text = PlayerPrefs.GetString("Reminder3" + "Hour")
                    + ":" + PlayerPrefs.GetString("Reminder3" + "Minute")
                    + "\n" + PlayerPrefs.GetString("Reminder3" + "Type");

                // Add reminder to global time as a timed event
                GlobalTime.TimedEvent ritem = new GlobalTime.TimedEvent();
                ritem.eventTitle = reminder3.name;
                ritem.eventHour = int.Parse(PlayerPrefs.GetString("Reminder3" + "Hour"));
                ritem.eventMinute = int.Parse(PlayerPrefs.GetString("Reminder3" + "Minute"));
                ritem.eventDay = globalTime.currentDay;
                ritem.eventMonth = globalTime.currentMonth;
                ritem.eventYear = globalTime.currentYear;
                ritem.itemID = 3;

                // Fetch the event type data
                if (PlayerPrefs.GetString("Reminder3" + "Type") == "Once")
                    ritem.timedEventType = GlobalTime.TimedEventType.ONCE;
                else if (PlayerPrefs.GetString("Reminder3" + "Type") == "Daily")
                    ritem.timedEventType = GlobalTime.TimedEventType.DAILY;
                else if (PlayerPrefs.GetString("Reminder3" + "Type") == "Weekly")
                    ritem.timedEventType = GlobalTime.TimedEventType.WEEKLY;
                else if (PlayerPrefs.GetString("Reminder3" + "Type") == "Monthly")
                    ritem.timedEventType = GlobalTime.TimedEventType.MONTHLY;
                else if (PlayerPrefs.GetString("Reminder3" + "Type") == "Yearly")
                    ritem.timedEventType = GlobalTime.TimedEventType.YEARLY;

                // Add events to timed event
                ritem.invokeEvents.AddListener(delegate
                {
                    reminderNotification.notificationTitle = notificationTitle;
                    reminderNotification.popupDescription = ritem.eventTitle;
                    reminderNotification.CreateOnlyPopup();
                });

                // Add the item to global time events and update the switch
                globalTime.timedEvents.Add(ritem);
                ritemObj.switchManager.UpdateUI();
            }

            // If reminder4 is assigned
            if (reminder4 != null)
            {
                // Get the component
                ReminderItem ritemObj = reminder4.GetComponent<ReminderItem>();

                // Check the data and set it deafults if it doesn't have any data
                if (!PlayerPrefs.HasKey("Reminder4" + "Title"))
                    ritemObj.SetDefaults();

                // Change the name and title
                reminder4.name = PlayerPrefs.GetString("Reminder4" + "Title");
                TextMeshProUGUI titleObj = reminder4.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                titleObj.text = reminder4.name;

                // Change reminder date
                TextMeshProUGUI dateObj = reminder4.transform.Find("Date").GetComponent<TextMeshProUGUI>();
                dateObj.text = PlayerPrefs.GetString("Reminder4" + "Hour")
                    + ":" + PlayerPrefs.GetString("Reminder4" + "Minute")
                    + "\n" + PlayerPrefs.GetString("Reminder4" + "Type");

                // Add reminder to global time as a timed event
                GlobalTime.TimedEvent ritem = new GlobalTime.TimedEvent();
                ritem.eventTitle = reminder4.name;
                ritem.eventHour = int.Parse(PlayerPrefs.GetString("Reminder4" + "Hour"));
                ritem.eventMinute = int.Parse(PlayerPrefs.GetString("Reminder4" + "Minute"));
                ritem.eventDay = globalTime.currentDay;
                ritem.eventMonth = globalTime.currentMonth;
                ritem.eventYear = globalTime.currentYear;
                ritem.itemID = 4;

                // Fetch the event type data
                if (PlayerPrefs.GetString("Reminder4" + "Type") == "Once")
                    ritem.timedEventType = GlobalTime.TimedEventType.ONCE;
                else if (PlayerPrefs.GetString("Reminder4" + "Type") == "Daily")
                    ritem.timedEventType = GlobalTime.TimedEventType.DAILY;
                else if (PlayerPrefs.GetString("Reminder4" + "Type") == "Weekly")
                    ritem.timedEventType = GlobalTime.TimedEventType.WEEKLY;
                else if (PlayerPrefs.GetString("Reminder4" + "Type") == "Monthly")
                    ritem.timedEventType = GlobalTime.TimedEventType.MONTHLY;
                else if (PlayerPrefs.GetString("Reminder4" + "Type") == "Yearly")
                    ritem.timedEventType = GlobalTime.TimedEventType.YEARLY;

                // Add events to timed event
                ritem.invokeEvents.AddListener(delegate
                {
                    reminderNotification.notificationTitle = notificationTitle;
                    reminderNotification.popupDescription = ritem.eventTitle;
                    reminderNotification.CreateOnlyPopup();
                });

                // Add the item to global time events and update the switch
                globalTime.timedEvents.Add(ritem);
                ritemObj.switchManager.UpdateUI();
            }
        }

        public void SetOnceType()
        {
            // Set reminder event type to once
            reminderEventType = ReminderEventType.ONCE;
        }

        public void SetDailyType()
        {
            // Set reminder event type to daily
            reminderEventType = ReminderEventType.DAILY;
        }

        public void SetMonthlyType()
        {
            // Set reminder event type to monthly
            reminderEventType = ReminderEventType.MONTHLY;
        }

        public void SetWeeklyType()
        {
            // Set reminder event type to weekly
            reminderEventType = ReminderEventType.WEEKLY;
        }

        public void SetYearlyType()
        {
            // Set reminder event type to yearly
            reminderEventType = ReminderEventType.YEARLY;
        }

        public void ChangeUpdateReminder(int index)
        {
            // Change the reminder data depending on the index and update the UI
            PlayerPrefs.SetInt("ReminderHelper", index);
            eventTitleObject.text = PlayerPrefs.GetString("Reminder" + PlayerPrefs.GetInt("ReminderHelper").ToString() + "Title");
            eventHourObject.text = PlayerPrefs.GetString("Reminder" + PlayerPrefs.GetInt("ReminderHelper").ToString() + "Hour");
            eventMinuteObject.text = PlayerPrefs.GetString("Reminder" + PlayerPrefs.GetInt("ReminderHelper").ToString() + "Minute");
            hourSelector.index = int.Parse(eventHourObject.text);
            minuteSelector.index = int.Parse(eventMinuteObject.text);
            hourSelector.UpdateUI();
            minuteSelector.UpdateUI();
        }

        public void UpdateReminder()
        {
            // Add reminder to global time as a timed event
            GlobalTime.TimedEvent ritem = new GlobalTime.TimedEvent();
            ritem.eventTitle = eventTitleObject.text;
            ritem.eventHour = int.Parse(eventHourObject.text);
            ritem.eventMinute = int.Parse(eventMinuteObject.text);
            ritem.eventDay = globalTime.currentDay;
            ritem.eventMonth = globalTime.currentMonth;
            ritem.eventYear = globalTime.currentYear;

            // Save data depending on the event type
            if (reminderEventType == ReminderEventType.ONCE)
            {
                ritem.timedEventType = GlobalTime.TimedEventType.ONCE;
                PlayerPrefs.SetString("Reminder" + PlayerPrefs.GetInt("ReminderHelper").ToString() + "Type", "Once");
            }

            else if (reminderEventType == ReminderEventType.DAILY)
            {
                ritem.timedEventType = GlobalTime.TimedEventType.DAILY;
                PlayerPrefs.SetString("Reminder" + PlayerPrefs.GetInt("ReminderHelper").ToString() + "Type", "Daily");
            }

            else if (reminderEventType == ReminderEventType.WEEKLY)
            {
                ritem.timedEventType = GlobalTime.TimedEventType.WEEKLY;
                PlayerPrefs.SetString("Reminder" + PlayerPrefs.GetInt("ReminderHelper").ToString() + "Type", "Weekly");
            }

            else if (reminderEventType == ReminderEventType.MONTHLY)
            {
                ritem.timedEventType = GlobalTime.TimedEventType.MONTHLY;
                PlayerPrefs.SetString("Reminder" + PlayerPrefs.GetInt("ReminderHelper").ToString() + "Type", "Monthly");
            }

            else if (reminderEventType == ReminderEventType.YEARLY)
            {
                ritem.timedEventType = GlobalTime.TimedEventType.YEARLY;
                PlayerPrefs.SetString("Reminder" + PlayerPrefs.GetInt("ReminderHelper").ToString() + "Type", "Yearly");
            }

            // If title is blank, then just save as My Reminder
            if (eventTitleObject.text == "" || eventTitleObject.text == null)
                PlayerPrefs.SetString("Reminder" + PlayerPrefs.GetInt("ReminderHelper").ToString() + "Title", "My Reminder");
            else
                PlayerPrefs.SetString("Reminder" + PlayerPrefs.GetInt("ReminderHelper").ToString() + "Title", eventTitleObject.text);

            // Save hour and minute data
            PlayerPrefs.SetString("Reminder" + PlayerPrefs.GetInt("ReminderHelper").ToString() + "Hour", eventHourObject.text);
            PlayerPrefs.SetString("Reminder" + PlayerPrefs.GetInt("ReminderHelper").ToString() + "Minute", eventMinuteObject.text);

            globalTime.timedEvents.Add(ritem);

            // Add events to timed event
            ritem.invokeEvents.AddListener(delegate
            {
                reminderNotification.notificationTitle = notificationTitle;
                reminderNotification.popupDescription = ritem.eventTitle;
                reminderNotification.CreateOnlyPopup();
            });

            // Update the UI depending on the reminder index
            if (PlayerPrefs.GetInt("ReminderHelper") == 1)
                reminder1.GetComponent<ReminderItem>().UpdateUI();
            else if (PlayerPrefs.GetInt("ReminderHelper") == 2)
                reminder2.GetComponent<ReminderItem>().UpdateUI();
            else if (PlayerPrefs.GetInt("ReminderHelper") == 3)
                reminder3.GetComponent<ReminderItem>().UpdateUI();
            else if (PlayerPrefs.GetInt("ReminderHelper") == 4)
                reminder4.GetComponent<ReminderItem>().UpdateUI();
        }
    }
}