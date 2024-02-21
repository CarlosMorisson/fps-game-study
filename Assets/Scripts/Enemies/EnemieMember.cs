using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieMember : MonoBehaviour
{
    [SerializeField]
    ParticleSystem bloodParticle;
    [SerializeField]
    float MemberLife=50;
    public void MemberDamage(float damage, Vector3 direction)
    {
        MemberLife -= damage;
        if (MemberLife <= 0 )
        {
            GameObject particle = Instantiate(bloodParticle.gameObject, this.transform.position, this.transform.rotation);
            Destroy(particle, 3f);
            this.transform.localScale = new Vector3(0, 0, 0);
            Enemie.instance.ActiveRagdoll();
        }
        Enemie.instance.ReceiveDamage(damage);
    }

}
