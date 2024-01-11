using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomName;

    public Dropdown dropdownMap;
    public Dropdown dropdownColor;
    public Dropdown dropdownMode;
    public Toggle togglePrivate;

    public void Start()
    {
        // D�sactiver Les dropdowns et le toggle
        dropdownMap.interactable = false;
        dropdownColor.interactable = false;
        dropdownMode.interactable = false;
        togglePrivate.interactable = false;
    }

    public void OnCreateRoomGame()
    {
        User.instance.roomName = roomName.text;
        SceneManager.LoadScene("Menu");
    }
}
