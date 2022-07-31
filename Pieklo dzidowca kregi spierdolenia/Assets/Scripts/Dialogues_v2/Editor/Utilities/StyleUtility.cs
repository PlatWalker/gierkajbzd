using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.dialogues.editor
{
    public static class StyleUtility
    {
        public static void AddStyleSheets(this VisualElement element, params string[] styleSheetNames)
        {
            foreach (var styleSheetName in styleSheetNames)
            {
                element.styleSheets.Add(Resources.Load<StyleSheet>($"StyleSheets/{styleSheetName}"));
            }
        }

        public static void AddClasses(this VisualElement element, params string[] classNames)
        {
            foreach (var className in classNames)
            {
                element.AddToClassList(className);
            }
        }
    }
}
