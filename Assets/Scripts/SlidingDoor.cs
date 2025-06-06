using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : TooltipObject
{
    public Animator animator;
    public Transform doorModel;
    //public GameObject obj;
    private bool isOpen = false;
    private float timer = 0f;
    [SerializeField] private float timeAmount = 1.5f;
    private bool isBusy = false;
    [SerializeField] private bool isLocked = false;

    public Pathfinding pathfinding;

    void Update()
    {
        isBusy = timer > 0f;

        if (timer > 0f) timer -= Time.deltaTime;
    }

    public override void Interact(PlayerController player)
    {

        if (!isLocked && !isBusy && Input.GetKeyDown(player.GetControl("Interact")))
        {
            isOpen = !isOpen;
            animator.SetBool("isOpen", isOpen);
            timer = timeAmount;

            if (isOpen)
            {
                Debug.Log($"Opening {name}");

                int passableLayer = LayerMask.NameToLayer("Default");
                if (passableLayer == -1)
                {
                    Debug.LogError("Layer 'Passable' does not exist!");
                    return;
                }

                SetLayerRecursively(gameObject, passableLayer);

                //pathfinding.Refresh();
            }
            else
            {
                Debug.Log($"Closing {name}");

                int passableLayer = LayerMask.NameToLayer("Walls");
                if (passableLayer == -1)
                {
                    Debug.LogError("Layer 'Passable' does not exist!");
                    return;
                }

                SetLayerRecursively(gameObject, passableLayer);

                //pathfinding.Refresh();
            }
        }
    }

    public void unlockDoor()
    {
        isLocked = false;
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        Debug.Log($"Layer {obj.layer}");
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}