using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.IconPacks;

namespace NETworkManager

{
    public class SettingsViewInfo
    {
        public SettingsViewManager.Name Name { get; set; }
        public string TranslatedName { get; set; }
        public Canvas Icon { get; set; }
        public SettingsViewManager.Group Group { get; set; }
        public string TranslatedGroup { get; set; }

        public SettingsViewInfo()
        {
        }

        public SettingsViewInfo(SettingsViewManager.Name name, UIElement packIconModern, SettingsViewManager.Group group)
        {
            Name = name;
            TranslatedName = SettingsViewManager.TranslateName(name, group);
            var canvas = new Canvas();
            canvas.Children.Add(packIconModern);
            Icon = canvas;
            Group = group;
            TranslatedGroup = SettingsViewManager.TranslateGroup(group);
        }
    }
}