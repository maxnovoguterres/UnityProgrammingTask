using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ActiveInputType
{
    KeyboardMouse,
    Gamepad,
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public InputActionAsset PlayerActions;
    
    [Header("Runtime Variables")]
    [ReadOnly] public ActiveInputType ActiveInputType;
    
    [NonSerialized] public InputActionMap PlayerMap;
    [NonSerialized] public InputActionMap UIMap;
    
    // Player
    [NonSerialized] public InputAction MoveAction;
    [NonSerialized] public InputAction LookAction;
    [NonSerialized] public InputAction AttackAction;
    [NonSerialized] public InputAction InteractAction;
    [NonSerialized] public InputAction InventoryAction;
    
    // UI
    [NonSerialized] public InputAction NavigateAction;
    [NonSerialized] public InputAction SubmitAction;
    [NonSerialized] public InputAction CancelAction;
    [NonSerialized] public InputAction PointAction;
    [NonSerialized] public InputAction ClickAction;
    [NonSerialized] public InputAction ScrollWheelAction;
    [NonSerialized] public InputAction MiddleClickAction;
    [NonSerialized] public InputAction RightClickAction;
    [NonSerialized] public InputAction CloseAction;
    [NonSerialized] public InputAction InventoryMenuAction;

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        MoveAction = PlayerActions.FindAction("Move");
        LookAction = PlayerActions.FindAction("Look");
        AttackAction = PlayerActions.FindAction("Attack");
        InteractAction = PlayerActions.FindAction("Interact");
        InventoryAction = PlayerActions.FindAction("Inventory");
        
        NavigateAction = PlayerActions.FindAction("Navigate");
        SubmitAction = PlayerActions.FindAction("Submit");
        CancelAction = PlayerActions.FindAction("Cancel");
        PointAction = PlayerActions.FindAction("Point");
        ClickAction = PlayerActions.FindAction("Click");
        ScrollWheelAction = PlayerActions.FindAction("ScrollWheel");
        MiddleClickAction = PlayerActions.FindAction("MiddleClick");
        RightClickAction = PlayerActions.FindAction("RightClick");
        CloseAction = PlayerActions.FindAction("Close");
        InventoryMenuAction = PlayerActions.FindAction("InventoryMenu");
    }
    
    public void SwitchToUIMap(bool disablePlayerMap = true)
    {
        UIMap.Enable();
        if (disablePlayerMap)
        {
            PlayerMap.Disable();
        }
    }

    public void SwitchToGameplayMap()
    {
        UIMap.Disable();
        PlayerMap.Enable();
    }
    
    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme == ActiveInputType.Gamepad.ToString())
        {
            ActiveInputType = ActiveInputType.Gamepad;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (input.currentControlScheme == "Keyboard&Mouse")
        {
            ActiveInputType = ActiveInputType.KeyboardMouse;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        Debug.Log($"Control scheme changed to: [{input.currentControlScheme}]");
    }
}