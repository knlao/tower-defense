using UnityEngine;

public class CamaraController : MonoBehaviour
{
    /*
     * Camera Controller Script
     * Controls the camera
     */

    [Header("Camera Behaviour")]
    public float panSpeed = 30f;
    public float fastPanSpeed = 50f;
    public float panBorderThickness;
    public float scrollSpeed = 5f;
    public Vector2 xLimit;
    public Vector2 yLimit;
    public Vector2 zLimit;
    public bool doMovement = true;
    
    private void Update()
    {
        if (GameManager.GameIsOver)
        {
            enabled = false;
            return;
        }
        
        // if (Input.GetKeyDown(KeyCode.C))
        //     _doMovement = !_doMovement;
        
        if (!doMovement) return;

        if (0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y) return;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.forward * (Input.GetKey(KeyCode.LeftControl) ? fastPanSpeed : panSpeed) * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(Vector3.back * (Input.GetKey(KeyCode.LeftControl) ? fastPanSpeed : panSpeed) * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right * (Input.GetKey(KeyCode.LeftControl) ? fastPanSpeed : panSpeed) * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(Vector3.left * (Input.GetKey(KeyCode.LeftControl) ? fastPanSpeed : panSpeed) * Time.deltaTime, Space.World);
        }

        var scroll = Input.GetAxis("Mouse ScrollWheel");

        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, xLimit.x, xLimit.y);
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, yLimit.x, yLimit.y);
        pos.z = Mathf.Clamp(pos.z, zLimit.x, zLimit.y);

        transform.position = pos;
    }
}
