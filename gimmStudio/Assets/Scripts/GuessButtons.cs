/* This script was created by Samuel Rose for the GIMMStudio space, focused for quests.
 * 
 * The purpose of this script is to provide a light framework for the buttons.
 *              
 * Biodigital jazz, man
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessButtons : MonoBehaviour
{
    [Tooltip("Which button is this? Order right to left, for some reason, 1-3")]
    public int whichButton;
    [SerializeField] private bool isStart = false;
    [SerializeField] private GameObject attachedText;
    //once pressed, send the managerclass the object and which button it is in the order. Called by onRelease when the button is done
    //OR by when the non-vr player presses the respective interact key.
    public void ThisButtonPressed()
    {
        print("pressed");
        JeanneManager jm = GameObject.Find("JeanneManager").GetComponent<JeanneManager>();
        if (isStart)
        {
            jm.CanNowGo();
            transform.parent.gameObject.SetActive(false);
            attachedText.SetActive(false);
            return;
        }
        if (jm != null) jm.AnswerGiven(this, whichButton);
    }
}
