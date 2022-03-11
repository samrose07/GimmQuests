/* This script was created by Samuel Rose for the GIMMStudio space, focused for quests.
 * 
 * The purpose of this script is to provide a framework for an object that is in the player's hand
 *              
 * Biodigital jazz, man
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool isHeld = false;
    public bool isHeldinArea = false;
    public GameObject heldPedestal;
    public PedestalHoldArea pha;
    public string name;

    private void Start()
    {
        name = gameObject.name;
    }

    private void Update()
    {
        if(isHeldinArea)
        {
            pha = gameObject.GetComponentInParent<PedestalHoldArea>();
            if (pha != null) heldPedestal = pha.gameObject;
        }
    }
}
