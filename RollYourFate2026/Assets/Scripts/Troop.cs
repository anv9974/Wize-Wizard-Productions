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

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();

        //for animation
        if (anim != null && velocity.sqrMagnitude > 0.001f)
        {
            Vector3 dir = velocity.normalized;
        }
    }

    public override Vector3 CalcSteering()
    {
        Vector3 totalForce = Vector3.zero;

        currentTowerLoc = GetClosestTower().transform;
        currentTower = GetClosestTower();

        if (currentTowerLoc != null && Vector3.Distance(transform.position, currentTowerLoc.position) <= attackDistance + 0.1f)
        {
            //Attack here
            base.Stop();
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

        return totalForce;
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
                tower.TakeDamage(DamageDealt);
                break;
            case "Archer":
                tower.TakeDamage(DamageDealt);
                break;
        }
    }
}