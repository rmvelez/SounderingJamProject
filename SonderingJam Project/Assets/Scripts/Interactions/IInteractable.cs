using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(InteractManager playerInteractManager, PlayerController playerController);
}

