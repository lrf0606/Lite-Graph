using UnityEngine;
using LiteGraphFrame;
using System;



public class LiteGraphRuntime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        LiteGraphNodeFactory.InitCustomFactory();
        LiteGraphNodeFactory.InitBuiltinFactory();
        Debug.Log("Run Event1");
        LiteGraphRuntimeUtil.RunLiteGrpah("Assets/LiteGraphFiles/Example.litegraph", 1);
       // Debug.Log("Run Event2");
       // LiteGraphRuntimeUtil.RunLiteGrpah("Assets/LiteGraphFiles/Example.litegraph", 2);
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
