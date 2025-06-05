using UnityEngine;
using System.Collections.Generic;

public class EncryptedDoor : TooltipObject
{
    public Animator animator;
    public Transform doorModel;
    private bool isOpen = false;
    private float timer = 0f;
    [SerializeField] private float timeAmount = 1.5f;
    private bool isBusy = false;
    [SerializeField] private bool isLocked = false;
    [SerializeField] private List<int> doorCodes = new();

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
        }
    }

    public string UnlockDoor(int code)
    {
        if (doorCodes.Count == 0)
        {
            return "The door is already unlocked";
        }
        for (int i = 0; i < doorCodes.Count; i++)
        {
            if (doorCodes[i] == code)
            {
                doorCodes.RemoveAt(i);
                if (doorCodes.Count != 0)
                {
                    return "Door unlock process 1 out of 2";
                }
                else
                {
                    isLocked = false;
                    return "Door opened successfully";
                }
            }
        }

        return "Wrong door code";


    }
}
