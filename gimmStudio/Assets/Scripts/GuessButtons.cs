using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessButtons : MonoBehaviour
{
    [Tooltip("Which button is this? Order left to right, 1-3")]
    public int whichButton;

    public void ThisButtonPressed()
    {
        print("pressed");
        JeanneManager jm = GameObject.Find("JeanneManager").GetComponent<JeanneManager>();
        if (jm != null) jm.AnswerGiven(this, whichButton);
    }
}
