using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // assign main player camera
    [SerializeField] private Transform muzzle;    // projectile spawn
    [SerializeField] private WeaponDefinition def;

    private int ammoInMag;
    private bool isReloading;
    private bool isBurstFiring;
    private float nextShotTime;

    private void Awake() { ammoInMag = def.magazineSize; }

    private void Update()
    {
        bool wantsFire = Input.GetButton("Fire1");
        bool wantsSemiTap = Input.GetButtonDown("Fire1");
        bool wantsReload = Input.GetKeyDown(KeyCode.R);

        if (wantsReload) StartCoroutine(Reload());

        if (isReloading || Time.time < nextShotTime) return;

        switch (def.fireMode)
        {
            case FireMode.Semi:
                if (wantsSemiTap) TryFireOne();
                break;
            case FireMode.Auto:
                if (wantsFire) TryFireOne();
                break;
            case FireMode.Burst:
                if (wantsSemiTap && !isBurstFiring) StartCoroutine(BurstFire());
                break;
        }
    }

    private IEnumerator Reload()
    {
        if (isReloading || ammoInMag == def.magazineSize) yield break;
        isReloading = true;
        // TODO: play reload SFX/animation
        yield return new WaitForSeconds(def.reloadTime);
        ammoInMag = def.magazineSize;
        isReloading = false;
    }

    private IEnumerator BurstFire()
    {
        isBurstFiring = true;
        for (int i = 0; i < def.burstCount; i++)
        {
            if (!TryFireOne()) break;
            yield return new WaitForSeconds(def.burstInterval);
        }
        isBurstFiring = false;
    }

    private bool TryFireOne()
    {
        if (ammoInMag <= 0) return false;

        ammoInMag--;
        nextShotTime = Time.time + (1f / Mathf.Max(def.fireRate, 0.01f));

        Vector3 dir = ApplySpread(playerCamera.transform.forward, def.spreadDegrees);

        if (def.projectilePrefab != null && muzzle != null)
        {
            var go = Instantiate(def.projectilePrefab, muzzle.position, Quaternion.LookRotation(dir));
            if (go.TryGetComponent<Projectile>(out var proj))
                proj.Init(dir * def.muzzleVelocity, def.damage);
        }
        // TODO: fire SFX/VFX
        return true;
    }

    private static Vector3 ApplySpread(Vector3 direction, float spreadDeg)
    {
        if (spreadDeg <= 0f) return direction;
        Quaternion rand = Quaternion.Euler(Random.Range(-spreadDeg, spreadDeg),
                                           Random.Range(-spreadDeg, spreadDeg),
                                           0f);
        return rand * direction;
    }

    // For HUD later
    public int AmmoInMag => ammoInMag;
    public int MagazineSize => def.magazineSize;
    public string WeaponName => def.displayName;
}
