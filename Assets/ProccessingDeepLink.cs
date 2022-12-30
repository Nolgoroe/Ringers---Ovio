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
        //UIManager.Instance.tempText.text = "Activated";

        deeplinkURL = link;

        HandleLaunchURL();

        //if (link.ToLower().Contains("ovio"))
        //{
        //    HandleLaunchURL();
        //}
    }

    void HandleLaunchURL()
    {
        GetDeviceID(out string androidID, out string customID);

        string name = string.Empty;

        if (!string.IsNullOrEmpty(androidID))
        {
            name = androidID;
        }
        else if (!string.IsNullOrEmpty(customID))
        {
            name = customID;
        }
        else
        {
            Debug.LogError("PROBLEM HERE");
            return;
        }

        //Get the userId, in our example it's just getting it from the PlayerPrefs:
        //var userId = PlayfabManager.instance.playerPlayfabUsername;
        var userId = name;

        //This should be your game identifier so we will know which game called our deep link
        //I think that Application.identifier is good here, in the onboarding of your game you should let us know what the identifier is.
        var identifier = Application.identifier;
        OpenUrl($"{_deepLinkPrefix}/{identifier}/{userId}");
    }

    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }

    public void GetDeviceID(out string androidID, out string customID)
    {
        androidID = string.Empty;
        customID = string.Empty;

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject contentResolver = activity.Call<AndroidJavaObject>("getContentResolver");
            AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
            androidID = secure.CallStatic<string>("getString", contentResolver, "android_id");

            if (androidID.Length > 20)
            {
                androidID = androidID.Substring(0, 20);
            }
            else
            {
                androidID = androidID.Substring(0, androidID.Length);
            }
        }
        else
        {
            customID = SystemInfo.deviceUniqueIdentifier;

            if (customID.Length > 20)
            {
                customID = customID.Substring(0, 20);
            }
            else
            {
                customID = customID.Substring(0, customID.Length);
            }

        }
    }
}
