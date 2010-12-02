using System;
using System.ComponentModel.Composition;

namespace Core.Abstractions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluginConfigurationAttribute : ExportAttribute
    {
        public override bool IsDefaultAttribute()
        {
            return true;
        }
    }
}