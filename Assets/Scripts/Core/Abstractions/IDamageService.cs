using UnityEngine;
public interface IDamageService
{
    public void Deal(object target, float amount, object source = null);
}
