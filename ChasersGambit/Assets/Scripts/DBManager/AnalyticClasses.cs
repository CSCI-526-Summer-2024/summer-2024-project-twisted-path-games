namespace DBManager
{
    // Add the System.Serializable Attribute to all classes to help serialize the class.
    [System.Serializable]
    public class SampleData
    {
        public string firstName;
        public string lastName;
    }
    
    [System.Serializable]
    public class NumberOfSwitches
    {
        public int numberOfSwitches;
    }
}