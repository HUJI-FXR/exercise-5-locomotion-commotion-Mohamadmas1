using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Monster AI")]
    [SerializeField] private Movement movement;
    private float monstertimer = 0;
    [SerializeField] private float monsterTime;
    [SerializeField] private float chasePlayerChance;
    private bool chasePlayer;
    [SerializeField] private float rotationRate;
    [SerializeField] private string playerTag;

    [Header("Combat")]
    [SerializeField] private Vector2 damageRange;
    [SerializeField] private float health;
    private float initialHealth;
    [SerializeField] private Renderer damageTintRenderer;

    void Start()
    {
        damageTintRenderer.material = Instantiate(damageTintRenderer.material);
        initialHealth = health;
    }

    void Update()
    {
        // Decide whether to chase the player
        monstertimer += Time.deltaTime;
        if (monstertimer >= monsterTime)
        {
            monstertimer = 0;
            chasePlayer = Random.Range(0, 1f) < chasePlayerChance;
        }

        // Look at the player if chasing, otherwise rotate randomly
        if (chasePlayer && GameManager.instance.player != null)
        {
            Vector3 direction = GameManager.instance.player.transform.position - transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationRate * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, Random.Range(-rotationRate, rotationRate) * Time.deltaTime, 0);
        }

        // Make the monster change color based on health
        float healthPercentage = health / initialHealth;
        damageTintRenderer.material.color = healthPercentage * Color.white + (1f - healthPercentage) * Color.red;
    }

    void FixedUpdate()
    {
        // move towards the player
        movement.Move(0, 1);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.instance.AddScore();
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("fuckkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk");
            float damage = Random.Range(damageRange.x, damageRange.y);
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Die();
        }
    }

    private void Die()
    {
        GameManager.instance.monsterCount--;
        Destroy(gameObject);
    }
}
