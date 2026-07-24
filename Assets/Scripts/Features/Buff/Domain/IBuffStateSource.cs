using System;

public interface IBuffStateSource
{
    BuffType Type { get; }
    BuffState State { get; }

    event Action<BuffState> StateChanged;
}
