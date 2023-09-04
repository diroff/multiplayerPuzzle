using Mirror;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private float _delay;
    [SerializeField] private Transform _spawnPoint;

    private float _timeFromLastShoot;

    private void Start()
    {
        _timeFromLastShoot = _delay;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        TimeChecker();
    }

    public void Shoot(bool rightDirection)
    {
        if (_timeFromLastShoot < _delay)
            return;

        _timeFromLastShoot = 0f;

        Vector3 direction;

        if (rightDirection)
            direction = Vector3.right;
        else
            direction = Vector3.left;

        CmdShoot(direction);
    }

    private void TimeChecker()
    {
        if (_timeFromLastShoot < _delay)
            _timeFromLastShoot += Time.deltaTime;
    }

    [Command]
    private void CmdShoot(Vector3 direction)
    {
        var spawnObject = Instantiate(_bullet, _spawnPoint.position, Quaternion.identity);

        spawnObject.Move(direction);
        NetworkServer.Spawn(spawnObject.gameObject);
    }
}
