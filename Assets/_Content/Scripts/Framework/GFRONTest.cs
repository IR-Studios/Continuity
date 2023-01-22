using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GRFON;
using System.IO;

public class GFRONTest : MonoBehaviour
{
    public ItemTest testItem;


    public void Start() 
    {
        test();
        Debug.Log(test());
    }

    public string test() 
    {
         var formatter = new GrfonFormatter();
        using(var memStream = new System.IO.MemoryStream()) {
            formatter.Serialize(memStream, testItem);
            return System.Text.Encoding.UTF8.GetString(memStream.ToArray());
    }

  

    static GrfonCollection ToCollection(List<string> strings) {
        GrfonCollection coll = new GrfonCollection();
        foreach (string s in strings) coll.Add(new GrfonValue(s));
        return coll;
    }
      
}
}
