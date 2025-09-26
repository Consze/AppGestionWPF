namespace WPFApp1.Entidades
{
    public class Arquetipos : EntidadNombrada
    {
        private string _categoriaID;
        public string CategoriaID
        {
            get => _categoriaID;
            set
            {
                if (_categoriaID != value)
                {
                    _categoriaID = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
