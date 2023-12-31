using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collided3 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] QuestionGenerator qg;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        qg.Task3Collided(other);
    }
    private void OnTriggerExit(Collider other)
    {
        qg.Task3Collided(null);
    }
}
