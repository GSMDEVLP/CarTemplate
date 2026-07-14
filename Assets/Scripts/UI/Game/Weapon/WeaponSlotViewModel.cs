using UnityEngine;

public sealed class WeaponSlotViewModel : ViewModelBase
{
    public ObservableProperty<Sprite> Icon { get; } = new ObservableProperty<Sprite>(null);
    public ObservableProperty<string> AmmoText { get; } = new ObservableProperty<string>("0/0");
    public ObservableProperty<string> CooldownText { get; } = new ObservableProperty<string>("");
    public ObservableProperty<bool> IsActive { get; } = new ObservableProperty<bool>(false);

    private readonly int _slotIndex;
    private readonly IWeaponHudSource _source;
    private float _cooldownRemaining;

    public WeaponSlotViewModel(int slotIndex, IWeaponHudSource source)
    {
        _slotIndex = slotIndex;
        _source = source;

        _source.SlotStateChanged += OnSlotStateChanged;
        _source.ActiveSlotChanged += OnActiveSlotChanged;
        ApplyState(_source.GetSlotState(_slotIndex));
    }

    private void OnSlotStateChanged(WeaponSlotState state)
    {
        if (state.SlotIndex == _slotIndex)
            ApplyState(state);
    }

    private void OnActiveSlotChanged(int slotIndex)
    {
        IsActive.Value = slotIndex == _slotIndex;
    }

    public void Refresh()
    {
        _cooldownRemaining = Mathf.Max(0f, _cooldownRemaining - Time.deltaTime);
        CooldownText.Value = _cooldownRemaining > 0f
            ? _cooldownRemaining.ToString("0.0")
            : "";
    }

    public override void Dispose()
    {
        _source.SlotStateChanged -= OnSlotStateChanged;
        _source.ActiveSlotChanged -= OnActiveSlotChanged;
    }

    private void ApplyState(WeaponSlotState state)
    {
        AmmoText.Value = $"{state.CurrentAmmo}/{state.MaxAmmo}";
        IsActive.Value = state.IsActive;
        _cooldownRemaining = state.CooldownRemaining;
        Refresh();
    }
}
