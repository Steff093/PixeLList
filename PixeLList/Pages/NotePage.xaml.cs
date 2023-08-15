using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;
using PixeLList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private NoteModel currentNote;
        public NotePage()
        {
            this.InitializeComponent();
        }
        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            List<NoteModel> notes = await JsonNote.LadeNotizenAusJSON();

            string titel = titleNotizTextBox.Text;
            string inhalt = notizTextbox.Text;
            string bitmap = currentNote.BitImagePath;

            if (string.IsNullOrEmpty(titel) || string.IsNullOrEmpty(inhalt))
            {
                ShowInfoBar("Fehler", InfoBarSeverity.Error, "Bitte Text und Titel einfügen!"); return;
            }

            if (titel.Length < 5)
            {
                ShowInfoBar("Warnung", InfoBarSeverity.Warning, "DIe Notiz wurde nicht gespeichert, " +
                    "Achten Sie darauf, das Ihr Titel mehr als 5 Zeichen enthält"); return;
            }

            if (inhalt.Length < 5)
            {
                ShowInfoBar("Warnung", InfoBarSeverity.Warning, "Die Notiz wurde nicht gespeichert," +
                    "Achten Sie bitte darauf, das IHr Text mehr als 5 Zeichen enthält"); return;
            }

            int maxId = notes.Any() ? notes.Max(note => note.Id) : 0;
            int neueId = maxId + 1;

            NoteModel neueNotiz = new NoteModel
            {
                Id = neueId,
                Title = titel,
                Text = inhalt,
                Erstellungsdatum = DateTime.Now,
                BitImagePath = bitmap
            };

            notes.Add(neueNotiz);
            string json = JsonConvert.SerializeObject(notes);
            StorageFile datei = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                "notizen.json", options: CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(datei, json);

            if (!string.IsNullOrEmpty(titel) && !string.IsNullOrEmpty(inhalt))
            {
                CleanInput();
            }
        }

        private void imageButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPickerAsync();
        }

        private async void FileOpenPickerAsync()
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
            if (currentNote == null)
                currentNote = new NoteModel();
        }

        private void ShowInfoBar(string title, InfoBarSeverity severity, string message)
        {
            successBar.IsOpen = true;
            successBar.Title = title;
            successBar.Severity = severity;
            successBar.Message = message;
        }

        private void CleanInput()
        {
            titleNotizTextBox.Text = string.Empty;
            notizTextbox.Text = string.Empty;
            imageControl.Source = null;
        }
    }
}
