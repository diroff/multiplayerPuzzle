using Mirror;
using UnityEngine;

public class CollectCoinComponent : NetworkBehaviour
{
    [SerializeField] private int _coinValue;

    public void CollectCoin(GameObject target)
    {
        var player = target.GetComponent<Player>();

        player.AddCoins(_coinValue);
    }
}