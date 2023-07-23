using Microsoft.UI.Xaml.Controls;
using PixeLList.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PixeLList.ViewModels
{
    public class NotesViewModel : INotifyPropertyChanged
    {
        private Note _selectedNote;

        public event PropertyChangedEventHandler PropertyChanged;

        private ComboBox _combobox;

        public ComboBox ComboBox
        {
            get { return _combobox; } set { _combobox = value; _combobox.Items.Clear(); }
        }

        private int selectedNoteIndex;
        public int SelectedNoteIndex
        {
            get { return selectedNoteIndex; } set { SetProperty(ref selectedNoteIndex, value); }
        }
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Note SelectedNote
        {
            get { return _selectedNote; }
            set { _selectedNote = value; OnPropertyChanged(nameof(SelectedNote)); }

        }
        public ObservableCollection<Note> Notes { get; set; }
        public NotesViewModel()
        {
            Notes = new ObservableCollection<Note>();
        }

        public void aktualisierteNotizen(List<Note> notes)
        {
            var newNotes = new List<Note>(notes);
            Notes.Clear();
            foreach (Note note in newNotes)
            {
                Notes.Add(note);
            }
        }
    }
}
