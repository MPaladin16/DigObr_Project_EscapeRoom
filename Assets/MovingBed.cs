using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class MovingBed : MonoBehaviour
{
    [SerializeField] GameObject _newTask;
    [SerializeField] GameObject _oldPic;
    private bool _bedmoved = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_bedmoved) { 
            float step = 2 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0.819999993f, 0.920000017f), step);
        }
    }

    public void MoveBed() {
        _bedmoved = true;
        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x -0.22f, transform.rotation.y, transform.rotation.z, 1),1);
        _newTask.SetActive(true);
        _oldPic.SetActive(false);
    }
}
