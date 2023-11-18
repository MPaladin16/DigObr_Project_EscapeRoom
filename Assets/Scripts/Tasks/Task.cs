using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Task
{
    public int questionType;
    public string question;
    public string codeSnippet;
    public int correctAnswer;
    public string answer1;
    public string answer2;
    public string answer3;

    public override string ToString()
    {
        return $"{((QuestionType)questionType).ToString()} - {question} - {codeSnippet} - {answer1} - {answer2} - {answer3} - {correctAnswer}";
    }
}
