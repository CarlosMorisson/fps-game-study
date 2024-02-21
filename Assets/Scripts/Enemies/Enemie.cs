using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour
{
    public static Enemie instance;
    public float Life;
    public float Speed;
    public Transform Target;
    public NavMeshAgent Agent;
    public List<Rigidbody> RagDollComponents;
    void Start()
    {
        instance = this;
        DeactiveRagdoll();
        Agent.speed = Speed;


    }

    public void ReceiveDamage(float damage)
    {
        Life -= damage;
    }
    public void ActiveRagdoll()
    {
        for (int i = 0; i < RagDollComponents.Count; i++)
        {
            RagDollComponents[i].useGravity = true;
            RagDollComponents[i].isKinematic = false;
        }
        Speed = 0;
        Agent.speed = Speed;
    }
    public void DeactiveRagdoll()
    {
        for (int i = 0; i < RagDollComponents.Count; i++)
        {
            RagDollComponents[i].useGravity = false;
            RagDollComponents[i].isKinematic = true;
        }
    }
    void Update()
    {
        FallowPlayer();
    }
    public virtual void FallowPlayer()
    {
        Agent.SetDestination(Target.position);
    }
}
