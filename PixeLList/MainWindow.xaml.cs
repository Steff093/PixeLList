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
using System.ComponentModel.Design;
using Windows.UI.StartScreen;
using System.Drawing;
using Windows.Storage;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Media.Devices;
using System.Collections.ObjectModel;
using Windows.UI;
using Microsoft.UI;


namespace PixeLList
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private NotesViewModel _notesViewModel;
        private NavigationViewItem _selectedNavItem;
        private ObservableCollection<NoteModel> allNotes;
        public MainWindow()
        {
            this.InitializeComponent();
            Title = "PixeLList";
            _notesViewModel = new NotesViewModel();
            allNotes = new ObservableCollection<NoteModel>();

            //ExtendsContentIntoTitleBar = true;
            //SetTitleBar(AppTitleBar);

        }

        private void navigateItem_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            NavigationViewItem selectedItem = navigateItem.SelectedItem as NavigationViewItem;

            if (args.IsSettingsInvoked)
            {
                contentFrame.CacheSize = 0; // Deaktiviert die Navigation-History
                contentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                var selected = selectedItem.Tag.ToString();

                switch (selected)
                {
                    case "Notizen":
                        contentFrame.Navigate(typeof(AllNotesList));
                        break;
                    case "Ordner":
                        contentFrame.Navigate(typeof(FolderNotePage));
                        break;
                }
            }
        }
        private void newNotiz_Click(object sender, RoutedEventArgs e)
        {
            NotePage page = new NotePage();

            contentFrame.Content = page;

            //LoadNotes();
        }
        //private async void picTureMenuFlyout_Click(object sender, RoutedEventArgs e)
        //{
        //    pictureDialog.IsEnabled = true;
        //    //LoadNotes();
        //    await pictureDialog.ShowAsync();

        //}
        //private async void LoadNotes()
        //{
        //    try
        //    {
        //        List<Note> notes = await JsonHelper.LadeNotizenAusJSON();
        //        pictureBox.Items.Clear();
        //        foreach (var note in notes)
        //        {
        //            pictureBox.Items.Add(note.Title);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        meldungsBlock.Text = "Fehler" + ex.Message;
        //    }
        //}

        //private void pictureDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        //{
        //    if (pictureBox.SelectedItem != null)
        //    {
        //        //FileOpenPicker();
        //        args.Cancel = true;
        //    }
        //}
    }
}

