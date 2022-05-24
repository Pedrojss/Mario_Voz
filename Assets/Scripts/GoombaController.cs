using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaController : MonoBehaviour
{
    public float speed;
    private bool moveRight;

    private bool isCrushed;
    public Animator animator;

    GameObject player;
    public GameObject goombaCollider;

    private Rigidbody2D rb;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveRight)
        {
            transform.Translate(2 * Time.deltaTime * speed, 0, 0);
        }
        else
        {
            transform.Translate(-2 * Time.deltaTime * speed, 0, 0);
        }

        if(transform.position.y < -30)
        {
            Death();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo") || collision.gameObject.CompareTag("Tube"))
        {
            if (moveRight)
            {
                moveRight = false;
            }
            else
            {
                moveRight = true;
            }
        }
    
        if(collision.gameObject.tag == "Player")
        {
            float yOffset = 0.5f;
            if(transform.position.y + yOffset < collision.transform.position.y)
            {
                player.GetComponent<Rigidbody2D>().velocity = Vector2.up * 7;
                isCrushed = true;
                animator.SetBool("isCrushed", isCrushed);
                speed = 0;
                goombaCollider.GetComponent<CircleCollider2D>().isTrigger = false;
                Invoke("Death", 1);
            }
            else
            {
                PlayerController.death = true;
            }
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
    
}
