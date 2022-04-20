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

    private void Update()
    {
        CheckPedestals();
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
        }
        canAdd = false;
    }
}
