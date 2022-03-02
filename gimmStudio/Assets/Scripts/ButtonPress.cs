using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
public class ButtonPress : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;

    bool isPressed;

    private void Start()
    {
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isPressed)
        {
            
            button.transform.localPosition = new Vector3(0, 0.24f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
            print(presser.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("exit");
        if(other.gameObject == presser)
        {
            print("released");
            button.transform.localPosition = new Vector3(0, 0.54f, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }
}
