using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private UIDocument uiDocument;
    [SerializeField]
    private VisualTreeAsset mainMenuUXML;
    [SerializeField]
    private VisualTreeAsset settingsUXML;
    [SerializeField]
    private InputActionReference pauseAction;
    [SerializeField]
    private AudioSource musicSource;

    private VisualElement menuContainer;
    private bool isPlaying = false;

    public void Start()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += OnPause;

        // Создаем контейнер для меню
        menuContainer = new VisualElement();
        menuContainer.style.position = Position.Absolute;
        menuContainer.style.top = 0;
        menuContainer.style.left = 0;
        menuContainer.style.width = Length.Percent(100);
        menuContainer.style.height = Length.Percent(100);
        uiDocument.rootVisualElement.Add(menuContainer);

        ShowMainMenu();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (isPlaying) ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        menuContainer.Clear();
        mainMenuUXML.CloneTree(menuContainer);

        menuContainer.Q<Button>("buttonPlay").clicked += StartGame;
        menuContainer.Q<Button>("buttonSettings").clicked += ShowSettings;
        menuContainer.Q<Button>("buttonExit").clicked += QuitGame;

        menuContainer.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
        isPlaying = false;
    }

    public void ShowSettings()
    {
        menuContainer.Clear();
        settingsUXML.CloneTree(menuContainer);

        Slider musicSlider = menuContainer.Q<Slider>("soundMusic");
        musicSlider.value = PlayerPrefs.GetFloat("Music", 0.5f);
        musicSlider.RegisterValueChangedCallback(v => {
            if (musicSource) musicSource.volume = v.newValue;
            PlayerPrefs.SetFloat("Music", v.newValue);
        });

        menuContainer.Q<Button>("buttonExit").clicked += ShowMainMenu;
    }

    public void StartGame()
    {
        menuContainer.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
        isPlaying = true;
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}