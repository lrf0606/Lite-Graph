using System;

namespace LiteGraphFrame
{
    [AttributeUsage(AttributeTargets.Field)]
    class NodeOutputAttribute : Attribute
    {
        // ������������
        public string FiledDescription { get; private set; }

        public NodeOutputAttribute(string filedDescription = "")
        {
            FiledDescription = filedDescription;
        }
    }
}


