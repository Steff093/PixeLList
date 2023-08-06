using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using PixeLList.Models;
using PixeLList.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace PixeLList.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotePage : Page
    {
        private Note currentNote;
        public NotePage()
        {
            this.InitializeComponent();
        }
        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            List<Note> notes = await JsonHelper.LadeNotizenAusJSON();

            string titel = titleNotizTextBox.Text;
            string inhalt = notizTextbox.Text;
            string bitmap = currentNote.BitImagePath;

            if (string.IsNullOrEmpty(titel) || string.IsNullOrEmpty(inhalt))
            {
                successBar.IsOpen = true;
                successBar.Title = "Fehler!";
                successBar.Severity = InfoBarSeverity.Error;
                successBar.Message = "Bitte Text und Titel einfügen!";
                return;
            }

            if (titel.Length < 5)
            {
                successBar.IsOpen = true;
                successBar.Title = "Warnung";
                successBar.Severity = InfoBarSeverity.Warning;
                successBar.Message = "DIe Notiz wurde nicht gespeichert, " +
                    "Achten Sie darauf, das Ihr Titel mehr als 5 Zeichen enthält";
                return;
            }

            int maxId = notes.Any() ? notes.Max(note => note.Id) : 0;
            int neueId = maxId + 1;

            Note neueNotiz = new Note { Id = neueId, Title = titel, Text = inhalt, Erstellungsdatum = DateTime.Now, BitImagePath = bitmap};
            notes.Add(neueNotiz);

            string json = JsonConvert.SerializeObject(notes);
            StorageFile datei = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                "notizen.json", options: CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(datei, json);

            if (!string.IsNullOrEmpty(titel) && !string.IsNullOrEmpty(inhalt))
            {
                successBar.IsOpen = true;
                titleNotizTextBox.Text = string.Empty;
                notizTextbox.Text = string.Empty;
            }
        }

        private void imageButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker();
        }

        private async void FileOpenPicker()
        {
            try
            {
                FileOpenPicker folderPicker = new()
                {
                    ViewMode = PickerViewMode.Thumbnail,
                    FileTypeFilter = { ".jpg", ".jpeg", ".png", ".gif" },
                };

                nint windowHandle = WindowNative.GetWindowHandle(App.Window);
                WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, windowHandle);
                var file = await folderPicker.PickSingleFileAsync();

                // Noch in Arbeit !
                // Funktion soll ein Bild, welches man auwählt, einer Notiz hinzufügen können
                if (file != null)
                {
                    using (var stream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        var bitmapImage = new BitmapImage();
                        await bitmapImage.SetSourceAsync(stream);

                        currentNote.BitImagePath = file.Path;

                        if (imageControl != null)
                        {
                            imageControl.Source = bitmapImage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                notizTextbox.Text = ex.Message;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(currentNote == null)
            {
                currentNote = new Note();
            }
        }
    }
}
