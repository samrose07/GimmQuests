/* This script was created by Samuel Rose for the GIMMStudio space, focused for quests.
 * 
 * The purpose of this script is to act as a "brain" for the lil robot boi
 * that will accomplish tasks.
 *              
 * Biodigital jazz, man
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*TODO:
 *  list of pedestals, list of placeable objects.
 *  navmesh, target to object that has same name as what player is holding + "for robot"
 *  move to pedestal when player places object on pedestal
 *  if object player is holding is on pedestal, remove it
 *  if object is not held and not on pedestal, drop object.
 *  
 * 
 */
public class JacksLilBuddy : MonoBehaviour
{
    [SerializeField] private List<GameObject> placeableGameObjects;
    [SerializeField] private List<GameObject> playerPedestals;
    [SerializeField] private List<GameObject> robotPedestals;
    [SerializeField] private ReticleBehavior playerReticleBehavior;
    [SerializeField] private GameObject objectToGoTo;
    private string objTGTName;
    private string addString = " for robot";
    private bool playerHoldingObject = false;
    private GameObject objectHeldByPlayer;
    private bool isHolding;
    private GameObject heldObj;
    [SerializeField] private HoldObjectArea hoa;
    private NavMeshAgent agent;
    private GameObject curDest;
    private int currentPedestalIndex = 0;

    public enum State
    {
        idle,
        MoveTo,
        Pickup,
        Place
    }

    private State state;
    // Start is called before the first frame update
    void Start()
    {
        state = State.idle;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState(state);
        objectHeldByPlayer = playerReticleBehavior.heldObj;
        playerHoldingObject = playerReticleBehavior.objectHeld;
        print(state);
    }

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
                PickupObject(objectToGoTo);
                break;
            case State.Place:
                PlaceObject();
                break;
        }
    }

    void BeIdle()
    {
        agent.isStopped = true;
        //play an animation
        if(playerHoldingObject)
        {
            StartFindObj(objectHeldByPlayer.name);
        }
    }

    void MoveTowards()
    {
        agent.isStopped = false;
        float distance = Vector3.Distance(gameObject.transform.position, agent.destination);
        if(distance <= 3)
        {
            if(isHolding)
            {
                state = State.Place;
            }
            if(!isHolding)
            {
                state = State.Pickup;
            }
        }
    }

    private void PickupObject(GameObject obj)
    {
        heldObj = obj;
        isHolding = true;
        hoa.HoldObject(heldObj);
        Rigidbody rb;
        rb = heldObj.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        SetDest(robotPedestals[currentPedestalIndex]);
        state = State.MoveTo;
    }

    void PlaceObject()
    {
        agent.isStopped = true;
        if(playerHoldingObject)
        {
            return;
        }
        else
        {
            hoa.StopHoldObject(heldObj);
            Pedestal p = curDest.GetComponent<Pedestal>();
            if (p != null)
            {
                heldObj.transform.parent = p.placementArea;
                heldObj.transform.position = p.placementArea.transform.position;
                p.hasObject = true;
            }
            isHolding = false;
            //heldObj = null;
            state = State.idle;
        }
       
    }

    void SetDest(GameObject g)
    {
        if (g.name.Contains("pedestal"))
        {
            if (g.GetComponent<Pedestal>().hasObject)
            {
                currentPedestalIndex += 1;
                //agent.SetDestination(robotPedestals[currentPedestalIndex].transform.position);
                g = robotPedestals[currentPedestalIndex];
            }
        }
        agent.SetDestination(g.transform.position);
        curDest = g;
    }
    void StartFindObj(string name)
    {
        string nameString = name + addString;
        objectToGoTo = GameObject.Find(nameString);
        if(objectToGoTo != null)
        {
            SetDest(objectToGoTo);
            state = State.MoveTo;
            
        }
    }
}
