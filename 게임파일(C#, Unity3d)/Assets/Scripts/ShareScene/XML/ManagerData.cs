// ManagerData is our custom class that holds our defined objects we want to store in XML format 

public struct GateData
{
    public int nGold;
}

 public class ManagerData 
 { 
    // We have to define a default instance of the structure 
   public Data manage;
   public Adjustment adjustment;
    // Default constructor doesn't really do anything at the moment 
   public ManagerData() { } 
   
   // �����Ұ͵�
   public struct Data 
   {
       public int nGold;
   }


     //! ������ �͵�
	public struct Adjustment
	{
        public float fangxiang;
        public float youmen;
        public float shache;
	}
}
