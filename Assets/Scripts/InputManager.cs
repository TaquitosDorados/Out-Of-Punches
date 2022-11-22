using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event StartTouch OnEndTouch;
    #endregion

    private PlayerController playerController;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerController = new PlayerController();
    }
    private void OnEnable()
    {
        playerController.Enable();   
    }
    private void OnDisable()
    {
        playerController.Disable();
    }
    void Start()
    {
        playerController.Touch.FirstContact.started += ctx => StartTouchPrimary(ctx);
        playerController.Touch.FirstContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    void StartTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null)
            OnStartTouch(Utility.ScreenToWorld(mainCamera, playerController.Touch.FirstPosition.ReadValue<Vector2>()), (float)context.startTime);
    }

    void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
            OnEndTouch(Utility.ScreenToWorld(mainCamera, playerController.Touch.FirstPosition.ReadValue<Vector2>()), (float)context.time);
    }

    public Vector2 PrimaryPosition()
    {
        return Utility.ScreenToWorld(mainCamera, playerController.Touch.FirstPosition.ReadValue<Vector2>());
    }
}
