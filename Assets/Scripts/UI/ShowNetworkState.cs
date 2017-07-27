public class ShowNetworkState : ShowDynamicText {

    protected override string TextToShow
    {
        get
        {
            return PhotonNetwork.connectionStateDetailed.ToString();
        }
    }
}
