
public readonly struct ActiveWeaponChanged : IEvent
{
    public readonly WeaponDefinition Data;
    public ActiveWeaponChanged(WeaponDefinition data)
    {
        Data = data;
    }
}
