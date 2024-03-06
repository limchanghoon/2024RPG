using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    private void Start()
    {
        layerMask = (-1) - (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Magic"));
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

    private void RayCastForHelp()
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, layerMask))
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
}
