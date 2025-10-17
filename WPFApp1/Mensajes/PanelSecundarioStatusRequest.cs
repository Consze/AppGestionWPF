using WPFApp1.Interfaces;

namespace WPFApp1.Mensajes
{
    public class PanelSecundarioStatusRequest
    {
        public bool PanelSecundarioExiste { get; set; }
        public IPanelContextualVM ViewModel { get; set; }
    }
}
