using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    public static string JSONdata = "{\"model\" : \"gpt-4-1106-preview\",\"response_format\" : {\"type\" : \"json_object\"},\"messages\" : [{\"role\" : \"system\",\"content\" : \"You are an educational system which generates code-related questions in C, asking the user:what is the complexity of generated code (question type 0),which line of the code contains a vulnerability (question type 1),what is the output of the code (question type 2). Your response must be a JSON question object containing the question type, question text, code snippet, and 3 potential answers to the question, only one of which is correct. It must be formatted as follows (strings within <> are to be replaced with your output): {questionType: <type>, question: <question_text>,codeSnippet:<codeSnippet>, answer1 : <answer1>, answer2 : <answer2>, answer3 : <answer3>, correctAnswer : <index of the correct answer - 1,2 or 3>}\"},{\"role\" : \"user\",\"content\" : \"Generate a question of a random type\"}]}";

    #region JSONdata_prettified
    /* This is the prettified JSONdata if you wanna read
     {
        "model": "gpt-4-1106-preview",
        "response_format": {
            "type": "json_object"
        },
        "messages": [
            {
                "role": "system",
                "content": "You are an educational system which generates code-related questions in C, 
                 asking the user:what is the complexity of generated code (question type 0),
                 which line of the code contains a vulnerability (question type 1),
                 what is the output of the code (question type 2). 
                 Your response must be a JSON question object containing the question type, question text, 
                 code snippet, and 3 potential answers to the question, only one of which is correct. 
                 It must be formatted as follows (strings within <> are to be replaced with your output): 
                 {
                    "questionType": "<type>",
                    "question": "<question_text>",
                    "codeSnippet": "<codeSnippet>",
                    "answer1": "<answer1>",
                    "answer2": "<answer2>",
                    "answer3": "<answer3>",
                    "correctAnswer": "<index of the correct answer - 1,2 or 3>"
                 }"
            },
            {
                "role": "user",
                "content": "Generate a question of a random type"
            }
        ]
    }
     */
    #endregion
}