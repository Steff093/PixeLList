using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using PixeLList.Models;
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
        public event EventHandler<NoteSavedEventArgs> NoteSaved;

        public NotePage()
        {
            this.InitializeComponent();

        }
        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            List<Note> notes = await JsonHelper.LadeNotizenAusJSON();

            string titel = titleNotizTextBox.Text;
            string inhalt = notizTextbox.Text;
            //int id = notes.Max(note => note.Id);
            //ShowNotification(titel, inhalt);

            if (string.IsNullOrEmpty(titel) || string.IsNullOrEmpty(inhalt))
            {
                successBar.IsOpen = true;
                successBar.Title = "Fehler!";
                successBar.Severity = InfoBarSeverity.Error;
                successBar.Message = "Bitte Text oder Titel einfügen!"; 
                return;
            }

            int maxId = notes.Any() ? notes.Max(note => note.Id) : 0;
            int neueId = maxId + 1;

            Note neueNotiz = new Note { Id = neueId, Title = titel, Text = inhalt, Erstellungsdatum = DateTime.Now};
            notes.Add(neueNotiz);

            string json = JsonConvert.SerializeObject(notes);
            StorageFile datei = await ApplicationData.Current.LocalFolder.CreateFileAsync("notizen.json", options: CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(datei, json);

            if (!string.IsNullOrEmpty(titel) && !string.IsNullOrEmpty(inhalt))
            {
                successBar.IsOpen = true;
                titleNotizTextBox.Text = string.Empty;
                notizTextbox.Text = string.Empty;
            }
        }

        //private async void imageButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        FileOpenPicker folderPicker = new();
        //        folderPicker.ViewMode = PickerViewMode.Thumbnail;
        //        folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        //        folderPicker.FileTypeFilter.Add(".jpg");
        //        folderPicker.FileTypeFilter.Add(".jpeg");
        //        folderPicker.FileTypeFilter.Add(".png");

        //        var hwd = WindowNative.GetWindowHandle(this);
        //        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwd);
        //        var folder = await folderPicker.PickSingleFileAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Test Irgendetwas
        //    }
        //}
        //public void ShowNotification(string title, string message)
        //{
        //    var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);

        //    var textNodes = toastXml.GetElementsByTagName("text");
        //    textNodes[0].AppendChild(toastXml.CreateTextNode(title));
        //    textNodes[1].AppendChild(toastXml.CreateTextNode(message));

        //    var notifier = ToastNotificationManager.CreateToastNotifier();
        //    var notification = new ToastNotification(toastXml);
        //    notifier.Show(notification);
        //}
    }
}
