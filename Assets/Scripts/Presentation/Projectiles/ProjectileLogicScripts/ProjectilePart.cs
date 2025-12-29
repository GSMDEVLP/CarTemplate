using UnityEngine;

public abstract class ProjectilePart : MonoBehaviour
{
    protected ProjectileContext Ctx { get; private set; }
    protected bool IsInitialized { get; private set; }

    public void Init(ProjectileContext ctx)
    {
        Ctx = ctx;
        IsInitialized = true;
        OnInit(ctx);
    }

    protected virtual void OnInit(ProjectileContext ctx) { }
}
