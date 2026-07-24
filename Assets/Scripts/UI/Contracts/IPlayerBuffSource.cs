using System;

public interface IPlayerBuffSource
{
    event Action<BuffState> BuffChanged;

    bool TryGetState(BuffType type, out BuffState state);
}
