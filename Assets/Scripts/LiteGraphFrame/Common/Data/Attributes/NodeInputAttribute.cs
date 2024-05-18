using System;

namespace LiteGraphFrame
{
    [AttributeUsage(AttributeTargets.Field)]
    class NodeInputAttribute : Attribute
    {
        //  Ù–‘÷–Œƒ√Ë ˆ
        public string FiledDescription { get; private set; }

        public NodeInputAttribute(string filedDescription = "")
        {
            FiledDescription = filedDescription;
        }
    }
}




