using UnityEngine;
using System.Collections;



public class GameSateData
{
   static public ManagerData myData = new ManagerData();
   static private string _data; 

    static public void SaveData()
	{
        ManageData();


	    _data = GameStateXML.SerializeObject(myData,"ManagerData"); 
        GameStateXML.CreateXML("Data.xml", _data);

	}

    static public void LoadData()
	{
        _data = GameStateXML.LoadXML("Data.xml");
		
        if(_data.ToString() != "") 
	    {
            myData = (ManagerData)GameStateXML.DeserializeObject(_data,"ManagerData");
	    }
	}

	
    static public void ManageData()
    {
        myData.manage.nGold = GameScene.g_nGold;
        //myData.manage.nTotalCount  = 100;
        //myData.manage.nArray = new int[10];
        //myData.manage.mFuck = new Fuck[10];

        //for (int i = 0; i < 10; i++)
        //{
        //    myData.manage.nArray[i] = i*10;
        //    myData.manage.mFuck[i].finger = i * 5;
        //    myData.manage.mFuck[i].midfinger = i * 15;
        //}

    }


}
