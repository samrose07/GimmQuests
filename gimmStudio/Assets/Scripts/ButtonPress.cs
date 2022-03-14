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
        //making sure its not been pressed
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //starting that press. Jumps the button to a "down" position to give appearance of pressing it!
        //invokes the onPress unity method.
        //TODO: add a sound effect?
        if(!isPressed)
        {
            
            button.transform.localPosition = new Vector3(0, 0.24f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
            //print(presser.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       // print("exit");
       //makes sure that the presser of the button is the one exiting the collision,
       //resets the position of the button, invokes the onRelease unity method.
        if(other.gameObject == presser)
        {
            print("released");
            button.transform.localPosition = new Vector3(0, 0.54f, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }
}
