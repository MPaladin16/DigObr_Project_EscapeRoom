using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.XR.Interaction.Toolkit;

public class TimerScript : MonoBehaviour
{

    [SerializeField] GameObject DoorGameWon;
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioSource audioSource;
    [SerializeField] TeleportationArea teleportationArea;
    // Start is called before the first frame update
    public float TimeLeft;
    public bool TimerOn = false;

    public TextMeshProUGUI TimerTxt;
    public GameObject indicator;
    public Material roomDone;

    private bool _task1Done = false;
    private bool _task2Done = false;
    private bool _task3Done = false;
    private bool _task4Done = false;
    private bool _started = false;



    void Start()
    {

        TimerOn = true;
        //audioSource.PlayOneShot(audioClip);
    }

    // Update is called once per frame

    void Update()
    {
        //GameWon();
        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                TimerOn = false;
                TimeLeft = 0;
                GameOver();
            }
        }
        if (_task1Done && _task2Done && _task3Done && _task4Done && TimerOn)
        {
            GameWon();
            TimerOn = false;
        }
        if (!TimerOn && _started) { DoorGameWon.transform.position = Vector3.MoveTowards(DoorGameWon.transform.position, new Vector3(-3.74181439985f, 1.21145131f, 1.136999995f), 2 * Time.deltaTime); }
    }

    public void GameOver()
    {
        _task1Done = false;
        TimerTxt.text = "You lost!";

    }

    public void GameWon()
    {
        TimerOn = false;
        indicator.GetComponent<Renderer>().sharedMaterial = roomDone;
        TimerTxt.color = Color.green;
        audioSource.PlayOneShot(audioClip);
        teleportationArea.enabled = true;
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        _started = true;

    }

    public void Task1Done()
    {
        _task1Done = true;
    }
    public void Task2Done()
    {
        _task2Done = true;
    }
    public void Task3Done()
    {
        _task3Done = true;
    }
    public void Task4Done()
    {
        _task4Done = true;
    }

    public void Task1UnDone()
    {
        _task1Done = false;
    }
    public void Task2UnDone()
    {
        _task2Done = false;
    }
    public void Task3UnDone()
    {
        _task3Done = false;
    }
    public void Task4UnDone()
    {
        _task4Done = false;
    }

}
