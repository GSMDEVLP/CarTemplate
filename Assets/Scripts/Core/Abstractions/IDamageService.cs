using UnityEngine;
public interface IDamageService
{
    public void Deal(object target, float amount, object source = null);

    void DealArea(Vector3 position, float radius, float amount, object source = null);
}
