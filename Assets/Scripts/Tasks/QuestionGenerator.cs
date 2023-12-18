using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class QuestionGenerator : MonoBehaviour
{
    //private readonly string endpointURL = "http://localhost:8080/";
    private readonly string endpointURL = "https://api.openai.com/v1/chat/completions";
    private readonly string APItoken = "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";
    [SerializeField] TextMeshProUGUI _text1, _text2, _text3;
    [SerializeField] GameObject _spot1, _spot2, _spot3;
    [SerializeField] GameObject[] _cubePool;

    private string unescape_custom (string input)
    {
        input = input.Replace ("\\", "\\");
        input = input.Replace ("\\t", "\t");
        input = input.Replace("\\r", "\r");

        return input;
    }

    public void Start()
    {
        StartCoroutine(GenerateQuestion(_text1));
        StartCoroutine(GenerateQuestion(_text2));
        StartCoroutine(GenerateQuestion(_text3));
    }

    public IEnumerator GenerateQuestion(TextMeshProUGUI text)
    {
        var data = Question.JSONdata;
        UnityWebRequest request = UnityWebRequest.Post(endpointURL, data, "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {APItoken}");
        yield return request.SendWebRequest();

        //var recv = "{\r\n  \"id\": \"chatcmpl-8MIE5ZChlmKWBqj8wVEiSlt4qScTu\",\r\n  \"object\": \"chat.completion\",\r\n  \"created\": 1700323969,\r\n  \"model\": \"gpt-4-1106-preview\",\r\n  \"choices\": [\r\n    {\r\n      \"index\": 0,\r\n      \"message\": {\r\n        \"role\": \"assistant\",\r\n        \"content\": \"{\\n    \\\"questionType\\\": 2,\\n    \\\"question\\\": \\\"What is the output of the code?\\\",\\n    \\\"codeSnippet\\\": \\n    \\\"#include <stdio.h>\\\\n\\\\nint main() {\\\\n    int x = 5;\\\\n    int y = x + 3;\\\\n    printf(\\\\\\\"%d\\\\\\\", y);\\\\n    return 0;\\\\n}\\\",\\n    \\\"answer1\\\": \\\"5\\\",\\n    \\\"answer2\\\": \\\"8\\\",\\n    \\\"answer3\\\": \\\"3\\\",\\n    \\\"correctAnswer\\\": 2\\n}\"\r\n      },\r\n      \"finish_reason\": \"stop\"\r\n    }\r\n  ],\r\n  \"usage\": {\r\n    \"prompt_tokens\": 186,\r\n    \"completion_tokens\": 109,\r\n    \"total_tokens\": 295\r\n  },\r\n  \"system_fingerprint\": \"fp_a24b4d720c\"\r\n}";
        
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            Debug.Log(request.downloadHandler.text);
        }
        else
        {   try {
                var recv = request.downloadHandler.text;
                recv = unescape_custom(recv);

                JObject jsonObj = JObject.Parse(recv);
                JObject response = (JObject)((JArray)jsonObj["choices"])[0];
                response = (JObject)response["message"];

                var unescapedResponse = response["content"];
                Debug.Log(unescapedResponse.ToString());
                Task deserializedData = JsonUtility.FromJson<Task>(unescapedResponse.ToString());
                Debug.Log(deserializedData.ToString());

                text.text = deserializedData.question + "\n" + deserializedData.codeSnippet;
            } catch {
                Debug.LogError("Something went wrong :( Try fetching the next question");
            } 
        }
    }
}
