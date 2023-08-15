// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.Labs.WinUI;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Drawing;
using System.Linq;

namespace PixeLList.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {   
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void klickbaresSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void switchToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender is not ToggleSwitch toggleSwitch)
                return;

            foreach (var item in this.multiExpander.Items.OfType<SettingsCard>())
                item.IsEnabled = toggleSwitch.IsOn;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            if(radioButton.IsChecked == true)
            {
                string selectedOption = radioButton.Content.ToString(); 
                switch (selectedOption)
                {
                    case "Acryl":
                        App.Window.SystemBackdrop = new DesktopAcrylicBackdrop(); break;

                    case "MicaBase":
                        App.Window.SystemBackdrop = new MicaBackdrop { Kind = MicaKind.Base }; break;

                    case "MicaBaseAlt":
                        App.Window.SystemBackdrop = new MicaBackdrop { Kind = MicaKind.BaseAlt }; break;
                }
            }
        }
    }
}
