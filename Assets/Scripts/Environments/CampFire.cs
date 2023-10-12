using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;
    private List<IDamageable> thingsToDamage = new List<IDamageable>();

    private void Start()
    {
        InvokeRepeating(nameof(DealDamage), 0, damageRate);
    }

    private void DealDamage()
    {
        for (int i =0; i < thingsToDamage.Count; i++)
        {
            thingsToDamage[i].takePhysicalDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            thingsToDamage.Add(damageable);
        }
    }

    // ½±°Ô 
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            thingsToDamage.Remove(damageable);
        }
    }
}
