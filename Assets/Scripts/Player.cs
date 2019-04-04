using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Configuration Params
    [Header("Player:")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] int health = 200;
    int maxHP;

    [Header("Projectile:")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileFiringPeriod   = 0.1f;
    [SerializeField] float projectileSpeed          = 12f;
    [Header("Projectile SFX")]
    [SerializeField] AudioClip projectileSound;
    [SerializeField] float projectileSoundVolume = 1;
    [Header("Death VFX")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 3f;
    [Header("SFXs")]
    [SerializeField] AudioClip DeathSound;
    [SerializeField] float DeathSoundVolume = 1;
    [SerializeField] AudioClip HitTakenSound;
    [SerializeField] float HitTakenSoundVolume = 1;

    Coroutine isFiringCoroutine;
    bool isFiring = false;

    float xMin;
    float xMax;
    float yMin;
    float yMax;


    // Use this for initialization
    void Start () {
        maxHP = health;
        SetUpMoveBoundaries();
        StartCoroutine(RecoverHP());
    }

    // Update is called once per frame
    void Update () {
        Move();
        Fire();
    }

    private void Move() {
        // Player moves backwards at 0.7x speed.
        var deltaX = Input.GetAxis("Horizontal") > 0 ?
            Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed:
            Input.GetAxis("Horizontal") * Time.deltaTime * (moveSpeed * 0.7f);
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPosition = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPosition = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPosition, newYPosition);
    }

    private void SetUpMoveBoundaries() {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    IEnumerator RecoverHP() {
        while (true) {
            if (isFiring == false && health < maxHP) {
                health += 1;
            }
            yield return new WaitForSeconds(1f);
        }      
    }

    private void Fire() {
        if (Input.GetButtonDown("Fire1")) {
            isFiringCoroutine = StartCoroutine(FireContinously());
            isFiring = true;
        }
        if (Input.GetButtonUp("Fire1")) {
            StopCoroutine(isFiringCoroutine);
            isFiring = false;
        }
    }

    IEnumerator FireContinously() {
        while (true) {
            GameObject laser = Instantiate(
                laserPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1),
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, 0);
            AudioSource.PlayClipAtPoint(projectileSound, Camera.main.transform.position, projectileSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        if (!damageDealer)
            return;
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer) {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (this.health <= 0)
            KillPlayer();
        else
            PlayerTookHit();
    }

    private void PlayerTookHit() {
        AudioSource.PlayClipAtPoint(HitTakenSound, Camera.main.transform.position, HitTakenSoundVolume);
    }

    private void KillPlayer() {
        AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, DeathSoundVolume);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, gameObject.transform.position, Quaternion.identity);
        Destroy(explosion, durationOfExplosion);
        FindObjectOfType<SceneLoader>().LoadGameOver();
    }
}
