using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    public JacksLilBuddy robotBoi;
    public Pedestal buddyPedestal;
    public Transform placementArea;
    public bool hasObject = false;
    public int NumInOrder;
    public GameObject heldObject;
    public GameObject CorrectGameObject;
    public bool hasCorrectGO = false;
    public bool isPlayerPedestal = false;


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
                return;
            }
            robotBoi.objectToGet = toGet;
            robotBoi.pedestalToPlaceUpon = buddyPedestal.gameObject;
            robotBoi.GOGETTHEOBJECT = true;
            robotBoi.isOnPedestal = false;
            robotBoi.isPlayerObjectOnPedestal = hasObject;
        }
    }
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
    /*
     * CHANGE THESE JLB
     * public GameObject objectToGet;
    public GameObject pedestalToPlaceUpon;
    public bool GOGETTHEOBJECT = false;
    public bool isOnPedestal = false;
    public bool isPlayerObjectOnPedestal = false;
     */
    
}
