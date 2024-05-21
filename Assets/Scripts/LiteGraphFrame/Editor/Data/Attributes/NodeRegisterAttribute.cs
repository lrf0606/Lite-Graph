using System;

namespace LiteGraphFrame
{
    [AttributeUsage(AttributeTargets.Class)]
    class NodeRegisterAttribute : Attribute
    {
        public string[] Titles { get; private set; }

        public NodeRegisterAttribute(params string[] titles)
        {
            Titles = titles;
        }

        public string GetLastTitle()
        {
            if (Titles.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return Titles[Titles.Length - 1];
            }
        }
    }
}
