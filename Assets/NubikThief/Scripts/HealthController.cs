using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private float _health;

    private void Start()
    {
        _health = _maxHealth;
    }

    public void AddHealth(float value)
    {
        _health += value;
        _health = Mathf.Min(_health, _maxHealth);
        EventBus.OnAddHealth?.Invoke(this);
    }

    public void TakeDamage(float value)
    {
        _health -= value;
        _health = Mathf.Max(_health, 0f);
        EventBus.OnTakeDamage?.Invoke(this);

        if (_health < 0.01f)
        {
            EventBus.OnDead?.Invoke(this);
        }
    }
}