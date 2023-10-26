using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine.UI;

public class FBManager : MonoBehaviour
{

    public static string fbContentDescription;
    public Text test;

    public void ShareLinkToFacebook( )
    {
        if(!FB.IsLoggedIn) {
            var perms = new List<string>( ) { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
        else {
            FB.ShareLink(
                new Uri("https://play.google.com/store/apps/details?id=atfone.orbit.path.android"),
                "Orbit Path",
                fbContentDescription,
                callback: ShareCallback
            );
        }
    }

    private void ShareCallback( IShareResult result )
    {
        if(result.Cancelled || !String.IsNullOrEmpty(result.Error)) {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else if(!String.IsNullOrEmpty(result.PostId)) {
            // Print post identifier of the shared content
            Debug.Log(result.PostId);
        }
        else {
            // Share succeeded without postID
            Debug.Log("ShareLink success!");
        }
    }

    private void AuthCallback( ILoginResult result )
    {
        Facebook.Unity.AccessToken.CurrentAccessToken = result.AccessToken;
        Debug.Log("Debug result :" + result.Error);
        Debug.Log("Debug  result.AccessToken :" + result.AccessToken);
        if(FB.IsLoggedIn) {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log("DEBUG aToken.UserId :" + aToken.UserId);
            // Print current access token's granted permissions
            string tempString = "";
            foreach(string perm in aToken.Permissions) {
                tempString += perm + ", ";
                Debug.Log(perm);
            }
            test.text = tempString;
            FB.ShareLink(
                new Uri("https://play.google.com/store/apps/details?id=atfone.orbit.path.android"),
                "Orbit Path",
                fbContentDescription,
                callback: ShareCallback
                );
        }
        else {
            Debug.Log("User cancelled login");
        }
    }
}
