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
    public string answer4;
    public string answer5;
    public string explanation;



    public override string ToString()
    {
        return $"{(QuestionType)questionType} - {question} - {codeSnippet} - {answer1} - {answer2} - {answer3} - {answer4} - {answer5} - {correctAnswer} - {GetCorrectAnswerString()}";
    }

    public string GetCorrectAnswerString()
    {
        switch (correctAnswer) { 
            case 1: 
                return answer1.ToString();
            case 2: 
                return answer2.ToString();
            case 3: 
                return answer3.ToString();
            case 4: 
                return answer4.ToString();
            case 5: 
                return answer5.ToString();
            default:
                Debug.LogError("Failed to fetch correct answer!");
                return null;
        }
    }
}
