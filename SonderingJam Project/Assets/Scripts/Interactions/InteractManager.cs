using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

public class InteractManager : MonoBehaviour
{
    List<GameObject> interactableObjects = new List<GameObject>();
    List<GameObject> trackedGhosts = new List<GameObject>();

    [Tooltip("Event called when we go from 0 to 1 at least 1 interractable object in range")]
    public UnityEvent OnInteractablesExist;

    [Tooltip("event called when we go from some number to 0 interactables in range")]
    public UnityEvent OnInteractablesDoNotExist;

    [Tooltip("the player controller on the player (drag it here)")]
    [SerializeField] private PlayerController playerController;

    [SerializeField] private SpriteRenderer broom;
    [SerializeField] public Animator broomAnimator;

    [SerializeField] private bool attacking = false ;
    public bool getAttacking() { return attacking; }
    [SerializeField] private float attackDuration = 1;
    float timeSinceAttackStarted = 0;

    private void Start()
    {
        broom.enabled = false;
        broomAnimator.SetFloat("AnimationTime", 1 / attackDuration);
    }

    private void FixedUpdate()
    {
        if (attacking)
        {


            //broom.enabled = true;
            //broomAnimator.SetTrigger("Attack");
            if (timeSinceAttackStarted < attackDuration)
            {
                timeSinceAttackStarted += Time.deltaTime;

            } else
            {
                attacking = false;
                broom.enabled = false;
            }

            if(trackedGhosts.Count > 0)
            {
                foreach (GameObject go in trackedGhosts)
                {
                    Ghost ghost = go.GetComponent<Ghost>();

                    ghost.Kill();
                }
                foreach(GameObject go in trackedGhosts)
                {
                    UntrackObject(go);
                }

            }
        } else
        {
            //broom.enabled = false;
        }
    }


    private void TrackObject(GameObject objectToTrack)
    {
        if (objectToTrack.CompareTag("Ghost"))
        {
            //if it's a ghost, we want to specifically track it as one
            trackedGhosts.Add(objectToTrack);
        }
        else
        {
            //otherwise, it's a generic tracked object - I could potentially refactor this to taks specifically but at this point there's no difference
            interactableObjects.Add(objectToTrack);

        }

        //interactables exist, so let everyone know
        //if (interactableObjects.Count == 1)
        //{
        //    OnInteractablesExist.Invoke();
        //}
    }

    //public so we can call it from another object after we "destroy" this one
    public void UntrackObject(GameObject trackedObject)
    {
        //only try to remove it if it's already in there
        if (interactableObjects.Contains(trackedObject))
        {
            interactableObjects.Remove(trackedObject);

            //see if we hit zero and let folks know
            //if (interactableObjects.Count == 0)
            //{
            //    //fire the event
            //    OnInteractablesDoNotExist.Invoke();
            //}
        }
        if (trackedGhosts.Contains(trackedObject))
        {
            trackedGhosts.Remove(trackedObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if an interactable enters our trigger area
        if (other.CompareTag("Interactable"))
        {
            Debug.Log("trigger enter"); 
            //then track it
            TrackObject(other.gameObject);

        } else if (other.CompareTag("Ghost"))
        {
            TrackObject(other.gameObject);

        }
    }


    private void OnTriggerExit(Collider other)
    {
        Debug.Log("untrackign object");
        // If the object exiting the trigger is an interactable object
        if (other.CompareTag("Interactable") || other.CompareTag("Ghost"))
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
            GameObject gameObj = interactableObjects[0];

            UntrackObject(gameObj);


            //interact only with the first one
            gameObj.GetComponent<IInteractable>().Interact(this, playerController);

        } else//if we're not interracting, then we're attacking.
        {
            if (!attacking)//we don't want to do any of this if we're not in the middle of an attack
            {

                Debug.Log("attacking?");
                attacking = true;

                broomAnimator.SetTrigger("Attack");
                broom.enabled = true;
                //also call kill on ghost here



                timeSinceAttackStarted = 0;
            }
            
        }


    }
}

