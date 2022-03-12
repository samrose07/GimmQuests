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
        baseLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState(state);
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
                PickupObject(objectToGet);
                break;
            case State.Place:
                PlaceObject();
                break;
        }
       // print(rem);
    }
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

    void MoveTowards()
    {
        if(!isHolding)
        {
            //if (isOnPedestal)
            //{
            //    // move to pedestal, then pickup, else, move to object then pickup
            //    agent.SetDestination(pedestalToPlaceUpon.transform.position);
            //    float d = Vector3.Distance(agent.transform.position, agent.destination);
            //    if (d <= distanceToPedestal)
            //    {
                    
            //        state = State.Pickup;
            //    }
            //}
            //else
            //{
                agent.SetDestination(objectToGet.transform.position);
                float d = Vector3.Distance(agent.transform.position, agent.destination);
                if (d <= distanceToPedestal)
                {
                    state = State.Pickup;
                }
            //}
        }
        else
        {
            if(isPlayerObjectOnPedestal)
            {
                agent.SetDestination(pedestalToPlaceUpon.transform.position);
                float d = Vector3.Distance(agent.transform.position, agent.destination);
                if (d <= distanceToPedestal)
                {
                    state = State.Place;
                }
            }
            if(!isPlayerObjectOnPedestal)
            {
                agent.SetDestination(new Vector3(7, 2, 10));
                float d = Vector3.Distance(agent.transform.position, agent.destination);
                if (d <= distanceToPedestal)
                {
                    state = State.Place;
                }
            }
        }
        
    }

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
            agent.SetDestination(new Vector3(7, 2, 10));
        }
        else
        {
            agent.SetDestination(pedestalToPlaceUpon.transform.position);
        }
        state = State.MoveTo;
    }

    void PlaceObject()
    {
        isHolding = false;
        hoa.StopHoldObject(objectToGet);
        Rigidbody rb;
        rb = objectToGet.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        if (!isPlayerObjectOnPedestal)
        {
            hoa.StopHoldObject(objectToGet);
        }
        if (pedestalToPlaceUpon != null)
        {
            Pedestal p = pedestalToPlaceUpon.GetComponent<Pedestal>();
            if (p != null)
            {
                objectToGet.transform.parent = p.placementArea;
                objectToGet.transform.position = p.placementArea.transform.position;
                p.hasObject = true;
            }
        }
        objectToGet = null;
        pedestalToPlaceUpon = null;
        GOGETTHEOBJECT = false;
        isOnPedestal = false;
        //isPlayerObjectOnPedestal = false;
        print("i got here");
        state = State.idle;
        

    }
    //public void ObjectToGrab(GameObject g)
    //{
    //    objectToGet = g;
    //}

    //public void RetrieveObject(GameObject g, GameObject pedestal, bool removeFromPedestal)
    //{
    //    if(removeFromPedestal)
    //    {
    //        RemoveFromPedestal(g, pedestal, new Vector3(7, 2, 10));
    //    }
    //    if(!removeFromPedestal)
    //    {
    //        bool GotObj = GetObj(g);
    //        if(GotObj)
    //        {
    //            PlaceOnPedestal(g, pedestal);
    //            return;
    //        }
    //        else
    //        {

    //            RetrieveObject(g, pedestal, removeFromPedestal);
    //            return;
    //        }
    //    }
    //}

    //public bool GetObj(GameObject g)
    //{
    //    if(isHolding)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        MoveToObject(g);
    //        return false;
    //    }
    //}

    //private void MoveToObject(GameObject g)
    //{
    //    agent.SetDestination(g.transform.position);
    //    float distance = Vector3.Distance(g.transform.position, agent.transform.position);
    //    if(distance <= 2)
    //    {
    //        hoa.HoldObject(g);
    //        isHolding = true;
    //    }
    //    return;
    //}

    //public void PlaceOnPedestal(GameObject g, GameObject pedestal)
    //{

    //    float distance = Vector3.Distance(agent.transform.position, pedestal.transform.position);
    //    if(distance <= 2)
    //    {
    //        Pedestal p = pedestal.GetComponent<Pedestal>();
    //        p.Place(g);
    //        objectToGet = null;
    //    }
    //}

    //public void RemoveFromPedestal(GameObject objToRemove, GameObject pedestal, Vector3 newPositionToPutObject)
    //{
    //    float distance = Vector3.Distance(agent.transform.position, pedestal.transform.position);
    //    if (distance >= 3)
    //    {
    //        agent.SetDestination(pedestal.transform.position);
    //        RemoveFromPedestal(objToRemove, pedestal, newPositionToPutObject);
    //    }
    //    else
    //    {
    //        hoa.HoldObject(objToRemove);
    //        MoveTo(newPositionToPutObject, true);
    //    }
    //}

    //public void MoveTo(Vector3 Loc, bool placeObjectOnGround)
    //{
    //    float distance = Vector3.Distance(agent.transform.position, Loc);
    //    if (distance >= 3)
    //    {
    //        agent.SetDestination(Loc);
    //        MoveTo(Loc, placeObjectOnGround);
    //    }
    //    else
    //    {
    //        hoa.StopHoldObject(objectToGet);
    //        objectToGet = null;
    //    }
    //}
    //void BeIdle()
    //{
    //    bool onPedestal;
    //    agent.isStopped = true;
    //    foreach(Pedestal p in playerPedestals)
    //    {
    //        if(p.hasObject)
    //        {
    //            onPedestal = CheckIfRelatedObjectIsOnPedestal(p.heldObject);
    //            if (onPedestal)
    //            {
    //                state = State.idle;
    //            }
    //            else
    //            {
    //                print("fkn got here too m8");
    //                StartFindObj(p.heldObject.name, false);
    //            }

    //        }
    //    }
    //    foreach(GameObject g in playersObjects)
    //    {
    //        PlaceableObject pobj;
    //        pobj = g.GetComponent<PlaceableObject>();
    //        if(!pobj.isHeld && !pobj.isHeldinArea)
    //        {
    //            foreach(GameObject g2 in placeableGameObjects)
    //            {
    //                if(g2.GetComponent<PlaceableObject>().isHeldinArea)
    //                {
    //                    rem = true;
    //                }
    //            }

    //            StartFindObj(pobj.gameObject.name, true);
    //        }
    //        else
    //        {
    //            rem = false;
    //        }
    //    }

    //}
    //bool CheckIfRelatedObjectIsOnPedestal(GameObject g)
    //{
    //    bool retVal = true;
    //    //print(g.name);
    //    if(!g.name.Contains(addString))
    //    {
    //        g.name = g.name + addString;
    //    }

    //   // print(g.name);
    //    foreach (GameObject gO in placeableGameObjects)
    //    {
    //        if(gO.name == g.name && !gO.GetComponent<PlaceableObject>().isHeldinArea)
    //        {
    //            retVal = false;
    //        }
    //    }
    //    return retVal;
    //}
    //void StartFindObj(string name, bool remove)
    //{

    //    string nameString = name;
    //    print(nameString);
    //    objectToGoTo = GameObject.Find(nameString);
    //    if(!remove)
    //    {
    //        if (objectToGoTo != null)
    //        {
    //            SetDest(objectToGoTo, remove);
    //            state = State.MoveTo;

    //        }
    //    }
    //    else
    //    {
    //        if(objectToGoTo != null)
    //        {
    //            SetDest(objectToGoTo, remove);
    //            state = State.MoveTo;
    //        }
    //    }

    //}
    //void SetDest(GameObject g, bool remove)
    //{
    //    if (remove)
    //    {
    //        curDest = g;
    //        if(isHolding)
    //        {
    //            curDest.transform.position = new Vector3(7f, 1f, 0f);
    //        }
    //        else
    //        {
    //            curDest = g;
    //        }
    //    }
    //    else
    //    {
    //        curDest = g;
    //        if (g.gameObject.name.Contains("pedestal"))
    //        {
    //            if (g.GetComponent<Pedestal>().hasObject)
    //            {
    //                currentPedestalIndex += 1;
    //                if (currentPedestalIndex == robotPedestals.Count) currentPedestalIndex = 0;
    //                //agent.SetDestination(robotPedestals[currentPedestalIndex].transform.position);
    //                curDest = robotPedestals[currentPedestalIndex].gameObject;
    //            }
    //        }
    //        agent.SetDestination(curDest.transform.position);
    //    }

    //}

    //void MoveTowards()
    //{
    //    agent.isStopped = false;
    //    float distance = Vector3.Distance(gameObject.transform.position, agent.destination);
    //    if(distance <= 3)
    //    {
    //        if (isHolding) state = State.Place;
    //        if (!isHolding) state = State.Pickup;
    //    }
    //}

    //void PickupObject(GameObject obj, bool remove)
    //{
    //    if(remove)
    //    {
    //        heldObj = obj;
    //        isHolding = true;
    //        hoa.HoldObject(heldObj);
    //        Rigidbody rb = heldObj.GetComponent<Rigidbody>();
    //        rb.isKinematic = true;
    //        SetDest(obj, remove);
    //        state = State.MoveTo;
    //    }
    //    if(!remove)
    //    {
    //        heldObj = obj;
    //        isHolding = true;
    //        hoa.HoldObject(heldObj);
    //        Rigidbody rb;
    //        rb = heldObj.GetComponent<Rigidbody>();
    //        rb.isKinematic = true;
    //        SetDest(robotPedestals[currentPedestalIndex].gameObject, false);
    //        state = State.MoveTo;
    //    }

    //}

    //void PlaceObject(bool remove)
    //{
    //    agent.isStopped = true;
    //    if(remove)
    //    {
    //        hoa.StopHoldObject(heldObj);
    //        heldObj.GetComponent<PlaceableObject>().isHeldinArea = false;
    //        isHolding = false;
    //        state = State.idle;
    //    }
    //    if (!remove)
    //    {
    //        if (playerHoldingObject)
    //        {
    //            return;
    //        }
    //        if (heldObj)
    //        {
    //            hoa.StopHoldObject(heldObj);
    //            Pedestal p = curDest.GetComponent<Pedestal>();
    //            if (p != null)
    //            {
    //                heldObj.transform.parent = p.placementArea;
    //                heldObj.transform.position = p.placementArea.transform.position;
    //                p.hasObject = true;
    //            }
    //            heldObj.GetComponent<PlaceableObject>().isHeldinArea = true;
    //            isHolding = false;
    //            //heldObj = null;
    //            state = State.idle;
    //        }
    //    }


    /*void BeIdle()
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


   */

    }

