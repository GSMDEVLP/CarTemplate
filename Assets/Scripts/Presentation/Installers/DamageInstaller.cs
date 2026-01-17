using UnityEngine;

public sealed class DamageInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private CollisionDamage[] collisionDamages;

    public void Install(GameServices services)
    {
        if (collisionDamages == null) return;
        for (int i = 0; i < collisionDamages.Length; i++)
            if (collisionDamages[i] != null)
                collisionDamages[i].Init(services.Damage);
    }
}
