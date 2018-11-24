using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script attached to Test_Enemy_Minigun GameObject. It is reponsible for handling the
/// movement, shooting and health of the enemy.
/// After spirtes are made, it will also handle the anmiations
/// </summary>

public class Test_Enemy_Miniguns : MonoBehaviour {

    #region This Object's Variables
    [Header("This Object's Variables")]
    public Rigidbody2D thisRigidbody2D; //To hold the Rigidbody2D of this object
    #endregion

    #region Variables for movement
    [Header("Variables For Movement")]
    //Starting movement variables
    public Enemy_Starting_Points starting_Points; //A reference to the Enemy_Starting_Points. To be used for intial movement
    private int starting_Point; //An integer that will hold the number of the starting point where the enemy has to go to first
    private Vector2 starting_Point_Coordinates; //A Vector2 to hold the Starting_Points Coordinates
    public bool reachedStart = false; //Variable that turns true when this enemy almost has reached starting_Point_Coordinates

    //For general movements and direction
    private Vector2 movementDirection; //To hold the movementDirection
    #endregion

    #region Built in Functions
    //Start of Awake
    private void Awake()
    {
        //This Object references
        thisRigidbody2D = this.GetComponent<Rigidbody2D>(); //Setting thisRigidbody2D values here

        //Getting the number of the starting point here and the Enemy_Starting_Points class reference value
        starting_Point = Random.Range(0, 4); //Getting the point randomly here
        starting_Points = GameObject.FindGameObjectWithTag("Starting_Points").GetComponent<Enemy_Starting_Points>(); //Passing reference to class Enemy_Starting_Points here
    } //End of Awake

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Start of FixedUpdate
    private void FixedUpdate()
    {
        if (!reachedStart) //Runs only if reachedStart is false
        {
            MoveToStartingPoint(); //Calling MoveTOStartingPoint here 
        } //End of if statement
        else //If reachedStart is true
        {

        } //End of else statement
    } //End of FixedUpdate
    #endregion

    #region Custom Functions
    //A simple function that will move THIS enemy to the Starting Point
    public void MoveToStartingPoint()
    {
        starting_Point_Coordinates = new Vector2(starting_Points.enemy_Starting_Points[starting_Point].position.x,
            starting_Points.enemy_Starting_Points[starting_Point].position.y); //Getting the Starting point to move here

        Debug.Log("Starting point co-ordinates are " + starting_Point_Coordinates);

        movementDirection = Vector2.MoveTowards(thisRigidbody2D.position, starting_Point_Coordinates, 2.5f * Time.deltaTime); //Moving towards starting point here
        thisRigidbody2D.MovePosition(movementDirection); //Moving enemy to starting point here

        //And if statement that runs when this objects and starting_Point_Coordinates magnitude is less than 0.25
        if ((thisRigidbody2D.position - starting_Point_Coordinates).magnitude < 0.25f)
        {
            reachedStart = true; //Turning reachedStart to true here. Used to allow random movement and turn this function calling off
        } //End of if statement
    } //End of MoveToStartingPoint 
    #endregion
}
