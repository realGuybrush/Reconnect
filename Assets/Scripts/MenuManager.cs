using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI endGameText;
    
    [SerializeField]
    private GameObject menu, endMenu;
    
    [SerializeField]
    private PlayerInput input;

    private InputAction escape;
    
    void Awake()
    {
        escape = input.actions.FindAction("Escape");
        escape.performed += HandleEscape;
    }
    
    void Start()
    {
        Time.timeScale = 0;
    }

    private void OnDestroy()
    {
        escape.performed -= HandleEscape;
    }

    private void HandleEscape(InputAction.CallbackContext callbackContext)
    {
        if (endMenu.activeSelf) Quit();
        if (menu.activeSelf)
            ResumeGame();
        else
            PauseGame();
        menu.SetActive(!menu.activeSelf);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    
    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void Restart()
    {
        ResumeGame();
        SceneManager.LoadScene("GameScene");
    }

    public void Quit()
    {
        ResumeGame();
        SceneManager.LoadScene("IntroScene");
    }

    public void EndGame(bool win)
    {
        endGameText.text = win ? "You have charged your ship and escaped!" :
            "The Electrician got you and removed you from the circuit.";
        endMenu.SetActive(true);
        PauseGame();
    }
}
