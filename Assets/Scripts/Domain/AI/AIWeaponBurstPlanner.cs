using UnityEngine;

public sealed class AIWeaponBurstPlanner
{
    private int _shotsRemaining;
    private float _nextShotTime;
    private float _nextBurstTime;

    public void Reset()
    {
        _shotsRemaining = 0;
        _nextShotTime = 0f;
        _nextBurstTime = 0f;
    }

    public bool CanFire(float time, AIWeaponSlot slot)
    {
        if (slot == null)
            return false;

        if (_shotsRemaining <= 0)
        {
            if (time < _nextBurstTime)
                return false;

            StartBurst(time, slot);
        }

        return time >= _nextShotTime;
    }

    public void ConsumeShot(float time, AIWeaponSlot slot)
    {
        if (slot == null)
            return;

        _shotsRemaining--;
        _nextShotTime = time + Mathf.Max(0.01f, slot.ShotInterval);

        if (_shotsRemaining <= 0)
        {
            float min = Mathf.Max(0f, slot.BurstCooldownMin);
            float max = Mathf.Max(min, slot.BurstCooldownMax);
            _nextBurstTime = time + Random.Range(min, max);
        }
    }

    private void StartBurst(float time, AIWeaponSlot slot)
    {
        int min = Mathf.Max(1, slot.BurstMinShots);
        int max = Mathf.Max(min, slot.BurstMaxShots);
        _shotsRemaining = Random.Range(min, max + 1);
        _nextShotTime = time;
    }
}
