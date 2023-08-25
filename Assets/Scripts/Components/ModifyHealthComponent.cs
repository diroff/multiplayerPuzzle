using UnityEngine;

public class ModifyHealthComponent : MonoBehaviour
{
    [SerializeField] private int _healthDelta;

    public void Apply(GameObject target)
    {
        var healthComponent = target.GetComponent<HealthComponent>();

        if (healthComponent != null)
            healthComponent.ModifyHealth(_healthDelta);
    }
}