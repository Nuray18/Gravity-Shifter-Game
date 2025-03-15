using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshRebuilder : MonoBehaviour
{
    private NavMeshSurface[] navMeshSurfaces;

    private void Awake()
    {
        UpdateNavMeshSurfaces();
    }

    public void UpdateNavMeshSurfaces()
    {
        navMeshSurfaces = FindObjectsOfType<NavMeshSurface>(); // Güncel referansları al
    }

    public void RebuildNavMeshes()
    {
        Debug.Log("RebuildNavMeshes called!");
        if (navMeshSurfaces == null || navMeshSurfaces.Length == 0) return;

        foreach (NavMeshSurface surface in navMeshSurfaces)
        {
            if (surface != null) // Null kontrolü ekledik
            {
                surface.BuildNavMesh();
            }
        }
    }
}
