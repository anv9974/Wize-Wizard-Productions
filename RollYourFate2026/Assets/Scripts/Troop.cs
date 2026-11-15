using UnityEngine;

public class Troop : Agent
{
    [Header("Behavior Weights")]
    public float seekWeight = 1.0f;
    public float arriveWeight = 1.0f;
    public float fleeWeight = 2.0f;
    
    public float arriveDistance = 2f;
    public float fleeDistance = 5f;
    public float attackDistance = 0.5f;

    private GameObject currentTower;
    private Transform currentTowerLoc;
    private Animator anim;

    [Header("Misc")]
    public int HP;
    public int DamageDealt;
    public float Speed;
    public float AttackSpeed;

    public float bombradius = 2f;

    public ParticleSystem explosionEffect;

    public GameObject arrowPrefab;
    private float timer;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

    }

    public override Vector3 CalcSteering()
    {
        Vector3 totalForce = Vector3.zero;

        if (GetClosestTower() == null)
        {
            return totalForce;
        }
        currentTowerLoc = GetClosestTower().transform;
        currentTower = GetClosestTower();

        if (currentTowerLoc != null && Vector3.Distance(transform.position, currentTowerLoc.position) <= attackDistance + 0.1f)
        {
            base.Stop();
            //Attack here
            if (timer >= AttackSpeed)
            {
                DealDamage();
                timer = 0f;
            }
        }
        /*
        THIS CAN BE USED TO MAKE TROOPS FLEE FROM TOWERS, MAYBE FOR BOMB, ETC?
        else if (currentTower != null && Vector3.Distance(transform.position, currentTower.position) < fleeDistance)
        {
            totalForce += (Vector3)Flee(currentTower.position) * fleeWeight;
        }
        */
        else if (currentTowerLoc != null && Vector3.Distance(transform.position, currentTowerLoc.position) - attackDistance <= arriveDistance)
        {
            Vector3 stopPoint = currentTowerLoc.position - (currentTowerLoc.position - transform.position).normalized * attackDistance;
            totalForce += (Vector3)Arrive(stopPoint, arriveDistance) * arriveWeight;
        }
        else if (currentTowerLoc != null)
        {
            totalForce += (Vector3)Seek(currentTowerLoc.position) * seekWeight;
        }

        return totalForce * Speed;
    }


//Returns the closest tower to the troop.
    GameObject GetClosestTower()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        GameObject closest = null;
        float minDistance = float.MaxValue;
        //sets mindist to really far away, then checks each tower to see if its closer.

        foreach (GameObject t in towers)
        {
            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = t;
            }
        }
        return closest;
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Used tags to check what kinda troop it is
    void DealDamage()
    {
        Tower tower = currentTower.GetComponent<Tower>();
        switch (gameObject.tag)
        {
            case "Generic":
                tower.TakeDamage(DamageDealt);
                Debug.Log("Meow");
                break;
            case "Brute":
                tower.TakeDamage(DamageDealt);
                break;
            case "Bomber":
                GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
                foreach (GameObject t in towers)
                {
                    if (Vector3.Distance(transform.position, t.transform.position) <= bombradius)
                    {
                        t.GetComponent<Tower>().TakeDamage(DamageDealt);
                    }
                }
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
                Camera.main.transform.GetComponent<CameraShake>().TriggerShake(0.2f, 1f);
                Destroy(gameObject);
                break;
            case "Archer":
                fireArrow(currentTowerLoc.position);
                //tower.TakeDamage(DamageDealt);
                break;
        }
    }

    void fireArrow(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        dir.z = 0f;
        dir.Normalize();

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
        arrow.GetComponent<Rigidbody2D>().linearVelocity = dir * 5f; // Change Speed
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TowerArrow"))
        {
            TakeDamage(2);
            Destroy(collision.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Orb"))
        {
            TakeDamage(1);
        }
    }

}