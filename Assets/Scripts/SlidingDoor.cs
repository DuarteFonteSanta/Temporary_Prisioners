using UnityEngine;

public class SlidingDoor : TooltipObject
{
    public Animator animator;
    public Transform doorModel;
    private bool isOpen = false;
    private float timer = 0f;
    [SerializeField] private float timeAmount = 1.5f;
    private bool isBusy = false;
    [SerializeField] private bool isLocked = false;

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

    public void unlockDoor()
    {
        isLocked = false;
    }
}