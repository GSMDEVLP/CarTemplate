using System;

public sealed class AIWeaponBurstPlanner
{
    private readonly Random _rng;
    private int _shotsRemaining;
    private float _nextShotTime;
    private float _nextBurstTime;

    public AIWeaponBurstPlanner(Random rng)
    {
        _rng = rng ?? new Random();
    }

    public void Reset()
    {
        _shotsRemaining = 0;
        _nextShotTime = 0f;
        _nextBurstTime = 0f;
    }

    public bool CanFire(float time, AIWeaponSlotData slot)
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

    public void ConsumeShot(float time, AIWeaponSlotData slot)
    {
        if (slot == null)
            return;

        _shotsRemaining--;
        _nextShotTime = time + Math.Max(0.01f, slot.ShotInterval);

        if (_shotsRemaining <= 0)
        {
            float min = Math.Max(0f, slot.BurstCooldownMin);
            float max = Math.Max(min, slot.BurstCooldownMax);
            _nextBurstTime = time + RangeFloat(min, max);
        }
    }

    private void StartBurst(float time, AIWeaponSlotData slot)
    {
        int min = Math.Max(1, slot.BurstMinShots);
        int max = Math.Max(min, slot.BurstMaxShots);
        _shotsRemaining = RangeInt(min, max);
        _nextShotTime = time;
    }

    private int RangeInt(int min, int maxInclusive) => _rng.Next(min, maxInclusive + 1);
    private float RangeFloat(float min, float max) => (float)(_rng.NextDouble() * (max - min) + min);
}
