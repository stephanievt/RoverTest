namespace RoverTest.ModelApplicationData
{
    public abstract class RoverObjectCollection
    {
        public List<T> LoadRoverData<T>()
        {
            string fileName = this.GetType().Name + ".json";
            //TODO: This is rover installable for file management
            const string roverFolder = "C:\\PriceBookRoverData\\";
            string jsonData = File.ReadAllText(roverFolder + fileName);
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
