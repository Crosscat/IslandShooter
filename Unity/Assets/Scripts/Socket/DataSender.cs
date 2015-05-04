using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class DataSender : MonoBehaviour
{

    // Use this for initialization
    IEnumerator Start()
    {
        WebSocket w = new WebSocket(new Uri("ws://localhost:4649/AirborneCheck"));
        yield return StartCoroutine(w.Connect());
        w.SendString("TEST");
        while (true)
        {
            string reply = w.RecvString();

            if (reply != null)
            {
                Debug.Log("Received: " + reply);
                string s = GameController.Player.isAirborne ? "YES" : "NO";
                w.SendString(s);
            }
            if (w.Error != null)
            {
                Debug.LogError("Error: " + w.Error);
                break;
            }
            yield return 0;
        }
        w.Close();
    }
}
