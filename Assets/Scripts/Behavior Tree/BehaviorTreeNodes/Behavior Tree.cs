using UnityEngine;

namespace Pathfinding.BehaviorTree
{
    public class BehaviorTree : Node
    {
        public BehaviorTree(string name) : base(name) { }

        public override Status Process()
        {
            currentChild = 0; // Si ça marche je me tire une balle dans les couilles

            Debug.Log("Le behavior tree se lance !");
            while (currentChild < children.Count)
            {
                var status = children[currentChild].Process();
                if (status != Status.Success)
                {
                    return status;
                }
                currentChild++;
            }
            return Status.Success;
        }
    }
}