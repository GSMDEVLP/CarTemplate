public sealed class WeaponStateStore
{
    private readonly WeaponDefinition[] _defs;
    private readonly WeaponModel[] _models;

    public WeaponStateStore(WeaponDefinition[] defs)
    {
        _defs = defs ?? new WeaponDefinition[0];
        _models = new WeaponModel[_defs.Length];

        for (int i = 0; i < _defs.Length; i++)
        {
            var def = _defs[i];
            if (def.Config == null)
                continue;

            _models[i] = new WeaponModel(def.Stats);
        }
    }

    public int Count => _models.Length;

    public bool IsConfigured(int index)
    {
        return index >= 0 && index < _models.Length && _models[index] != null && _defs[index].Config != null;
    }

    public bool TryGetSlot(int index, out WeaponDefinition def, out WeaponModel model)
    {
        def = default;
        model = null;

        if (index < 0 || index >= _models.Length)
            return false;

        model = _models[index];
        def = _defs[index];
        return model != null && def.Config != null;
    }

    public WeaponStatus GetStatus(int index, float now)
    {
        if (index < 0 || index >= _models.Length)
            return default;

        var model = _models[index];
        if (model == null)
            return default;

        return new WeaponStatus(
            model.CurrentAmmo,
            model.MaxAmmo,
            model.CooldownRemaining(now));
    }

    public void Tick(float dt)
    {
        for (int i = 0; i < _models.Length; i++)
            _models[i]?.Tick(dt);
    }

    public bool CanFire(int index, float now)
    {
        if (index < 0 || index >= _models.Length) return false;
        var model = _models[index];
        return model != null && model.CanFire(now);
    }

    public float CooldownRemaining(int index, float now)
    {
        if (index < 0 || index >= _models.Length) return 0f;
        var model = _models[index];
        return model != null ? model.CooldownRemaining(now) : 0f;
    }

    public int CurrentAmmo(int index)
    {
        if (index < 0 || index >= _models.Length) return 0;
        var model = _models[index];
        return model != null ? model.CurrentAmmo : 0;
    }

    public int MaxAmmo(int index)
    {
        if (index < 0 || index >= _models.Length) return 0;
        var model = _models[index];
        return model != null ? model.MaxAmmo : 0;
    }

    public float CooldownDuration(int index)
    {
        return index >= 0 && index < _defs.Length ? _defs[index].Stats.Cooldown : 0f;
    }
}
