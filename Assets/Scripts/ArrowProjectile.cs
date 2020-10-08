using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public static ArrowProjectile Create(Vector3 position, Enemy enemy)
    {
        
        Transform arrowTransform = Instantiate(GameAssets.Instance.ArrowProjectile, position, Quaternion.identity);

        ArrowProjectile arrowProjectile = arrowTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy);
        return arrowProjectile;
    }
    private Enemy targetEnemy;
    private Vector3 laswtMoveDirection;
    private float timeToDie = 2f;
    private void Update()
    {
        Vector3 moveDirection;
        if (targetEnemy != null)
        {
            moveDirection = (targetEnemy.transform.position - transform.position).normalized;
            laswtMoveDirection = moveDirection;
        }
        else
        {
            moveDirection = laswtMoveDirection;
        }
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDirection));
        laswtMoveDirection = moveDirection;
        float moveSpeed = 20f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        timeToDie -= Time.deltaTime;
        if (timeToDie <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void SetTarget(Enemy targetEnemy)
    {
        this.targetEnemy = targetEnemy;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            int damageAmount = 10;
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);
            Destroy(gameObject);
        }
    }
}
