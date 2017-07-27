using UnityEngine;

public class PhotonSettings : MonoBehaviour {

	void Awake () {
        PhotonNetwork.sendRate = 30; 
        PhotonNetwork.sendRateOnSerialize = 30;
	}
}
