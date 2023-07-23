using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using PixeLList.Models;
using PixeLList.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PixeLList.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllNotesList : Page
    {
        public ObservableCollection<Note> Notes { get; set; }
        public NotesViewModel ViewModel;

        public AllNotesList()
        {
            this.InitializeComponent();
            this.ViewModel = new NotesViewModel();
            this.DataContext = this.ViewModel;

            contentFrame = new Frame();

            if (ViewModel.Notes.Count == 0 && noNotesMessage == null)
                noNotesMessage.Visibility = Visibility.Visible;
            else if (noNotesMessage != null)
                noNotesMessage.Visibility = Visibility.Collapsed;

            LoadNotesAsync();

            JsonHelper.CreateBackupSystem();
        }
        private async void LoadNotesAsync()
        {
            //Hier auf Fehler überprüfen !

            // Laden der Notizen aus der JSON-Datei
            List<Note> notes = await JsonHelper.LadeNotizenAusJSON();

            // Aktualisieren der Notizen in der bestehenden ObservableCollection
            ViewModel.Notes.Clear();
            foreach (var note in notes)
            {
                if (note.Id == 0)
                {
                    int maxID = ViewModel.Notes.Any() ? ViewModel.Notes.Max(note => note.Id) : 0; note.Id = maxID + 1;
                }
                ViewModel.Notes.Add(note);
            }
        }
        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            await JsonHelper.SpeichereNotizen(ViewModel?.Notes.ToList());
            editPopup.IsOpen = false; // Schließe das Popup-Fenster
            ViewModel.OnPropertyChanged(nameof(ViewModel.SelectedNote));
            LoadNotesAsync();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            editPopup.IsOpen = false; // Schließe das Popup-Fenster
        }

        private async void deleteFlyout_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            if (menuItem != null)
            {
                var note = menuItem.DataContext as Note;
                if (note != null)
                {
                    ViewModel.Notes.Remove(note);
                    // Speichern der Notizen in der JSON-Datei nach dem Löschen
                    await JsonHelper.SpeichereNotizen(ViewModel.Notes.ToList());
                }
            }
        }

        private void bearbeitenFlyout_Click(object sender, RoutedEventArgs e)
        {
            editPopup.IsOpen = true;
            var menuItem = sender as MenuFlyoutItem;
            if (menuItem != null)
            {
                var note = menuItem.DataContext as Note;
                if (note != null)
                {
                    ViewModel.SelectedNote = note;
                    editPopup.IsOpen = true;
                }
            }
        }

        private void stackPanel_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            Button moreButton = stackPanel.FindName("moreButton") as Button;

            if (moreButton != null)
            {
                int selectIndex = NoteListView.SelectedIndex;
                if (selectIndex == ViewModel.SelectedNoteIndex)
                {
                    moreButton.Visibility = Visibility.Collapsed;
                    ViewModel.SelectedNoteIndex = -1;  // Zurücksetzen des Index auf -1
                }
                else
                {
                    moreButton.Visibility = Visibility.Visible;
                    ViewModel.SelectedNoteIndex = selectIndex;
                }
            }
        }
    }
}

