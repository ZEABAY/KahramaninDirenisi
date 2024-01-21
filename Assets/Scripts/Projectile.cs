using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public string targetTag; // Ekstra �zellik: vurulacak hedefin etiketi
    private GunSystem gunSystem;
    private void Awake()
    {
        gunSystem = GameObject.FindGameObjectWithTag("GunSystem").GetComponent<GunSystem>();

        if (targetTag == "Enemy")
        {
            damage = gunSystem.damage;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(targetTag))
        {
            // Vurulan hedefin etiketini kontrol et
            if (targetTag == "Player")
            {
                // Player'a hasar verme kodu
                PlayerHit();
            }
            else if (targetTag == "Enemy")
            {
                // D��man'a hasar verme kodu

                EnemyHit(collision.collider);
            }
        }
        gameObject.GetComponent<ParticleSystem>().Play();
        float duration = GetComponent<ParticleSystem>().main.duration;
        Destroy(gameObject, duration);
    }

    private void PlayerHit()
    {
        gunSystem.health -= damage;
    }

    private void EnemyHit(Collider enemyCollider)
    {

        Enemy enemy = enemyCollider.GetComponent<Enemy>();

        gunSystem.enemiesKilled += enemy.TakeDamage(damage);

    }

}