using Mirror;
using UnityEngine;

public class EnterTriggerComponent : NetworkBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] private EnterEvent _action;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(_tag))
        {
            _action?.Invoke(other.gameObject);
        }
    }
}