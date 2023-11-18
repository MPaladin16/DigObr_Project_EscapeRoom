using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBedWhenMoved : MonoBehaviour
{
    [SerializeField] GameObject bed;
    [SerializeField] GameObject _collider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
            
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _collider) {
            bed.transform.GetComponent<MovingBed>().MoveBed();
        }
    }
}
