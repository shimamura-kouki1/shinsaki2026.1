using TMPro;
using UnityEngine;

public class PasswordGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private TextMeshProUGUI _passwordText;

    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        _root.SetActive(true);
    }

    public void Hide()
    {
        _root.SetActive(false);
    }

    public void UpdatePassword(string text)
    {
        _passwordText.text = text;
    }
}
