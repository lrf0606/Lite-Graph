using System;

namespace LiteGraphFrame
{
    [AttributeUsage(AttributeTargets.Class)]
    class NodeRegisterAttribute : Attribute
    {
        public string Directory;
        public string Title;

        public NodeRegisterAttribute(string directory, string title)
        {
            Directory = directory;
            Title = title;
        }
    }
}
