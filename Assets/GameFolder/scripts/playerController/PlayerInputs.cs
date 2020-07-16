using UnityEngine;
using Cinemachine;

namespace ControllerInputs
{
    public class PlayerInputs : MonoBehaviour
    {
        private Animator animator;
        private CharacterController characterController;
        private Vector3 movement;
        [SerializeField] Transform cameraMain;
        [SerializeField] float movementSpeed = 5f;
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
        }

        private void movementHandling()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (!animator.GetBool("isAttacking") && !animator.GetBool("armingInProcess"))
            {
                xAxis = Input.GetAxis("Horizontal");
                zAxis = Input.GetAxis("Vertical");

                movement = Camera.main.transform.right * xAxis + Camera.main.transform.forward * zAxis;
                handlingAnimationMovement();

                characterController.Move(movement * movementSpeed * Time.deltaTime);

                gravityApplyer();
                jump();
            }
        }

        private void gravityApplyer()
        {
            if (isGrounded && velocity.y < .1f)
            {
                velocity.y = -.5f;
                animator.SetBool("isInAir", !isGrounded);
            }

            velocity.y += gravity * Time.deltaTime;

            characterController.Move(velocity * gravityForce * Time.deltaTime);
            animator.SetBool("isInAir", !isGrounded);
        }

        private void handlingAnimationMovement()
        {
            animator.SetFloat("horizontal", xAxis, smoothBlend, Time.deltaTime * 2);
            animator.SetFloat("vertical", zAxis, smoothBlend, Time.deltaTime * 2);
        }

        private void characterDirectionCamera()
        {
            if (!animator.GetBool("isTargetLocked"))
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
                transform.rotation = Quaternion.Euler(0f, cameraMain.rotation.eulerAngles.y, 0f);
            }
        }


        private void jump()
        {/**
        *jumpFormula squareRoot(height * -2 * gravity);
        */
            if (Input.GetKeyDown(KeyCode.Space)
                && isGrounded && !animator.GetBool("arming"))
            {
                animator.SetTrigger("isJumping");
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
        }
    }
}
