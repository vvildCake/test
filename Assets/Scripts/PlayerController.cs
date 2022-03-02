using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float WalkSpeed = 4f;
    public float CrouchWalkSpeed = 2f;
    public float MidairSteerAccel = 1f;
    public float JumpForce = 100f;

    public float Health = 100f;

    interface IMoveMode
    {
        void OnJump();
        void OnCrouch();
        float GetSpeed(float direction);
    }

    IMoveMode currentMoveMode;

    IMoveMode moveWalk;
    IMoveMode moveCrouch;
    IMoveMode moveMidair;
    
    float distToGround;
    Vector3 halfLenght;
    bool wasInMidair;
    Rigidbody rigidBody;
    float lastVelocityX;
    float jumpVelocityX;
    
    void Awake()
    {
        moveWalk = new MoveModeWalk { owner = this };
        moveCrouch = new MoveModeCrouch { owner = this };
        moveMidair = new MoveModeMidair { owner = this };
        
        currentMoveMode = moveWalk;
        rigidBody = GetComponent<Rigidbody>();

        var bounds = GetComponent<BoxCollider>().bounds;
        distToGround = bounds.extents.y + 0.05f;
        halfLenght = new Vector3(bounds.extents.x, 0, 0);
    }

    //Should be called in Update() to properly process inputs and calculate expected velocity
    public void Move(float direction, bool jumpRequested, bool crouchRequested)
    {
        if(jumpRequested)
            currentMoveMode.OnJump();
        else if(crouchRequested)
            currentMoveMode.OnCrouch();

        JumpUpdate();
        
        Vector3 delta = new Vector3();
        float velocityX = currentMoveMode.GetSpeed(direction);
        delta.x += Time.fixedDeltaTime * velocityX;

        lastVelocityX = velocityX;
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        var lava = collisionInfo.gameObject.GetComponent<Lava>();
        if(lava == null)
            return;
        
        //OnCollisionStay called during physics update, using fixedDeltaTime
        Health -= lava.DamagePerSecond * Time.fixedDeltaTime; 
    }

    //Applying calculated velocity in FixedUpdate so it's in sync with the rest of physics engine
    void FixedUpdate()
    {
        var velocity = rigidBody.velocity;
        velocity.x = lastVelocityX;
        rigidBody.velocity = velocity;
    }

    void JumpStart(float force)
    {
        rigidBody.AddForce(force*Vector3.up);
        jumpVelocityX = lastVelocityX;
        currentMoveMode = moveMidair;
        wasInMidair = true;
    }

    void JumpLand()
    {
        currentMoveMode = moveWalk;
        wasInMidair = false;
    }

    void JumpUpdate()
    {
        if(!wasInMidair && IsInMidair())
            JumpStart(force: 0f); //start a free fall if we reach a cliff
        else if(wasInMidair && !IsInMidair())
            JumpLand(); //land once we reach the ground
    }

    void CrouchingStart()
    {
        transform.localScale = Vector3.one;
        transform.localPosition += 0.5f*Vector3.down;
        currentMoveMode = moveCrouch;
    }

    void CrouchingStop()
    {
        transform.localScale = new Vector3(1.0f, 2.0f, 1.0f);
        transform.localPosition += 0.5f*Vector3.up;
        currentMoveMode = moveWalk;
    }
    
    bool IsInMidair()
    {
        var pos = transform.position;
        //We are in midair if neither our front nor our back is on the ground
        return
            !Physics.Raycast(pos + halfLenght, Vector3.down, out _, distToGround) &&
            !Physics.Raycast(pos - halfLenght, Vector3.down, out _, distToGround);
    }
    
    abstract class MoveModeBase : IMoveMode
    {
        public PlayerController owner;
        
        public abstract void OnJump();
        public abstract void OnCrouch();
        public abstract float GetSpeed(float direction);
    }

    class MoveModeWalk : MoveModeBase
    {
        public override void OnJump()
        {
            owner.JumpStart(owner.JumpForce);
        }

        public override void OnCrouch()
        {
            owner.CrouchingStart();
        }

        public override float GetSpeed(float direction)
        {
            return direction*owner.WalkSpeed;
        }
    }

    class MoveModeCrouch : MoveModeBase
    {
        //Both jump and crouch buttons make you stop crouching 
        public override void OnJump()
        {
            owner.CrouchingStop();
        }

        public override void OnCrouch()
        {
            owner.CrouchingStop();
        }

        public override float GetSpeed(float direction)
        {
            return direction*owner.CrouchWalkSpeed;
        }
    }
    
    class MoveModeMidair : MoveModeBase
    {
        //Not allowing neither double jumps nor crouching in midair for now, doing nothing here
        public override void OnJump() { }
        public override void OnCrouch() { }

        public override float GetSpeed(float direction)
        {
            owner.jumpVelocityX += direction * owner.MidairSteerAccel * Time.fixedDeltaTime;
            return owner.jumpVelocityX;
        }
    }
}
