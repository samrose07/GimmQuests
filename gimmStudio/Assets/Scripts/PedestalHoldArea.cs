using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalHoldArea : MonoBehaviour
{
    PlaceableObject pO;
    Pedestal pedestal;
    public bool isRobotPedestal;

    private void Start()
    {
        pedestal = GetComponentInParent<Pedestal>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (!isRobotPedestal)
        {
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
}
