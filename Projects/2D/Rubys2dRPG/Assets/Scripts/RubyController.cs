using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{

    public float moveSpd = 5.0f;

    public int maxHealth = 5;
    public int health { get { return this.currHealth; } }
    public int currHealth;

    public bool isInvincible;
    public float timeInvincible = 2.0f;
    private float invincibleTimer;

    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private Vector2 position;
    private Vector2 move;
    private Vector2 lookDirection;

    public GameObject projectilePrefab;
    public AudioClip sfxRubyHit, sfxConsumableHealth;
    private AudioSource audioSource;

    // Start is called once before the first frame update
    void Start()
    {
        this.rigidbody2d = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.audioSource = GetComponent<AudioSource>();
        this.lookDirection = new Vector2(1, 0);
        this.move = new Vector2(0, 0);
        this.currHealth = maxHealth;
        this.position = this.rigidbody2d.position;
    }

    // Update is called once per frame.
    void Update()
    {
        float inpHorizontal = Input.GetAxis("Horizontal");
        float inpVertical = Input.GetAxis("Vertical");
        bool inpFire2 = Input.GetButtonDown("Fire2");
        bool inpInteract = Input.GetKeyDown(KeyCode.E);

        this.MovePlayer(inpHorizontal, inpVertical);

        if (this.isInvincible)
        {
            this.InvincibleStart();
        }

        if (inpFire2)
        {
            this.Launch();
        }

        if (inpInteract)
        {
            this.InteractWithNPC();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyRobotController enemyRobot =
            other.collider.GetComponent<EnemyRobotController>();

        if (other != null)
        {
            if (enemyRobot && !this.isInvincible)
            {
                this.PlaySound(this.sfxRubyHit);
                this.ChangeHealth(-1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HealthCollectible healthCollectible = 
            other.GetComponent<HealthCollectible>();
        HazardSpikes hazardSpikes = 
            other.GetComponent<HazardSpikes>();

        if (other != null)
        {
            if (healthCollectible)
            {
                if (this.health >= this.maxHealth)
                {
                    return;
                }
                else
                {
                this.PlaySound(this.sfxConsumableHealth);
                this.ChangeHealth(1);
                Destroy(healthCollectible.gameObject);
                }
            }

            if (hazardSpikes && !this.isInvincible)
            {
                this.PlaySound(this.sfxRubyHit);
                this.ChangeHealth(-1);
            }
        }
        return;
    }

    public void InvincibleStart()
    {
        this.invincibleTimer -= Time.deltaTime;

        if (this.invincibleTimer <= 0)
        {
            this.isInvincible = false;
        }
        return;
    }

    public void ChangeHealth(int amount)
    {

        if (amount < 0)
        {
            if (this.isInvincible)
            {
                return;
            }
            else
            {
                this.isInvincible = true;
                this.invincibleTimer = this.timeInvincible;
            }
        }

        this.currHealth = Mathf.Clamp(this.currHealth + amount, 0, this.maxHealth);

        UIHealthBar.instance.setValue(this.currHealth / (float)this.maxHealth);

        return;
    }

    public void PlaySound(AudioClip clip)
    {
        this.audioSource.PlayOneShot(clip);

        return;
    }

    private void Launch()
    {
        GameObject projectileObject =
            Instantiate(
                this.projectilePrefab,
                this.position + Vector2.up * 0.5f,
                Quaternion.identity);

        CogProjectile cogProjectile =
                projectileObject.GetComponent<CogProjectile>();

        this.animator.SetTrigger("Launch");
        cogProjectile.Launch(this.lookDirection, 300.0f, this.audioSource);

        return;
    }

    public void MovePlayer(float hori, float vert)
    {
        this.move = new Vector2(hori, vert);
        
        // Adjust player's direction based on horizontal/vertical user inputs.
        if ( !Mathf.Approximately(this.move.x, 0.0f) ||
             !Mathf.Approximately(this.move.y, 0.0f) )
        {
            this.lookDirection.Set(this.move.x, this.move.y);
            this.lookDirection.Normalize();
        }

        // Play corresponding animations based on player's direction.
        this.animator.SetFloat("Look X", this.lookDirection.x);
        this.animator.SetFloat("Look Y", this.lookDirection.y);
        this.animator.SetFloat("Speed", this.move.magnitude);

        // Assign player's position equal to rigidbody's position.
        this.position = this.rigidbody2d.position;

        // Calculate player's new position.
        this.position += this.move * this.moveSpd * Time.deltaTime;

        // Update player's new position.
        this.rigidbody2d.MovePosition(this.position);

        return;
    }

    private void InteractWithNPC()
    {
        RaycastHit2D hit = Physics2D.Raycast(
                this.position + Vector2.up * 0.2f,
                this.lookDirection, 1.5f,
                LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character != null)
            {
                character.displayDialog();
            }
        }
    }
}
