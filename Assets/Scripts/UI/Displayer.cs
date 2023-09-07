using Mirror;
using TMPro;
using UnityEngine;

public abstract class Displayer : NetworkBehaviour
{
    [SerializeField] protected TextMeshProUGUI TextField;

    protected Player Player;

    public void SetPlayer(Player player)
    {
        Player = player;
        Subscribe();
    }

    protected abstract void Subscribe();

    protected virtual void ShowValue(int count)
    {
        TextField.text = "Value:" + count;
    }
}