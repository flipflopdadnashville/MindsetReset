﻿using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UI;
using TMPro;
using Michsky.DreamOS;

// mailFrom must be Gmail or you have to change smtpServer properties to your mail provider
// mailFrom must have enabled "Allow less secure applications" in google account and I recommend use for it new mail account for safety
// https://myaccount.google.com/lesssecureapps
// mailTo can be from any mail provider


public class RateGame : MonoBehaviour {

    [Header("Variables")]
    // [Tooltip("Link to your app in Google Play or App Store")]
    // public string link = "[Your application store link]";
    public MazecraftGameManager instance;
    public PortalBell rewardsScreenManager;
    public NotificationCreator notificationCreator;
    public Sprite spriteVariable;
    [Tooltip("Mail from which messages will be sent")]
    public string mailFrom = "[Your mail from which messages will be sent]";
    [Tooltip("Password to mailFrom")]
    public string mailFromPassword = "[Password to mailFrom]";
    [Tooltip("Mail to which you want to send feedback")]
    public string mailTo="[Your feedback mail]";
    public string firstName;
    public string lastName;
    // [Tooltip("Display app rating window after x game opened.")]
    // public int remindRating; // open rating window at X game start

    [Header("References")]
    // PanelAppRating
    //public Button[] starButton;
    //public GameObject mainPanel;
    //public Button acceptButton;
    // PanelRateMarket
    //public GameObject ratingMarketPanel;
    // PanelFeedback
    //public GameObject feedBackPanel;
    // public InputField userMail;
    // public InputField userMessage;
    public GameObject noteTitle;
    public GameObject note;



    //[HideInInspector] public int ratedApp; // rate stor value can be used for something after rating


    // public void Init(int gameOpenCounter)
    // {
    //     // get remind rating app value
    //     int remind = PlayerPrefs.GetInt("remindRating", remindRating);
    //     // get is rated app value
    //     bool isRated = PlayerPrefs.GetInt("isAppRated", 0) == 1 ? true : false;

    //     Debug.Log("Game open: " + gameOpenCounter + ", remind rating: " + remind + ", is app rated: " + isRated);

    //     if (remind <= gameOpenCounter && !isRated)
    //     {
    //         ratedApp = 0;
    //         RateApplication(0);
    //         mainPanel.SetActive(true);
    //         acceptButton.interactable = false;
    //         ratingMarketPanel.SetActive(false);
    //         feedBackPanel.SetActive(false);
    //     }
    //     else
    //     {
    //         mainPanel.SetActive(false);
    //         ratingMarketPanel.SetActive(false);
    //         feedBackPanel.SetActive(false);
    //     }
    // }


    // public void RateApplication(int rate)
    // {
    //     ratedApp = rate;

    //     // active rate button if use click some stars
    //     if (rate > 0)
    //         acceptButton.GetComponent<Button>().interactable = true;

    //     // enable stars equal than user rated
    //     for (int i=0; i < rate; i++)
    //     {
    //         foreach (Transform t in starButton[i].transform)
    //         {
    //             t.gameObject.SetActive(true);
    //         }
    //     }

    //     // enable stars greater than user rated
    //     for (int i = rate; i < starButton.Length; i++)
    //     {
    //         foreach (Transform t in starButton[i].transform)
    //         {
    //             t.gameObject.SetActive(false);
    //         }

    //     }
    // }

    // public void AcceptRating()
    // {
    //     // analytics rating
    //     AnalyticsManager.ReportRateApp(ratedApp);
    //     if (ratedApp >= 4)
    //         ShowRateMarket();
    //     else
    //         ShowFeedBack();
    // }


    // private void ShowFeedBack()
    // {
    //     // close main panel and show feedback panel
    //     mainPanel.SetActive(false);
    //     feedBackPanel.SetActive(true);
    // }
    // private void ShowRateMarket()
    // {
    //     // close main panel and show rating market panel
    //     mainPanel.SetActive(false);
    //     ratingMarketPanel.SetActive(true);
    // }

