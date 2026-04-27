using UnityEngine;
using UnityEngine.InputSystem; // Thêm thư viện này

public class DraggableItem : MonoBehaviour
{
    public string itemID;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 originalPosition;
    private float fixedZ;

    void Start()
    {
        fixedZ = transform.position.z;
        originalPosition = transform.position;
    }

    // Với Input System mới, OnMouseDown vẫn có thể hoạt động nếu bạn có PhysicsRaycaster 
    // trên Camera, nhưng cách an toàn nhất là dùng Mouse.current
    void OnMouseDown()
    {
        // Lấy vị trí chuột mới
        Vector2 mousePos = Mouse.current.position.ReadValue();
        
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        
        Vector3 curScreenPoint = new Vector3(mousePos.x, mousePos.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        
        curPosition.z = fixedZ; 
        transform.position = curPosition;
    }

    void OnMouseUp()
    {
        TrayManager.Instance.TryPlaceItem(this);
    }
    
    public void MoveToBack()
    {
        transform.position = originalPosition;
    }

    public void SetPlaced(Transform targetSlot)
    {
        // 1. Tắt script này để không cho kéo thả nữa
        this.enabled = false; 

        // 2. Chuyển sang làm con của Slot
        transform.SetParent(targetSlot);

        // 3. Đưa X, Y về 0 để vào giữa slot. 
        // Set Z là -1 (hoặc một con số âm nhỏ) để nó nổi lên trên mặt khay.
        // Lưu ý: Trong không gian Local, Z âm sẽ đưa vật thể lại gần Camera hơn so với cha của nó.
        transform.localPosition = new Vector3(0, 0, 1f);

        // 4. Khóa góc xoay
        transform.localRotation = Quaternion.identity; 
    }
}
    