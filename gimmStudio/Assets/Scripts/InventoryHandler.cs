/* Script created by Samuel Rose for use in the GIMM Studio space.
 * 
 * Contributors so far: Samuel Rose
 * 
 * The purpose of this script so far is to act as a handler for the inventory, to
 * be passed from the reticle behavior script. I feel like this could be a part
 * of the player manager, but i don't want to interfere with anything other people are
 * doing at the moment so I will be keeping it seperate for now. Lmk if you
 * have any questions. If you are a future cohort working on this, you can
 * contact me through my website at samuelrose.dev and we can talk about
 * any issues you might be having (:
 * 
 * #biodigital jazz, man
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    [Header("The inventory list")]
    public List<GameObject> inventory;
    public Text inventoryText;
    public GameObject inventoryPanel;
    private string newInventText = "";
    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<GameObject>();
    }

    /// <summary>
    /// Add a collected game object to the inventory, as called by the 
    /// reticle behavior script.
    /// </summary>
    /// <param name="g">The game object passed to be added to the inventory list. Example: Ted's floppy disk object.</param>
    public void AddToInventory(GameObject g)
    {
        bool isAlreadyAdded = false;

        //first, check to see if inventory has anything in it
        //print("inventory count = " + inventory.Count);
        if(inventory.Count > 0)
        {
            //then check each gameobject to see if they match what we are trying to pass
            foreach(GameObject go in inventory)
            {
                //if so, then isAlreadyAdded is true, otherwise false.
                if (go == g) isAlreadyAdded = true;
            }
        }
        //only add the game object to the inventory if it is either:
        //  A) The first object being added to it
        //  B) The only of its kind being added.
        if (!isAlreadyAdded)
        {
            
            if(inventory.Count == 0)
            {
                newInventText += g.name.ToUpper();
            }
            else if (inventory.Count > 0)
            {
                newInventText += ", ";
                newInventText += g.name.ToUpper();
            }
            inventory.Add(g);
            inventoryPanel.SetActive(true);
            Invoke("InventoryDisplay", 3);

        }
        

        inventoryText.text = newInventText;
        //print("inventory count = " + inventory.Count);
    }

    private void InventoryDisplay()
    {
        inventoryPanel.SetActive(false);
    }
}
