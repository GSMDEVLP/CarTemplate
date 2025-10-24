
public interface IWeapon 
{
    public bool CanFire { get; }
    public void Fire(FireContext ctx);
}
