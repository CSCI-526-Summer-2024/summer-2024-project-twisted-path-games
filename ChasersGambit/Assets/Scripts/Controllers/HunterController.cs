using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
namespace Controllers
{
    public class HunterController : MonoBehaviour
    {
        // Speed fields
        private float speed = 2.8f; // normal chase speed
        private float patrolSpeed = 2;
        private float increasedSpeed = 3; // speed when the player gains control

        // Direction Controls
        float horizontalInput;
        float verticalInput;

        // Patrolling fields
        public Transform[] patrolPoints;
        private int currentPatrolIndex;

        private float positionThreshold = 0.1f;
        private float huntedProximityDistance = 5.0f;

        // Hunter rigid body component
        private Rigidbody rb;
        private CapsuleCollider capsuleCollider;
        public Light indicator;
        public bool lightOn = false;

        // Hunted target and AI fields
        public GameObject hunted1;
        public GameObject hunted2;
        
        public float hunted1Proximity;
        public float hunted2Proximity;
        
        private FlashlightToggle hunted1FlashlightToggle;
        private FlashlightToggle hunted2FlashlightToggle;
        
        private NavMeshAgent agent;

        // Control bools
        public bool isChaseActive = true;
        private bool isCurrentlyChasing = false;
        public bool controlled = false;

        void Start()
        {
            // Prevent rotation based on physics interactions
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            indicator.enabled = false;

            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.speed = speed;

            capsuleCollider = GetComponent<CapsuleCollider>();
            hunted1FlashlightToggle = hunted1.GetComponentInChildren<FlashlightToggle>();
            hunted2FlashlightToggle = hunted2.GetComponentInChildren<FlashlightToggle>();

            if (patrolPoints.Length > 0)
            {
                currentPatrolIndex = 0;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            }

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
                UpdateVignetteNew();
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
            indicator.enabled = false;
            hunted1Proximity = Vector3.Distance(hunted1.transform.position, rb.transform.position);
            hunted2Proximity = Vector3.Distance(hunted2.transform.position, rb.transform.position);

            if (hunted1Proximity < huntedProximityDistance && hunted1.activeSelf)
            {
                if (hunted1FlashlightToggle.isOn)
                {
                    StartChase(hunted1);   
                }
                else if(hunted2Proximity < huntedProximityDistance && hunted2.activeSelf)
                {
                    StartChase(hunted2);
                }
                else
                {
                    agent.SetDestination(agent.transform.position);
                }
            }

            else if (hunted2Proximity < huntedProximityDistance && hunted2.activeSelf)
            {
                if (hunted2FlashlightToggle.isOn)
                {
                    StartChase(hunted2);
                }
                else if(hunted1Proximity < huntedProximityDistance && hunted1.activeSelf)
                {
                    StartChase(hunted1);
                }
                else
                {
                    agent.SetDestination(agent.transform.position);
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

            agent.SetDestination(transform.position);

            if (controlled){
                GetInputs();
                MoveHunter();
                indicator.enabled = true;
            }
            else{
                indicator.enabled = false;
            }
        }

        void StartChase(GameObject hunted)
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
            
            agent.speed = speed;
            agent.SetDestination(hunted.transform.position);
            Debug.Log("Chasing!");
            
        }

        void HandlePatrolling()
        {
            agent.speed = patrolSpeed;
            if (isCurrentlyChasing)
            {
                isCurrentlyChasing = false;
                Debug.Log("Chase ended!");
            }

            if (agent.remainingDistance < positionThreshold && !agent.pathPending && patrolPoints.Length != 0)
            {
                // Move to the next patrol point
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
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
            float distance = Vector3.Distance(hunted1.transform.position, rb.transform.position);
            return distance <= huntedProximityDistance;
        }
        public float dangerProximityDistance = 3.0f;
        public Image dangerVignette;

        private void UpdateVignette()
        {
            if ((hunted1Proximity <= dangerProximityDistance && hunted1.activeSelf) || (hunted2Proximity <= dangerProximityDistance && hunted2.activeSelf))
            {
                ShowVignette(true);
            }
            else
            {
                ShowVignette(false);
            }
        }

        private void UpdateVignetteNew()
        {
            float distance = Mathf.Min(hunted1Proximity, hunted2Proximity);
            Color color = dangerVignette.color;
            if ((hunted1Proximity <= dangerProximityDistance && hunted1.activeSelf) || (hunted2Proximity <= dangerProximityDistance && hunted2.activeSelf))
            {
                color.a = Mathf.Clamp01(1 - (distance / dangerProximityDistance)) * 0.4f;
                dangerVignette.color = color;
            }      
            else {
                color.a = 0;
                dangerVignette.color = color;
            }
    
        }

        private void ShowVignette(bool show)
        {
            Color color = dangerVignette.color;
            color.a = show ? 0.5f : 0.0f; // Adjust the alpha value to make it visible/invisible
            dangerVignette.color = color;
        }
    }
}