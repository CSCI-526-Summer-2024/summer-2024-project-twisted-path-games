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
                    Debug.Log("Chasing!");
                    agent.SetDestination(target.transform.position);
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