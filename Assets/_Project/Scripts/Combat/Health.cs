using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    private float current;

    private void Awake() { current = maxHealth; }

    public void ApplyDamage(float amount, Vector3 hitPoint, Vector3 hitNormal)
    {
        current -= amount;
        if (current <= 0f)
        {
            // TODO: replace with pool/despawn & death FX
            Destroy(gameObject);
        }
    }
}
