using System.Collections.Generic;
using UnityEngine;

namespace DBManager
{
    // Add the System.Serializable Attribute to all classes to help serialize the class.
    [System.Serializable]
    public class NumberOfSwitches
    {
        public int numberOfSwitches;
        public bool wasSuccessfull;
    }
    public class NumberofAggro
    {
        public int numAggro;
        public bool wasSuccessful;
        public float timeSinceFirstAggro;
    }

    public class HunterMovement
    {
        public List<Vector3> positions;
    }
}