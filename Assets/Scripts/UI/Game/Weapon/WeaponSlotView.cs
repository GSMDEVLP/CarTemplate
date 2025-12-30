using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class WeaponSlotView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI cooldownText;
    // [SerializeField] private GameObject activeHighlight;

    private WeaponSlotViewModel _vm;

    public void Bind(WeaponSlotViewModel vm)
    {
        _vm = vm;

        _vm.Icon.OnChanged += OnIconChanged;
        _vm.AmmoText.OnChanged += OnAmmoChanged;
        _vm.CooldownText.OnChanged += OnCooldownChanged;
        _vm.IsActive.OnChanged += OnActiveChanged;

        OnIconChanged(_vm.Icon.Value);
        OnAmmoChanged(_vm.AmmoText.Value);
        OnCooldownChanged(_vm.CooldownText.Value);
        OnActiveChanged(_vm.IsActive.Value);
    }

    private void OnDestroy()
    {
        if (_vm == null) return;

        _vm.Icon.OnChanged -= OnIconChanged;
        _vm.AmmoText.OnChanged -= OnAmmoChanged;
        _vm.CooldownText.OnChanged -= OnCooldownChanged;
        _vm.IsActive.OnChanged -= OnActiveChanged;
    }

    private void OnIconChanged(Sprite s)
    {
        if (icon != null) icon.sprite = s;
    }

    private void OnAmmoChanged(string v)
    {
        if (ammoText != null) ammoText.text = v;
    }

    private void OnCooldownChanged(string v)
    {
        if (cooldownText != null) cooldownText.text = v;
    }

    private void OnActiveChanged(bool v)
    {
        // if (activeHighlight != null) activeHighlight.SetActive(v);
    }
}
