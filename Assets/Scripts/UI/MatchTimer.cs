using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MatchTimer : MonoBehaviour
{

    public Text description;
    public Text time;

    public event EmptyDelegate OutOfTime;

    private float __timeLeft = 0;
    private float TimeLeft
    {
        get
        {
            return __timeLeft;
        }
        set
        {
            __timeLeft = Mathf.Max(0, value);

            int seconds = Mathf.RoundToInt(__timeLeft);

            int displayMinutes = seconds / 60;
            int displaySecondsTens = (seconds % 60) / 10;
            int displaySeconds = seconds % 10;

            time.text = displayMinutes + ":" + displaySecondsTens + displaySeconds;
        }
    }

    public void StartTimer(int seconds)
    {
        TimeLeft = seconds;
    }

    public void StopTimer()
    {
        TimeLeft = 0;
    }

    private void Update()
    {
        if (TimeLeft > 0)
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft == 0)
            {
                Assert.IsTrue(OutOfTime != null, "Something should be listening to the timer");
                OutOfTime.Invoke();
            }
        }
    }

    public void SetDescription(string newDescription)
    {
        description.text = newDescription;
    }


}
