using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAttack : MonoBehaviour
{
    [SerializeField]
    private float _damage;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<MainPlayerCollider>())
        {
            Attack(collider.gameObject.GetComponentInParent<HealthController>());
        }
    }

    private void Attack(HealthController healthController)
    {
        healthController.TakeDamage(_damage);
    }
}