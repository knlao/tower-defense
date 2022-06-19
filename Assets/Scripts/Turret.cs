using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Turret : MonoBehaviour
{
    /*
     * Turret Script
     * Find nearest enemy
     * Aim that enemy
     */
    
    [Header("General")]
    public float[] rangeList;
    public Transform rotationCenter;
    public float rotateSpeed = 10f;
    public string enemyTag;
    public Transform firePoint;
    public Vector3 enemyOffset;

    [Header("Masks")]
    public LayerMask unwalkableMask;
    public LayerMask temporarilyUnwalkableMask;

    [Header("Use Bullets (Default)")]
    public float[] fireRateList;
    public int[] damageList;
    public GameObject bulletPrefab;
    public GameObject muzzleFlash;
    public float muzzleFlashDuration;

    [Header("Use Laser")]
    public bool useLaser;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;
    public float[] damageOverTimeList;
    public float slowPercentage = 0.5f;

    [Header("Upgrade Levels")]
    public int rangeLevel = 1;
    public int fireRateLevel = 1;
    public int damageLevel = 1;
    public int damageOverTimeLevel = 1;

    [Header("Upgrade Costs")]
    public int[] rangeUpgradeCost;
    public int[] damageUpgradeCost;
    public int[] fireRateUpgradeCost;
    public int[] damageOverTimeUpgradeCost;
    

    private Transform _target;
    private float _fireCountdown;
    private Enemy _targetEnemy;

    private float _laserSoundCountdown = 0;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0, 0.5f);
    }

    private void Update()
    {
        _laserSoundCountdown -= Time.deltaTime;
        _fireCountdown -= Time.deltaTime;

        if (_target == null)
        {
            FindObjectOfType<SFXManager>().StopGunShot();
            if (!(useLaser && lineRenderer.enabled))
                return;
            lineRenderer.enabled = false;
            impactEffect.Stop();
            impactLight.enabled = false;
        }
        
        // Aiming
        LockOnTarget();
        
        // Shooting
        if (useLaser)
        {
            Laser();
        }
        else
        {
            if (_fireCountdown <= 0f)
            {
                Shoot();
                _fireCountdown = fireRateList[fireRateLevel-1];
            }
            
        }
    }

    private void LockOnTarget()
    {
        if (_target == null) return;
        if (rotationCenter == null) return;
        var dir = _target.position + enemyOffset - transform.position;
        var lookRotation = Quaternion.LookRotation(dir);
        var rotation = Quaternion.Lerp(rotationCenter.rotation, lookRotation, Time.deltaTime * rotateSpeed).eulerAngles;
        rotationCenter.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    private void Laser()
    {
        if (_target == null) return;
        if (firePoint == null) return;

        _targetEnemy.TakeDamage(damageOverTimeList[damageOverTimeLevel-1] * Time.deltaTime);
        _targetEnemy.Slow(slowPercentage);
        
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, _target.position + enemyOffset);

        var dir = firePoint.position - _target.position + enemyOffset;
        
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);

        impactEffect.transform.position = _target.position + enemyOffset + dir.normalized;

        if (_laserSoundCountdown < 0)
        {
            FindObjectOfType<SFXManager>().PlayClip(3);
            _laserSoundCountdown = FindObjectOfType<SFXManager>().clips[3].length;
        }
    }

    private void UpdateTarget()
    {
        var enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        var shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (var enemy in enemies)
        {
            var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (shortestDistance < distanceToEnemy) continue;
            shortestDistance = distanceToEnemy;
            nearestEnemy = enemy;
        }

        if (nearestEnemy != null && shortestDistance <= rangeList[rangeLevel - 1])
        {
            _target = nearestEnemy.transform;
            _targetEnemy = _target.GetComponent<Enemy>();
        }
        else _target = null;
    }

    private void Shoot()
    {
        if (firePoint == null) return;
        var bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        var bullet = bulletGo.GetComponent<Bullet>();
        bullet.damage = damageList[damageLevel - 1];
        if (bullet != null)
        {
            bullet.Seek(_target);
            if (bullet.explosionRadius > 0)
                FindObjectOfType<SFXManager>().PlayClip(1);
            else
                FindObjectOfType<SFXManager>().PlayGunShot();
            if (muzzleFlash != null)
            {
                var flash = Instantiate(muzzleFlash, firePoint);
                Destroy(flash, muzzleFlashDuration);
            }
        }
    }

    public void Upgrade(string type)
    {
        switch (type)
        {
            case "range":
                
                if (PlayerStats.Money < rangeUpgradeCost[rangeLevel-1])
                {
                    Debug.LogWarning("Not enough money up upgrade!");
                    return;
                }

                if (rangeLevel >= rangeList.Length)
                {
                    Debug.LogWarning("This option is already at maximum!");
                    return;
                }

                PlayerStats.Money -= rangeUpgradeCost[rangeLevel - 1];
                rangeLevel++;
                
                break;
            case "damage":
                if (!useLaser)
                {
                
                    if (PlayerStats.Money < damageUpgradeCost[damageLevel-1])
                    {
                        Debug.LogWarning("Not enough money up upgrade!");
                        return;
                    }

                    if (damageLevel >= damageList.Length)
                    {
                        Debug.LogWarning("This option is already at maximum!");
                        return;
                    }

                    PlayerStats.Money -= damageUpgradeCost[damageLevel-1];
                    damageLevel++;
                    
                }
                break;
            case "fire_rate":
                if (!useLaser)
                {
                
                    if (PlayerStats.Money < fireRateUpgradeCost[fireRateLevel-1])
                    {
                        Debug.LogWarning("Not enough money up upgrade!");
                        return;
                    }

                    if (fireRateLevel >= fireRateList.Length)
                    {
                        Debug.LogWarning("This option is already at maximum!");
                        return;
                    }

                    PlayerStats.Money -= fireRateUpgradeCost[fireRateLevel-1];
                    fireRateLevel++;
                    
                }
                break;
            case "damage_over_time":
                if (useLaser)
                {
                
                    if (PlayerStats.Money < damageOverTimeUpgradeCost[damageOverTimeLevel-1])
                    {
                        Debug.LogWarning("Not enough money up upgrade!");
                        return;
                    }

                    if (damageOverTimeLevel >= damageOverTimeList.Length)
                    {
                        Debug.LogWarning("This option is already at maximum!");
                        return;
                    }

                    PlayerStats.Money -= damageOverTimeUpgradeCost[damageOverTimeLevel-1];
                    damageOverTimeLevel++;
                    
                }
                break;
            default:
                Debug.LogWarning(type + " is not an option to upgrade");
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeList[rangeLevel - 1]);
    }
}
