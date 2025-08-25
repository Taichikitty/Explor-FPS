using UnityEngine;

public enum FireMode { Semi, Burst, Auto }

[CreateAssetMenu(menuName = "FPS/Weapon Definition", fileName = "NewWeapon")]
public class WeaponDefinition : ScriptableObject
{
    [Header("Identity")]
    public string displayName = "Burst Rifle";

    [Header("Ballistics")]
    public float damage = 12f;
    public float muzzleVelocity = 60f;
    public float spreadDegrees = 1.5f;

    [Header("Cadence")]
    public FireMode fireMode = FireMode.Burst;
    public int burstCount = 3;
    public float fireRate = 9f;          // rounds/sec throttle
    public float burstInterval = 0.07f;  // delay between shots in a burst

    [Header("Ammo")]
    public int magazineSize = 30;
    public float reloadTime = 1.8f;

    [Header("Refs")]
    public GameObject projectilePrefab;
    public Transform defaultMuzzleVfx;

    [Header("FX")]
    public AudioClip fireSfx;
    public AudioClip reloadSfx;
}
