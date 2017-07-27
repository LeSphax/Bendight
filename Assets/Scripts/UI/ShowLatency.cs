public class ShowLatency : ShowDynamicText {

    protected override string TextToShow
    {
        get
        {
            return PhotonNetwork.GetPing().ToString() + " ms";
        }
    }
}
