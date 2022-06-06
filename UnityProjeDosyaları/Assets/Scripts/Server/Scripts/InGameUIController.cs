using UnityEngine;
using UnityEngine.InputSystem;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] GameObject EscapeMenu;

    private bool isOpenedEscapeMenu = false;

    public UIController inputActions;

    private InputAction escapeMenuButton;

    private void Awake()
    {
        inputActions = new UIController();
    }

    private void OnEnable()
    {
        escapeMenuButton = inputActions.Player.ESCMenu;
        escapeMenuButton.Enable();
        escapeMenuButton.performed += EscapeMenuFunction;
    }

    private void OnDisable()
    {
        escapeMenuButton.Disable();
    }

    private void EscapeMenuFunction(InputAction.CallbackContext callbackContext)
    {
        if (isOpenedEscapeMenu)
            CloseEscapeMenu();
        else
            OpenEscapeMenu();
    }

    private void OpenEscapeMenu()
    {
        EscapeMenu.SetActive(true);
        isOpenedEscapeMenu = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void CloseEscapeMenu()
    {
        EscapeMenu.SetActive(false);
        isOpenedEscapeMenu = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ForResumeButton()
    {
        CloseEscapeMenu();
    }
}