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
        ActiveRagdoll();


    }

    public void ReceiveDamage(float damage)
    {
        Life -= damage;
        DeactiveRagdoll();
    }
    public void ActiveRagdoll()
    {
        for (int i = 0; i < RagDollComponents.Count; i++)
        {
            RagDollComponents[i].useGravity = true;
            RagDollComponents[i].isKinematic = false;
        }
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
