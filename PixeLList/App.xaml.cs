using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Octokit;
using PixeLList.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Management.Deployment;
using Windows.UI.Notifications;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PixeLList
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Microsoft.UI.Xaml.Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (m_window == null)
            {
                //m_window = new WelcomePage();
                //m_window.Activate();

                //await Task.Delay(5000);

                // Erstellen und Anzeigen des MainWindow
                var mainWindow = new MainWindow();
                mainWindow.Activate();

                // Überprüfen, ob ein Update verfügbar ist

                //bool updateAvailable = await IsUpdateAvailable();

                //if (updateAvailable)
                //{
                //    // Ein Update ist verfügbar, zeige die Update-Benachrichtigung an
                //    ShowUpdateNotification();

                //    await DownloadAndInstallUpdate();
                //}
            }
        }

        private Version GetCurrentAppVersion()
        {
            var version = typeof(App).Assembly.GetName().Version;
            return new Version(version.Major, version.Minor, version.Build);
        }

        private async Task<bool> IsUpdateAvailable()
        {
            var github = new GitHubClient(new ProductHeaderValue("PixeLList"));
            var release = await github.Repository.Release.GetAll("Steff093", "PixeLList");

            if (release.Count == 0)
                return false;

            Version latestVersion = new Version(release[0].TagName);
            Version currentVersion = GetCurrentAppVersion();

            return latestVersion > currentVersion;
        }

        private void ShowUpdateNotification()
        {
            var toastXmlString = $@"
        <toast>
            <visual>
                <binding template='ToastGeneric'>
                    <text>Eine neue Version ist verfügbar!</text>
                    <text>Klicken Sie hier, um das Update herunterzuladen und zu installieren.</text>
                </binding>
            </visual>
            <actions>
                <action content='Update' arguments='update' />
            </actions>
        </toast>";

            var toastXml = new Windows.Data.Xml.Dom.XmlDocument();
            toastXml.LoadXml(toastXmlString);

            var toast = new Windows.UI.Notifications.ToastNotification(toastXml);

            var toastNotifier = Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toast);
        }

        private async Task DownloadAndInstallUpdate()
        {
            var github = new GitHubClient(new ProductHeaderValue("PixeLList"));
            var releases = await github.Repository.Release.GetAll("Steff093", "PixeLList");

            var latestRelease = releases[0]; // Annahme: Die neueste Version ist das erste Release in der Liste

            var asset = latestRelease.Assets.FirstOrDefault(a => a.Name.EndsWith(".msix"));
            if (asset != null)
            {
                using (var client = new WebClient())
                {
                    var tempFilePath = System.IO.Path.GetTempFileName();

                    // Herunterladen des Release-Archivs (MSIX-Paket)
                    await client.DownloadFileTaskAsync(asset.BrowserDownloadUrl, tempFilePath);

                    // Installieren des heruntergeladenen Pakets
                    PackageManager packageManager = new PackageManager();
                    var result = await packageManager.UpdatePackageAsync(new Uri(tempFilePath), null, DeploymentOptions.ForceTargetApplicationShutdown);
                    if (result.IsRegistered)
                    {
                        // Das Update wurde erfolgreich installiert
                        // Führe entsprechende Aktionen aus, z.B. Neustart der Anwendung
                        RestartApp();
                    }
                    else
                    {
                        // Fehler beim Installieren des Updates
                        // Behandele den Fehler entsprechend
                    }
                }
            }
        }

        private void RestartApp()
        {
            // Schließen der aktuellen Anwendung
            Current.Exit();

            // Starten der Anwendung neu
            var launchString = $"app-uri:PixeLList"; // Ersetzen Sie "PixeLList" durch den Namen Ihrer Anwendung
            var options = new Windows.System.LauncherOptions();
            options.TargetApplicationPackageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName;
            Windows.System.Launcher.LaunchUriAsync(new Uri(launchString), options);
        }

        private Window m_window;


    }
}
