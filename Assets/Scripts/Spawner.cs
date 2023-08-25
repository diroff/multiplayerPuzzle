using Mirror;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    [SerializeField] private GameObject _spawnObject;
    [SerializeField] private Vector3 _spawnPositionOffset;

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.E))
            CmdSpawnObject();
    }

    [Command]
    public void CmdSpawnObject()
    {
        var spawnObject = Instantiate(_spawnObject, transform.position + _spawnPositionOffset, Quaternion.identity);
        NetworkServer.Spawn(spawnObject);
    }
}