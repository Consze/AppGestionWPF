using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Win32;

namespace WPFApp1
{
    public class AniadirProductoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ElegirImagenCommand{ get; }

        public AniadirProductoViewModel()
        {
            ElegirImagenCommand = new RelayCommand<object>(ElegirImagen);
        }

        public void ElegirImagen(object parameter)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|Todos los archivos (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Title = "Seleccionar imagen";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            Nullable<bool> resultado = openFileDialog.ShowDialog();

            if (resultado == true)
            {
                string rutaArchivo = openFileDialog.FileName;
                
                //System.Console.WriteLine($"Ruta de la imagen seleccionada: {rutaArchivo}");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
