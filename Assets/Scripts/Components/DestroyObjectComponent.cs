using Mirror;
using UnityEngine;

public class DestroyObjectComponent : NetworkBehaviour
{
    [SerializeField] private GameObject _objectToDestroy;

    public void DestroyObject()
    {
        //Destroy(_objectToDestroy);
        NetworkServer.Destroy(_objectToDestroy);
    }

    public void DestroyObject(GameObject gameObject)
    {
        NetworkServer.Destroy(gameObject);
    }
}