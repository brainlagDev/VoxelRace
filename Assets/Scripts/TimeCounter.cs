//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    private Text TimeCount;
    public Text RealTimeText;
    public static int Minutes = 0;
    public static int Seconds = 0;
    private float LocalTime = 0.0f;
    string MinutesStr;
    string SecondsStr;
    void Start()
    {
        TimeCount = GetComponent<Text>();
    }
    void FixedUpdate()
    {
        //  Racing time:
        if (TimeCount != null)
        {
            if (Minutes < 10)
                MinutesStr = "0" + Minutes;
            else
                MinutesStr = Minutes.ToString();
            if (Seconds < 10)
                SecondsStr = "0" + Seconds;
            else
                SecondsStr = Seconds.ToString();
            TimeCount.text = MinutesStr + ":" + SecondsStr;
        }

        try
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isAlive)
            {
                LocalTime += 0.02f;
                if (LocalTime >= 1.0f)
                {
                    LocalTime -= 1.0f;
                    ++Seconds;
                }
                if (Seconds == 60)
                {
                    Seconds = 0;
                    ++Minutes;
                }
            }
        }
        catch { }

        //  Realtime
        if (RealTimeText != null)
        {
            string[] date = { System.DateTime.Now.Day.ToString(), System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(),
            System.DateTime.Now.Hour.ToString(), System.DateTime.Now.Minute.ToString(), System.DateTime.Now.Second.ToString()};

            for (int i = 0; i < date.Length; i++)
            {
                if (date[i].Length < 2)
                    date[i] = "0" + date[i];
                else if (date[i].Length > 2)
                    date[i] = date[i][date[i].Length - 2].ToString() + date[i][date[i].Length - 1].ToString();
            }

            RealTimeText.text = date[0] + "/" + date[1] + "/" + date[2]
                + "\n" + date[3] + ":" + date[4] + ":" + date[5];
        }
    }
}
