using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeSeconds = 4f;
    private Rigidbody rb;
    private float damage;

    public void Init(Vector3 velocity, float damage)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        this.damage = damage;
        rb.linearVelocity = velocity;
        Invoke(nameof(Despawn), lifeSeconds);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var point = collision.contacts.Length > 0 ? collision.contacts[0].point : transform.position;
        var normal = collision.contacts.Length > 0 ? collision.contacts[0].normal : -transform.forward;

        if (collision.collider.TryGetComponent<IDamageable>(out var dmg))
            dmg.ApplyDamage(damage, point, normal);

        Despawn();
    }

    private void Despawn() { Destroy(gameObject); }
}
