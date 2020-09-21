using UnityEngine;

namespace ControllerInputs
{
    public class PlayerInputs : MonoBehaviour
    {
        private Animator animator;
        private CharacterController characterController;
        private Vector3 movement;
        [SerializeField] Transform cameraMain;
        private float movementSpeed = 3f;
        private float xAxis, zAxis;

        [SerializeField] LayerMask groundMask;
        [SerializeField] [Range(.1f, 1f)] float groundDistance = 1f;
        [SerializeField] Transform groundCheck;
        [SerializeField] Vector3 velocity;
        [SerializeField] bool isGrounded;
        private float gravity = -9.81f;
        private float gravityForce = 1.8f;
        private float jumpHeight = 1.2f;

        private float smoothBlend = .1f;

        [SerializeField] Transform followCamera;


        void Start()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            movementHandling();
            characterDirectionCamera();

            if (!animator.GetBool(AnimatorAshesh.arming))
                animator.SetBool(AnimatorAshesh.canAttack, false);

            if (animator.GetBool(AnimatorAshesh.isTargetLocked))
                movementSpeed = 1.7f;
            else
                movementSpeed = 3f;
        }

        /*
        Handles the movement of the charachter
        */
        private void movementHandling()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (!animator.GetBool(AnimatorAshesh.armingInProcess))
            {
                xAxis = Input.GetAxis("Horizontal");
                zAxis = Input.GetAxis("Vertical");

                movement = Camera.main.transform.right * xAxis + Camera.main.transform.forward * zAxis;
                handlingAnimationMovement();

                /*to let character change position when attacking*/
                if (animator.GetBool(AnimatorAshesh.isAttacking))
                    characterController.Move(movement * 0f * Time.deltaTime);
                else
                    characterController.Move(movement * movementSpeed * Time.deltaTime);

                gravityApplyer();
                jump();
            }
        }

        /*
        Add the gravity to a player so that when 
        he jumps is gonna fall down, or when i falls
        from anywhere
        */
        private void gravityApplyer()
        {
            if (isGrounded && velocity.y < .1f)
            {
                velocity.y = -.5f;
                animator.SetBool(AnimatorAshesh.isInAir, !isGrounded);
            }

            velocity.y += gravity * Time.deltaTime;

            characterController.Move(velocity * gravityForce * Time.deltaTime);
            animator.SetBool(AnimatorAshesh.isInAir, !isGrounded);
        }

        /*
        Set the speed for the animation
        */
        private void handlingAnimationMovement()
        {
            animator.SetFloat(AnimatorAshesh.horizontal, xAxis, smoothBlend, Time.deltaTime * 2);
            animator.SetFloat(AnimatorAshesh.vertical, zAxis, smoothBlend, Time.deltaTime * 2);
        }

        /*
        Changes the mod of the camera based on 
        the targetLocked flag, if the target is locked
        the camera is gonna face the direction of the player,
        if the targetLocked is false it is gonna free the camera
        */
        private void characterDirectionCamera()
        {
            if (!animator.GetBool(AnimatorAshesh.isTargetLocked))
            {
                Vector3 realDirection = Camera.main.transform.TransformDirection(new Vector3(xAxis, 0, zAxis));
                realDirection.y = 0;

                if (realDirection.magnitude > 0.1f)
                {
                    Quaternion newRotation = Quaternion.LookRotation(realDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 6);
                }
            }
            else
            {
                if (zAxis != 0 || xAxis != 0)
                {
                    followCamera.rotation = transform.rotation;
                    insiderCamSwap(transform, false);
                }
                else
                    insiderCamSwap(followCamera, true);
            }
        }

        /*
        Allows the player to jump when
        pressing the space button
        */
        private void jump()
        {/*
        *jumpFormula squareRoot(height * -2 * gravity);
        */
            if (Input.GetButtonDown("GamepadJump")
                && isGrounded && !animator.GetBool(AnimatorAshesh.arming))
            {
                animator.SetTrigger(AnimatorAshesh.isJumping);
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
        }

        #region Utility
        private void insiderCamSwap(Transform who,bool isFreed)
        {
            animator.SetBool(AnimatorAshesh.isCameraFreed, isFreed);
            who.rotation = Quaternion.Euler(0f, cameraMain.rotation.eulerAngles.y, 0f);
        }
        #endregion
    }
}
