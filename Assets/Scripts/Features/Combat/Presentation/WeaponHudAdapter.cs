using System;

public sealed class WeaponHudAdapter : IWeaponHudSource, IDisposable
{
    private readonly WeaponService _service;
    private readonly IEventBus _bus;
    private int _activeSlot;

    public int SlotCount => _service.WeaponCount;

    public event Action<WeaponSlotState> SlotStateChanged;
    public event Action<int> ActiveSlotChanged;

    public WeaponHudAdapter(WeaponService service, IEventBus bus, int activeSlot)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _activeSlot = activeSlot;

        _service.SlotStateChanged += OnSlotStateChanged;
        _bus.Subscribe<ActiveWeaponChanged>(OnActiveWeaponChanged);
    }

    public WeaponSlotState GetSlotState(int slotIndex)
    {
        var status = _service.GetStatus(slotIndex);

        return new WeaponSlotState(
            slotIndex,
            status.CurrentAmmo,
            status.MaxAmmo,
            status.CooldownRemaining,
            _service.CooldownDuration(slotIndex),
            slotIndex == _activeSlot);
    }

    public void Dispose()
    {
        _service.SlotStateChanged -= OnSlotStateChanged;
        _bus.Unsubscribe<ActiveWeaponChanged>(OnActiveWeaponChanged);
    }

    private void OnSlotStateChanged(int slotIndex)
    {
        SlotStateChanged?.Invoke(GetSlotState(slotIndex));
    }

    private void OnActiveWeaponChanged(ActiveWeaponChanged e)
    {
        _activeSlot = e.SlotIndex;
        ActiveSlotChanged?.Invoke(_activeSlot);
    }
}
