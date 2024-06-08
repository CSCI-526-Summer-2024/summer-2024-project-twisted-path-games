using UnityEngine;
using UnityEngine.AI;

public class HunterController : MonoBehaviour
{
    public float speed;
    public float increasedSpeed;
    float horizontalInput;
    float verticalInput;

    // Hunter rigid body component
    Rigidbody rb;

    public GameObject hunted;
    public NavMeshAgent agent;
    public bool isChaseActive = true; 

    void Start()
    {
        // Prevent rotation based on physics interactions
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.speed = speed;

        if (isChaseActive)
        {
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (isChaseActive)
        {
            agent.isStopped = false;
            agent.SetDestination(hunted.transform.position);
        }
        else
        {
            agent.isStopped = true;
            rb.isKinematic = false;
            GetInputs();
            MoveHunter();
        }
    }

    // Get keyboard inputs
    private void GetInputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    // Move Hunter independent of frame rate
    private void MoveHunter()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * increasedSpeed * verticalInput);
        transform.Translate(Vector3.right * Time.deltaTime * increasedSpeed * horizontalInput);
    }
}
