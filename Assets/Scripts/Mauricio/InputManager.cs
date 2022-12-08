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
    private MauricioController mauricio;
    private JoeBehaviour joe;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerController = new PlayerController();
        mauricio = FindObjectOfType<MauricioController>();
        joe = FindObjectOfType<JoeBehaviour>();
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
        playerController.Touch.Tap.started += ctx => StartTap(ctx);
        playerController.Touch.Hold.performed += ctx => StartHold(ctx);
        playerController.Touch.Hold.canceled += ctx => EndHold(ctx);

        playerController.Debug.Debug.performed += ctx => { joe.KOd(); };
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

    void StartTap(InputAction.CallbackContext context)
    {
        if (!mauricio.KO)
        {
            mauricio.Punch();
        } else
        {
            mauricio.KoTapping();
        }

    }
    void StartHold(InputAction.CallbackContext context)
    {
        mauricio.startBlocking();
    }
    void EndHold(InputAction.CallbackContext context)
    {
        mauricio.endBlocking();
    }
}
