using System;

namespace LiteGraphFrame
{
    [AttributeUsage(AttributeTargets.Class)]
    class NodeRegisterAttribute : Attribute
    {
        private string m_Directory;
        private string m_Title;
        public string Directory => m_Directory;
        public string Title => m_Title;

        public NodeRegisterAttribute(string directory, string title)
        {
            m_Directory = directory;
            m_Title = title;
        }
    }
}
