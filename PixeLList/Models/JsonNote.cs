using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace PixeLList.Models
{
    public class JsonNote
    {

        /*
    Zuerst wird die Methode LadeNotizenAusJSON aufgerufen, um die Liste der vorhandenen Notizen aus der JSON-Datei zu laden. 
    Die await-Anweisung stellt sicher, dass der Ladevorgang abgeschlossen ist, bevor der Code fortgesetzt wird.

    Anschließend wird in der geladenen Liste nach der zu aktualisierenden Notiz gesucht.
    Dazu wird in diesem Beispiel angenommen, dass jede Notiz eine eindeutige Kennung (Id) hat. 
    Die FirstOrDefault-Methode wird verwendet, um die erste Notiz in der Liste zu finden, die die angegebene Bedingung erfüllt. 
    In diesem Fall wird die Bedingung verwendet, um die Notiz mit der passenden Id zu finden.

    Wenn eine passende Notiz gefunden wurde (vorhandeneNotiz != null), werden die Eigenschaften der vorhandenen Notiz mit den Werten der übergebenen Notiz aktualisiert.
    In diesem Fall wird der Title und der Text der vorhandenen Notiz mit den Werten der übergebenen Notiz aktualisiert.

    Nachdem die Notiz aktualisiert wurde, wird die Liste der Notizen zurück in das JSON-Format konvertiert, indem die JsonConvert.SerializeObject-Methode verwendet wird. 
    Diese Methode nimmt ein Objekt entgegen und gibt eine JSON-Zeichenfolge zurück, die das Objekt repräsentiert.

    Schließlich wird das aktualisierte JSON in der JSON-Datei gespeichert. 
    Zuerst wird die Datei mit der GetFileAsync-Methode aus dem lokalen Speicher abgerufen. 
    Anschließend wird die FileIO.WriteTextAsync-Methode verwendet, um den aktualisierten JSON-Text in die Datei zu schreiben.

    Durch diesen Ablauf werden die Änderungen an einer Notiz in der Liste vorgenommen und das aktualisierte JSON in der Datei gespeichert.
*/

        private static readonly string logFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "_log.txt");

        public static async Task<List<NoteModel>> LadeNotizenAusJSON()
        {
            try
            {
                StorageFile datei = await ApplicationData.Current.LocalFolder.GetFileAsync("notizen.json");

                string json = await FileIO.ReadTextAsync(datei);

                List<NoteModel> notizen = JsonConvert.DeserializeObject<List<NoteModel>>(json);

                await SchreibeLog("Notizen aus JSON geladen."); // Lognachricht hinzufügen

                return notizen;
            }
            catch (FileNotFoundException)
            {
                await CreateBackupSystem();

                await SchreibeLog("Notizen.json nicht gefunden."); // Lognachricht hinzufügen

                return new List<NoteModel>();
            }
        }

        public static async Task SpeichereNotizen(List<NoteModel> notizen)
        {
            string json = JsonConvert.SerializeObject(notizen);

            StorageFile datei = await ApplicationData.Current.LocalFolder.CreateFileAsync("notizen.json", CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(datei, json);

            Debug.WriteLine("Notizen in JSON gespeichert."); // Lognachricht hinzufügen
        }

        public static async Task SchreibeLog(string logDatei)
        {
            using (var stream = new StreamWriter(logFilePath, true))
            {
                await stream.WriteLineAsync(logDatei);
            }
        }

        public static async Task CreateBackupSystem()
        {
            try
            {
                StorageFile originialFile = await ApplicationData.Current.LocalFolder.GetFileAsync("notizen.json");

                StorageFolder backupFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Backups", CreationCollisionOption.OpenIfExists);

                string timeStamp = DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss");
                string backupfile = $"notizen_backup_{timeStamp}.json";


                StorageFile backupFile = await originialFile.CopyAsync(backupFolder, backupfile, NameCollisionOption.GenerateUniqueName);
            }
            catch (Exception ex)
            {
                await SchreibeLog(ex.Message);
            }
        }
        public static async Task UpdateNotiz(NoteModel notiz)
        {
            List<NoteModel> notizen = await LadeNotizenAusJSON();

            NoteModel vorhandeneNotizen = notizen.FirstOrDefault(n => n.Id == notiz.Id);

            if (vorhandeneNotizen != null)
            {
                // Aktualisiere die Eigenschaften der vorhandenen Notiz mit den Werten der übergebenen Notiz
                vorhandeneNotizen.Title = notiz.Title;
                vorhandeneNotizen.Text = notiz.Text;

                string json = JsonConvert.SerializeObject(notizen);

                StorageFile datei = await ApplicationData.Current.LocalFolder.GetFileAsync("notizen.json");
                await FileIO.WriteTextAsync(datei, json);
            }
        }
    }
}
