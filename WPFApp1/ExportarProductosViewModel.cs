using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WPFApp1
{
    public class ExportarProductosViewModel : INotifyPropertyChanged
    {
        private bool _procesando;
        public bool Procesando
        {
            get { return _procesando; } 
            set
            {
                if (_procesando != value) {
                    _procesando = value;
                    OnPropertyChanged(nameof(Procesando));
                }
            }
        }
        public ICommand ExportarXLSXCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        //constructor
        public ExportarProductosViewModel()
        {
            ExportarXLSXCommand = new RelayCommand<object>(async (param) => await ExportarXLSX(param));
            this._procesando = false;
        }

        public async Task ExportarXLSX(Object parameter)
        {
            await ExportarXLSXAsync().ConfigureAwait(false);
        }
        public async Task ExportarXLSXAsync()
        {
            if (!this._procesando) 
            {
                this.Procesando = true;
                try { 
                    List <Productos> Productos = await Task.Run(() => ProductosRepository.LeerProductos());
                    bool resultado = await Task.Run(() => ProductosRepository.CrearLibro(Productos));
                    this.Procesando = false;
                    if (resultado)
                    {
                        System.Windows.MessageBox.Show("Se exportaron los productos.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Hubo un error al intentar exportar los productos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                finally
                {
                    this.Procesando = false;
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
