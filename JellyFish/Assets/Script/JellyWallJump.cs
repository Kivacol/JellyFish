using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyWallJump : MonoBehaviour
{
    Rigidbody2D rb;
    JellyJump jellyJump;
    JellyMove jellyMove;

    [SerializeField] public bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed;

    [SerializeField] public bool isWallJumping;
    [SerializeField] private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime;
    [SerializeField] private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuraion;

    [SerializeField] private float wallJumpForceX;
    [SerializeField] private float wallJumpForceY;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jellyJump = GetComponent<JellyJump>();
        jellyMove = GetComponent<JellyMove>();
    }

    // Update is called once per frame
    void Update()
    {
        WallSlid();
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    void WallSlid()
    {
        if(IsWalled() && !jellyJump.isGround)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        else
        {
            isWallSliding = false;
        }
    }

    public void WallJump()
    {
        if (isWallSliding)
        {
            jellyJump.canDoubleJump = false;

            if (isWallSliding)
            {
                isWallJumping = false;
                wallJumpingCounter = wallJumpingTime;

                CancelInvoke(nameof(StopWallJumping));
            }
            else
            {
                wallJumpingCounter -= Time.deltaTime;
            }

            if (wallJumpingCounter > 0f)
            {
                isWallJumping = true;
                wallJumpingCounter = 0f;

                if (jellyMove.isFacingRight)
                {                    
                    rb.velocity = new Vector2(-transform.localScale.x * wallJumpForceX, wallJumpForceY);
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else if (!jellyMove.isFacingRight)
                {
                    rb.velocity = new Vector2(transform.localScale.x * wallJumpForceX, wallJumpForceY);
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }

                Invoke(nameof(StopWallJumping), wallJumpingDuraion);
            }
        }
    }

    void StopWallJumping()
    {
        isWallJumping = false;
    }
}
