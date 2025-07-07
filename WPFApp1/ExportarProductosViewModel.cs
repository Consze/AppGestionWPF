using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WPFApp1
{
    public class ExportarProductosViewModel : INotifyPropertyChanged
    {
        public bool ExportacionEnProceso { get; set; }
        public ICommand ExportarXLSXCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        //constructor
        public ExportarProductosViewModel()
        {
            ExportarXLSXCommand = new RelayCommand<object>(ExportarXLSX);
            this.ExportacionEnProceso = false;
        }

        public void ExportarXLSX(object parameter)
        {
            if (!this.ExportacionEnProceso) 
            {
                this.ExportacionEnProceso = true;
                List <Productos> Productos = ProductosRepository.LeerProductos();
                bool resultado = ProductosRepository.CrearLibro(Productos); //<------- Tarea asíncronica
                if (resultado)
                {
                    System.Windows.MessageBox.Show("Se exportaron los productos.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    System.Windows.MessageBox.Show("Hubo un error al intentar exportar los productos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                System.Console.WriteLine("Ya hay una exportación en proceso");
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
