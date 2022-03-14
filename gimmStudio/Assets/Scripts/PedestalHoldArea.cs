/* This script was created by Samuel Rose for the GIMMStudio space, focused for quests.
 * 
 * The purpose of this script is to send the pedestal information about collisions, which 
 *then determine what the little robot boi is doing!
 *              
 * Biodigital jazz, man
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalHoldArea : MonoBehaviour
{
    #region variables
    PlaceableObject pO;
    Pedestal pedestal;
    public bool isRobotPedestal;

    #endregion

    #region start and trigger methods
    private void Start()
    {
        //make sure we have a pedestal.
        pedestal = GetComponentInParent<Pedestal>();
    }
    private void OnTriggerStay(Collider other)
    {
        //once collision happens, check if the gameobject this script is attached to is a robot pedestal or not.
        if (!isRobotPedestal)
        {
            //if not (is a player pedestal instead), set the parenting of the object placed within
            /* to this pedestalHoldArea. 
             * We then make sure the booleans are all correct for the object AND the pedestal.
             * Afterwhich we tell the pedestal to send the robot the information, regardless if it's correct or not.
             */
            pO = other.gameObject.GetComponent<PlaceableObject>();
            if (pO != null)
            {
                other.gameObject.transform.parent = this.transform;
                other.gameObject.transform.position = this.transform.position;
                pO.isHeldinArea = true;
                pO.heldPedestal = gameObject;
                pedestal.heldObject = other.gameObject;
                pedestal.hasObject = true;
                if (pedestal.heldObject == pedestal.CorrectGameObject)
                {
                    pedestal.hasCorrectGO = true;
                }
                if (pedestal.hasObject)
                {
                    pedestal.SendJLB(other.gameObject, false);
                }
            }
        }
        else
        {
            /* If a robot pedestal, do all the same things except we don't send the robot the information,
             * we tell the parent pedestal that an object is indeed here!
             */
            pO = other.gameObject.GetComponent<PlaceableObject>();
            if (pO != null && !pO.isHeld)
            {
                other.gameObject.transform.parent = this.transform;
                other.gameObject.transform.position = this.transform.position;
                pO.isHeldinArea = true;
                pO.heldPedestal = gameObject;
                pedestal.heldObject = other.gameObject;
                pedestal.hasObject = true;
                if (pedestal.heldObject == pedestal.CorrectGameObject)
                {
                    pedestal.hasCorrectGO = true;
                }
                pedestal.TellIfOn(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
       if(!isRobotPedestal)
        {
            if (pO != null)
            {
                other.gameObject.transform.parent = null;
                pO.isHeldinArea = false;
                pO.heldPedestal = null;
                pedestal.hasObject = false;
                if (!pedestal.hasObject) pedestal.hasCorrectGO = false;
               pedestal.SendJLB(other.gameObject, true);
                //print("got here for some fucking reason");
            }
        }
        else
        {
            if (pO != null)
            {
                other.gameObject.transform.parent = null;
                pO.isHeldinArea = false;
                pO.heldPedestal = null;
                pedestal.hasObject = false;
                if (!pedestal.hasObject) pedestal.hasCorrectGO = false;
                pedestal.TellIfOn(false);
            }
        }
    }
    #endregion
}
