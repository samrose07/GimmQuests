/* Script created by Samuel Rose for use in the GIMM Studio space.
 * 
 * Contributors so far: Samuel Rose
 * 
 * The purpose of this script so far is to act as a handler for reticle interaction,
 * as I am not sure if we have one of those yet. Made under the guise of quests being
 * the sole need for it so far. Feel free to let me know if you have any questions (:
 * 
 * If i were to add this to the PUN network, simply change the update fx
 * to only call the function if the view.isMine
 * 
 *          On top of that, you can make the playerManager have a public boolean that
 *          is true only if the view.isMine and allow this script (and any others I make)
 *          to do their things only if that boolean is true.
 * 
 * #biodigital jazz, man
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleBehavior : MonoBehaviour
{
    //creating a var that lets us know if we hit something.
    private bool hitSomething = false;

    //a game object to attribute the raycast to
    private GameObject raycastedObj;

    //need a camera
    [SerializeField] private Camera cam;

    //need the inventory to access
    [SerializeField] private InventoryHandler inventoryHandler;
    // Start is called before the first frame update
    private int holdTime = 0;
    private int holdThreshold = 100;
    public bool objectHeld = false;
    public GameObject heldObj;
    [SerializeField] private HoldObjectArea hoa;
    // Update is called once per frame
    void Update()
    {
        //once per frame, cast out a ray in front. This calls that!
        DoTheRaycast();

        //check for some inputs
        HandleSomeInputs();

        //print(heldObj);
    }

    #region Raycast things, man

    //Here's the fx
    private void DoTheRaycast()
    {
        //used to send out a raycast and get whats being hit. sets the object the ray hits to the raycastedObj variable so we can *do things*
        //int layerMask = 1 << 8;
        RaycastHit hit;
        Vector3 fwd = cam.transform.TransformDirection(Vector3.forward);
        Ray screenRay = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(screenRay, out hit, 3.0f))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);
            raycastedObj = hit.transform.gameObject;
            if(raycastedObj.tag == "EllertsonTrack")
            {
                raycastedObj.transform.position = Vector3.MoveTowards(raycastedObj.transform.position, screenRay.GetPoint(2.0f), 0.01f);
            }
        }
    }

    #endregion

    #region input checks

    private void HandleSomeInputs()
    {
        string input = Input.inputString;
        switch(input)
        {
            case "f":
                print(raycastedObj.name);
                bool active = CheckActiveState(raycastedObj);
                bool isInteractable = CheckTag(raycastedObj);
                if(isInteractable && active)
                {
                    inventoryHandler.AddToInventory(raycastedObj);
                    //raycastedObj.SetActive(false);
                }
                break;
            case "e":
                if(CheckTag(raycastedObj) && raycastedObj.name.Contains("3 collider"))
                {
                    GuessButtons gb = raycastedObj.GetComponent<GuessButtons>();
                    if(gb != null)
                    {
                        gb.ThisButtonPressed();
                    }
                }
                break;
        }

        if (Input.GetKey("e"))
        {
            if(!objectHeld)
            {
                holdTime++;
                
            }
            
        }
        if (Input.GetKeyUp("e"))
        {
            if (holdTime >= holdThreshold && !objectHeld)
            {
                print(raycastedObj.name);
                if(raycastedObj.name.Contains("pickup")) PickupObject(raycastedObj);
                holdTime = 0;
                return;
            }
            if(objectHeld)
            {
                DropObject(heldObj);
            }
            holdTime = 0;
        }
    }

    private void PickupObject(GameObject obj)
    {
        heldObj = obj;
        objectHeld = true;
        hoa.HoldObject(heldObj);
        Rigidbody rb;
        rb = heldObj.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        PlaceableObject pO = heldObj.GetComponent<PlaceableObject>();
        pO.isHeld = true;
    }

    private void DropObject(GameObject obj)
    {
        Rigidbody rb;
        rb = heldObj.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        hoa.StopHoldObject(obj);
        PlaceableObject pO = heldObj.GetComponent<PlaceableObject>();
        pO.isHeld = false;
        heldObj = null;
        objectHeld = false;
        

    }
    private bool CheckActiveState(GameObject g)
    {
        return g.activeInHierarchy;
    }

    private bool CheckTag(GameObject g)
    {
        return g.CompareTag("interactable");
    }

    
    #endregion
}
