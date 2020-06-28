using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePoints : MonoBehaviour
{

    public float speed;
    public TextMesh floatNum;
    
    void Start()
    {
    }

    
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        transform.localScale = new Vector2(transform.localScale.x - 0.01f, transform.localScale.y - 0.01f);
    }
}
