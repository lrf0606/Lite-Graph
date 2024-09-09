using UnityEngine;
using LiteGraphFrame;



public class LiteGraphRuntime : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        // 只需初始化一次
        LiteGraphNodeFactory.InitCustomFactory();

        // 运行时测试
        int eventId = 1; // EventNodeEventTest1.GetEventId()的返回值
        LiteGraphRuntimeUtil.RunLiteGrpah("Assets/LiteGraphFiles/Example.litegraph", eventId);
        eventId = 2; // EventNodeEventTest2.GetEventId()的返回值
        LiteGraphRuntimeUtil.RunLiteGrpah("Assets/LiteGraphFiles/Example.litegraph", eventId);
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
