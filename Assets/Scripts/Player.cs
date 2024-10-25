using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody rb;
    [SerializeField]private Transform modelTransform;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private string meshObjectName = "MeshObject"; // will be fix
    private float currentSpeed;
    private bool isRunning;
    public float collectionRadius = 5f;

    private Animator animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();

                //if (_animator == null)
                //{
                //    Debug.LogError("Animator component not found in children!");
                //}
            }
            return _animator;
        }
    }

    private void Awake()
    {
        Transform meshTransform = transform.Find(meshObjectName);
        if (meshTransform != null)
        {
            _animator = meshTransform.GetComponent<Animator>();
        }
        rb = GetComponent<Rigidbody>();
        //if (rb == null)
        //{
        //    Debug.LogError("Rigidbody not found!");
        //}

        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimations();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, collectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Loot"))
            {
                hitCollider.GetComponent<Loot>().MoveToPlayer();
            }
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isRunning = true;
            currentSpeed = runSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            isRunning = false;
            currentSpeed = moveSpeed;
        }

        if (Input.GetMouseButtonDown(0) && animator != null)
        {
            animator.SetTrigger("Shot");
        }
    }

    private void HandleMovement()
    {
        if (rb == null) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            rb.velocity = new Vector3(
                movement.x * currentSpeed,
                rb.velocity.y,
                movement.z * currentSpeed
            );
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        float movementMagnitude = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

        if (movementMagnitude < 0.1f)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }
        else if (movementMagnitude > 5f && !isRunning)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Walk", true);
            animator.SetBool("Run", false);
        }
        else if (isRunning)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Run", true);
        }
    }
}