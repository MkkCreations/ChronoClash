using System;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;

public class ImageTools : MonoBehaviour
{
    public static string path;
    public static byte[] texture;

    public static void OpenFileExplorer()
    {
        #if UNITY_EDITOR
            path = EditorUtility.OpenFilePanel("Show all images", "", "png");
        #endif
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
