using UnityEngine;

public class MachineGunWeapon : WeaponBase
{
     private readonly IEventBus _bus;
    private readonly IDamageService _damage;

    private float _currentHeat;
    private bool _overheated;

    public MachineGunWeapon(
        WeaponConfig cfg,
        WeaponRuntime rt,
        ITime tm,
        IEventBus bus,
        IDamageService damage)
        : base(cfg, rt, tm)
    {
        _bus = bus;
        _damage = damage;
    }

    
    public void Tick(float deltaTime)
    {
        if (_currentHeat > 0f)
        {
            _currentHeat -= Rt.CoolRatePerSec * deltaTime;
            if (_currentHeat <= 0f)
            {
                _currentHeat = 0f;
                if (_overheated)
                {
                    _overheated = false;
                    // можно кинуть событие "охлаждение завершено"
                    // _bus.Publish(new WeaponCooledDown(Cfg.ID));
                }
            }
        }
    }

    protected override void OnFire(FireContext ctx)
    {
        _currentHeat += Rt.HeatPerShot;
        if (_currentHeat >= Rt.MaxHeat)
        {
            _currentHeat = Rt.MaxHeat;
            _overheated = true;
            // можно кинуть событие "оружие перегрелось"
            // _bus.Publish(new WeaponOverheated(Cfg.ID));
        }

        // обычный прямой выстрел
        var go = Object.Instantiate(
            Rt.ProjectilePrefab,
            ctx.Origin,
            Quaternion.LookRotation(ctx.Direction));

        var root = go.GetComponent<ProjectileRoot>();
        if (root == null)
        {
            Object.Destroy(go);
            return;
        }

        var pctx = new ProjectileContext
        {
            Rt = Rt,
            Owner = ctx.Owner,
            Target = null,
            DamageService = _damage
        };
        root.Init(pctx);
        Tick(Time.deltaTime);

        _bus.Invoke(new WeaponFired(ctx.Owner, Cfg));
    }
}
