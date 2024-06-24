using UnityEngine;
using UnityEngine.AI;
namespace Controllers
{
    public class HunterController : MonoBehaviour
    {
        public float speed;
        public float paceSpeed;
        public float increasedSpeed;
        float horizontalInput;
        float verticalInput;

        private Vector3 startLocation;
        public Vector3 checkPoint1;
        public Vector3 checkPoint2;
        private float positionThreshold = 0.1f;
        private Vector3 currentMovementTarget;

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

            startLocation = transform.position;
            currentMovementTarget = startLocation;

            if (isChaseActive)
            {
                rb.isKinematic = true;
            }
    }

        void Update()
        {
            if (isChaseActive)
            {
                HandleAgent();
            }
            else
            {
                HandlePlayerControl();
            }
         }

        private void HandleAgent()
        {
            agent.isStopped = false;
            capsuleCollider.enabled = true;

            if (CheckHuntedProximity())
            {
                StartChase();
            }
            else if (GameState.DidAnyHuntedExit)
            {
                if (IsAtPosition(transform.position, startLocation))
                {
                    HandlePatrolling();
                }
                else
                {
                    ReturnToStartLocation();
                }
            }
            else
            {
                HandlePatrolling();
            }
        }

        void HandlePlayerControl()
        {
            agent.isStopped = true;
            rb.isKinematic = false;
            capsuleCollider.enabled = false;
            GetInputs();
            MoveHunter();
        }

        void StartChase()
        {
            currentMovementTarget = target.transform.position;
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
            agent.speed = speed;
            agent.SetDestination(target.transform.position);
        }

        void ReturnToStartLocation()
        {
            currentMovementTarget = startLocation;
            agent.SetDestination(startLocation);
        }

        void HandlePatrolling()
        {
            if (isCurrentlyChasing)
            {
                isCurrentlyChasing = false;
                Debug.Log("Chase ended!");
            }
            else
            {
                if (IsAtPosition(transform.position, currentMovementTarget))
                {
                    UpdatePatrolTarget();
                }
            }
        }

        void UpdatePatrolTarget()
        {
            agent.speed = paceSpeed;
            if (currentMovementTarget == startLocation || currentMovementTarget == checkPoint2)
            {
                currentMovementTarget = checkPoint1;
                agent.SetDestination(checkPoint1);
                Debug.Log("Moving to checkPoint1");
            }
            else if (currentMovementTarget == checkPoint1)
            {
                currentMovementTarget = checkPoint2;
                agent.SetDestination(checkPoint2);
                Debug.Log("Moving to checkPoint2");
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

        private bool IsAtPosition(Vector3 currentPos, Vector3 checkPoint)
        {
            return Vector3.Distance(currentPos, checkPoint) < positionThreshold;
        }
    }

}