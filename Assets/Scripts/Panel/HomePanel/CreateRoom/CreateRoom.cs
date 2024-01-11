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
        // D�sactiver Les dropdowns
        dropdownMap.interactable = false;
        dropdownColor.interactable = false;
        dropdownMode.interactable = false;
        // Décocher le toggle
        togglePrivate.isOn = false;
    }

    public void OnCreateRoomGame()
    {
        User.instance.roomName = roomName.text;
        SceneManager.LoadScene("Menu");
    }

    public void OnPrivateGameToggle()
    {
        // Si le toggle est activé, on désactive l'input du nom de la salle et on insere un nom de salle aléatoire
        if (togglePrivate.isOn)
        {
            roomName.interactable = false;
            roomName.text = "";
            roomName.text = GenerateRandomRoomName();
        }
        else
        {
            roomName.interactable = true;
            roomName.text = "";
        }
    }

    //Fonction qui génére un nommbre de 8 chiffres aléatoires
    public string GenerateRandomRoomName()
    {
        string roomName = "";
        for (int i = 0; i < 8; i++)
        {
            roomName += Random.Range(0, 9);
        }
        return roomName;
    }

}
