using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;

public class TimerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float TimeLeft;
    public bool TimerOn = false;

    public TextMeshProUGUI TimerTxt;
    public GameObject indicator;
    public Material roomDone;
    void Start()
    {

        TimerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn) {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else {
                TimerOn = false;
                TimeLeft = 0;
                GameOver();
            }
        }
    }

    public void GameOver() {
    
    }

    public void GameWon() {
        TimerOn = false;
        indicator.GetComponent<Renderer>().sharedMaterial = roomDone;
        TimerTxt.color = Color.green;
    }

    void updateTimer(float currentTime) {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00} : {1:00}", minutes, seconds);

    }
}
