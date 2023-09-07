using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : NetworkBehaviour
{
    [SerializeField] private float _power;
    [SerializeField] private int _damage;
    [SerializeField] private float _lifeTime;
    [SerializeField] private Collider2D _collider;

    private float _timeToDestroying = 0f;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isServer)
            return;

        if(_timeToDestroying >= _lifeTime)
            NetworkServer.Destroy(gameObject);

        _timeToDestroying += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServer)
            OnCollisionOnServer(collision.gameObject);
    }

    [Server]
    private void OnCollisionOnServer(GameObject collisionWith)
    {
        if(collisionWith.TryGetComponent(out HealthComponent healthComponent))
            OnCollisionWithDamage(healthComponent);
    }

    private void OnCollisionWithDamage(HealthComponent healthComponent)
    {
        healthComponent.ModifyHealth(-_damage);
        NetworkServer.Destroy(gameObject);
    }

    public void IgnoreCollisionWith(Collider2D otherCollider)
    {
        Physics2D.IgnoreCollision(_collider, otherCollider);
    }

    public void Move(Vector3 direction)
    {
        _rigidbody.AddForce(direction * _power, ForceMode2D.Impulse);
    }
}