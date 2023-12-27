using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;

public class TimerScript : MonoBehaviour
{

    [SerializeField] GameObject DoorGameWon;
    [SerializeField] AudioSource audioClip;
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
        if (_task1Done && _task2Done && _task3Done && _task4Done) {
            GameWon();
        }
    }

    public void GameOver() {
    
    }

    public void GameWon() {
        TimerOn = false;
        indicator.GetComponent<Renderer>().sharedMaterial = roomDone;
        TimerTxt.color = Color.green;
        float step = 2 * Time.deltaTime;
        DoorGameWon.transform.position = Vector3.MoveTowards(DoorGameWon.transform.position, new Vector3(-3.74181439985f, 1.2541145131f, 1.136999995f), step);
        audioClip.Play();

    }

    void updateTimer(float currentTime) {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00} : {1:00}", minutes, seconds);

    }

    public void Task1Done() {
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