    // public void RateLater()
    // {
    //     // analytics action type
    //     AnalyticsManager.ReportRateType("Rate later");
    //     // set next rating window open after "remindRating"
    //     PlayerPrefs.SetInt("remindRating", PlayerPrefs.GetInt("remindRating", remindRating) + remindRating);
    //     // close App Rating window
    //     gameObject.SetActive(false);
    // }
    // public void CloseWindow(bool isRated)
    // {
    //     // analytics action type
    //     if (isRated)
    //         AnalyticsManager.ReportRateType("Rated and close");
    //     // close App Rating window
    //     gameObject.SetActive(false);
    //     // set app is rated 
    //     PlayerPrefs.SetInt("isAppRated", 1);
    // }
    // public void RateMarket()
    // {
    //     // analytics action type
    //     AnalyticsManager.ReportRateType("Rated and open Google Play");
    //     // open your app website on Google Play
    //     Application.OpenURL(link);
    //     // set app is rated 
    //     PlayerPrefs.SetInt("isAppRated", 1);
    // }

/*    public void SendMail()
    {
        // analytics action type
        AnalyticsManager.ReportRateType("Rated and send Feedback");
        // mail message
        string message = PlayerPrefs.GetString("FirstName") + " " + PlayerPrefs.GetString("LastName") +
                         " started the 5 Things Exercise. Here is their input...\n\n" +
                         "----------------------------------------------------------\n\n" +
                         "Feedback Message:\n" +
                         "----------------------------------------------------------\n" +
                         note.GetComponent<TMP_InputField>().text + "\n" +
                         "----------------------------------------------------------\n" +
                         "Feedback Data:\n" +
                         "----------------------------------------------------------\n" +
                         "User mail: " + noteTitle.GetComponent<TMP_InputField>().text + "\n" +
                         "Bundle Identifier: " + Application.identifier + "\n" +
                         "Application Build Version: " + Application.version + "\n" +
                         "Unity Build Version: " + Application.unityVersion + "\n" +
                         "Device System Language: " + Application.systemLanguage + "\n" +
                         "Platform: " + Application.platform + "\n";
        // create mail content
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(mailFrom);
        mail.To.Add(mailTo);
        if(instance.notificationCount == 0){
            mail.Subject = "5 Things Exercise Started";
        }
        else if(instance.notificationCount == 5){
            mail.Subject = "5 Things Exercise Completed";
        }
        mail.Body = message;
        // gmail server configuration, credentials, certificate etc
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, mailFromPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);

        notificationCreator.notificationTitle = "Message sent"; // Set notification title
        notificationCreator.notificationDescription = "Your message was sent to: " + mailTo + " successfully. Thanks!"; // Set notification description
        notificationCreator.popupDescription = "Your message was successfully sent to: " + mailTo + " Thanks!"; // Set popup notification description
        notificationCreator.notificationIcon = spriteVariable; // Set notification icon
        notificationCreator.enablePopupNotification = true; // Enable or disable popup notification
        //notificationCreator.popupDescription = notificationDescriptions[notificationCount]; // Set popup notification description
        notificationCreator.enableButtonIcon = true;
        //notificationCreator.CreateButton("title", spriteVariable, null, true); // Create buttons (title, icon, button event, close on click)
        notificationCreator.CreateNotification(); // Create a notification depending on variables

        //Debug.Log("From RateGame, notification count is: " + instance.notificationCount);
        if(instance.notificationCount == 0){
            //instance.GetComponent<UIManager>().ToggleGame();
            //instance.GetComponent<UIManager>().ToggleControls();
            rewardsScreenManager.DisplayRewardsScreen();
        }        
        else if(instance.notificationCount == 5){
            rewardsScreenManager.CloseWindow();
        }

        //Debug.Log("Mail send:\n" +message);

        //CloseWindow(false);
    }
*/}