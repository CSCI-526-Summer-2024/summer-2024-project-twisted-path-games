using Unity.VisualScripting;
using UnityEngine;
using Controllers;

public class SwitchHunted : MonoBehaviour
{
    public GameObject hunted1;
    public Camera hunted1Cam;
    public GameObject hunted1PlayIcon;

    public GameObject hunted2;
    public Camera hunted2Cam;
    public GameObject hunted2PlayIcon;

    public GameObject[] hunters = new GameObject[0];

    private HunterController hunterController;
    private bool _isHunted1Enabled;
    
    // Start is called before the first frame update
    void Start()
    {
        GameState.DisableGo(hunted2);
        GameState.EnableGo(hunted1);
        _isHunted1Enabled = true;

        hunted1PlayIcon.SetActive(true);

        Debug.Log(GameState.SessionId);
        
        SetCameraPerspective();

        if (hunters.Length != 0)
        {
            hunterController = hunters[0].GetComponent<HunterController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hunterController != null && hunterController.isChaseActive && Input.GetKeyDown(KeyCode.Space) && !GameState.DidAnyHuntedExit)
        {
            SwitchHuntedFocus();
            //UpdateHunter();
            _isHunted1Enabled = !_isHunted1Enabled;
            SetCameraPerspective();

            GameState.NumberOfSwitches++;
        }
    }

    void SwitchHuntedFocus()
    {
        if (_isHunted1Enabled)
        {
            GameState.DisableGo(hunted1);
            GameState.EnableGo(hunted2);
            
            
            hunted1PlayIcon.SetActive(false); 
            hunted2PlayIcon.SetActive(true);

            //hunted1.SetActive(false);
        }
        else
        {
            GameState.DisableGo(hunted2);
            GameState.EnableGo(hunted1);

            hunted1PlayIcon.SetActive(true);
            hunted2PlayIcon.SetActive(false);

            //hunted2.SetActive(false);
        }
    }

    void SetCameraPerspective()
    {
        hunted1Cam.GameObject().SetActive(_isHunted1Enabled);
        hunted2Cam.GameObject().SetActive(!_isHunted1Enabled);
    }
    
}
