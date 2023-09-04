using Mirror;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    [SerializeField] private GameObject _spawnObject;
    [SerializeField] private int _spawnCount;

    [Header("Spawn Point")]
    [SerializeField] private float _xMinimumRange;
    [SerializeField] private float _xMaximumRange;
    [SerializeField] private float _yMinimumRange;
    [SerializeField] private float _yMaximumRange;

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if(Input.GetKeyDown(KeyCode.E))
            CmdSpawnObject(new Vector3(transform.position.x + 0.2f, transform.position.y, 0));
    }

    [Command]
    public void CmdSpawnObject(Vector3 spawnPoint)
    {
        var spawnObject = Instantiate(_spawnObject, spawnPoint, Quaternion.identity);
        NetworkServer.Spawn(spawnObject);
    }

    [Server]
    private void SrvSpawnObject()
    {
        Vector3 spawnPoint = transform.position;

        var spawnObject = Instantiate(_spawnObject, spawnPoint, Quaternion.identity);
        NetworkServer.Spawn(spawnObject);
    }

    public void ServerSpawn()
    {
        SrvSpawnObject();
    }

    public void SpawnObjects()
    {
        RangeValidation();

        for (int i = 0; i < _spawnCount; i++)
        {
            var spawnPoint = new Vector3(Random.Range(_xMinimumRange, _xMaximumRange), Random.Range(_yMinimumRange, _yMaximumRange));

            CmdSpawnObject(spawnPoint);
        }
    }

    private void RangeValidation()
    {
        if(_xMinimumRange >= _xMaximumRange)
            _xMinimumRange = _xMaximumRange - 1f;

        if (_yMinimumRange >= _yMaximumRange)
            _yMinimumRange = _yMaximumRange - 1f;
    }
}