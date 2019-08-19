using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public class EnemyRobotController : MonoBehaviour
{
    public bool vertical = false;
    public float moveSpeed = 3.0f;
    private float direction = 1.0f;

    public int maxHealth = 5;
    public int currHealth;
    public int health { get { return this.currHealth; } }
    private bool broken;

    public float changeTime = 3.0f;
    private float timer;

    public ParticleSystem smokeEffect;

    public AudioClip sfxRobotFixed;
    public AudioClip sfxRobotBroken;
    public AudioClip sfxEnemyHit1, sfxEnemyHit2;

    private Rigidbody2D rigidbody2d;
    private Vector2 position;
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        this.rigidbody2d = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.audioSource = GetComponent<AudioSource>();

        this.timer = this.changeTime;
        this.currHealth = this.maxHealth;
        this.broken = true;
    }

    void Update()
    {

        if (!this.broken)
        {
            return;
        }
        else
        {
            this.timer -= Time.deltaTime;
        }

        if (this.timer <= 0)
        {
            this.direction = this.direction * -1;
            this.timer = this.changeTime;
        }

        this.position = this.rigidbody2d.position;

        if (this.vertical)
        {
            this.animator.SetFloat("MoveX", 0.0f);
            this.animator.SetFloat("MoveY", this.direction);
            this.position.y += Time.deltaTime * this.moveSpeed * this.direction;
        }
        else
        {
            this.animator.SetFloat("MoveX", this.direction);
            this.animator.SetFloat("MoveY", 0.0f);
            this.position.x += Time.deltaTime * this.moveSpeed * this.direction;
        }
        this.rigidbody2d.MovePosition(this.position);
    }

    public void Fix()
    {
        this.playSound(this.sfxEnemyHit1);
        this.playSound(this.sfxRobotFixed);
        this.audioSource.clip = null;
        this.broken = false;
        this.animator.SetTrigger("Fixed");
        this.rigidbody2d.simulated = false;
        this.smokeEffect.Stop();
    }

    public void playSound(AudioClip clip)
    {
        this.audioSource.PlayOneShot(clip);
    }

    
}