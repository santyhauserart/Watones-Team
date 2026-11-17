using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Player player;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int maxBullets = 3;
    [SerializeField] private int degrees = 30;
    private float nextShootTime;
    private List<GameObject> bullets = new List<GameObject>();
    private void OnEnable()
    {
        nextShootTime = Time.time + 1f;
    }
    private void Update()
    {
        if (Time.time >= nextShootTime)
        {
            Shoot();
            nextShootTime = Time.time + fireRate;
        }
    }
    private void Shoot()
    {
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;
        float bulletCenter = (0 + maxBullets - 1) / 2f;
        for (int i = 0; i < maxBullets; i++)
        {
            float angle = (i - bulletCenter) * degrees;
            Vector2 turned = Quaternion.Euler(0, 0, angle) * playerDirection;
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            newBullet.GetComponent<BulletEnemy>().SetDirection(turned);
            bullets.Add(newBullet);
        }
    }
    public void ClearBullets()
    {
        foreach (GameObject bullet in bullets)
        {
            if (bullet != null)
            {
                Destroy(bullet);
            }
        }
        bullets.Clear();
    }
}
