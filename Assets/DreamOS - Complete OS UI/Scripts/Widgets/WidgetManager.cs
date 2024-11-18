using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    public class WidgetManager : MonoBehaviour
    {
        // Resources
        public WidgetLibrary widgetLibrary;
        public Transform widgetParent;
        public Transform widgetLibraryParent;
        public GameObject widgetButton;

        // Settings
        public bool sortListByName = true;

        void Start()
        {
            PrepareWidgets();
        }

        private static int SortByName(WidgetLibrary.WidgetItem o1, WidgetLibrary.WidgetItem o2)
        {
            return o1.widgetTitle.CompareTo(o2.widgetTitle);
        }

        public void PrepareWidgets()
        {
            foreach (Transform child in widgetParent)
                Destroy(child.gameObject);

            foreach (Transform child in widgetLibraryParent)
                Destroy(child.gameObject);

            if (sortListByName == true)
                widgetLibrary.widgets.Sort(SortByName);
			
			for (int i = 0; i < widgetLibrary.widgets.Count; ++i)
            {
                // Spawn widgets
                GameObject go = Instantiate(widgetLibrary.widgets[i].widgetPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(widgetParent, false);
                go.gameObject.name = widgetLibrary.widgets[i].widgetTitle;

                // Spawn widget button
                GameObject gob = Instantiate(widgetButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                gob.transform.SetParent(widgetLibraryParent, false);
                gob.gameObject.name = widgetLibrary.widgets[i].widgetTitle;

                // Set icon
                Transform imageObj;
                imageObj = gob.transform.Find("Icon").GetComponent<Transform>();
                imageObj.GetComponent<Image>().sprite = widgetLibrary.widgets[i].widgetIcon;

                // Set ID tags
                TextMeshProUGUI titleText;
                titleText = gob.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                titleText.text = widgetLibrary.widgets[i].widgetTitle;
                TextMeshProUGUI descriptionText;
                descriptionText = gob.transform.Find("Description").GetComponent<TextMeshProUGUI>();
                descriptionText.text = widgetLibrary.widgets[i].widgetDescription;

                // Add button events
                SwitchManager widgetSwitch;
                widgetSwitch = gob.transform.Find("Switch").GetComponent<SwitchManager>();

                WidgetItem tempWidget;
                tempWidget = go.GetComponent<WidgetItem>();

                widgetSwitch.OnEvents.AddListener(delegate
                {
                    widgetSwitch.isOn = true;
                    widgetSwitch.UpdateUI();
                    tempWidget.EnableWidget();

                });

                widgetSwitch.OffEvents.AddListener(delegate
                {
                    widgetSwitch.isOn = false;
                    widgetSwitch.UpdateUI();
                    tempWidget.DisableWidget();
                });

                if (PlayerPrefs.GetString(tempWidget.gameObject.name + "Widget") == "" && tempWidget.isOn == true)
                {
                    widgetSwitch.isOn = true;
                    widgetSwitch.gameObject.SetActive(false);
                    widgetSwitch.gameObject.SetActive(true);
                }

                else if (PlayerPrefs.GetString(tempWidget.gameObject.name + "Widget") == "" && tempWidget.isOn == false)
                {
                    widgetSwitch.isOn = false;
                    widgetSwitch.gameObject.SetActive(false);
                    widgetSwitch.gameObject.SetActive(true);
                }

                else if (PlayerPrefs.GetString(tempWidget.gameObject.name + "Widget") == "enabled")
                {
                    widgetSwitch.isOn = true;
                    widgetSwitch.gameObject.SetActive(false);
                    widgetSwitch.gameObject.SetActive(true);
                }

                else if (PlayerPrefs.GetString(tempWidget.gameObject.name + "Widget") == "disabled")
                {
                    widgetSwitch.isOn = false;
                    widgetSwitch.gameObject.SetActive(false);
                    widgetSwitch.gameObject.SetActive(true);
                }

                try
                {
                    WindowDragger tempDragger;
                    tempDragger = go.GetComponent<WindowDragger>();
                    tempDragger.dragArea = GameObject.Find("Widget Drag Area").GetComponent<RectTransform>();
                }

                catch { }
            }
        }
    }
}