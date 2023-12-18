using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] QuestionGenerator questionGenerator;
    [SerializeField] GameObject Canvas1;
    [SerializeField] GameObject Canvas2;
    [SerializeField] GameObject Canvas3;

    [SerializeField] GameObject complexitySpot1;
    [SerializeField] GameObject complexitySpot2;
    [SerializeField] GameObject complexitySpot3;
    void Start()
    {
        /*
        TextMeshPro text1 = Canvas1.transform.GetChild(0).transform.GetChild(0).gameObject.transform.GetComponent<TextMeshPro>();
        TextMeshPro text2 = Canvas2.transform.GetChild(0).transform.GetChild(0).gameObject.transform.GetComponent<TextMeshPro>();
        TextMeshPro text3 = Canvas3.transform.GetChild(0).transform.GetChild(0).gameObject.transform.GetComponent<TextMeshPro>();

        questionGenerator.GenerateQuestion(Canvas1, text1);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
