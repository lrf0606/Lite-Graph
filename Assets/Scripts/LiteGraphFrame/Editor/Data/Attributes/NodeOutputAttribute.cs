using System;

namespace LiteGraphFrame
{
    [AttributeUsage(AttributeTargets.Field)]
    class NodeOutputAttribute : Attribute
    {
        // ÊôÐÔÖÐÎÄÃèÊö
        private string m_FiledDescription;
        public string FiledDescription => m_FiledDescription;

        public NodeOutputAttribute(string filedDescription = "")
        {
            m_FiledDescription = filedDescription;
        }
    }
}


