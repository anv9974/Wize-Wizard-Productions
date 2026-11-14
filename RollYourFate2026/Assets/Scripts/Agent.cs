using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    [Header("Agent Physics")]
    public float maxSpeed = 5f;
    public float maxForce = 10f;
    public float mass = 1f;

    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public Vector3 acceleration;

    public virtual void Update()
    {
        acceleration = Vector3.zero;
        
        Vector3 steeringForce = CalcSteering();
        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);
        ApplyForce(steeringForce);

        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    public abstract Vector3 CalcSteering();

    public Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desiredVelocity = (targetPos - transform.position).normalized * maxSpeed;
        return desiredVelocity - velocity;
    }

    public Vector3 Flee(Vector3 targetPos)
    {
        Vector3 desiredVelocity = (transform.position - targetPos).normalized * maxSpeed;
        return desiredVelocity - velocity;
    }

    public Vector3 Arrive(Vector3 targetPos, float arrivalDistance)
    {
        Vector3 toTarget = targetPos - transform.position;
        float dist = toTarget.magnitude;
        float speed = dist < arrivalDistance ? maxSpeed * (dist / arrivalDistance) : maxSpeed;
        Vector3 desiredVelocity = toTarget.normalized * speed;
        return desiredVelocity - velocity;
    }

    public void Stop()
    {
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
    }
}