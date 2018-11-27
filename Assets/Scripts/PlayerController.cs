using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A script attached to the Player. Handles movement, life, shooting and weapon changes
/// </summary>

public class PlayerController : MonoBehaviour {

    #region Variables and declarations
    [Header("Movement Variables")]
    public float health = 5;
    public float speed = 2;
    public float carbineFireDelay = 0.3f;
    public float shotgunFireDelay = 0.25f;
    public float minigunFireDelay = 0.10f;

    public enum WeaponSelected
    {
        SemiAutoCarbine,
        Shotgun,
        MiniGun
    }

    [Header("Weapon Variables")]
    public GameObject bullet;
    public Transform bulletSpawn;
    public WeaponSelected weaponSelected = WeaponSelected.SemiAutoCarbine;

    [Header("Flicker Player variables")]
    public SpriteRenderer spriteRenderer; //To be used for the flicker effect
    public Color spriteColor; //To be used to store the color and allow the flicker effect by minipulating it's a value
    public bool isFlickering; //Turns true when player is flickering
    public float flickerDuration;
    public int flickerCount;

    private WaitForSeconds flickerWait; //Used in FlickerPlayer IEnumerator. Controls the flickering of the player.

    [Space]
    public AudioClip rev;

    [HideInInspector]
    public bool canMove = true, canShoot = true, damagable = true;
    [HideInInspector]
    public Vector3 mousePosition;

    // How different vertical speed is from horizontal speed
    private float verticalSpeedModifier;
    private Animator anim;
    private Rigidbody2D rb2d;
    private AudioSource audio;
    private float NextFire = 0;
    private bool playRev = true;
    #endregion

    #region Built-in Functions
    private void Awake() //Start of Awake
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;
        flickerWait = new WaitForSeconds(flickerDuration);
    } //End of Awake

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        verticalSpeedModifier = speed * 1.2f;
    } //End of Start

    //Start of Update
    private void Update()
    {
        // Set animation trigger for wheelie
        anim.SetFloat("Speed", Input.GetAxis("Horizontal"));

        // Play engine rev one time when going left
        if (Input.GetAxis("Horizontal") > 0 && playRev) {
            audio.PlayOneShot(rev);
            playRev = false;
        }
        if (Input.GetAxis("Horizontal") <= 0) {
            playRev = true;
        }

        // If fire clicked and firerate time has passed, fire
        if (Input.GetButton("Fire1") && Time.time > NextFire && canShoot)
        {
            Vector3 mouseClickPositon = GetMousePosition(); //Getting firing direction here
            switch (weaponSelected)
            {
                case WeaponSelected.SemiAutoCarbine:
                    NextFire = Time.time + carbineFireDelay; //Adding firing delay
                    Instantiate(bullet, bulletSpawn.position, GetAngleToMouse(bulletSpawn.position, GetMousePosition())); //Firing bullet here
                    break;
                case WeaponSelected.Shotgun:
                    NextFire = Time.time + shotgunFireDelay; //Adding firing delay
                    Quaternion bullet1FireAngle = GetAngleToMouse(bulletSpawn.position, GetMousePosition(), 15);
                    Quaternion bullet2FireAngle = bullet1FireAngle * Quaternion.Euler(0, 0, 45);
                    Quaternion bullet3FireAngle = bullet1FireAngle * Quaternion.Euler(0, 0, -45);
                    Instantiate(bullet, bulletSpawn.position, bullet1FireAngle); //Firing bullet1 here
                    Instantiate(bullet, bulletSpawn.position, bullet2FireAngle); //Firing bullet2 here
                    Instantiate(bullet, bulletSpawn.position, bullet3FireAngle); //Firing bullet3 here
                    break;
                case WeaponSelected.MiniGun:
                    NextFire = Time.time + minigunFireDelay; //Adding firing delay
                    Instantiate(bullet, bulletSpawn.position, GetAngleToMouse(bulletSpawn.position, GetMousePosition())); //Firing bullet here
                    break;
            }
            audio.Play();
        }
    }


    //Start of Fixed Update
    public void FixedUpdate()
    {
        DoMovement();
    } //End of FixedUpdate

    /// <summary>
    /// Start of OnCollisionEnter2D. To be used for various effects, including flicker effect
    /// and handling of player damage
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFlickering) {
            return;
        }

        // I know this is kindof redundant but we're pressed for time
        if (!damagable)
            return;

        if (collision.gameObject.CompareTag("EnemyBullet")) //Handling collisions with EnemyBullet tagged objects here. Use this to control player damage and effects on player
        {
            TakeDamage();
            StartCoroutine(FlickerPlayer()); //Starting FlickerPlayer routine to start flickering player here
        } //End of if statement
    } //End of OnCollisionEnter

    /// <summary>
    /// Start of OnTriggerEnter2D. To be used for various effects, including flicker effect
    /// and handling of player damage
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision) {
        if (isFlickering) {
            return;
        }
        if (collision.gameObject.CompareTag("Explosion"))
        {
            TakeDamage();
            StartCoroutine(FlickerPlayer()); //Starting FlickerPlayer routine to start flickering player here
        }
    }

    private void TakeDamage() {

        // Don't take damage if player is invulnerable
        if (!damagable)
            return;

        health = Math.Max(0, health - 1);
        isFlickering = true;

        if (health <= 0) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    #endregion

    #region Custom Functions
    private void DoMovement()
    {
        if (canMove)
        {
            rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * verticalSpeedModifier);
        }

    }

    /// <summary>
    /// A function that takes the MousePosition in world space and returns it.
    /// Useful for weapons and shooting direction
    /// </summary>
    public Vector3 GetMousePosition()
    {
        mousePosition = Input.mousePosition; //Getting the position of the mouse click here
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); //Converting the click to world space co-ordinates here

        return mousePosition;
    }

    public Quaternion GetAngleToMouse(Vector3 obj, Vector3 mouse, float clampAngle = 30) {

        // Find angle in radians between obj and mouse
        // convert radians to degrees
        // clamp rotation values between -'clampAngle' and 'clampAngle' degrees to prevent being able to shoot at odd angles
        // convert degrees to quaternion
        return Quaternion.Euler(0, 0, Mathf.Clamp(Mathf.Atan2(obj.y - mouse.y, obj.x - mouse.x) * Mathf.Rad2Deg, -clampAngle, clampAngle));
    }

    /// <summary>
    /// Sets the Weapon Selected. This can be changed from a powerup / key bindings.
    /// </summary>
    /// <param name="newWeaponSelected"> the new weapon that needs to be selected.</param>
    public void SetSelectedWeapon(WeaponSelected newWeaponSelected)
    {
        weaponSelected = newWeaponSelected;
    }

    /// <summary>
    /// A function that will flickerThePlayer sprite overtime to give it a retro hit effect. Uses seconds
    /// Called in OnCollisionEnter2D
    /// </summary>
    IEnumerator FlickerPlayer()
    {
        for (var i = 0; i < flickerCount; i++) {
            spriteColor.a = 0.0f;
            spriteRenderer.color = spriteColor;
            yield return flickerWait;
            spriteColor.a = 1.0f;
            spriteRenderer.color = spriteColor;
            yield return flickerWait;
        }
        isFlickering = false;

        yield return null;
    } //End of FlickerPlayer
    #endregion
}
