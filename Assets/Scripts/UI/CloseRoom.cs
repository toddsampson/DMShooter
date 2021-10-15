using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CloseRoom : MonoBehaviour
{
    public Slots slots;

    public void Close()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        slots.SetSlots();
    }
}