using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public PlayerControls Controls;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Controls = new PlayerControls();
        Controls.UIInput.Disable();
        Controls.PlayerInput.Enable();
    }

    public void EnablePlayer()
    {
        Controls.UIInput.Disable();
        Controls.PlayerInput.Enable();
    }

    public void EnableUI()
    {
        Controls.PlayerInput.Disable();
        Controls.UIInput.Enable();
    }
}