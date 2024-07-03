using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MLZ.Core.Models;

namespace MLZ.ViewModels
{
    public partial class FischViewModel : ObservableObject
    {
        private Fisch? _selectedFisch;

        public FischViewModel()
        {
            Fische = new ObservableCollection<Fisch>
            {
                new Fisch { Id = 1, Name = "Forelle", Lake = "Bodensee", Method = "Angeln", Date = DateTime.Now },
                new Fisch { Id = 2, Name = "Hecht", Lake = "Ammersee", Method = "Netz", Date = DateTime.Now },
                new Fisch { Id = 3, Name = "Karpfen", Lake = "Chiemsee", Method = "Angeln", Date = DateTime.Now }
            };
            AddFischCommand = new AsyncRelayCommand(AddFischAsync);
            EditFischCommand = new AsyncRelayCommand(EditFischAsync);
            DeleteFischCommand = new AsyncRelayCommand(DeleteFischAsync);
            SelectedFisch = new Fisch { Date = DateTime.Now };
        }

        public ObservableCollection<Fisch> Fische { get; private set; }

        public Fisch? SelectedFisch
        {
            get => _selectedFisch;
            set
            {
                if (SetProperty(ref _selectedFisch, value))
                {
                    OnPropertyChanged(nameof(CanAddOrEditFisch));
                }
            }
        }

        public bool CanAddOrEditFisch => !string.IsNullOrWhiteSpace(SelectedFisch?.Name) &&
                                         !string.IsNullOrWhiteSpace(SelectedFisch?.Lake) &&
                                         !string.IsNullOrWhiteSpace(SelectedFisch?.Method);

        public IAsyncRelayCommand AddFischCommand { get; }
        public IAsyncRelayCommand EditFischCommand { get; }
        public IAsyncRelayCommand DeleteFischCommand { get; }

        private async Task AddFischAsync()
        {
            if (SelectedFisch != null && CanAddOrEditFisch)
            {
                Debug.WriteLine($"Adding Fisch: {SelectedFisch.Name}");
                Fische.Add(new Fisch
                {
                    Id = Fische.Count + 1,
                    Name = SelectedFisch.Name,
                    Lake = SelectedFisch.Lake,
                    Method = SelectedFisch.Method,
                    Date = SelectedFisch.Date
                });
                SelectedFisch = new Fisch { Date = DateTime.Now }; // Reset the selected fish for new entry
                await Task.CompletedTask;
            }
        }

        private async Task EditFischAsync()
        {
            if (SelectedFisch != null && CanAddOrEditFisch)
            {
                Debug.WriteLine($"Editing Fisch: {SelectedFisch.Name}");
                // Hier würden Sie normalerweise die Logik zum Bearbeiten des Fisches hinzufügen.
                var fischToEdit = Fische.FirstOrDefault(f => f.Id == SelectedFisch.Id);
                if (fischToEdit != null)
                {
                    fischToEdit.Name = SelectedFisch.Name;
                    fischToEdit.Lake = SelectedFisch.Lake;
                    fischToEdit.Method = SelectedFisch.Method;
                    fischToEdit.Date = SelectedFisch.Date;
                }
                await Task.CompletedTask;
            }
        }

        private async Task DeleteFischAsync()
        {
            if (SelectedFisch != null && Fische.Contains(SelectedFisch))
            {
                Debug.WriteLine($"Deleting Fisch: {SelectedFisch.Name}");
                Fische.Remove(SelectedFisch);
                SelectedFisch = new Fisch { Date = DateTime.Now }; // Reset after deletion
                await Task.CompletedTask;
            }
        }
    }
}
