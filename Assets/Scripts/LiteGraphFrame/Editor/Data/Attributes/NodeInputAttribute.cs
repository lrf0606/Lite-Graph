using System;

namespace LiteGraphFrame
{
    [AttributeUsage(AttributeTargets.Field)]
    class NodeInputAttribute : Attribute
    {
        // ÊôÐÔÖÐÎÄÃèÊö
        private string m_FiledDescription;
        public string FiledDescription => m_FiledDescription;

        public NodeInputAttribute(string filedDescription = "")
        {
            m_FiledDescription = filedDescription;
        }
    }
}




