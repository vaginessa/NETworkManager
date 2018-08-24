using System.Windows;
using System.Windows.Controls;

namespace NETworkManager
{
    public class ApplicationViewInfo
    {
        public ApplicationViewManager.Name Name { get; set; }
        public string TranslatedName { get; set; }
        public Canvas Icon { get; set; }

        public ApplicationViewInfo()
        {

        }

        public ApplicationViewInfo(ApplicationViewManager.Name name, UIElement packIconModern)
        {
            Name = name;
            TranslatedName = ApplicationViewManager.GetTranslatedNameByName(name);
            var canvas = new Canvas();
            canvas.Children.Add(packIconModern);
            Icon = canvas;
        }
    }
}