using System.Collections.Generic;
using UnityEngine;


public class AttackAreaDetection : MonoBehaviour
{
    private IAttacker attacker;
    private IHealthTarget currentTarget;

    private List<IHealthTarget> detectedTargets = new List<IHealthTarget>();

    void Start()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        attacker = GetComponent<IAttacker>();

        if (attacker == null)
        {
            Debug.LogError("No IAttacker component found on " + gameObject.name);
            return;
        }

        sphereCollider.radius = attacker.Range;
    }

    void OnTriggerEnter(Collider other)
    {
        if (attacker == null) return;

        IHealthTarget newTarget = other.GetComponent<IHealthTarget>();

        if (newTarget != null && newTarget.ID != attacker.ID && !detectedTargets.Contains(newTarget))
        {
            detectedTargets.Add(newTarget);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (attacker == null) return;

        IHealthTarget exitingTarget = other.GetComponent<IHealthTarget>();

        if (exitingTarget != null && detectedTargets.Contains(exitingTarget))
        {
            detectedTargets.Remove(exitingTarget);
        }
    }

    void Update()
    {
        if (attacker == null || detectedTargets.Count == 0 || attacker.TargetSelector == null || currentTarget != null) return;

        currentTarget = attacker.TargetSelector.SelectTarget(detectedTargets);
        if (currentTarget != null)
        {
            StartCoroutine(attacker.Attack(currentTarget));
        }
    }

}