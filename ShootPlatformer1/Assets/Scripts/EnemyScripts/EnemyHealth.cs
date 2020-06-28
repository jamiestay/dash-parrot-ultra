using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int health;
    public GameObject bloodEffect;
    public CameraShake camShake;
    private Rigidbody2D rb;
    public float dazedTime;
    public float startDazedTime;
    public int rbMass;
    

    public GameObject numberFloat;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        if(dazedTime <= 0)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.mass = 50f;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            rb.mass = rbMass;
            rb.bodyType = RigidbodyType2D.Dynamic;
            dazedTime -= Time.deltaTime;
        }


        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        Vector2 Poss = new Vector2(transform.position.x, transform.position.y + Random.Range(1, 1.5f));
        numberFloat.GetComponent<TextMesh>().text = damage.ToString();
        if (damage >= 4)
        {
            numberFloat.GetComponent<TextMesh>().color = Color.yellow;
        }
        else
        {
            numberFloat.GetComponent<TextMesh>().color = Color.white;
        }
        Instantiate(numberFloat, Poss, Quaternion.identity);
        dazedTime = startDazedTime;
        camShake.Shake();
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        health -= damage;
    }
}
