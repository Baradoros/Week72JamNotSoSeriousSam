using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script attached to the Player. Handles movement, life, shooting and weapon changes
/// </summary>

public class PlayerController : MonoBehaviour {

    #region Variables and declarations
    [Header("Movement Variables")]
    public float health = 5;
    public float speed = 2;
    public float fireDelay = 0.5f;

    [Header("Weapon Variables")]
    public GameObject bullet;
    public Transform bulletSpawn;

    [Header("Flicker Player variables")]
    public SpriteRenderer spriteRenderer; //To be used for the flicker effect
    public Color spriteColor; //To be used to store the color and allow the flicker effect by minipulating it's a value
    private WaitForSeconds flickerWait; //Used in FlickerPlayer IEnumerator. Controls the flickering of the player.
    public bool isFlickering; //Turns true when player is flickering

    [Space]
    public AudioClip rev;

    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public Vector3 mousePosition; //To hold the mousePosition

    private float verticalSpeedModifier;
    private Animator anim;
    private Rigidbody2D rb2d;
    private AudioSource audio;
    private float nextFire = 0;
    private bool playRev = true;
    #endregion

    #region Built-in Functions
    private void Awake() //Start of Awake
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;
        flickerWait = new WaitForSeconds(0.2f);
    } //End of Awake

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        verticalSpeedModifier = speed * 0.8f;
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
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            Vector3 mouseClickPositon = GetMousePosition(); //Getting firing direction here

            nextFire = Time.time + fireDelay; //Adding firing delay
            Instantiate(bullet, bulletSpawn.position, GetAngleToMouse(bulletSpawn.position, GetMousePosition())); //Firing bullet here
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
        if (collision.gameObject.CompareTag("EnemyBullet") && !isFlickering) //Handling collisions with EnemyBullet tagged objects here. Use this to control player damage and effects on player
        {
            StartCoroutine(FlickerPlayer()); //Starting FlickerPlayer routine to start flickering player here
        } //End of if statement
    } //End of OnCollisionEnter

    /// <summary>
    /// Start of OnTriggerEnter2D. To be used for various effects, including flicker effect
    /// and handling of player damage
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Explosion") && !isFlickering) //Handling collisions with EnemyBullet tagged objects here. Use this to control player damage and effects on player
        {
            StartCoroutine(FlickerPlayer()); //Starting FlickerPlayer routine to start flickering player here
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

    public Quaternion GetAngleToMouse(Vector3 obj, Vector3 mouse) {

        // Find angle in radians between obj and mouse
        // convert radians to degrees
        // clamp rotation values between -30 and 30 degrees to prevent being able to shoot at odd angles
        // convert degrees to quaternion
        return Quaternion.Euler(0, 0, Mathf.Clamp(Mathf.Atan2(obj.y - mouse.y, obj.x - mouse.x) * Mathf.Rad2Deg, -30, 30));
    }

    /// <summary>
    /// A function that will flickerThePlayer sprite overtime to give it a retro hit effect. Uses seconds
    /// Called in OnCollisionEnter2D
    /// </summary>
    IEnumerator FlickerPlayer()
    {
        isFlickering = true; //Turning this variable true here so that the player does not receive any other hit until flickering is complete

        //First Flicker
        spriteColor.a = 0.0f;
        spriteRenderer.color = spriteColor;
        yield return flickerWait;
        spriteColor.a = 1.0f;
        spriteRenderer.color = spriteColor;
        yield return flickerWait;

        //Second Flicker
        spriteColor.a = 0.0f;
        spriteRenderer.color = spriteColor;
        yield return flickerWait;
        spriteColor.a = 1.0f;
        spriteRenderer.color = spriteColor;
        yield return flickerWait;

        //Third Flicker
        spriteColor.a = 0.0f;
        spriteRenderer.color = spriteColor;
        yield return flickerWait;
        spriteColor.a = 1.0f;
        spriteRenderer.color = spriteColor;
        yield return flickerWait;

        isFlickering = false;

        yield return null;
    } //End of FlickerPlayer
    #endregion
}
