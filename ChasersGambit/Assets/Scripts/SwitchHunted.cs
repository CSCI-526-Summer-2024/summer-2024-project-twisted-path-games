using Unity.VisualScripting;
using UnityEngine;
using Controllers;

public class SwitchHunted : MonoBehaviour
{
    public GameObject hunted1;

    public GameObject hunted2;

    public GameObject hunter;
    public Camera hunted1Cam;
    public Camera hunted2Cam;
    private MonoBehaviour[] _scripts;
    private bool _isHunted1Enabled;
    
    // Start is called before the first frame update
    void Start()
    {
        GameState.DisableGo(hunted2);
        GameState.EnableGo(hunted1);
        _isHunted1Enabled = true;

        SetCameraPerspective();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !GameState.DidAnyHuntedExit)
        {
            SwitchHuntedFocus();
            UpdateHunter();
            SetCameraPerspective();
        }
    }

    void SwitchHuntedFocus()
    {
        if (_isHunted1Enabled)
        {
            GameState.DisableGo(hunted1);
            GameState.EnableGo(hunted2);
        }
        else
        {
            GameState.DisableGo(hunted2);
            GameState.EnableGo(hunted1);
        }
    }

    void UpdateHunter()
    {
        if (_isHunted1Enabled)
        {
            hunter.GetComponent<HunterController>().hunted = hunted2;
            Debug.Log("Hunter is hunting P2");
        }
        else
        {
            hunter.GetComponent<HunterController>().hunted = hunted1;
            Debug.Log("Hunter is hunting P1");
        }
        _isHunted1Enabled = !_isHunted1Enabled;
    }

    void SetCameraPerspective()
    {
        hunted1Cam.GameObject().SetActive(_isHunted1Enabled);
        hunted2Cam.GameObject().SetActive(!_isHunted1Enabled);
    }
    
}
