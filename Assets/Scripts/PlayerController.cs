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
    #endregion
}
