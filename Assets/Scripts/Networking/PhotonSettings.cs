using UnityEngine;

public class PhotonSettings : MonoBehaviour {

	void Awake () {
        //Augment from 10 to 30 to improve the precision of the players position
        PhotonNetwork.sendRate = 30; 
        PhotonNetwork.sendRateOnSerialize = 30;
	}
}
