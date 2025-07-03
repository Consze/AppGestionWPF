using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
namespace WPFApp1
{
    public class ExcelServicios
    {
        public ExcelServicios()
        {

        }

        /** TODO: 03/07/25, 00:30hs: Refactorizar para utilizar listas genericas.
        public static bool CrearLibro(List<T> Coleccion)
        {
            XSSFWorkbook WorkBook = new XSSFWorkbook();
            ISheet Hoja;
            Hoja = WorkBook.CreateSheet("Productos");

            // Crear Filas
            IRow EncabezadoFila = Hoja.CreateRow(0);
            EncabezadoFila.CreateCell(0).SetCellValue("Parametro 1");
            EncabezadoFila.CreateCell(1).SetCellValue("Parametro 2");
            EncabezadoFila.CreateCell(2).SetCellValue("Parametro 3");

            // Escribir datos
            int NumeroDeFila = 0;
            for (int i = 0; i < Coleccion.Count; i++)
            {
                IRow Fila = Hoja.CreateRow(NumeroDeFila++);
                Fila.CreateCell(0).SetCellValue(Coleccion[i].Parametro1);
                Fila.CreateCell(1).SetCellValue(Coleccion[i].Parametro2);
                Fila.CreateCell(2).SetCellValue(Coleccion[i].Parametro3.ToString());
            }

            try
            {
                using (FileStream file = new FileStream(".\\Exportaciones\\Nombre_Archivo.xlsx", FileMode.Create, FileAccess.Write))
                {
                    WorkBook.Write(file);
                }
                return true;
            }
            catch(Exception ex)
            {
                System.Console.WriteLine($"Error {ex.Message}");
                return false;
            }
        }
        */

        /** TODO: 02/07/25, 23:58hs: Terminar implementación
        public void LeerLibro()
        {
            using (FileStream file = new FileStream("ruta.xlsx", FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook workbook = new XSSFWorkbook(file);
            }
        }
        */
    }
}
