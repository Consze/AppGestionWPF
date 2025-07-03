using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WPFApp1
{
    public class ExportarProductosViewModel : INotifyPropertyChanged
    {
        public ICommand ExportarXLSXCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        //constructor
        public ExportarProductosViewModel()
        {
            ExportarXLSXCommand = new RelayCommand<object>(ExportarXLSX);
        }

        public void ExportarXLSX(object parameter)
        {
            //System.Windows.MessageBox.Show("Mensaje de Prueba.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
