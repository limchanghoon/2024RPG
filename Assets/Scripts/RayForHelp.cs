using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RayForHelp : MonoBehaviour
{
    private Camera _mainCamera;
    public float range;

    HelpForRay _help;
    Vector3 screenCenter;

    int layerMask;

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        layerMask = (-1) - (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Magic") | LayerMask.NameToLayer("Ignore Raycast"));
        screenCenter = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
    }

    private void Update()
    {
        RayCastForHelp();

        if (_help != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _help.Interact1();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                _help.Interact2();
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _help = null;
        GameManager.Instance.npcHelpUI.Close();
    }

    private void RayCastForHelp()
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        float _range = range;
        if (Physics.Raycast(ray, out hit, _range, layerMask))
        {       
            HelpForRay temp = hit.transform.GetComponent<HelpForRay>();
            if (temp == null)
            {
                if (_help == null) return;
                _help.CloseHelp();
                _help = null;
                return;
            }
            if (_help == temp) return;
            if (_help != null)
                _help.CloseHelp();
            _help = temp;
            _help.OpenHelp();
        }
        else
        {
            if (_help == null) return;
            _help.CloseHelp();
            _help = null;
        }
    }

    public void ResetHelp()
    {
        if (_help != null)
            _help.CloseHelp();
        _help = null;
    }

    public void TurnOff()
    {
        if (_help != null)
            _help.CloseHelp();
        _help = null;
        this.enabled = false;
    }

    public void TurnOn()
    {
        if (_help != null)
            _help.CloseHelp();
        _help = null;
        this.enabled = true;
    }
}
