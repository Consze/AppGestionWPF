namespace WPFApp1
{
    public class Messenger
    {
        private Dictionary<Type, List<Action<object>>> _subscripciones = new Dictionary<Type, List<Action<object>>>();
        private static Messenger _InstanciaDefault;
        private Messenger() {}
        public static Messenger Default
        {
            get
            {
                if (_InstanciaDefault == null)
                {
                    _InstanciaDefault = new Messenger();
                }
                return _InstanciaDefault;
            }
        }
        public void Subscribir<T>(Action<T> action)
        {
            Type mensajeTipo = typeof(T);

            if (!_subscripciones.ContainsKey(mensajeTipo))
            {
                _subscripciones[mensajeTipo] = new List<Action<object>>();
            }

            _subscripciones[mensajeTipo].Add(obj => action((T)obj));
        }
        public void Publish<T>(T mensaje)
        {
            Type mensajeTipo = typeof(T);
            if (_subscripciones.ContainsKey(mensajeTipo))
            {
                foreach (var action in _subscripciones[mensajeTipo])
                {
                    action(mensaje);
                }
            }
        }
    }
}
