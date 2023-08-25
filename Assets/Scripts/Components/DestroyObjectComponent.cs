using Mirror;
using UnityEngine;

public class DestroyObjectComponent : NetworkBehaviour
{
    [SerializeField] private GameObject _objectToDestroy;

    public void DestroyObject()
    {
        Destroy(_objectToDestroy);
    }
}