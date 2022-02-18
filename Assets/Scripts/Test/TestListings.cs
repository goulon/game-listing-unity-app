using System;
using UnityEngine;
using UnityEngine.Replay;
using System.Collections;
using UnityEngine.Networking;

namespace Unity.Metacast.Demo
{
    /// <summary>
    ///     Populate UIBrowser with test json data
    /// </summary>
    public class TestListings : MonoBehaviour
    {
        /// <summary>
        ///     Start is called on the frame when a script is enabled just
        ///     before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            //TODO Instead of a TextAsset pass JSON result from the web server.
            StartCoroutine(GetJSON("http://localhost:3000/games"));
        }
        /// <summary>
        ///     GetJSON is called by Start when a script is enabled just
        ///     before any of the Update methods are called the first time.
        /// </summary>
        private IEnumerator GetJSON(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    var responseText = webRequest.downloadHandler.text;
                    var formattedResponseText = "{\"listings\":" + responseText + "}";
                    UIBrowser.instance.Init(formattedResponseText);
                }
            }
        }
    }
}