using UnityEngine;

namespace TheWarOfAddicts.Utility
{
    public static class ExtensionUtility
    {
        public static float WidthDistance(this Transform trans, Transform target)
        {
            return Mathf.Abs(target.position.x - trans.position.x);
        }
    }
}