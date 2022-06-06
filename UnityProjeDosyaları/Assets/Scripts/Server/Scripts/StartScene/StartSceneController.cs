using UnityEngine;
using UnityEngine.InputSystem;

public class StartSceneController : MonoBehaviour
{
    [SerializeField] public GameObject startMenu;
    [SerializeField] public GameObject usernameMenu;

    public UIController inputActions;

    public bool isPressedAnyKey = false;

    private InputAction pressAnyKey;

    private void Awake()
    {
        inputActions = new UIController();
    }

    private void OnEnable()
    {
        pressAnyKey = inputActions.Player.PressAnyKey;
        pressAnyKey.Enable();
        pressAnyKey.performed += PassStartMenu;
    }

    private void OnDisable()
    {
        pressAnyKey.Disable();
    }

    private void PassStartMenu(InputAction.CallbackContext callbackContext)
    {
        if (isPressedAnyKey)
            return;
        else
            CloseAndOpenMenu();
    }

    private void CloseAndOpenMenu()
    {
        isPressedAnyKey = true;
        startMenu.gameObject.SetActive(false);
        usernameMenu.gameObject.SetActive(true);
    }
}
