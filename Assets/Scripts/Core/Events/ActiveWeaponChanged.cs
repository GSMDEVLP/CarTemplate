public readonly struct ActiveWeaponChanged : IEvent
{
    public readonly WeaponConfig Config;
    public ActiveWeaponChanged(WeaponConfig config)
    {
        Config = config;
    }
}
