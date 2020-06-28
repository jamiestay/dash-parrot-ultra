using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respecy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Pay Respects");
        }
        else
        {
            Debug.Log("you Pressed E");
        }
    }
}
