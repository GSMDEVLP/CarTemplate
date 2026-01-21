using NVec3 = System.Numerics.Vector3;


public interface IDamageService
{
    public void Deal(EntityId  target, float amount, DamageContext ctx);
    void DealArea(NVec3 position, float radius, float amount, DamageContext ctx);
}
