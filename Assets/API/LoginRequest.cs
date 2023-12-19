using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class LoginRequest : MonoBehaviour
{
    public InputField userInputField;
    public InputField passwordInputField;
    private string _username;
    private string _password;

    public void Login()
    {
        _username = userInputField.text; // Récupération du nom d'utilisateur depuis l'InputField
        _password = passwordInputField.text; // Récupération du mot de passe depuis l'InputField

        StartCoroutine(SendLoginRequest(_username, _password));
    }

    public class UserData
    {
        public string username;
        public string password;
    }

    private IEnumerator SendLoginRequest(string username, string password)
    {
        var user = new UserData();
        user.username = username; // Utilisation du nom d'utilisateur passé en paramètre
        user.password = password; // Utilisation du mot de passe passé en paramètre

        string json = JsonUtility.ToJson(user);

        var req = new UnityWebRequest("http://localhost:8080/api/auth/login", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        // Envoyez la requête et attendez la réponse
        yield return req.SendWebRequest(); // Attendre la réponse de la requête

        // Vérifiez la réponse de l'API.
        if (req.result == UnityWebRequest.Result.ConnectionError ||
            req.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Erreur de connexion : " + req.error);
        }
        else
        {
            Debug.Log("Authentification réussie !");
        }
    }
}
