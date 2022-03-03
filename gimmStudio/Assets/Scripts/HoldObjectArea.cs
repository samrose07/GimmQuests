using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldObjectArea : MonoBehaviour
{
    [SerializeField] private GameObject holdArea;
    [SerializeField] private bool isHolding = false;
    private GameObject theObjectToHold;

    public void HoldObject(GameObject obj)
    {
        obj.transform.parent = holdArea.transform;
        theObjectToHold = obj;
        isHolding = true;
    }

    public void StopHoldObject(GameObject obj)
    {
        obj.transform.parent = null;
        theObjectToHold = null;
        isHolding = false;
    }

    private void Update()
    {
        if (isHolding) theObjectToHold.transform.position = holdArea.transform.position;
    }
}
