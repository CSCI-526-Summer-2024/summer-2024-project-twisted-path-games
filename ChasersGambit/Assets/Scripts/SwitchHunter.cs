using Unity.VisualScripting;
using UnityEngine;
using Controllers;

public class SwitchHunter : MonoBehaviour
{
    public GameObject hunted1;

    public GameObject hunted2;

    public GameObject hunter1;
    public GameObject hunter2;
    public Camera hunted1Cam;
    public Camera hunted2Cam;
    private MonoBehaviour[] _scripts;
    private bool _isHunter1Enabled;
    
    // Start is called before the first frame update
    void Start()
    {
        GameState.DisableGo(hunted2);
        GameState.EnableGo(hunted1);
        _isHunter1Enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchHunterFocus();
            //UpdateHunter();
        }
    }

    void SwitchHunterFocus()
    {
        hunter2.GetComponent<HunterController>().controlled = !hunter2.GetComponent<HunterController>().controlled;
        hunter1.GetComponent<HunterController>().controlled = !hunter1.GetComponent<HunterController>().controlled;
    }

    void UpdateHunter()
    {
        if (_isHunter1Enabled)
        {
            hunter1.GetComponent<HunterController>().hunted = hunted2;
            Debug.Log("Hunter is hunting P2");
        }
        else
        {
            hunter1.GetComponent<HunterController>().hunted = hunted1;
            Debug.Log("Hunter is hunting P1");
        }
        _isHunter1Enabled = !_isHunter1Enabled;
    }

}
