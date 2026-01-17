public sealed class WeaponProxy : IWeapon
{
    private readonly int _index;
    private readonly WeaponService _service;

    public WeaponProxy(int index, WeaponService service)
    {
        _index = index;
        _service = service;
    }

    public bool CanFire => _service.CanFire(_index);
    public float CooldownRemaining => _service.CooldownRemaining(_index);
    public float CooldownDuration => _service.CooldownDuration(_index);
    public int CurrentAmmo => _service.CurrentAmmo(_index);
    public int MaxAmmo => _service.MaxAmmo(_index);

    public void Fire(FireContext ctx)
    {
        var req = new FireRequest(ctx.Origin, ctx.Direction, ctx.Owner);
        _service.TryFire(_index, req);
    }
}
