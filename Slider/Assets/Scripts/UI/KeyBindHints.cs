using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindHints : MonoBehaviour
{
    private InputSettings controls;
    public KeyTimer[] keyTimers;


    [System.Serializable]
    public class KeyTimer
    {
        public enum ControlTypes
        {
            WASD,
            E,
            Q,
            Tab,
        }
        public ControlTypes controlType;
        public float timeUntilHint;
        public string message;
        public bool displayed;
    }

    private void Awake()
    {
        controls = new InputSettings();
        controls.Player.Move.performed += context => CheckControls(KeyTimer.ControlTypes.WASD);
        controls.Player.CycleEquip.performed += context => CheckControls(KeyTimer.ControlTypes.Q);
        controls.Player.Action.performed += context => CheckControls(KeyTimer.ControlTypes.E);
        controls.UI.OpenArtifact.performed += context => CheckControls(KeyTimer.ControlTypes.Tab);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
    }

    void Update()
    {
        foreach (KeyTimer kt in keyTimers)
        {
            if (!kt.displayed)
            {
                kt.timeUntilHint -= Time.deltaTime;
                if (kt.timeUntilHint <= 0)
                {
                    DisplayHint(kt);
                }
            }
        }
    }

    private void DisplayHint(KeyTimer kt)
    {
        Debug.Log(kt.message);
    }

    public void CheckControls(KeyTimer.ControlTypes controlType)
    {
        foreach (KeyTimer kt in keyTimers)
        {
            if (kt.controlType == controlType)
            {
                kt.displayed = true;
            }
        }
    }
}