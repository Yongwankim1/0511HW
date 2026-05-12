using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public bool IsTalk {  get; private set; }
    public void DebugInfo()
    {
        if (IsTalk)
        {
            return;
        }
        Debug.Log("[E] 대화하기");
    }

    public void Interact()
    {
        if (IsTalk)
        {
            Debug.Log("이미 대화하였습니다.");
            return;
        }
        Debug.Log("NPC와 대화");

        IsTalk = true;
    }
}
