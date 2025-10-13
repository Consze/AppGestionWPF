using WPFApp1.DTOS;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;

namespace WPFApp1.Servicios
{
    public class ServicioVentas
    {
        private readonly IConmutadorEntidadGenerica<Factura> facturaServicio;
        private readonly IConmutadorEntidadGenerica<Factura_Detalles> facturaDetalleServicio;
        private readonly IConmutadorEntidadGenerica<Factura_pagos> facturaPagosServicio;
        private readonly IConmutadorEntidadGenerica<Cuentas_Contables> cuentasContablesServicio;
        private readonly IConmutadorEntidadGenerica<Medios_Pago> mediosPagoServicio;
        private readonly IConmutadorEntidadGenerica<Sucursal> sucursalesServicio;

        public ServicioVentas(IConmutadorEntidadGenerica<Factura> _servicioFacturas, IConmutadorEntidadGenerica<Factura_Detalles> _servicioFacturaDetalles,
            IConmutadorEntidadGenerica<Factura_pagos> _servicioFacturaPagos)
        {
            facturaServicio = _servicioFacturas;
            facturaDetalleServicio = _servicioFacturaDetalles;
            facturaPagosServicio = _servicioFacturaPagos;
        }

        public bool VenderProductos(List<Ventas> listaVentas)
        {
            try
            {
                List<Sucursal> sucursales = sucursalesServicio.RecuperarList();
                Factura nuevaFactura = new Factura
                {
                    SucursalID = sucursales[0].ToString() // TODO: CRUD + UX para entidad sucursales - 13/10/2025
                };
                string facturaID = facturaServicio.Insertar(nuevaFactura);

                foreach (Ventas registro in listaVentas)
                {
                    Factura_Detalles detalleActual = new Factura_Detalles
                    {
                        FacturaID = facturaID,
                        ProductoSKU = registro.ItemVendido.ProductoSKU,
                        PrecioVenta = registro.precioVenta,
                        Cantidad = registro.Cantidad
                    };
                    Factura_pagos pagoActual = new Factura_pagos
                    {
                        FacturaID = facturaID,
                        MedioPagoID = registro.MedioPagoID,
                        Monto = registro.precioVenta
                    };

                    facturaDetalleServicio.Insertar(detalleActual);
                    facturaPagosServicio.Insertar(pagoActual);
                }

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
