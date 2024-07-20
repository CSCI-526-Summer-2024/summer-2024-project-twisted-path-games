using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Controllers;
using System.Collections;

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

    HuntedController hunted1Controller;
    HuntedController hunted2Controller;
    // Start is called before the first frame update
    void Start()
    {
        hunted1Controller = hunted1.GetComponent<HuntedController>();
        hunted2Controller = hunted2.GetComponent<HuntedController>();
        
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
            if (_isHunted1Enabled)
            {
                hunted1Controller.ToggleFlashlightOff();            
            }
            else
            {
                hunted2Controller.ToggleFlashlightOff();
            }
        
            _isHunted1Enabled = !_isHunted1Enabled;
            sleepFor1s();
            SetCameraPerspective();

            GameState.NumberOfSwitches++;
        }
    }

    IEnumerator sleepFor1s()
    {
        yield return new WaitForSeconds(1f);        
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
        if (_isHunted1Enabled)
        {
            hunted1Controller.ToggleFlashlightOn();            
        }
        else
        {
            hunted2Controller.ToggleFlashlightOn();
        }
    }
    
}
