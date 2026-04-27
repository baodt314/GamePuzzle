using UnityEngine;
using System.Collections.Generic;

public class TraySection : MonoBehaviour
{
    public string assignedID = ""; 
    public Transform[] subSlots;   // Kéo thả 3 Transform con vào đây
    private GameObject[] itemsInSlots = new GameObject[3]; 

    public bool IsFull() {
        foreach (var slot in itemsInSlots) if (slot == null) return false;
        return true;
    }

    public bool IsEmpty() {
        foreach (var slot in itemsInSlots) if (slot != null) return false;
        return true;
    }

    public bool TryAddItem(DraggableItem item)
    {
        for (int i = 0; i < subSlots.Length; i++)
        {
            if (itemsInSlots[i] == null) 
            {
                // Nếu ngăn đang hoàn toàn trống, bắt đầu nhận ID của vật phẩm này
                if (IsEmpty()) assignedID = item.itemID;

                itemsInSlots[i] = item.gameObject;

                // Di chuyển vật thể 3D bằng Transform thay vì RectTransform
                item.transform.SetParent(subSlots[i]);
                item.transform.localPosition = Vector3.zero; 
                item.transform.localRotation = Quaternion.identity;

                item.SetPlaced(subSlots[i]); // Truyền Transform vào để script Item xử lý

                CheckMatch();
                return true; 
            }
        }
        return false;
    }

    void CheckMatch()
    {
        int count = 0;
        foreach (var obj in itemsInSlots) if (obj != null) count++;

        if (count == 3)
        {
            Invoke(nameof(ClearSection), 0.2f);
        }
    }

    void ClearSection()
    {
        for (int i = 0; i < itemsInSlots.Length; i++)
        {
            if (itemsInSlots[i] != null)
            {
                Destroy(itemsInSlots[i]);
                itemsInSlots[i] = null;
            }
        }
        assignedID = "";
    }
}