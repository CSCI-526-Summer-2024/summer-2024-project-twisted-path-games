using UnityEngine;
using UnityEngine.AI;
namespace Controllers
{
    public class HunterController : MonoBehaviour
    {
        public float speed;
        public float increasedSpeed;
        float horizontalInput;
        float verticalInput;

        // Hunter rigid body component
        private Rigidbody rb;
        private CapsuleCollider capsuleCollider;

        public GameObject target;
        public NavMeshAgent agent;
        public bool isChaseActive = true;

        public float thresholdDistance = 5.0f;

        private bool isCurrentlyChasing = false;
        void Start()
        {
            // Prevent rotation based on physics interactions
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.speed = speed;

            capsuleCollider = GetComponent<CapsuleCollider>();

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
                capsuleCollider.enabled = true;

                if (CheckHuntedProximity())
                {
                    if (!isCurrentlyChasing)
                    {
                        isCurrentlyChasing = true;
                        GameState.numAggro++;
                        Debug.Log("Chase started!");
                        if (GameState.firstChase == -1.0f)
                        {
                            GameState.firstChase = Time.time; // Record the start time of the first chase
                        }
                    }

                    Debug.Log("Chasing!");
                    agent.SetDestination(target.transform.position);
                }
                else
                {
                    if (isCurrentlyChasing)
                    {
                        isCurrentlyChasing = false;
                        Debug.Log("Chase ended!");
                    }
                }
            }
            else
            {
                agent.isStopped = true;
                rb.isKinematic = false;
                capsuleCollider.enabled = false;
                GetInputs();
                MoveHunter();
            }
        }

        public void UpdateTarget(GameObject newTarget)
        {
            target = newTarget;
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

        private bool CheckHuntedProximity()
        {
            float distance = Vector3.Distance(target.transform.position, rb.transform.position);
            return distance <= thresholdDistance;
        }
    }

}