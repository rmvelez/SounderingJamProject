using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    List<GameObject> interactableObjects = new List<GameObject>();

    [Tooltip("Event called when we go from 0 to 1 at least 1 interractable object in range")]
    public UnityEvent OnInteractablesExist;

    [Tooltip("event called when we go from some number to 0 interactables in range")]
    public UnityEvent OnInteractablesDoNotExist;

    [Tooltip("the player controller on the player (drag it here)")]
    [SerializeField] private PlayerController playerController;

    private void TrackObject(GameObject objectToTrack)
    {
        //add the object to our list of tracked objects
        interactableObjects.Add(objectToTrack);

        //interactables exist, so let everyone know
        if (interactableObjects.Count == 1)
        {
            OnInteractablesExist.Invoke();
        }
    }

    //public so we can call it from another object after we "destroy" this one
    public void UntrackObject(GameObject trackedObject)
    {
        //only try to remove it if it's already in there
        if (interactableObjects.Contains(trackedObject))
        {
            interactableObjects.Remove(trackedObject);

            //see if we hit zero and let folks know
            if (interactableObjects.Count == 0)
            {
                //fire the event
                OnInteractablesDoNotExist.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        //if an interactable enters our trigger area
        if (other.CompareTag("Interactable"))
        {
            Debug.Log("trigger enter"); 
            //then track it
            TrackObject(other.gameObject);

        } else if(other.CompareTag("Ghost"))
        {
            Debug.Log("boo");
            //then track it
            //TrackObject(other.gameObject);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        // If the object exiting the trigger is an interactable object
        if (other.CompareTag("Interactable"))
        {
            UntrackObject(other.gameObject);
        }
    }

    public void Interact()
    {

        //while (interactableObjects.Count > 0 && interactableObjects[0] == null)
        //{

        //    UntrackObject(interactableObjects[0]);
        //}
        Debug.Log("interact object calld (on object?)");

        //make sure we still have objects
        if (interactableObjects.Count > 0)
        {
            //interact only with the first one
            interactableObjects[0].GetComponent<IInteractable>().Interact(this, playerController);

        }


    }
}

