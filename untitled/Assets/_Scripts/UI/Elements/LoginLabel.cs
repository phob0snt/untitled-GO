using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class LoginLabel : MonoBehaviour
{
    public static UnityEvent<UserRegisterDto> OnRegister = new();
    public static UnityEvent<UserLoginDto> OnLogin = new();

    [SerializeField] private TMP_InputField _usernameInput;
    [SerializeField] private TMP_InputField _emailInput;
    [SerializeField] private TMP_InputField _passwordInput;

    [SerializeField] private TMP_InputField _usernameOrEmailInput;
    [SerializeField] private TMP_InputField _loginPasswordInput;

    [SerializeField] private Button _goToRegisterScreenButton;
    [SerializeField] private Button _goToLoginScreenButton;

    [SerializeField] private Button _registerButton;
    [SerializeField] private Button _loginButton;

    [SerializeField] private TMP_Text _errorsDisplay;
    [SerializeField] private TMP_Text _loginErrorsDisplay;

    [SerializeField] private GameObject _loginTab;
    [SerializeField] private GameObject _registerTab;

    public static LoginLabel Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        _registerButton.onClick.AddListener(Register);
        _loginButton.onClick.AddListener(Login);
        _goToRegisterScreenButton.onClick.AddListener(() => { _loginTab.SetActive(false); _registerTab.SetActive(true); });
        _goToLoginScreenButton.onClick.AddListener(() => { _loginTab.SetActive(true); _registerTab.SetActive(false); });
    }

    private void OnDisable()
    {
        _registerButton.onClick.RemoveAllListeners();
        _loginButton.onClick.RemoveAllListeners();
        _goToRegisterScreenButton.onClick.RemoveAllListeners();
        _goToLoginScreenButton.onClick.RemoveAllListeners();
    }

    private void Register()
    {
        UserRegisterDto dto = new()
        {
            UserName = _usernameInput.text,
            Email = _emailInput.text,
            Password = _passwordInput.text,
        };
        OnRegister.Invoke(dto);
    }

    private void Login()
    {
        UserLoginDto dto = new()
        {
            UsernameOrEmail = _usernameOrEmailInput.text,
            Password = _loginPasswordInput.text
        };
        OnLogin.Invoke(dto);
    }

    public void DisplayRegisterError(string err)
    {
        _errorsDisplay.text = err;
    }

    public void DisplayLoginError(string err)
    {
        _loginErrorsDisplay.text = err;
    }

}
