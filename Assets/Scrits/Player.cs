using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private Camera eyes;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float smooth;
    float _smoothValx, _smoothValZ;
    [SerializeField]
    private float jumpHeight = 10f;
    [SerializeField]
    private float gravity = -9.81f;
    private Vector3 playerVelocity;
    private bool grounded;
    float mRotationY = 0f;
    [SerializeField]
    private float lookSensitivity;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        grounded = true;
    }


    void Update()
    {
        Movement();
        Camera();
    }

    public void Movement()
    {

        grounded = controller.isGrounded;
        Vector2 smoothedInput = SmoothedInput();
        float horizontal = smoothedInput.x;
        float vertical = smoothedInput.y;
        Vector3 move = transform.forward * vertical * speed + transform.right * horizontal * speed;

        move.y = 0f;
        controller.Move(move * Time.deltaTime);

        if (grounded)
        {
            playerVelocity.y = -1f;
        }


        if (Input.GetButton("Jump") && grounded)
        {
            playerVelocity.y = 0f;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -1f * gravity);
        }

        var flags = controller.Move(playerVelocity * Time.deltaTime);
        bool collidedUp = (flags & CollisionFlags.CollidedAbove) != 0;
        if (collidedUp)
        {
            playerVelocity.y = 0f;
        }
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

    }

    public void Camera()
    {
        float mouse_x = Input.GetAxis("Mouse X");
        float mouse_y = Input.GetAxis("Mouse Y");
        transform.Rotate(0f, mouse_x * lookSensitivity, 0f);
        mRotationY += mouse_y * lookSensitivity;
        mRotationY = Mathf.Clamp(mRotationY, -90f, 90f);
        eyes.transform.localEulerAngles = new Vector3(-mRotationY, eyes.transform.localEulerAngles.y, eyes.transform.localEulerAngles.z);

    }


    public Vector2 SmoothedInput()
    {
        float dead = 0.001f;
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        float rX, ry;

        //Horizontal
        float targetx = input.x;
        _smoothValx = Mathf.MoveTowards(_smoothValx, targetx, smooth * Time.unscaledDeltaTime);
        rX = (Mathf.Abs(_smoothValx) < dead) ? 0f : _smoothValx;

        //Vertical
        float targety = input.y;
        _smoothValZ = Mathf.MoveTowards(_smoothValZ, targety, smooth * Time.unscaledDeltaTime);
        ry = (Mathf.Abs(_smoothValZ) < dead) ? 0f : _smoothValZ;

        return new Vector2(rX, ry);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("End"))
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadSceneAsync(nextSceneIndex);
            }
            else
            {
                Debug.LogWarning("No more scenes to load. You're at the last scene.");
            }
        }
    }
}
