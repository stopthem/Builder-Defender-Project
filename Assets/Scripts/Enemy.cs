using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position)
    {
        Transform enemyPrefab = Resources.Load<Transform>("Enemy");
        Transform enemyTransform = Instantiate(enemyPrefab , position , Quaternion.identity);

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }
    
    private Transform target;
    private Rigidbody2D rigidbody2d;
    private HealthSystem healthSystem;
    private float lookForTargetTimer, lookForTargetTimerMax = .2f;
    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        if (BuildingManager.Instance.GetHQBuiliding() != null)
        {
            target = BuildingManager.Instance.GetHQBuiliding().transform;
        }
        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        Destroy(gameObject);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargeting();
    }

    private void HandleTargeting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void HandleMovement()
    {
        if (target.position != null)
        {
            Vector3 moveDirection = (target.position - transform.position).normalized;
            float moveSpeed = 6f;
            rigidbody2d.velocity = moveDirection * moveSpeed;
        }
        else
        {
            rigidbody2d.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Building building = other.gameObject.GetComponent<Building>();
        if (building != null)
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
            Destroy(gameObject);
        }
    }
    private void LookForTargets()
    {
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Building building = collider2D.GetComponent<Building>();
            if (building != null)
            {
                if (target == null)
                {
                    target = building.transform;
                }
                else
                {
                    if (Vector3.Distance(transform.position, building.transform.position) < Vector3.Distance(transform.position , target.position))
                    {
                        target = building.transform;
                    }
                }
            }
        }
        if (target == null)
        {
            if (BuildingManager.Instance.GetHQBuiliding() != null)
            {
                target = BuildingManager.Instance.GetHQBuiliding().transform;
            }
            
        }
    }
}
