using UnityEngine;


public class Perspective : MonoBehaviour
{
    //toggle this to switch perspectives
    public bool fpv = true;

    //camera position for top down viewing
    private Vector3 topDownPosition = new Vector3(0,22,0);
    
    //camera angle viewing from the top
    private Quaternion topDownAngle = Quaternion.Euler(90,0,0);

    //camera angle viewing from the hunted
    private Quaternion fpvAngle = Quaternion.Euler(0,0,0);
    
    //placeholder start location for camera
    //todo, replace this with something to be behind the character at all times
    private Vector3 mazeStart = new Vector3(-2,2,-13); 

    //increase this value for the transition to be more abrupt
    public float smooth = 10.0f;

    public GameObject player;

    //placeholder offset for being slightly behind character
    public Vector3 offset = new Vector3(0,10,0);

    private float x;
    private float y;
    public float sensitivity = -1f;
    private Vector3 rotate;

    // Start is called before the first frame update
    void Start()
    {
        //if (fpv)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (fpv) {
            //TODO position should be following the player
            //transform.position = mazeStart;

            //rotates camera back to 0,0,0
            //transform.rotation = Quaternion.Slerp(transform.rotation, fpvAngle, Time.deltaTime * smooth);
        }
        else {

            //moves camera to top of map
            transform.position = topDownPosition;
            //rotates camera to look down
            transform.rotation = Quaternion.Slerp(transform.rotation, topDownAngle, Time.deltaTime * smooth);
            
        }
    }
    void LateUpdate(){
        //follow behind the player
        if (fpv){
            transform.position = player.transform.position;

            //TODO camera rotation should also move with player rotation, to indicate direction faced
            x = Input.GetAxis("Mouse Y");
            y = Input.GetAxis("Mouse X");

            rotate = new Vector3(x, y * sensitivity, 0);
            transform.eulerAngles = transform.eulerAngles - rotate;
        }
    }
}
