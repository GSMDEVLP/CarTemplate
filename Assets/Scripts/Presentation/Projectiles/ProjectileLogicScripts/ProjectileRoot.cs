using UnityEngine;

public class ProjectileRoot : MonoBehaviour
{
    [SerializeField] private ProjectilePart[] parts;

    public void Init(ProjectileContext ctx)
    {
        if (parts == null || parts.Length == 0)
            parts = GetComponents<ProjectilePart>();

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] != null)
                parts[i].Init(ctx);
        }
    }
}
