using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigi;

    private bool slamming = false;
    private bool jumping = false;

    private float distToGround;

    private int score = 0;
    private float scoreTimer = 1;   //How long to walk before 1 point added to score

    private float jumpHoldTimer = .4f;

    private float jumpPower = 100;
    private float jumpSubtractHeld = -10f;
    private float jumpSubtractRelease = -20f;
    private float maxFall = -20;
    private float slamSpeed = -50f;

    private float forwardSpeed = 5f;
    private float forwardInc = .05f;   //Speed to add every point increase

    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();

        distToGround = GetComponent<Collider2D>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!slamming){
            transform.position = new Vector3(
                transform.position.x + ((forwardSpeed + (forwardInc * score)) * Time.deltaTime), transform.position.y, transform.position.z);
        }

        if (slamming && IsGrounded()){
            slamming = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (IsGrounded())   //Start jump off ground
            {
                rigi.AddForce(Vector2.up * jumpPower);
            } else if (jumping) //Hold jump to get higher
            {
                rigi.AddForce(Vector2.up * jumpSubtractHeld);
            } else if (!slamming)   //Slam down from the air. Prevent multi-slam from multi-clicks
            {
                rigi.AddForce(Vector2.up * slamSpeed);
                slamming = true;
            }
        } else
        {
            if (!IsGrounded())  //Subtract from jump until maxFall speed
            {
                rigi.AddForce(Vector2.up * jumpSubtractRelease);
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private bool IsGrounded(){
        return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f);
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
