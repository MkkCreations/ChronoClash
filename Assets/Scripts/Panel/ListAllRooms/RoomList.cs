using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomList : MonoBehaviourPunCallbacks
{
    public GameObject roomPrefab;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            GameObject newRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity, transform);
            newRoom.GetComponent<Room>().Name.text = room.Name;
        }
    }
}
