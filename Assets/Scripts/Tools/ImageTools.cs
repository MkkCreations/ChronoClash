using System;
using System.Collections;
using SFB;
//using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class ImageTools : MonoBehaviour
{
    public static string path;
    public static byte[] texture;

    public static void OpenFileExplorer()
    {
        //path = EditorUtility.OpenFilePanel("Show all images", "", "png");

        // Open file with filter
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" )
        };
        path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false)[0];

    }

    // Read file from path
    public static IEnumerator GetTexture(Action UpdateAvatar)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(www.error);
        else
        {
            texture = ((DownloadHandlerTexture)www.downloadHandler).data;
            UpdateAvatar();
        }
    }

    // Convert img from base64 to Texture2D
    public static Texture2D CreateTextureFromString(string image)
    {
        Texture2D tex = new(256, 256, TextureFormat.RGBA32, false);
        tex.LoadImage(Convert.FromBase64String(image));
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 8, 8), new Vector2(0f, 0f)).texture;
    }
}
