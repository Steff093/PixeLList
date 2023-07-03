using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PixeLList.Models;
using PixeLList.Pages;
using PixeLList.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PixeLList
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private NotesViewModel _notesViewModel;
        private NavigationViewItem _selectedNavItem;
        public MainWindow()
        {
            this.InitializeComponent();
            Title = "PixeLList";
            _notesViewModel = new NotesViewModel();
            contentFrame.DataContext = _notesViewModel;
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

        }

        private void navigateItem_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                contentFrame.CacheSize = 0; // Deaktiviert die Navigation-History
                contentFrame.Navigate(typeof(SettingsPage));
                _selectedNavItem = null; // Zurücksetzen des ausgewählten NavigationViewItems
            }
            else
            {
                NavigationViewItem item = args.InvokedItem as NavigationViewItem;
                if (item != null && item.Tag.ToString() == "Notizen")
                {
                    contentFrame.Navigated += ContentFrame_Navigated;
                    if (navigateItem.SelectedItem is NavigationViewItem selectedItem && selectedItem.Tag.ToString() == "Notizen")
                    {
                        // Das ausgewählte Element ist bereits "Notizen"
                        return;
                    }
                    _selectedNavItem = item;
                    contentFrame.Navigate(typeof(AllNotesList));
                }
                else
                {
                    contentFrame.Navigate(typeof(AllNotesList));
                }
            }
        }
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(AllNotesList) && navigateItem.SelectedItem != _selectedNavItem)
            {
                navigateItem.SelectedItem = _selectedNavItem;
            }
        }
        private void newNotiz_Click(object sender, RoutedEventArgs e)
        {
            NotePage page = new NotePage();

            contentFrame.Content = page;
        }
        private void allNotiz_Click(object sender, RoutedEventArgs e)
        {
            AllNotesList allnotes = new AllNotesList();

            contentFrame.Content = allnotes;
        }
    }
}
