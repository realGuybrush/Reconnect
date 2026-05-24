using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorial;
    
    [SerializeField]
    private PlayerInput input;

    private InputAction escape;
    
    void Start()
    {
        escape = input.actions.FindAction("Escape");
        escape.performed += Escape;
    }

    private void OnDestroy()
    {
        escape.performed -= Escape;
    }

    private void Escape(InputAction.CallbackContext callbackContext)
    {
        tutorial.SetActive(false);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
