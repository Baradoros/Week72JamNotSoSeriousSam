using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script attached to Test_Enemy_Minigun GameObject. It is reponsible for handling the
/// movement, shooting and health of the enemy.
/// After spirtes are made, it will also handle the anmiations
/// </summary>

public class Test_Enemy_Miniguns : MonoBehaviour {

    #region This Object's Variables and Player Variables
    [Header("This Object's and Player Variables")]
    public Rigidbody2D thisRigidbody2D; //To hold the Rigidbody2D of this object
    public GameObject playerObject; //To hold the Player

    //Health and scoring
    public int health = 5;
    private int enemyScore = 50; // The ammount of score this enemy adds when it dies
    #endregion

    #region Variables for shooting
    [Header("Variables for shooting")]
    public float nextFire; //When this variable turns zero, it will trigger a bullet fire
    public GameObject enemyBullet; //To hold the enemyBullet
    public Transform bulletSpawner; //To hold bulletSpawner
    #endregion

    #region Variables for movement
    [Header("Variables For Movement")]
    //Starting movement variables
    public Enemy_Starting_Points starting_Points; //A reference to the Enemy_Starting_Points. To be used for intial movement
    private int starting_Point; //An integer that will hold the number of the starting point where the enemy has to go to first
    private Vector2 starting_Point_Coordinates; //A Vector2 to hold the Starting_Points Coordinates
    public bool reachedStart = false; //Variable that turns true when this enemy almost has reached starting_Point_Coordinates

    //For Random movement
    public Enemy_Random_Points random_Points; //A reference to the Enemy_Random_Points class. To be used for Random movement
    /// <summary>
    /// An integer that will hold the number of the random point where the enemy has to move to
    /// </summary>
    private int random_Point;
    private Vector2 random_Point_Coordinates; //A Vector 2 to hold the random_Point coordinates
    private float time2Move; //A variable that controls movement time. To be used to control movement rate
    private float movementTimer; //A variable that when goes greater than time2Move's value, will trigger movement

    //For general movements and direction
    private Vector2 movementDirection; //To hold the movementDirection
    #endregion

    #region Built in Functions
    //Start of Awake
    private void Awake()
    {
        //This Object references
        thisRigidbody2D = this.GetComponent<Rigidbody2D>(); //Setting thisRigidbody2D values here
        playerObject = GameObject.FindGameObjectWithTag("Player"); //Find Player by tag and passing it here. To be used to locate player for shooting

        //Getting the number of the starting point here and passing value of starting_Points
        starting_Point = Random.Range(0, 4); //Setting a random value here
        starting_Points = GameObject.FindGameObjectWithTag("Starting_Points").GetComponent<Enemy_Starting_Points>(); //Passing reference to class Enemy_Starting_Points here

        //Getting the number of the random point here and passing value of random_Points
        random_Point = Random.Range(0, 19); //Setting a random value here
        random_Points = GameObject.FindGameObjectWithTag("Random_Points").GetComponent<Enemy_Random_Points>(); //Passing reference to class Enemy_Random_Points here
    } //End of Awake

    // Use this for initialization
    void Start()
    {
        time2Move = Random.Range(1.0f, 2.0f); //Giving a random value to time2Move
        nextFire = Random.Range(1.5f, 2.0f); //Giving a random firing rate here
    } //End of Start

    // Update is called once per frame
    void Update()
    {

        // Check if HP <= 0 and kill this enemy if true
        if (health <= 0) {
            Die();
        }
    }

    //Start of FixedUpdate
    private void FixedUpdate()
    {
        nextFire -= Time.deltaTime; //Substracting Time.deltaTime to move Next Fire towards zero.

        //Running this only when nextFire is less than zero and enemy has teached starting point
        if(nextFire < 0 && reachedStart)
        {
            Debug.Log("Bullet was fired");
            ShootBullet(); //Calling ShootBullet here
        } //End of if statement

        if (!reachedStart) //Runs only if reachedStart is false
        {
            MoveToStartingPoint(); //Calling MoveTOStartingPoint here 
        } //End of if statement
        else if(time2Move >= 0) //Run only If reachedStart is false and time2Move is greater than zero
        {
            MoveToRandomPoint(); //Calling MoveToRandomPoint here
        } //End of else statement
    } //End of FixedUpdate

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == 11) { // Player layers start at 8, 11 is PlayerProjectile
            TakeDamage(1);
        }
    }
    #endregion

    #region Custom Functions
    //A simple function that will move THIS enemy to the Starting Point
    public void MoveToStartingPoint()
    {
        starting_Point_Coordinates = new Vector2(starting_Points.enemy_Starting_Points[starting_Point].position.x,
            starting_Points.enemy_Starting_Points[starting_Point].position.y); //Getting the coordinates here


        movementDirection = Vector2.MoveTowards(thisRigidbody2D.position, starting_Point_Coordinates, 2.5f * Time.deltaTime); //Moving towards starting point here
        thisRigidbody2D.MovePosition(movementDirection); //Moving enemy to starting point here

        //And if statement that runs when this objects and starting_Point_Coordinates magnitude is less than 0.25
        if ((thisRigidbody2D.position - starting_Point_Coordinates).magnitude < 0.25f)
        {
            reachedStart = true; //Turning reachedStart to true here. Used to allow random movement and turn this function calling off
        } //End of if statement
    } //End of MoveToStartingPoint 

    /// <summary>
    /// A function that will be called after the enemy has reached startig point.
    /// it is responsible for moving the player towards the random point and is responsible for changing the
    /// points as well
    /// </summary>
    public void MoveToRandomPoint()
    {
        time2Move -= Time.deltaTime; //Substracting Time.dealtaTime frm time2Move to reduce it's value

        random_Point_Coordinates = new Vector2(random_Points.enemy_Random_Points[random_Point].position.x,
            random_Points.enemy_Random_Points[random_Point].position.y); //Getting the coordinates here


        movementDirection = Vector2.MoveTowards(thisRigidbody2D.position, random_Point_Coordinates, 2.5f * Time.deltaTime); //Moving towards random point here
        thisRigidbody2D.MovePosition(movementDirection); //Moving enemy to random point here

        if(time2Move < 0) //Running only when time2move is less than zero
        {
            random_Point = Random.Range(0, 19); //Setting a new Random value here
            time2Move = Random.Range(1.0f, 1.5f); //Setting a Random Range for time2move here
        } //End of if statement
    } //End of MoveToRandomPoint

    /// <summary>
    /// A function that will be called everytime nextFire will turn zero.
    /// This will fire a bullet
    /// </summary>
    public void ShootBullet()
    {
        Vector3 dir = playerObject.transform.position - bulletSpawner.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bulletSpawner.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Instantiate(enemyBullet, bulletSpawner.position, bulletSpawner.rotation); //Instantiating the bullet here

        //Using this if statement to set a new value for NextFire
        if(nextFire <= 0)
        {
            nextFire = Random.Range(1.5f, 2.0f);
        } //End of if statement
    } //End of ShootBullet

    // Method to deal damage to this enemy
    public void TakeDamage(int damage) {
        health -= damage;
    }

    // Method called when this enemy reaches 0HP
    public void Die() {
        GameManager.manager.score += enemyScore;
        Destroy(gameObject);
    }
    #endregion
}
