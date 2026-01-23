public sealed class AIWeaponSelector
{
    public int SelectSlot(AIWeaponSlotData[] slots, float distance, float dot, bool hasLineOfSight)
    {
        if (slots == null || slots.Length == 0)
            return -1;

        int best = -1;
        int bestPriority = int.MinValue;
        float bestRange = float.MaxValue;

        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            if (slot == null) continue;

            if (slot.Kind == WeaponKind.Homing)
                continue;

            if (distance < slot.MinRange || distance > slot.MaxRange)
                continue;

            if (dot < slot.MinDot || dot > slot.MaxDot)
                continue;

            if (slot.RequiresLineOfSight && !hasLineOfSight)
                continue;

            if (slot.Priority > bestPriority ||
                (slot.Priority == bestPriority && slot.MaxRange < bestRange))
            {
                best = i;
                bestPriority = slot.Priority;
                bestRange = slot.MaxRange;
            }
        }

        return best;
    }
}
