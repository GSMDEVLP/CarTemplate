using UnityEngine;


public interface IDamageService
{
    public void Deal(object target, float amount, DamageContext ctx);

    void DealArea(Vector3 position, float radius, float amount, DamageContext ctx);
}
