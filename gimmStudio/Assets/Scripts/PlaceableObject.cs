/* This script was created by Samuel Rose for the GIMMStudio space, focused for quests.
 * 
 * The purpose of this script is to provide a framework for an object that is in the player's or robot's hand
 *              
 * Biodigital jazz, man
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{

    #region variables
    public bool isHeld = false;
    public bool isHeldinArea = false;
    public GameObject heldPedestal;
    public PedestalHoldArea pha;
    public string name;
    Rigidbody rb;
    #endregion

    #region start and update
    private void Start()
    {
        //get the name and the rigidbody
        name = gameObject.name;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //check to see if we in a held area, and if so, check to see
        //if we are in a pedestal, and set the local heldPedestal to that.
        if(isHeldinArea)
        {
            pha = gameObject.GetComponentInParent<PedestalHoldArea>();
            if (pha != null) heldPedestal = pha.gameObject;
        }

    }
    #endregion
}
