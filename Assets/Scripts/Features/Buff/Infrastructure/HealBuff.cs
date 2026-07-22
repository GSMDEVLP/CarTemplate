
public class HealBuff : IBuff
{
    public BuffType BuffType => healBuffDefinition.Type;
    
    private HealBuffDefinition healBuffDefinition;

    public HealBuff(HealBuffDefinition healBuffDefinition)
    {
        this.healBuffDefinition = healBuffDefinition;
    }

    public void Apply(EntityId targetId)
    {
        if (!UnityEntityRegistry.TryGet(targetId, out var entity))
            return;

        if (entity.TryGetComponent(out IHealable healable))
        {
            healable.Heal(healBuffDefinition.HealAmount);
        }
    }
    public void Remove(EntityId targetId)
    {
    }
}
