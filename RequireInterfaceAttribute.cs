using UnityEngine;

namespace Bipolar
{
    public class RequireInterfaceAttribute : PropertyAttribute
    {
        public System.Type RequiredType { get; private set; }

        public RequireInterfaceAttribute(System.Type type)
        {
            RequiredType = type;
        }
    }

    public class AddComponentButtonAttribute : PropertyAttribute
    {
        public float Width { get; private set; } = 60;
        public System.Type OverrideType { get; private set; }

        public AddComponentButtonAttribute(float overrideWidth, System.Type overrideType = null) : this(overrideType)
        {
            Width = overrideWidth;
        }

        public AddComponentButtonAttribute(System.Type overrideType = null)
        {
            OverrideType = overrideType;
        }
    }

    public class NewAssetButtonAttribute : PropertyAttribute
    {
        public string AssetPath { get; private set; } = "";
        public bool WithCloneButton { get; private set; } = false;

        public NewAssetButtonAttribute(string assetPath = "", bool withCloneButton = false) : this(withCloneButton) 
        {
            AssetPath = assetPath;
        }

        public NewAssetButtonAttribute(bool withCloneButton)
        {
            WithCloneButton = withCloneButton;
        }
    }
}
