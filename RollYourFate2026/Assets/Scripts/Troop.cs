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

    private Transform currentTower;
    private Animator anim;

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

        currentTower = GetClosestTower();

        if (currentTower != null && Vector3.Distance(transform.position, currentTower.position) <= attackDistance + 0.1f)
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
        else if (currentTower != null && Vector3.Distance(transform.position, currentTower.position) - attackDistance <= arriveDistance)
        {
            Vector3 stopPoint = currentTower.position - (currentTower.position - transform.position).normalized * attackDistance;
            totalForce += (Vector3)Arrive(stopPoint, arriveDistance) * arriveWeight;
        }
        else if (currentTower != null)
        {
            totalForce += (Vector3)Seek(currentTower.position) * seekWeight;
        }

        return totalForce;
    }


//Returns the closest tower to the troop.
    Transform GetClosestTower()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        Transform closest = null;
        float minDistance = float.MaxValue;
        //sets mindist to really far away, then checks each tower to see if its closer.

        foreach (GameObject t in towers)
        {
            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = t.transform;
            }
        }
        return closest;
    }
}