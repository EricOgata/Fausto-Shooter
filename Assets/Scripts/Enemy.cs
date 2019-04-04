using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Enemy")]
    [SerializeField] float health = 10;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    float shotCounter;
    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 12f; // Negative Speed: The projectile is going left
    [Header("Projectile SFX")]
    [SerializeField] AudioClip projectileSound;
    [SerializeField] float projectileSoundVolume = 1;

    [Header("Death VFX")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [Header("Death SFX")]
    [SerializeField] AudioClip enemyDeathSound;
    [SerializeField] float enemyDeathSoundVolume = 1;


    // Use this for initialization
    void Start () {
        GenerateNewShotCounter();
    }

    // Update is called once per frame
    void Update () {
        CountDownAndShoot();
	}

    private void CountDownAndShoot() {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f) {
            Fire();
            GenerateNewShotCounter();
        }
    }

    private void GenerateNewShotCounter() {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void Fire() {
        GameObject laser = Instantiate(
                laserPrefab, transform.position,
                Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(projectileSound, Camera.main.transform.position, projectileSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer) {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (this.health <= 0) {
            Die();
        }
    }

    private void Die() {        
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, gameObject.transform.position, Quaternion.identity);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(enemyDeathSound, Camera.main.transform.position, enemyDeathSoundVolume);
    }
}
