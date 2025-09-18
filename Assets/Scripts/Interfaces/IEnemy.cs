using UnityEngine;

public interface IEnemy
{
    GameObject GameObject { get; }
    Transform Transform { get; }
    bool IsDestroyed { get; } // Nouvelle propriété pour vérifier si détruit
}