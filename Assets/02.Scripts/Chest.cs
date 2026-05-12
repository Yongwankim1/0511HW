using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool IsOpen {  get; private set; }

    public void Interact()
    {
        if(IsOpen)
        {
            Debug.Log("이미 상자가 열려있습니다");
            return;
        }
        OpenChest();
    }
    public void DebugInfo()
    {
        if (IsOpen) return;
        Debug.Log("[E] 상자 열기");
    }

    void OpenChest()
    {
        Debug.Log("상자가 열렸습니다");
        IsOpen = true;
    }
}
