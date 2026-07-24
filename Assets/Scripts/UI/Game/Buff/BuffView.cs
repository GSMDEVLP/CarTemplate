using TMPro;
using UnityEngine;

public sealed class BuffView : MonoBehaviour
{
    [SerializeField] private BuffType _buffType;
    [SerializeField] private GameObject _contentRoot;
    [SerializeField] private TextMeshProUGUI _valueText;

    private BuffViewModel _viewModel;

    public BuffType BuffType => _buffType;

    public void Bind(BuffViewModel viewModel)
    {
        Unbind();
        _viewModel = viewModel;

        if (_viewModel == null)
            return;

        _viewModel.ValueText.OnChanged += OnValueChanged;
        _viewModel.IsVisible.OnChanged += OnVisibilityChanged;

        OnValueChanged(_viewModel.ValueText.Value);
        OnVisibilityChanged(_viewModel.IsVisible.Value);
    }

    private void OnValueChanged(string value)
    {
        if (_valueText != null)
            _valueText.text = value;
    }

    private void OnVisibilityChanged(bool isVisible)
    {
        if (_contentRoot != null)
            _contentRoot.SetActive(isVisible);
    }

    private void Unbind()
    {
        if (_viewModel == null)
            return;

        _viewModel.ValueText.OnChanged -= OnValueChanged;
        _viewModel.IsVisible.OnChanged -= OnVisibilityChanged;
        _viewModel = null;
    }

    private void OnDestroy()
    {
        Unbind();
    }
}
