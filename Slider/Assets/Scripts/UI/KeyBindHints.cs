using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyBindHints : MonoBehaviour
{
    private InputSettings controls;
    public KeyTimer[] keyTimers;
    public TextMeshProUGUI hintText;
    public GameObject canvas;
    public static bool doubleSizeMode = false;
    public static bool highContrastMode = false;
    private int currNumHintsDisplayed = 0;

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
        public bool enabled;
    }

    private void Awake()
    {
        controls = new InputSettings();
        controls.Player.Move.performed += context => CheckControls(KeyTimer.ControlTypes.WASD);
        controls.Player.CycleEquip.performed += context => CheckControls(KeyTimer.ControlTypes.Q);
        controls.Player.Action.performed += context => CheckControls(KeyTimer.ControlTypes.E);
        controls.UI.OpenArtifact.performed += context => CheckControls(KeyTimer.ControlTypes.Tab);
    }

    void Start()
    {
        hintText.gameObject.SetActive(false);
        hintText.text = "";
        canvas.SetActive(false);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        foreach (KeyTimer kt in keyTimers)
        {
            if (!kt.displayed)
            {
                kt.timeUntilHint -= Time.deltaTime;
                if (kt.timeUntilHint <= 0 && !kt.enabled)
                {
                    //StopAllCoroutines();
                    currNumHintsDisplayed++;
                    DisplaySentence(kt);
                    kt.enabled = true;
                }
            }
        }
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

    public void DisplaySentence(KeyTimer kt)
    {
        string hint = kt.message;
        CheckSize();
        canvas.SetActive(true);
        hintText.gameObject.SetActive(true);
        //StartCoroutine(TypeSentence(hint));
        hintText.text += hint + "\n";
        StartCoroutine(HintWait(kt));
    }

    IEnumerator HintWait(KeyTimer kt)
    {
        yield return new WaitForSeconds(5);
        RemoveHint(kt);
    }

    public void RemoveHint(KeyTimer kt)
    {
        print(hintText.text);
        if (currNumHintsDisplayed > 1)
        {
            hintText.text = hintText.text.Substring(0, hintText.text.IndexOf(kt.message)) + hintText.text.Substring(hintText.text.IndexOf(kt.message) + kt.message.Length + 1);
        }
        else
        {
            canvas.SetActive(false);
            hintText.gameObject.SetActive(false);
            hintText.text = "";
        }
        kt.enabled = false;
        kt.timeUntilHint = 10;
        //kt.displayed = false;
        currNumHintsDisplayed--;
    }


    private void CheckSize()
    {
        if (doubleSizeMode)
        {
            canvas.transform.localScale = new Vector3(2, 2, 2);
        }
        else
        {
            canvas.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}

