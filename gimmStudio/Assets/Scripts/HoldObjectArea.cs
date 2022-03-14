/* This script was created by Samuel Rose for the GIMMStudio space, focused for quests.
 * 
 * The purpose of this script is to act as "hands" for the robot and the player if not in VR.
 * if in VR, the player will just hold the object in their hands.
 *              
 * Biodigital jazz, man
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldObjectArea : MonoBehaviour
{
    #region variables
    [SerializeField] public GameObject holdArea;
    [SerializeField] private bool isHolding = false;
    private GameObject theObjectToHold;
    #endregion

    #region created methods
    /* A method to be called by either the player or the robot. Makes sure the parent of the
     * held object is this script's gameobject and that the holding boolean is true.
     */
    public void HoldObject(GameObject obj)
    {
        obj.transform.parent = holdArea.transform;
        theObjectToHold = obj;
        isHolding = true;
    }
    /* A method to be called by either the player or the robot. does the exact opposite of above.
     */
    public void StopHoldObject(GameObject obj)
    {
        obj.transform.parent = null;
        theObjectToHold = null;
        isHolding = false;
    }
    #endregion

    //this update method makes sure the position of the held object is where the parent is to 
    //give the illusion of holding it.
    private void Update()
    {
        if (isHolding) theObjectToHold.transform.position = holdArea.transform.position;
    }
}
