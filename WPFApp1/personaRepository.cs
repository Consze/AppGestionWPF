using System.Data.SQLite;

namespace WPFApp1
{
    public class personaRepository
    {
        public static string cadena { get; set; } = ".\\datos\\base.db";
        public static List<Persona> LeerRegistros()
        {
            List<Persona> listaRegistros = new List<Persona>();
            ConexionDB instancia = new ConexionDB();
            string consulta = "SELECT * FROM personas";
            using (SQLiteCommand comando = new SQLiteCommand(consulta, instancia.Conexion))
            {
                using (SQLiteDataReader Lector = comando.ExecuteReader())
                {
                    while (Lector.Read())
                    {
                        int id = Convert.ToInt32(Lector["persona_id"]);
                        string nombre = Lector["nombre"].ToString();
                        int altura = Convert.ToInt32(Lector["altura"]);
                        int peso = Convert.ToInt32(Lector["peso"]);
                        Persona nuevoRegistro = new Persona(id, nombre, altura, peso);
                        listaRegistros.Add(nuevoRegistro);
                    }
                }
            }
            instancia.CerrarConexionDB();
            return listaRegistros;
        }

        /// <summary>
        /// Añade una registro a la tabla Personas de la base de datos
        /// </summary>
        /// <param name="nuevaPersona">Objeto de la clase Persona</param>
        /// <returns>Boolean</returns>
        public static bool AniadirPersona(Persona nuevaPersona)
        {
            ConexionDB Instancia = new ConexionDB();
            string Consulta = "INSERT INTO Personas (nombre, altura, peso) VALUES (@nombre, @altura, @peso)";
            try
            {
                using (SQLiteCommand comando = new SQLiteCommand(Consulta, Instancia.Conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", nuevaPersona.nombre);
                    comando.Parameters.AddWithValue("@altura", nuevaPersona.altura);
                    comando.Parameters.AddWithValue("@peso", nuevaPersona.peso);
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
                return false;
            }
            finally
            {
                Instancia.CerrarConexionDB();
            }
        }

        public static bool EliminarPersona(int IDPersona)
        {
            ConexionDB Instancia = new ConexionDB();
            try
            {
                string _consulta = "DELETE FROM Personas WHERE persona_id = @id";
                using (SQLiteCommand comando = new SQLiteCommand(_consulta, Instancia.Conexion))
                {
                    comando.Parameters.AddWithValue("@id", IDPersona);
                    int filasAfectadas = comando.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                Instancia.CerrarConexionDB();
            }
        }

        /// <summary>
        /// Modifica un registro de la tabla de personas en la base de datos.
        /// </summary>
        /// <param name="personaEditar">Un objeto de tipo 'Persona'. Se utiliza su ID para evaluar los cambios</param>
        /// <returns></returns>
        public static bool EditarPersona(Persona personaEditar)
        {
            ConexionDB Instancia = new ConexionDB();
            Persona registroVigente = new Persona(0, "", 0, 0);
            registroVigente = RecuperarRegistro(personaEditar.id);

            if (registroVigente.id > 0) // Validar el registro 
            { 
                FlagsCambios propiedades = new FlagsCambios();

                //1 - Comprobar cambios y construir consulta
                string consulta = "UPDATE Personas SET ";

                if (personaEditar.nombre != registroVigente.nombre)
                {
                    consulta += "nombre = @nombre";
                    propiedades.nombreCambiado = true;
                    propiedades.Count += 1;
                }
                if (personaEditar.altura != registroVigente.altura)
                {
                    if (consulta.Contains(","))
                    {
                        consulta += ", altura = @altura";
                    }
                    else
                    {
                        consulta += "altura = @altura";
                    }
                    propiedades.alturaCambiada = true;
                    propiedades.Count += 1;
                }
                if (personaEditar.peso != registroVigente.peso)
                {
                    if (consulta.Contains(","))
                    {
                        consulta += ", peso = @peso";
                    }
                    else
                    {
                        consulta += "peso = @peso";
                    }
                    propiedades.pesoCambiado = true;
                    propiedades.Count += 1;
                }
                consulta += " WHERE persona_id = @id;";

                if (propiedades.Count > 0)
                {
                    using (SQLiteCommand comando = new SQLiteCommand(consulta, Instancia.Conexion))
                    {
                        comando.Parameters.AddWithValue("@id", registroVigente.id);
                        if (propiedades.nombreCambiado) { comando.Parameters.AddWithValue("@nombre", personaEditar.nombre); }
                        if (propiedades.alturaCambiada) { comando.Parameters.AddWithValue("@altura", personaEditar.altura); }
                        if (propiedades.pesoCambiado) { comando.Parameters.AddWithValue("@peso", personaEditar.peso); }

                        int filasAfectadas = comando.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            Instancia.CerrarConexionDB();
                            return true;
                        }
                        else
                        {
                            Instancia.CerrarConexionDB();
                            return false;
                        }
                    }
                }
                else
                {
                    Instancia.CerrarConexionDB();
                    return false;
                }
            }
            else
            {
                Console.WriteLine("No existe el registro en la tabla");
                Instancia.CerrarConexionDB();
                return false;
            }
        }

        /// <summary>
        /// Recupera un registro individual de la tabla de personas
        /// </summary>
        /// <param name="IDPersona">ID del registro</param>
        /// <returns>Un objeto de clase 'Persona'</returns>
        public static Persona RecuperarRegistro(int IDPersona)
        {
            Persona registro = new Persona(0, "", 0, 0);
            ConexionDB Instancia = new ConexionDB();
            string consulta = "SELECT * FROM Personas WHERE persona_id = @id";
            using (SQLiteCommand Comando = new SQLiteCommand(consulta, Instancia.Conexion))
            {
                Comando.Parameters.AddWithValue("@id", IDPersona);
                using (SQLiteDataReader Lector = Comando.ExecuteReader())
                {
                    while (Lector.Read())
                    {
                        registro.nombre = Lector["nombre"].ToString();
                        registro.id = Convert.ToInt32(Lector["persona_id"]);
                        registro.altura = Convert.ToInt32(Lector["altura"]);
                        registro.peso = Convert.ToInt32(Lector["peso"]);
                    }
                }
            }
            Instancia.CerrarConexionDB();
            return registro;
        }
    }
}
