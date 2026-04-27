using UnityEngine;
using System.Linq;

public class TrayManager : MonoBehaviour
{
    public static TrayManager Instance;
    public TraySection[] sections; 
    public float dropDistance = 250f; // Khoảng cách nhận diện trong UI (Pixel)

    void Awake() => Instance = this;

    public void TryPlaceItem(DraggableItem item)
    {
        // Bắn một tia từ vị trí vật phẩm, hướng thẳng vào sâu trong màn hình (trục Z+)
        // Vector3.forward là hướng (0, 0, 1)
        Ray ray = new Ray(item.transform.position, Vector3.forward);
        RaycastHit hit;

        // Bắn tia dài 10 đơn vị (đủ để chạm tới khay nằm phía sau)
        if (Physics.Raycast(ray, out hit, 10f))
        {
            // Kiểm tra xem vật bị bắn trúng có script TraySection không
            TraySection targetSection = hit.collider.GetComponent<TraySection>();

            if (targetSection != null)
            {
                // Kiểm tra logic: Ngăn trống hoặc trùng ID và chưa đầy
                if ((targetSection.IsEmpty() || targetSection.assignedID == item.itemID) && !targetSection.IsFull())
                {
                    if (targetSection.TryAddItem(item))
                    {
                        Debug.Log("Raycast trúng khay: " + targetSection.name);
                        return;
                    }
                }
            }
        }

        // Nếu không bắn trúng khay nào hoặc khay không hợp lệ
        Debug.Log("Raycast không trúng khay nào phù hợp");
        item.MoveToBack();
    }
}