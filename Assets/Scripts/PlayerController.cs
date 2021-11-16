using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player Controller designed for non-gravity world. 
//Wanting exact precision over variable jumps & falls with no other physics to worry about makes this style work
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigi;

    private bool slamming = false;
    private bool jumping = false;
    private bool grounded = false;
    private bool walled = false;

    private float distToGround;

    private int score = 0;
    private float scoreTimer = 1;   //How long to walk before 1 point added to score

    private float jumpHoldTimer = .4f;

    private float jumpPower = 100;
    private float jumpSubtractHeld = -1500f;
    private float jumpSubtractRelease = -2000f;
    private float maxFall = -1000;
    private float slamSpeed = -5000f;

    private float forwardSpeed = 5f;
    private float forwardInc = .05f;   //Speed to add every point increase

    private int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();

        distToGround = GetComponent<Collider2D>().bounds.extents.y;

        layerMask = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        grounded = IsGrounded();
        walled = AgainstWall();

        if (!slamming && !walled)   //Add constant Horizontal Movement
        {
            transform.position = new Vector3(
                transform.position.x + ((forwardSpeed + (forwardInc * score)) * Time.deltaTime), transform.position.y, transform.position.z);
        }

        if (slamming && grounded)
        {
            slamming = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (grounded)   //Start jump off ground
            {
                rigi.AddForce(Vector2.up * jumpPower);
            }
            else if (jumping) //Hold jump to get higher
            {
                rigi.AddForce(Vector2.up * (jumpSubtractHeld * Time.deltaTime));
            }
            else if (!slamming)   //Slam down from the air. Prevent multi-slam from multi-clicks
            {
                rigi.AddForce(Vector2.up * (slamSpeed * Time.deltaTime));
                slamming = true;
            }
        }
        else
        {
            if (!grounded && !slamming)  //Subtract from jump until maxFall speed
            {
                rigi.AddForce(Vector2.up * (jumpSubtractRelease * Time.deltaTime));
            }
        }

        if (transform.position.y < -30) //Safety Check in case player drops below death plane to prevent softlock
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private bool IsGrounded(){
        //Debug.DrawRay(transform.position, Vector2.down, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distToGround + .1f, layerMask);
        return (hit.collider != null && hit.transform.tag == "Ground");  //Prevent jumping off collectibles or spikes
    }

    private bool AgainstWall(){
        RaycastHit2D hit = (Physics2D.Raycast(transform.position, Vector2.right, distToGround + .1f, layerMask));
        if (hit.collider != null && hit.transform.tag == "Ground")  //Prevent stopping at collectibles or spikes
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Collectible")
        {
            score++;
        } else if (other.tag == "Hurt")
        {
            Die();
        }
    }
}
