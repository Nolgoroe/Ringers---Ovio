using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class ProccessingDeepLink : MonoBehaviour
{
    public static ProccessingDeepLink Instance { get; private set; }
    public string deeplinkURL;

    private string _deepLinkPrefix = "oviogg://app/user";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Application.deepLinkActivated += OnDeepLinkActivated;
            if (!String.IsNullOrEmpty(Application.absoluteURL))
            {
                // Cold start and Application.absoluteURL not null so process Deep Link.
                OnDeepLinkActivated(Application.absoluteURL);
            }
            // Initialize DeepLink Manager global variable.
            else deeplinkURL = "[none]";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDeepLinkActivated(string link)
    {
        deeplinkURL = link;

        if (link.ToLower().Contains("ovio"))
        {
            HandleLaunchURL();
        }
    }

    void HandleLaunchURL()
    {
        //Get the userId, in our example it's just getting it from the PlayerPrefs:
        var userId = PlayfabManager.instance.playerPlayfabUsername;

        //This should be your game identifier so we will know which game called our deep link
        //I think that Application.identifier is good here, in the onboarding of your game you should let us know what the identifier is.
        var identifier = Application.identifier;
        OpenUrl($"{_deepLinkPrefix}/{identifier}/{userId}");
    }

    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }
}
