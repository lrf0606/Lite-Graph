using System;

namespace LiteGraphFrame
{
    [AttributeUsage(AttributeTargets.Field)]
    class NodeOutputAttribute : Attribute
    {
        //  Ù–‘÷–Œƒ√Ë ˆ
        public string FiledDescription { get; private set; }

        public NodeOutputAttribute(string filedDescription = "")
        {
            FiledDescription = filedDescription;
        }
    }
}


