using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using WPFApp1.DTOS;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;
using System.IO;

namespace WPFApp1.ViewModels
{
    public class PanelSecundarioEdicionLoteViewModel : IPanelContextualVM, INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _MostrarListaProductos;
        public bool MostrarListaProductos
        {
            get { return _MostrarListaProductos; }
            set
            {
                if (_MostrarListaProductos != value)
                {
                    _MostrarListaProductos = value;
                    OnPropertyChanged(nameof(MostrarListaProductos));
                }
            }
        }
        private bool _MostrarOpcionesFlag;
        public bool MostrarOpcionesFlag
        {
            get { return _MostrarOpcionesFlag; }
            set
            {
                if (_MostrarOpcionesFlag != value)
                {
                    _MostrarOpcionesFlag = value;
                    OnPropertyChanged(nameof(MostrarOpcionesFlag));
                }
            }
        }
        private bool _BotonHabilitado;
        public bool BotonHabilitado
        {
            get { return _BotonHabilitado; }
            set
            {
                if (_BotonHabilitado != value)
                {
                    _BotonHabilitado = value;
                    OnPropertyChanged(nameof(BotonHabilitado));
                }
            }
        }
        public ICommand VerListaEdicionCommand { get; }
        public ICommand MostrarOpcionesCommand { get; }
        public ObservableCollection<ProductoCatalogo> ColeccionProductosEditar { get; set; }
        public PanelSecundarioEdicionLoteViewModel()
        {
            _MostrarListaProductos = true;
            _MostrarOpcionesFlag = false;
            _BotonHabilitado = false;
            ColeccionProductosEditar = new ObservableCollection<ProductoCatalogo>();

            VerListaEdicionCommand = new RelayCommand<object>(VerListaEdicion);
            MostrarOpcionesCommand = new RelayCommand<object>(MostrarOpciones);
        }
        private void VerListaEdicion(object parameter)
        {
            MostrarListaProductos = !MostrarListaProductos;
        }
        private void MostrarOpciones(object parameter)
        {
            MostrarOpcionesFlag = !MostrarOpcionesFlag;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Dispose()
        {
            //Messenger.Default.Unregister(this);
            GC.SuppressFinalize(this);
        }
    }
}
