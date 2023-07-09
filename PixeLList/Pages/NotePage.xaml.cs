using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using PixeLList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using WinRT.Interop;
using System.Threading.Tasks;
using PixeLList.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

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

            ViewModel = Ioc.Default.GetService<MainWindowViewModel?>();
        }

        public MainWindowViewModel? ViewModel { get; set; }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            List<Note> notes = await JsonHelper.LadeNotizenAusJSON();

            string titel = titleNotizTextBox.Text;
            string inhalt = notizTextbox.Text;
            string datum = dateTime.Text;
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
            //Test für mein Github

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

        private async void imageButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogResult result = await imageSettingsDialog.ShowAsync();
            if (result is ContentDialogResult.Primary)
            {
                await OpenFolderAsync();
            }
        }


        // Hier stimmt noch etwas nicht, er startet nicht das OpenFileDiaglog ( in dem Fall FilePicker)
        private async Task<StorageFolder?> OpenFolderAsync()
        {
            FolderPicker folgerPicker = new();
            folgerPicker.FileTypeFilter.Add("*");
            var hwd = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(folgerPicker, hwd);
            return await folgerPicker.PickSingleFolderAsync();
        }

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
