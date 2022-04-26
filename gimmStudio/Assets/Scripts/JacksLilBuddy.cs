/* This script was created by Samuel Rose for the GIMMStudio space, focused for quests.
 * 
 * The purpose of this script is to act as a "brain" for the lil robot boi
 * that will accomplish tasks.
 * 
 * I apologize for the italian dish this code is, but it works!
 *              
 * Biodigital jazz, man
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JacksLilBuddy : MonoBehaviour
{
    #region variables and states
    public GameObject myArms;
    public GameObject objectToGet;
    public GameObject pedestalToPlaceUpon;
    public float distanceToPedestal = 0;
    public bool GOGETTHEOBJECT = false;
    public bool isOnPedestal = false;
    public bool isPlayerObjectOnPedestal = false;
    public NavMeshAgent agent;
    public bool isHolding = false;
    public HoldObjectArea hoa;
    private Vector3 baseLocation;
    [SerializeField] private GameObject dropArea;
    [SerializeField] private Vector3 dropLocation;
    public enum State
    {
        idle,
        MoveTo,
        Pickup,
        Place
    }

    private State state;

    #endregion

    #region start and update
    // Start is called before the first frame update
    void Start()
    {
        state = State.idle;
        agent = GetComponent<NavMeshAgent>();
        baseLocation = transform.position;
        dropLocation = dropArea.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState(state);
    }

    #endregion

    #region created methods

    //this method handles the states themselves, callign a function for each of them
    //to do their respective things.
    void UpdateState(State currentState)
    {
        switch(currentState)
        {
            case State.idle:
                BeIdle();
                break;
            case State.MoveTo:
                MoveTowards();
                break;
            case State.Pickup:
                PickupObject(objectToGet);
                break;
            case State.Place:
                PlaceObject();
                break;
        }
       // print(rem);
    }
    /*This method returns the agent to a starting location, and only changes states if
     * the public boolean GOGETTHEOBJECT is true. If so, the whole chain of command happens.
     * If not, jus chill my boi
     */
    void BeIdle()
    {
        if (agent.transform.position != baseLocation)
        {
            agent.SetDestination(baseLocation);
        }
        if (!GOGETTHEOBJECT) return;
        else
        {
            state = State.MoveTo;
        }
    }

    /* this method checks to see if the robot is already holding an object or not.
     * If it isn't, set the destination to the object determined by the pedestal
     * that changed the state of the robot. I.E., player puts the cube on a pedestal,
     * robot needs to get that cube.
     *
     */
    void MoveTowards()
    {
        if(!isHolding)
        {
            //once the robot gets near the object it has to pick up, switch the state to pickup.
            //to optimize a bit, call the set destination only if the current destination is not equal to the needed destination. TODO.
                agent.SetDestination(objectToGet.transform.position);
                float d = Vector3.Distance(agent.transform.position, agent.destination);
                if (d <= distanceToPedestal)
                {
                    state = State.Pickup;
                }
        }
        else
        {
            //if we are holding it, check to see if the player version of the object is on a pedestal.
            //if so, the new destination is the pedestal related to it, and once we get near it,
            //place it.
            if(isPlayerObjectOnPedestal)
            {
                agent.SetDestination(pedestalToPlaceUpon.transform.position);
                float d = Vector3.Distance(agent.transform.position, agent.destination);
                if (d <= distanceToPedestal)
                {
                    state = State.Place;
                }
            }
            //if the related player object is not on the pedestal, then we can assume it was removed and we need to take
            //the robot object away from the pedestal it is on. Destination is an arbitrary value here, can change to wherever.
            if(!isPlayerObjectOnPedestal)
            {
                agent.SetDestination(dropLocation);
                float d = Vector3.Distance(agent.transform.position, agent.destination);
                if (d <= distanceToPedestal)
                {
                    state = State.Place;
                }
            }
        }
        
    }
    
    /* this method takes in a GameObject g that is the object the robot has to get.
     * It is only called when the robot is near enough to the object.
     * Sets the private var of isHolding to true to indicate ownership of the object.
     * Then calls the local HoldObjectArea script to initiate the hold to get the correct location.
     * has to turn on the kinematic bool in the rigidbody of the object so demorphing doesn't happen.
     * Then it makes sure the objects respective script indicates that it's being held.
     * Once this is done, set the destination in regards to whether or not the respective
     * player object is on a pedestal or not, just like above.
     */
    void PickupObject(GameObject g)
    {
        isHolding = true;
        hoa.HoldObject(g);
        Rigidbody rb;
        rb = g.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        PlaceableObject p = g.GetComponent<PlaceableObject>();
        p.isHeld = true;
        if(!isPlayerObjectOnPedestal)
        {
            agent.SetDestination(dropLocation);
        }
        else
        {
            agent.SetDestination(pedestalToPlaceUpon.transform.position);
        }
        myArms.transform.Rotate(new Vector3(90, 0, 0));
        state = State.MoveTo;
    }

    /* This method is the exact opposite of the one above. The differences here are
     * that we are giving the object being held its ownership back.
     * Then we are setting the transform parent to the pedestal IF that's where we are.
     * Otherwise, we jus droppin it yannowhatimsayin.
     * After that, return the public vars to the original state so we can start again,
     * and set the state to IDLE.
     */
    void PlaceObject()
    {
        isHolding = false;
        hoa.StopHoldObject(objectToGet);
        Rigidbody rb;
        rb = objectToGet.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        PlaceableObject p = objectToGet.GetComponent<PlaceableObject>();
        p.isHeld = false;
        if (!isPlayerObjectOnPedestal)
        {
            hoa.StopHoldObject(objectToGet);
        }
        if (pedestalToPlaceUpon != null)
        {
            Pedestal p2 = pedestalToPlaceUpon.GetComponent<Pedestal>();
            if (p != null)
            {
                objectToGet.transform.parent = p2.placementArea;
                objectToGet.transform.position = p2.placementArea.transform.position;
                p2.hasObject = true;
            }
        }
        objectToGet = null;
        pedestalToPlaceUpon = null;
        GOGETTHEOBJECT = false;
        isOnPedestal = false;
        myArms.transform.Rotate(new Vector3(-90, 0, 0));
        state = State.idle;
    }
    #endregion 
}

