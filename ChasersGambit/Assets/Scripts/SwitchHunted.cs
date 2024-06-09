using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        DisableGO(hunted2);
        EnableGO(hunted1);
        _isHunted1Enabled = true;

        SetCameraPerspective();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
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
            DisableGO(hunted1);
            EnableGO(hunted2);
        }
        else
        {
            DisableGO(hunted2);
            EnableGO(hunted1);
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
    
    void DisableGO(GameObject gb)
    {
        _scripts = gb.GetComponents<MonoBehaviour>();
        foreach(var script in _scripts)
        {
            script.enabled = false;
        }
    }

    void EnableGO(GameObject gb)
    {
        _scripts = gb.GetComponents<MonoBehaviour>();
        foreach (var script in _scripts)
        {
            script.enabled = true;
        }
    }

    void SetCameraPerspective()
    {
        hunted1Cam.GameObject().SetActive(_isHunted1Enabled);
        hunted2Cam.GameObject().SetActive(!_isHunted1Enabled);
    }
    
}
