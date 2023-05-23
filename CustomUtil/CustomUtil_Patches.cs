using HarmonyLib;

namespace CustomUtil
{
    public class CustomUtil_Patches
    {
        [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.Awake))]
        public static class AssignLanterTypePatch
        {
            public static void Postfix(ItemDrop __instance)
            {
                if (__instance.gameObject.name == "Lantern")
                {
                    if(__instance.m_itemData.m_customData.ContainsKey("HipLantern"))return;
                    __instance.m_itemData.m_customData.Add("HipLantern", "1");
                }
            }
        }
    }
}