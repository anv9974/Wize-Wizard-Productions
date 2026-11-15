using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;

public class Tower : MonoBehaviour
{
    public int HP;
    private int maxHP;
    public int DamageDealt;

    private float timer;
    public float attackSpeed = 1f;
    public int towerType; // 0 = Arrow, 1 = Orb, 2 = Bomb

    public float range = 5f;

    public SpriteRenderer spriteRenderer;
    public Sprite[] towerSprites;
    public float bombradius = 2f;
    public GameObject arrowPrefab;
    public GameObject orbPrefab;
    public Vector3 projectileSpawnOffset = new Vector3(0f, 0.5f, 0f);
    public static event Action<float> OnTowerDestroyed;
    private bool dead = false;
    public ParticleSystem explosionEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (findTarget() == null)
        {
            return;
        }
        if (timer >= attackSpeed)
        {
            DealDamage();
            timer = 0f;
        }

    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        GetComponent<DamageFlash>()?.Flash();
        if (HP <= 0 && !dead)
        {
            dead = true;
            OnTowerDestroyed?.Invoke(0.1f);
            Destroy(gameObject);
            Camera.main.transform.GetComponent<CameraShake>().TriggerShake(0.5f, 0.2f);
            return;
        }

        if (HP < maxHP / 3)
        {
            spriteRenderer.sprite = towerSprites[2];
        }
        else if (HP < maxHP / 3 * 2)
        {
            spriteRenderer.sprite = towerSprites[1];
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            TakeDamage(2);
            Destroy(collision.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            TakeDamage(2);
            Destroy(collision.gameObject);
        }
    }

    public GameObject findTarget()
    {
        List<GameObject> enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Generic"));
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Archer"));
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Brute"));
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Bomber"));
        GameObject closest = null;
        float minDistance = float.MaxValue;
        //sets mindist to really far away, then checks each enemy to see if its closer.

        foreach (GameObject e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist <= range)
            {
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = e;
                }
            }
        }
        return closest;
    }

    void DealDamage()
    {
        switch (towerType)
        {
            case 0:
                fireArrow(findTarget().transform.position);
                break;
            case 1:
                fireOrb(findTarget().transform.position);
                break;
            case 2:
                BombAttack();
                break;
            case 3:
                fireArrow(findTarget().transform.position);
                fireOrb(findTarget().transform.position);
                break;
        }
    }

    void fireArrow(Vector3 targetPos)
    {
        Vector3 spawnPos = transform.position + projectileSpawnOffset;
        Vector3 dir = targetPos - spawnPos;
        dir.z = 0f;
        dir.Normalize();

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject arrow = Instantiate(arrowPrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
        arrow.GetComponent<Rigidbody2D>().linearVelocity = dir * 5f; // Change Speed
        Collider2D arrowCol = arrow.GetComponent<Collider2D>();
        foreach (Collider2D c in GetComponents<Collider2D>())
        {
            Physics2D.IgnoreCollision(arrowCol, c);
        }
        Destroy(arrow, 5f);
    }

    void fireOrb(Vector3 targetPos)
    {
        Vector3 spawnPos = transform.position + projectileSpawnOffset;
        Vector3 dir = targetPos - spawnPos;
        dir.z = 0f;
        dir.Normalize();

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject orb = Instantiate(orbPrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
        orb.GetComponent<Rigidbody2D>().linearVelocity = dir * 2f; // Change Speed
        Collider2D orbCol = orb.GetComponent<Collider2D>();
        foreach (Collider2D c in GetComponents<Collider2D>())
        {
            Physics2D.IgnoreCollision(orbCol, c);
        }
    }

    void BombAttack()
    {
        List<GameObject> enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Generic"));
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Archer"));
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Brute"));
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Bomber"));
        //sets mindist to really far away, then checks each enemy to see if its closer.
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        foreach (GameObject e in enemies)
        {
            if (Vector3.Distance(transform.position, e.transform.position) <= bombradius)
                    {
                        e.GetComponent<Troop>().TakeDamage(DamageDealt);
                    }
        }
    }
}
