using Mirror;
using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : NetworkBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onHeal;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private HealthChangeEvent _onChange;

    [SyncVar] private int _currentValue;
    [SyncVar] private bool _isAlive;

    public UnityAction<int> HealthChanged;

    public bool IsAlive => _isAlive;

    private void Start()
    {
        Respawn();
    }

    public void Respawn()
    {
        _currentValue = _health;
        _isAlive = true;
        HealthChanged?.Invoke(_currentValue);
    }

    [Server]
    public void ModifyHealth(int healthDelta)
    {
        _currentValue += healthDelta;
        _onChange?.Invoke(_currentValue);
        HealthChanged?.Invoke(_currentValue);

        if (healthDelta < 0)
        {
            _onDamage?.Invoke();
        }

        if (healthDelta >= 0)
        {
            _onHeal?.Invoke();
        }

        if (_currentValue <= 0)
        {
            _onDie?.Invoke();
            _isAlive = false;
        }
    }

    [Serializable]
    public class HealthChangeEvent : UnityEvent<int>
    {
    }
}