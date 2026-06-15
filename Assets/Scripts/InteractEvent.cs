using UnityEngine;

public class InteractEvent : IGameEvent
{
    public Interactable Interactable { get; }

    public InteractEvent (Interactable interactable)
    {
        Interactable = interactable;
    }
}
