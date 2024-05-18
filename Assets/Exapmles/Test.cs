using UnityEngine;
using LiteGraphFrame;


public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LiteGraphRuntimeUtil.RunLiteGrpah("Assets/LiteGraphFiles/New Lite Graph.bp", EBPEventId.Test1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
