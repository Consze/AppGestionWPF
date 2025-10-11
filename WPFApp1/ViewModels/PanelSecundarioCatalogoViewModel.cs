using System.Collections.ObjectModel;
using System.ComponentModel;
using WPFApp1.DTOS;

namespace WPFApp1.ViewModels
{
    public class PanelSecundarioCatalogoViewModel : INotifyPropertyChanged
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
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
