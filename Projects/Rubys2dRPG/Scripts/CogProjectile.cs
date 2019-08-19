using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogProjectile : MonoBehaviour
{

    public AudioClip throwCog;
    public ParticleSystem projectileCollisionPrefab;

    private Rigidbody2D rigidbody2d;

    private float timer;
    private float resetTimer = 3.0f;

    void Awake()
    {
        this.rigidbody2d = GetComponent<Rigidbody2D>();
        this.timer = resetTimer;
    }

    // Update is called once per frame.
    private void Update()
    {
        this.timer -= Time.deltaTime;

        if (this.timer <= 0)
        {
            Destroy(gameObject);
            this.timer = this.resetTimer;
        }
    }

    public void Launch( Vector2 direction, float force, AudioSource audiosource)
    {
        RubyController controller = GetComponent<RubyController>();

        controller = audiosource.GetComponent<RubyController>();
        controller.PlaySound(throwCog);

        this.rigidbody2d.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ParticleSystem cogCollisionEffect =
            Instantiate(
                this.projectileCollisionPrefab,
                this.rigidbody2d.position,
                Quaternion.identity);

        EnemyRobotController enemy = other.collider.GetComponent<EnemyRobotController>();

        if (enemy != null)
        {
            enemy.Fix();
        }
        Destroy(gameObject);
        this.timer = this.resetTimer;
    }
}
