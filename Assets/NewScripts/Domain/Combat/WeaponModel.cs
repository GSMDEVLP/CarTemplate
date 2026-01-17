public sealed class WeaponModel
{
    private readonly WeaponStats _stats;
    private float _nextFireTime;
    private int _currentAmmo;
    private float _heat;
    private bool _overheated;

    public WeaponModel(WeaponStats stats)
    {
        _stats = stats;
        _currentAmmo = stats.MaxAmmo;
    }

    public int CurrentAmmo => _currentAmmo;
    public int MaxAmmo => _stats.MaxAmmo;
    public float CooldownDuration => _stats.Cooldown;
    public float Heat => _heat;
    public bool IsOverheated => _overheated;

    public float CooldownRemaining(float now)
    {
        float remaining = _nextFireTime - now;
        return remaining > 0f ? remaining : 0f;
    }

    public FireDecision TryFire(float now)
    {
        if (now < _nextFireTime)
            return new FireDecision(false, FireFailReason.Cooldown);

        if (_currentAmmo <= 0)
            return new FireDecision(false, FireFailReason.NoAmmo);

        if (_stats.MaxHeat > 0f && _overheated)
            return new FireDecision(false, FireFailReason.Overheated);

        _nextFireTime = now + _stats.Cooldown;
        _currentAmmo--;

        if (_stats.MaxHeat > 0f && _stats.HeatPerShot > 0f)
        {
            _heat += _stats.HeatPerShot;
            if (_heat >= _stats.MaxHeat)
            {
                _heat = _stats.MaxHeat;
                _overheated = true;
            }
        }

        return new FireDecision(true, FireFailReason.None);
    }

    public void Tick(float deltaTime)
    {
        if (_stats.CoolRatePerSec <= 0f || _heat <= 0f)
            return;

        _heat -= _stats.CoolRatePerSec * deltaTime;
        if (_heat <= 0f)
        {
            _heat = 0f;
            _overheated = false;
        }
    }
    
    public bool CanFire(float now)
    {
        if (now < _nextFireTime) return false;
        if (_currentAmmo <= 0) return false;
        if (_stats.MaxHeat > 0f && _overheated) return false;
        return true;
    }


    public void RefillAmmo()
    {
        _currentAmmo = _stats.MaxAmmo;
    }
}
