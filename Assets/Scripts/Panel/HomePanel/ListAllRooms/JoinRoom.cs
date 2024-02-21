using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class JoinRoom : MonoBehaviour
{
    public TMP_InputField nameRoom; 

    public void JoinRoomInList(string roomName)
    {
        User.instance.roomName = roomName;
    }

    public void JoinPrivateRoom()
    {
        User.instance.roomName = nameRoom.text;
        User.instance.isForPrivateRoom = true;
        SceneManager.LoadScene("Menu");
    }
}
