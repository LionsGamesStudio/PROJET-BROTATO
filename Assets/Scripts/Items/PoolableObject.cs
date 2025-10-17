using UnityEngine;

/// <summary>
/// A helper component attached to prefabs that need to be pooled.
/// It stores a reference to its original prefab ID, allowing pooled instances
/// to be correctly identified and returned to the correct pool.
/// </summary>
public class PoolableObject : MonoBehaviour
{
    // This ID will be unique for each prefab asset.
    public int PrefabId { get; private set; }

    public void SetPrefabId(int id)
    {
        PrefabId = id;
    }
}