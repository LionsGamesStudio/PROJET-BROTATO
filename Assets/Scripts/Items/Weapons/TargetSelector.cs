using System.Collections.Generic;

public interface TargetSelector
{
    IHealthTarget SelectTarget(List<IHealthTarget> possibleTargets);
}