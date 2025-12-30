using TMPro;
using UnityEngine;

public class HealthView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;
    private HealthViewModel _vm;

    public void Bind(HealthViewModel vm)
    {
        _vm = vm;
        _vm.HpText.OnChanged += OnHpChanged;
        OnHpChanged(_vm.HpText.Value);
    }

    private void OnDestroy()
    {
        if (_vm == null) return;
        _vm.HpText.OnChanged -= OnHpChanged;
    }

    private void OnHpChanged(string value)
    {
        if (hpText != null) hpText.text = value;
    }
}
