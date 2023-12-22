using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json;

public class LoginRequest : MonoBehaviour
{
    public InputField userInputField;
    public InputField passwordInputField;
    public Button button;
    private string _username;
    private string _password;


    public void Login()
    {
        _username = userInputField.text; // Récupération du nom d'utilisateur depuis l'InputField
        _password = passwordInputField.text; // Récupération du mot de passe depuis l'InputField

        SendLoginRequest(_username, _password);
    }

    public class UserData
    {
        public string username;
        public string password;

        public UserData(string usr, string pwd)
        {
            username = usr;
            password = pwd;
        }
    }

    private void SendLoginRequest(string username, string password)
    {

        var request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:8081/api/auth/login");
        request.ContentType = "application/json";
        request.Method = "POST";

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            string json = JsonUtility.ToJson(new UserData(username, password));
           
            streamWriter.Write(json);
        }

        var response = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
        }
    }
}
