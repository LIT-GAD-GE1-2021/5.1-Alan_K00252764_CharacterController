using UnityEngine;
using System.Collections;

public class AdventurerController : MonoBehaviour
{

    public float jumpSpeed;
    public float horizontalSpeed = 10;
    public LayerMask whatIsGround;
    public Transform groundcheck;
    private float groundRadius = 0.5f;
    private float headRadius = 0.3f;
    private bool grounded;
    private bool jump;
    bool facingRight = true;
    private float hAxis;
    private Rigidbody2D theRigidBody;
    private Animator theAnimator;


    void Start()
    {
        jump = false;
        grounded = false;
        theRigidBody = GetComponent<Rigidbody2D>();
        theAnimator = GetComponent<Animator>();

    }

    void Update()
    {
  
        jump = Input.GetKeyDown(KeyCode.Space);
        hAxis = Input.GetAxis("Horizontal");
        theAnimator.SetFloat("hspeed", Mathf.Abs(hAxis));

        // Every frame i.e. everytime Unity calls Update, call the Physics2D.Overlap function
        // which takes three parameters:
        //  1. the position around which to "draw" the circle
        //  2. the radius of the circle
        //  3. the layer to check for overlaps in
        //
        // The function returns the Collider2D component (e.g. the BoxCollider2D component, or the
        // CircleCollider2D component, etc) of the game object the circle collides with. If it doesn't
        // collide with any game object then it returns null.
        Collider2D colliderWeCollidedWith = Physics2D.OverlapCircle(groundcheck.position, groundRadius, whatIsGround);

        /*
         * To convert one variable type to another we must "cast" it. In order to cast a variable we place
         * the type we want to cast it to infront of the variable name. I'm casting a variable of type
         * Collider2D to a variable of type bool, if the Collider2D variable contains a value (i.e. a Collider2D
         * object) then bool it is converted to will be true, otherwise it will be false. I store this 'converted'
         * value if the variable grounded.
         */
        grounded = (bool)colliderWeCollidedWith;

        // The Animator Controller attached to the Animator component has a property called Ground
        // which the Animator Controller uses to transition from one state to another. We must set
        // this Ground property to true when the Hero is on the ground and false otherwise.
        //
        // Because the Ground property on the Animator Controller is a boolean we need to use the
        // SetBool function to set it (see it in use below).

        theAnimator.SetBool("ground", grounded);

        // The Animator has a vspeed parameter which should be set to the vertical (y) velocity of
        // the character. This is used by the Animator in a blend tree to blend various 'falling'
        // animations depending on the velocity the character is falling at.

        // First the the y velocity of the character
        float yVelocity = theRigidBody.velocity.y;

        // Now use it to set the vspeed parameter
        theAnimator.SetFloat("vspeed", yVelocity);



        if (grounded)
        {
            if ((hAxis > 0) && (facingRight == false))
            {
                Flip();
            }
            else if ((hAxis < 0) && (facingRight == true))
            {
                Flip();
            }
        }

    }

    /*
     * The FixedUpdate get called at fixed intervals by Unity and this is the function you use to apply
     * forces to your game objects as this function is used by Unity to keep the Physics system up-to-date.
     * You should try to keep the code within this function to a bare minimum as we don't want to slow down
     * the physics system.
     */
    void FixedUpdate()
    {

        // If not jumping then allow the character to be moved left or right
        if (grounded && !jump)
        {
            /* Set the velocity of the RigidBody component attached to the game object. We will keep the 
             * vertical velocity the same (it should be 0 anyway as the character is in the ground) but we will 
             * set the horizontal velocity to be equal to the horizontalSpeed * hAxis i.e. some value
             * between -horizontalSpeed and +horizontalSpeed
             */

        }
        else if (grounded && jump)
        {
            // Set the velocity, this time we keep the horizontal velocity the same but change the vertical (y)
            // velocity to jumpSpeed
            theRigidBody.velocity = new Vector2(theRigidBody.velocity.x, jumpSpeed);
        }

    }

    /*
     * This is a new function that I wrote to flip the character in the x direction. The idea is simple. When 
     * the game starts make sure that the character is facing right (it asumes that the artist has drawn the
     * character and all it's animations facing right). We will keep track of whether the character is facing
     * right using the variable facingRight which will be either true or false (initially true). Anytime we
     * want to flip the character we toggle the value of the facingRight variable and flip the character. To 
     * flip the character we change the sign of it's x scale i.e. if it's x scale is 1 we change it to -1 and 
     * if it is -1 we change it to 1. To change the sign of any number simply multiply it by -1.
     */
    private void Flip()
    {
        //saying facingright is equal to not facingright (we are facing the opposite direction)
        facingRight = !facingRight;

        // Get the local scale. Local scale, similar to local position and rotation, is the scale of the
        // game object relative to it's parent. Sine this game object has no parent it's local scale is the
        // same as it's global scale

        // Every Unity script has access to a variable called transform which contains the Transform component
        // attached to this game object.

        Vector3 theScale = transform.localScale;

        //flip the x axis
        theScale.x *= -1;

        //apply it back to the local scale
        transform.localScale = theScale;
    }


}
