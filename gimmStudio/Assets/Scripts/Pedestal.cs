/* This script was created by Samuel Rose for the GIMMStudio space, focused for quests.
 * 
 * The purpose of this script is to delegate the robot in the scene to "do something"
 * once its respective PedestalHoldArea indicates a change to the collisions.
 *              
 * Biodigital jazz, man
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    #region variables
    public JacksLilBuddy robotBoi;
    public Pedestal buddyPedestal;
    public Transform placementArea;
    public bool hasObject = false;
    public int NumInOrder;
    public GameObject heldObject;
    public GameObject CorrectGameObject;
    public bool hasCorrectGO = false;
    public bool isPlayerPedestal = false;
    #endregion

    #region created methods

    /* This method is called by PedestalHoldArea.
     * Basically, the method asks whether we are removing an object or not,
     * as dictated by whether the PHA sends this from triggerstay or triggerexit.
     * 
     * once we determine that, make sure to set the gameobject that robot needs
     * to find correctly, by finding an object with the same name + " for robot"
     * Then change the variables inside of the robot that dictate what it will be doing!
     */
    public void SendJLB(GameObject toGet, bool remove)
    {
        string name = toGet.name + " for robot";
        toGet = GameObject.Find(name);
        if(remove)
        {
            robotBoi.objectToGet = toGet;
            robotBoi.pedestalToPlaceUpon = null;
            robotBoi.GOGETTHEOBJECT = true;
            robotBoi.isOnPedestal = true;
            robotBoi.isPlayerObjectOnPedestal = hasObject;
        }
        if(!remove)
        {
           
            if(buddyPedestal.hasObject)
            {
                //do nothing if the robot's pedestal already has something. An edge case that might happen, but probably wont.
                return;
            }
            robotBoi.objectToGet = toGet;
            robotBoi.pedestalToPlaceUpon = buddyPedestal.gameObject;
            robotBoi.GOGETTHEOBJECT = true;
            robotBoi.isOnPedestal = false;
            robotBoi.isPlayerObjectOnPedestal = hasObject;
        }
    }

    //this method makes sure to tell the robot if there is an object on the pedestal,
    //called by PedestalHoldArea.
    public void TellIfOn(bool what)
    {
        if (what)
        {
            robotBoi.isOnPedestal = true;
        }
        else
        {
            robotBoi.isOnPedestal = false;
        }
    }

    #endregion
}
