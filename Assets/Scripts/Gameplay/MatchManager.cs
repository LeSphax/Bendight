using Photon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MatchManager : PunBehaviour
{
    private static Dictionary<int, Avatar> avatarList = new Dictionary<int, Avatar>();

    public static void RegisterAvatar(int playerID, Avatar avatar)
    {
        avatarList.Add(playerID, avatar);
    }

    public static void RemoveAvatar(int playerID)
    {
        avatarList.Remove(playerID);
    }

    private const int MATCH_LENGTH_IN_SECONDS = 60;

    public Transform spawns;
    public List<GameObject> panels;
    public Text centerText;
    public GameObject playAgainButton;
    public List<GameObject> choosingRolesUI;
    public MatchTimer timer;    

    private string winner;

    private enum MatchState
    {
        WAITING_PLAYER,
        CHOOSING_ROLES,
        PLAYING,
        END
    }
    private MatchState state;
    private MatchState State
    {
        get { return state; }
        set
        {
            if (PhotonNetwork.isMasterClient)
                photonView.RPC("SyncState", PhotonTargets.Others, value);
            switch (value)
            {
                case MatchState.WAITING_PLAYER:
                    panels.ForEach(panel => panel.SetActive(true));
                    choosingRolesUI.ForEach(panel => panel.SetActive(false));
                    centerText.text = "Waiting for another player ...";

                    playAgainButton.SetActive(false);

                    timer.SetDescription("");
                    timer.StopTimer();
                    break;
                case MatchState.CHOOSING_ROLES:
                    panels.ForEach(panel => panel.SetActive(true));
                    choosingRolesUI.ForEach(panel => panel.SetActive(true));
                    centerText.text = "What do you want to play ?";

                    playAgainButton.SetActive(false);

                    timer.SetDescription("");
                    timer.StopTimer();
                    break;
                case MatchState.PLAYING:
                    panels.ForEach(panel => panel.SetActive(false));

                    Avatar myAvatar = avatarList[PhotonNetwork.player.ID];
                    timer.StartTimer(MATCH_LENGTH_IN_SECONDS);
                    timer.SetDescription(myAvatar.AvatarTypeSettings.TimerDescription);
                    break;
                case MatchState.END:
                    panels.ForEach(panel => panel.SetActive(true));
                    choosingRolesUI.ForEach(panel => panel.SetActive(false));
                    centerText.text = "The " + winner + " won the game !";

                    if (PhotonNetwork.isMasterClient)
                        playAgainButton.SetActive(true);

                    timer.SetDescription("");
                    timer.StopTimer();
                    break;
                default:
                    throw new UnhandledEnumCase(state);
            }
            state = value;
        }
    }

    [PunRPC]
    private void SyncState(MatchState state)
    {
        State = state;
    }

    private void Start()
    {
        State = MatchState.WAITING_PLAYER;
        timer.OutOfTime += OutOfTime;
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(ResourcePaths.Avatar, Vector3.zero, Quaternion.identity, 0, new object[] { PhotonNetwork.player.ID, PhotonNetwork.isMasterClient ? Team.Blue : Team.Red });
    }

    //Called on masterClient when the other client connects
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);
        Assert.IsTrue(PhotonNetwork.isMasterClient);
        State = MatchState.CHOOSING_ROLES;
    }

    [PunRPC]
    public void StartMatch(int type)
    {
        Debug.Log("StartMatch " + type + "     " + state);
        if (!PhotonNetwork.isMasterClient)
        {
            photonView.RPC("StartMatch", PhotonTargets.MasterClient, (int)((AvatarType)type).OtherType());
        }
        else if (State == MatchState.CHOOSING_ROLES)
        {
            AvatarType myType = (AvatarType)type;
            foreach (var playerID in avatarList.Keys)
            {
                AvatarType avatarType = playerID == PhotonNetwork.player.ID ? myType : myType.OtherType();
                int spawnNumber = avatarType == AvatarType.DODGER ? 0 : 1;

                Avatar avatar = avatarList[playerID];

                avatar.OutOfHealth += OutOfHealth;
                avatar.Spawn(avatarType, spawns.GetChild(spawnNumber).position);
            }
            State = MatchState.PLAYING;
        }
    }

    private void OutOfHealth()
    {
        EndMatch("Shooter");
    }

    private void EndMatch(string winnerName)
    {
        photonView.RPC("SetWinnerName", PhotonTargets.All, winnerName);
        State = MatchState.END;
        foreach (var playerID in avatarList.Keys)
        {
            Avatar avatar = avatarList[playerID];

            avatar.photonView.RPC("RemoveAvatar", PhotonTargets.All);
            avatar.OutOfHealth -= OutOfHealth;
        }
    }

    private void OutOfTime()
    {
        EndMatch("Dodger");
    }

    [PunRPC]
    private void SetWinnerName(string winnerName)
    {
        winner = winnerName;
    }

    public void PlayAgain()
    {
        State = MatchState.CHOOSING_ROLES;
    }

}
