using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;

public class PlayerController : MonoBehaviour
{
    public float Acceleration = 15.0f;
    public float Speed = 1.0f;
    public float Jumpforce = 185.0f;
    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private float LastJump;
    private float Velocity;
    private bool facingRight = true;
    private bool isGrounded = true;
    public Animator animator;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    public static bool death = false;
    private float countdown = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        actions.Add("salta",Jump);
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        animator.SetBool("isGrounded", isGrounded);
        Horizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(Horizontal));
        if(Horizontal != 0.0f)
            Velocity = Mathf.Clamp(Velocity + Horizontal * Acceleration * Time.deltaTime, -1.0f, 1.0f);
        else
            Velocity -= Velocity * Acceleration * Time.deltaTime;

        if(Horizontal < 0.0f && facingRight) FlipPlayer();
        if(Horizontal > 0.0f && !facingRight) FlipPlayer();

        if(death){
            Speed = 0;
            Jumpforce = 0;
            animator.SetTrigger("isDead");
            countdown -= Time.deltaTime;
            if(countdown > 0f)
            {
                Invoke("Death", 0.5f);
            };
        }
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2((Mathf.Abs(Velocity) < 0.01f ? 0.0f : Velocity) * Speed, Rigidbody2D.velocity.y);
    }

    void FlipPlayer()
    {
        facingRight = !facingRight;
        Vector2 playerScale = gameObject.transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            Rigidbody2D.velocity = Vector2.down * 5;
        }
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech){
        actions[speech.text].Invoke();
    }

    private void Jump()
    {
        if(isGrounded)
        {
            Rigidbody2D.AddForce(Vector2.up * Jumpforce);
        }
    }

    void Death()
    {
        GetComponent<Collider2D>().isTrigger = true;
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, 5f);
    }
}


