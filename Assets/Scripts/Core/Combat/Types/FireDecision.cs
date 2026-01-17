public enum FireFailReason
{
    None,
    Cooldown,
    NoAmmo,
    Overheated
}

public readonly struct FireDecision
{
    public readonly bool Success;
    public readonly FireFailReason Reason;

    public FireDecision(bool success, FireFailReason reason)
    {
        Success = success;
        Reason = reason;
    }
}
