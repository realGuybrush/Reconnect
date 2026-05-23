using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    
    [SerializeField]
    private PlayerInput playerInput;
    
    [SerializeField]
    private GameObject trigger;

    private InputAction click;

    private void Start()
    {
        click = playerInput.actions.FindAction("Attack");
        click.started += LMBPressed;
        click.canceled += LMBReleased;
    }

    private void Update()
    {
        var pos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = new Vector3(pos.x, pos.y, 0f);
    }

    private void OnDestroy()
    {
        click.performed -= LMBPressed;
        click.canceled -= LMBReleased;
    }

    private void LMBPressed(InputAction.CallbackContext callbackContext)
    {
        trigger.SetActive(true);
    }

    private void LMBReleased(InputAction.CallbackContext callbackContext)
    {
        trigger.SetActive(false);
    }

}
