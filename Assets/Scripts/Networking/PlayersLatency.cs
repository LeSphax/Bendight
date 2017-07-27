using ExitGames.Client.Photon;
using Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

//Store the ping of our player in the properties of the room, making it accessible to other players.
class PlayersLatency : PunBehaviour
{
    void Start()
    {
        InvokeRepeating("RefreshPing", 0, 1);
    }

    void RefreshPing()
    {
        Hashtable PlayerCustomProps = new Hashtable();

        PlayerCustomProps["Ping"] = PhotonNetwork.GetPing();

        PhotonNetwork.player.SetCustomProperties(PlayerCustomProps);
    }

    public static int OtherPlayerPing()
    {
        Assert.IsTrue(PhotonNetwork.playerList.Length == 2);
        PhotonPlayer otherPlayer = PhotonNetwork.playerList.Where(player => player.ID == MatchManager.OtherPlayerID).First();
        return (int)otherPlayer.CustomProperties["Ping"];
    }

    public static float OneWayTripOtherPlayer()
    {
        return (PhotonNetwork.GetPing() / 2.0f + PlayersLatency.OtherPlayerPing() / 2.0f) / 1000;
    }


}
