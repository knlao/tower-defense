using UnityEngine;

public class Bullet : MonoBehaviour
{
    /*
     * Bullet Script
     * Player's bullet
     */
    
    [Header("Bullet Behaviour")]
    public float speed = 70f;
    public string enemyTag;
    public Vector3 enemyOffset;

    [Header("Impact")]
    public float explosionRadius = 0f;
    public AnimationCurve explosionRate;
    public GameObject impactEffect;
    public int damage = 50;
    
    private Transform _target;
    private Vector3 _targetPos;

    public void Seek(Transform target)
    {
        _target = target;
    }

    private void Update()
    {
        try
        {
            if (_target == null)
            {
                HitTarget();
            }
            _targetPos = _target.position;

            var dir = _targetPos + enemyOffset - transform.position;
            var distanceThisFrame = speed * Time.deltaTime;
        
            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }
        
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            transform.rotation = Quaternion.LookRotation(dir);
        }
        catch (MissingReferenceException e)
        {
            print("we got an error, but nothing really happened: " + e.Message);
        }
    }

    private void HitTarget()
    {
        var effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        if (explosionRadius > 0)
        {
            Explode();
        }
        else
        {
            if (_target != null)
                Damage(_target);
        }
        
        Destroy(gameObject);
    }

    private void Explode()
    {
        FindObjectOfType<SFXManager>().PlayClip(2);
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider1 in colliders)
        {
            if (collider1.CompareTag(enemyTag))
            {
                Damage(collider1);
            }
        }
    }

    private void Damage(Component enemy)
    {
        var e = enemy.GetComponent<Enemy>();
        if (e != null)
        {
            if (explosionRadius > 0)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                e.TakeDamage(damage * explosionRate.Evaluate(1 - dist / explosionRadius));
            }
            else
            {
                e.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
