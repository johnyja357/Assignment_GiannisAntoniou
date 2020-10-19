public class FoodObject 
{
    private static readonly object padlock = new object();
    private static FoodObject instance = null;

    public FoodData[] FoodDatas;  
    
    public static FoodObject Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                    instance = new FoodObject();

                return instance;
            }
        }
    }
}
