using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public abstract class ShowDynamicText : MonoBehaviour
{

    private Text label;
    protected abstract string TextToShow
    {
        get;
    }

    protected void Start()
    {
        label = GetComponent<Text>();
    }

    protected void Update()
    {
        label.text = TextToShow;
    }
}