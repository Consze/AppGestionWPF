using System.Collections.ObjectModel;
using System.ComponentModel;
using WPFApp1.DTOS;
using WPFApp1.Interfaces;

namespace WPFApp1.ViewModels
{
    public class PanelSecundarioCatalogoViewModel : IPanelContextualVM, INotifyPropertyChanged
    {
        private int _ContadorItemsElegidos;
        public int ContadorItemsElegidos
        {
            get { return _ContadorItemsElegidos; }
            set
            {
                if (_ContadorItemsElegidos != value)
                {
                    _ContadorItemsElegidos = value;
                    OnPropertyChanged(nameof(ContadorItemsElegidos));
                }
            }
        }
        public ObservableCollection<ProductoBase> ColeccionProductosVenta { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public PanelSecundarioCatalogoViewModel()
        {
            _ContadorItemsElegidos = 0;
            ColeccionProductosVenta = new ObservableCollection<ProductoBase>();
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
