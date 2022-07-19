using System;

namespace DOTS_Exercise.Data.Settings.Attributes
{
    public class SettingsAttribute : Attribute
    {
        public Type Interface;
        public string SetSettingsMethodName;
    }
}