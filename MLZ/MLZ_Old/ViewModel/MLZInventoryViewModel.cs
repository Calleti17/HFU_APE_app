using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using MLZ.Models;
using MLZ.Commands;
using Microsoft.Maui.Controls;

namespace MLZ.ViewModel
{
    public class MLZInventoryViewModel : INotifyPropertyChanged
    {
        #region Fields

        private Model selectedApp = new Model();
        private decimal totalSum;
        private bool isEditing;
        private bool isAdding;
        private Model originalApp;

        #endregion

        #region Properties

        public ObservableCollection<Model> Apps { get; set; } = new ObservableCollection<Model>
        {
            new Model { Id = 1, Fisch = "Hecht", See = "ZHSEE", Anzahl = 25.0m, Description = "Dropshot" },
            new Model { Id = 2, Fisch = "Zander", See = "GreifenSee", Anzahl = 10.0m, Description = "Texas-Rig" },
            new Model { Id = 3, Fisch = "Egli", See = "BodenSee", Anzahl = 500.0m, Description = "Fliegenfischen" },
            new Model { Id = 4, Fisch = "Rotauge", See = "ThunerSee", Anzahl = 5.0m, Description = "StippAngeln" },
            new Model { Id = 5, Fisch = "Karpfen", See = "Tiefenbrunnen", Anzahl = 10.0m, Description = "GrundAngeln" }
        };

        public Model SelectedApp
        {
            get { return selectedApp; }
            set
            {
                selectedApp = value;
                OnPropertyChanged(nameof(SelectedApp));
            }
        }

        public decimal TotalSum
        {
            get { return totalSum; }
            set
            {
                totalSum = value;
                OnPropertyChanged(nameof(TotalSum));
            }
        }

        public bool IsEditing
        {
            get { return isEditing; }
            set
            {
                isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
                OnPropertyChanged(nameof(CanAdd));
                OnPropertyChanged(nameof(CanEditOrDelete));
                OnPropertyChanged(nameof(IsFormEnabled));
                OnPropertyChanged(nameof(IsConfirmEnabled));
                RaiseCanExecuteChanged();
            }
        }

        public bool IsAdding
        {
            get { return isAdding; }
            set
            {
                isAdding = value;
                OnPropertyChanged(nameof(IsAdding));
                OnPropertyChanged(nameof(CanAdd));
                OnPropertyChanged(nameof(CanEditOrDelete));
                OnPropertyChanged(nameof(IsFormEnabled));
                OnPropertyChanged(nameof(IsConfirmEnabled));
                RaiseCanExecuteChanged();
            }
        }

        public bool CanAdd => !IsEditing && !IsAdding;

        public bool CanEditOrDelete => !IsAdding && !IsEditing;

        public bool IsFormEnabled => IsAdding || IsEditing;

        public bool IsConfirmEnabled => IsAdding || IsEditing;

        public ICommand AddAppCommand { get; private set; }
        public ICommand EditAppCommand { get; private set; }
        public ICommand DeleteAppCommand { get; private set; }
        public ICommand ConfirmCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand CalculateTotalSumCommand { get; private set; }

        #endregion

        #region Constructor

        public MLZInventoryViewModel()
        {
            AddAppCommand = new RelayCommand(StartAddApp, () => CanAdd);
            EditAppCommand = new RelayCommand(StartEditApp, () => CanEditOrDelete);
            DeleteAppCommand = new RelayCommand(DeleteAppExecute, () => CanEditOrDelete);
            ConfirmCommand = new RelayCommand(ConfirmExecute, () => IsAdding || IsEditing);
            CancelCommand = new RelayCommand(CancelExecute, () => IsAdding || IsEditing);
            CalculateTotalSumCommand = new RelayCommand(CalculateTotalSumExecute);

            CalculateTotalSumExecute(); // Initial calculation
        }

        #endregion

        #region Command Methods

        private void StartAddApp()
        {
            IsAdding = true;
            ClearSelectedApp();
        }

        private void StartEditApp()
        {
            if (SelectedApp != null)
            {
                originalApp = new Model
                {
                    Id = SelectedApp.Id,
                    Fisch = SelectedApp.Fisch,
                    See = SelectedApp.See,
                    Anzahl = SelectedApp.Anzahl,
                    Description = SelectedApp.Description
                };
                IsEditing = true;
            }
        }

        private void ConfirmExecute()
        {
            if (IsAdding)
            {
                if (SelectedApp == null || !IsValid(SelectedApp))
                {
                    Application.Current.MainPage.DisplayAlert("Fehler", "Bitte füllen Sie alle Felder korrekt aus.", "OK");
                    return;
                }

                Model newApp = new Model
                {
                    Id = Apps.Count > 0 ? Apps.Max(a => a.Id) + 1 : 1,
                    Fisch = SelectedApp.Fisch,
                    See = SelectedApp.See,
                    Anzahl = SelectedApp.Anzahl,
                    Description = SelectedApp.Description
                };

                Apps.Add(newApp);
                CalculateTotalSumExecute();
                ClearSelectedApp();
                IsAdding = false;
            }
            else if (IsEditing)
            {
                var app = Apps.FirstOrDefault(a => a.Id == SelectedApp.Id);
                if (app != null)
                {
                    app.Fisch = SelectedApp.Fisch;
                    app.See = SelectedApp.See;
                    app.Anzahl = SelectedApp.Anzahl;
                    app.Description = SelectedApp.Description;

                    OnPropertyChanged(nameof(Apps));
                    CalculateTotalSumExecute();
                }
                IsEditing = false;
            }
        }

        private void CancelExecute()
        {
            if (IsEditing && originalApp != null)
            {
                SelectedApp.Fisch = originalApp.Fisch;
                SelectedApp.See = originalApp.See;
                SelectedApp.Anzahl = originalApp.Anzahl;
                SelectedApp.Description = originalApp.Description;
                IsEditing = false;
            }

            if (IsAdding)
            {
                IsAdding = false;
                ClearSelectedApp();
            }
        }

        private void DeleteAppExecute()
        {
            if (SelectedApp != null)
            {
                Apps.Remove(SelectedApp);
                SelectedApp = null;
                CalculateTotalSumExecute();
            }
        }

        private void CalculateTotalSumExecute()
        {
            TotalSum = Apps.Sum(app => app.Anzahl);
        }

        #endregion

        #region Helper Methods

        private bool IsValid(Model app)
        {
            if (string.IsNullOrWhiteSpace(app.Fisch) || app.Fisch.Length > 20)
                return false;
            if (string.IsNullOrWhiteSpace(app.See) || app.See.Length > 20)
                return false;
            if (app.Anzahl <= 0)
                return false;
            if (string.IsNullOrWhiteSpace(app.Description) || app.Description.Length > 20)
                return false;

            return true;
        }

        private void ClearSelectedApp()
        {
            SelectedApp = new Model();
        }

        private void RaiseCanExecuteChanged()
        {
            (AddAppCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (EditAppCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeleteAppCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ConfirmCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (CancelCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
