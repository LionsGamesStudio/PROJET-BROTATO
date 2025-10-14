using System;
using FluxFramework.Core;
using UnityEngine;

/// <summary>
/// Defines a unique identity for a GameObject, making it an "Entity".
/// This component is the single source of truth for the entity's unique ID.
/// All other components on this GameObject that need an ID (like HealthComponent, AttackerComponent)
/// should fetch it from here.
/// </summary>
public class Entity : FluxMonoBehaviour, IEntity
{
    // Private backing field, not serialized to ensure it's unique per runtime instance.
    private int _id;

    public int ID
    {
        get
        {
            // Generate a new ID only if it hasn't been initialized yet.
            if (_id == 0)
            {
                // Using a GUID hash is a good way to get a reasonably unique ID at runtime.
                _id = Guid.NewGuid().GetHashCode();
            }
            return _id;
        }
    }
}