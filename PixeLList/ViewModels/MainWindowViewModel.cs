using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Storage;

namespace PixeLList.ViewModels
{
    [ObservableObject]
    public partial class MainWindowViewModel
    {
        [ObservableProperty]
        private StorageFolder? _folder;

        public MainWindowViewModel()
        {
        }
    }
}
