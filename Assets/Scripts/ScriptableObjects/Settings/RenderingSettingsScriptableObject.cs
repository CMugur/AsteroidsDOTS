using UnityEngine;
using DOTS_Exercise.Data.Settings.Attributes;
using DOTS_Exercise.Data.Settings.Interfaces;

namespace DOTS_Exercise.Data.Settings
{
    [Settings(Interface = typeof(IRequestRenderingSettings), SetSettingsMethodName = "SetRenderingSettings"), 
        CreateAssetMenu(fileName = "Rendering Settings Scriptable Object", menuName = "Scriptable Objects/Settings/Rendering Settings")]
    public class RenderingSettingsScriptableObject : SettingsScriptableObject
    {
        public Mesh UnitMesh;
        public Material UnitMaterial;
    }
}