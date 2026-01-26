namespace RoverTest.ModelApplicationData
{
    public abstract class RoverObjectCollection
    {

        public List<T> LoadRoverData<T>()
        {
            //TODO: Magic string.
            // You know where this could be handled? Project template lays down the folder. 
            // And we store it then in a thingy.
            string fileFolder =
                "C:\\Dev\\RoverTest\\RoverExtras\\MockModelImplementation\\MockBusinessObjects\\TestEnvironmentData\\";
            string fileName = this.GetType().Name + ".json";
            string jsonData = File.ReadAllText(fileFolder + fileName);
            List<T> objectList = RoverInternals.DeserializeJsonArray<T>(jsonData);
            foreach (var item in objectList)
            {
                if (item is RoverDataObject roverObject)
                {
                    roverObject.IsRoverTestData = true;
                }
            }

            return objectList;
        }
    }
}
