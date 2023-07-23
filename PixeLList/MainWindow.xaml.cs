using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PixeLList.Pages;
using PixeLList.ViewModels;
using PixeLList.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Uno.Extensions.Specialized;
using System.ComponentModel.Design;
using Windows.UI.StartScreen;
using System.Drawing;

namespace PixeLList
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private NotesViewModel _notesViewModel;
        private NavigationViewItem _selectedNavItem;
        private AllNotesList _allnotesList;
        public MainWindow()
        {
            this.InitializeComponent();
            Title = "PixeLList";
            _notesViewModel = new NotesViewModel();

            LoadNotes();
            
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
                    // Das ausgewählte Element ist bereits "Notizen"
                    if (navigateItem.SelectedItem is NavigationViewItem selectedItem && selectedItem.Tag.ToString() == "Notizen")
                        return;

                    _selectedNavItem = item;
                    contentFrame.Navigate(typeof(AllNotesList));
                }
                else
                    contentFrame.Navigate(typeof(AllNotesList));
            }
        }
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(AllNotesList))
                navigateItem.SelectedItem = _selectedNavItem;
        }
        private void newNotiz_Click(object sender, RoutedEventArgs e)
        {
            NotePage page = new NotePage();

            contentFrame.Content = page;

            LoadNotes();
        }
        private void allNotiz_Click(object sender, RoutedEventArgs e)
        {
            AllNotesList allnotes = new AllNotesList();

            contentFrame.Content = allnotes;
        }

        private void picTureMenuFlyout_Click(object sender, RoutedEventArgs e)
        {
            //FileOpenPicker();

            pictureDialog.IsEnabled = true;
            pictureDialog.ShowAsync();
            LoadNotes();

        }
        private async void LoadNotes()
        {
            try
            {
                List<Note> notes = await JsonHelper.LadeNotizenAusJSON();
                pictureBox.Items.Clear();
                foreach (var note in notes)
                {
                    pictureBox.Items.Add(note.Title);
                }
                //ContentDialogResult result = await pictureDialog.ShowAsync();
                //if (pictureBox.SelectedItem != null && result == ContentDialogResult.Primary) { }
                //FileOpenPicker();

            }
            catch (Exception ex)
            {
                meldungsBlock.Text = ex.Message;
            }
        }
        private async void FileOpenPicker()
        {
            try
            {
                FileOpenPicker folderPicker = new();
                folderPicker.ViewMode = PickerViewMode.Thumbnail;
                folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
                folderPicker.FileTypeFilter.Add(".jpg");
                folderPicker.FileTypeFilter.Add(".jpeg");
                folderPicker.FileTypeFilter.Add(".png");

                var hwd = WindowNative.GetWindowHandle(this);
                WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwd);
                var folder = await folderPicker.PickSingleFileAsync();
            }
            catch (Exception ex)
            {
                pictureDialog.Content = ex.Message;
            }
        }
    }
}

