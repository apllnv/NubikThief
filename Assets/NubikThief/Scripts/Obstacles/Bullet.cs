using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _damage;

    [SerializeField]
    private float _startForce;

    private bool _canAttack; //чтобы избежать многократного нанесения урона из-за составного коллайдера, для котрого OnCollisionEnter срабатывает несколько раз при поадании

    private void Start()
    {
        _canAttack = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerMove>())
        {
            if (_canAttack)
            {
                Attack(collision.gameObject.GetComponent<HealthController>());
            }
            _canAttack = false;
        }

        Destroy(gameObject);
    }

    private void Attack(HealthController healthController)
    {
        healthController.TakeDamage(_damage);
    }
}
