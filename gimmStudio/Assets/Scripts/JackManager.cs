/* This script was created by Samuel Rose for the GIMMStudio space, focused for quests.
 * 
 * The purpose of this script is to gameify the robot and player interactions.
 * Currently working on it.
 * 
 * TODO: ALL OF IT LMAO
 *              
 * Biodigital jazz, man
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackManager : MonoBehaviour
{
    [SerializeField] private Pedestal[] robotPedestals;
    private int amountOfCorrectObjectsOnPedestals = 0;
    [SerializeField] private GameObject artifactGlass;
    [SerializeField] private GameObject artifact;
    private bool canAdd = true;
    [SerializeField] private AudioSource robotAudioSource;
    [SerializeField] private bool playSong = false;
    [SerializeField] private GameObject robot;
    [SerializeField] private float rotateY = -.1f;
    [SerializeField] private GameObject[] robotAttaches;
    bool canInvoke = true;

    private void Update()
    {
        CheckPedestals();
        if(playSong)
        {
            robot.transform.Rotate(new Vector3(0, rotateY, 0));
            if (canInvoke) Invoke("ChangeDir", 1);
            canInvoke = false;
        }
    }

    void ChangeDir()
    {
        rotateY *= -1;
        canInvoke = true;
    }

    void CheckPedestals()
    {
        amountOfCorrectObjectsOnPedestals = 0;
        foreach(Pedestal p in robotPedestals)
        {
            if (p.hasCorrectGO) amountOfCorrectObjectsOnPedestals++;
        }
        if (amountOfCorrectObjectsOnPedestals == 3) ExecuteOrderGiveArtifact();
    }

    void ExecuteOrderGiveArtifact()
    {
        if(canAdd)
        {
            artifactGlass.SetActive(false);
            artifact.AddComponent<Rigidbody>();
            robotAudioSource.Play();
            playSong = true;
            robot.GetComponent<JacksLilBuddy>().myArms.transform.Rotate(new Vector3(90, 0, 0));
            foreach (GameObject g in robotAttaches) g.SetActive(true);
        }
        canAdd = false;
    }
}
