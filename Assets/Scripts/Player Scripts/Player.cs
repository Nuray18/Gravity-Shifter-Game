using System.Collections;
using Unity.Burst;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private CharacterController characterController;

    private Vector3 gravityDir = Vector3.down;
    private Vector3 velocity;

    public bool canChangeGravity = true;

    public float speed = 5f;
    public float jumpForce = 4f;
    public float gravity = 9.81f;

    public float gravityRotationSpeed = 5f;
    public float gravityChangeCooldown = 10f;

    private float nextGravityChangeTime = 0f;

    private bool isWalking = false;
    public LayerMask surfaceLayer;

    //private Rigidbody rb;

    void Awake()
    {
        //rb = GetComponent<Rigidbody>();

        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckGravityChange();
        MoveThePlayer();
    }

    void MoveThePlayer()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 move = transform.TransformDirection(input) * speed;

        isWalking = move != Vector3.zero;

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            velocity = -gravityDir * Mathf.Sqrt(jumpForce * 2f * gravity);
        }

        // Only apply gravity if not grounded
        if (!IsGrounded())
        {
            velocity += gravityDir * gravity * Time.deltaTime;
        }

        characterController.Move((move + velocity) * Time.deltaTime);
    }

    public bool IsWalking() => isWalking; // lambda function

    public bool IsPlayerGrounded() // public to have access IsGrounded()
    {
        return IsGrounded(); // Calls the private IsGrounded method
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, gravityDir, characterController.height / 2 + 0.1f, surfaceLayer);
    }


    void CheckGravityChange()
    {
        // burda zaman ve farklilik kontrolu yapiliyor
        if (Time.time < nextGravityChangeTime || !canChangeGravity)// Eger koyulan sure siniri gecmemis ise RETURN yap. Eger sure gecmis ise ama canChangeGravity = false ise RETURN 
        {
            return;
        }

        Vector3 newGravityDir = GetNewGravityDirection();

        if(newGravityDir != gravityDir) // ayni yuzey directionunda degil ise gravity yi degistir
        {
            nextGravityChangeTime = Time.time + gravityChangeCooldown;
            StartCoroutine(ChangeGravityAndRotatePlayer(newGravityDir));
        }
    }

    Vector3 GetNewGravityDirection()
    {
        // burda ise degisim uygulaniyor
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 1 tuşu
        {
            return Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // 2 tuşu
        {
            return Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // 3 tuşu
        {
            return Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) // 4 tuşu
        {
            return Vector3.down;
        }

        return gravityDir;
    }

    IEnumerator ChangeGravityAndRotatePlayer(Vector3 newGravityDir) // taking the gravityDir from ChangeGravityDirection Function
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -newGravityDir) * transform.rotation;
        float rotationDuration = 1f / gravityRotationSpeed;
        float elapsedTime = 0f;

        characterController.center = new Vector3(0, characterController.center.y, 0);

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        gravityDir = newGravityDir;
        velocity = Vector3.zero;

        characterController.Move(gravityDir * 0.1f);
    }

    public void ResetVelocity()
    {
        velocity = Vector3.zero;
    }

    public void ResetGravity()
    {
        gravityDir = Vector3.down;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}


