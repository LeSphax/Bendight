using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This script automatically connects to Photon (using the settings file).
/// Then it joins or create the Bendight room
/// </summary>
public class AutoConnectToRoom : Photon.MonoBehaviour
{
    public byte Version = 1;

    /// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
    private bool ConnectInUpdate = true;


    public virtual void Start()
    {
        PhotonNetwork.autoJoinLobby = false;    // we join the Bendight room. always. no need to join a lobby to get the list of rooms.
    }

    public virtual void Update()
    {
        if (ConnectInUpdate && !PhotonNetwork.connected)
        {
            Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");

            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
        }
    }

    public virtual void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Bendight", new RoomOptions() { MaxPlayers = 2 }, null);
    }

    public virtual void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("Bendight", new RoomOptions() { MaxPlayers = 2 }, null);
    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Failed to connect: " + cause);
    }
}
