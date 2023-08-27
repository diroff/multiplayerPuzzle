using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : NetworkBehaviour
{
    [SerializeField] private float _power;
    [SerializeField] private float _lifeTime;

    private float _timeToDestroying = 0f;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(_timeToDestroying >= _lifeTime)
            NetworkServer.Destroy(gameObject);

        _timeToDestroying += Time.deltaTime;
    }

    public void Move(Vector3 direction)
    {
        _rigidbody.AddForce(direction * _power, ForceMode2D.Impulse);
    }
}