using System;
using System.Collections.Generic;

namespace SistemaPrestamos
{
    // Clase estática que gestiona el login y usuarios del sistema
    public static class GestorLogin
    {
        // Diccionario que almacena usuarios y sus roles (ID -> Rol)
        public static Dictionary<string, string> usuarios = new Dictionary<string, string>
        {
            { "admin01", "Administrador" },    // Usuario administrador
            { "jefe_tco", "Ventas" },          // Jefe de ventas
            { "jefe_onco", "Seguros" },        // Jefe de seguros
            { "jefe_adm", "Administración" }  // Jefe de administración
        };

        // Método para iniciar sesión
        public static string IniciarSesion()
        {
            Console.Write("Ingrese su ID de usuario: ");
            string id = Console.ReadLine();

            // Verifica si el usuario existe
            if (usuarios.ContainsKey(id))
            {
                Console.WriteLine($"Bienvenido, {id} ({usuarios[id]})");
                return id;  // Devuelve el ID si es válido
            }

            Console.WriteLine("❌ ID no válido.");
            return null;  // Devuelve null si no es válido
        }

        // Obtiene el rol de un usuario según su ID
        public static string ObtenerRol(string id)
        {
            return usuarios.ContainsKey(id) ? usuarios[id] : null;
        }

        // Método para crear nuevas áreas/departamentos
        public static void CrearArea(string nuevaArea)
        {
            // Verifica que el área no exista previamente
            if (!GestorPrestamos.Departamentos.Contains(nuevaArea))
            {
                GestorPrestamos.Departamentos.Add(nuevaArea);
                Console.WriteLine($"✅ Área '{nuevaArea}' creada exitosamente.");
            }
            else
            {
                Console.WriteLine("❌ El área ya existe.");
            }
        }

        // Asigna un jefe a un área específica
        public static void AsignarJefe(string id, string area)
        {
            if (usuarios.ContainsKey(id))
            {
                usuarios[id] = area;  // Actualiza el rol del usuario
                Console.WriteLine($"✅ El usuario '{id}' ahora es jefe de área de {area}.");
            }
            else
            {
                Console.WriteLine("❌ El ID de usuario no existe.");
            }
        }
    }

    // Clase estática que gestiona los préstamos de equipos
    public static class GestorPrestamos
    {
        // Lista de departamentos disponibles
        public static List<string> Departamentos = new List<string> { "Ventas", "Seguros", "Administración" };

        // Array para almacenar los préstamos (máximo 100)
        private static Prestamo[] prestamos = new Prestamo[100];

        // Contador de préstamos registrados
        private static int contadorPrestamos = 0;

        // Clase interna que representa un préstamo
        public class Prestamo
        {
            public string NombreAgente { get; set; }  // Nombre del agente que recibe el préstamo
            public string DNI { get; set; }            // DNI del agente
            public string Equipo { get; set; }         // Tipo de equipo prestado
            public string Caracteristicas { get; set; }// Detalles del equipo
            public string Area { get; set; }           // Área/departamento
            public DateTime Fecha { get; set; }        // Fecha del préstamo
            public string Estado { get; set; }         // Estado actual (Prestado/Devuelto/etc)
            public DateTime? FechaDevolucion { get; set; } // Nullable, porque puede no haber devolución aún
        }

        // Registra un nuevo préstamo en el sistema
        public static void RegistrarPrestamo()
        {
            // Verifica si hay espacio para más préstamos
            if (contadorPrestamos >= prestamos.Length)
            {
                Console.WriteLine("\n❌ No se pueden registrar más préstamos.");
                return;
            }

            // Solicita datos del préstamo
            Console.Write("\nNombre del agente: ");
            string nombre = Console.ReadLine();

            Console.Write("DNI del agente: ");
            string dni = Console.ReadLine();

            Console.Write("Equipo prestado: ");
            string equipo = Console.ReadLine();

            Console.Write("Características del equipo: ");
            string caracteristicas = Console.ReadLine();

            Console.Write("Área asignada: ");
            string area = Console.ReadLine();

            // Valida que el área exista
            if (!Departamentos.Contains(area))
            {
                Console.WriteLine("❌ Área no válida.");
                return;
            }

            // Crea y guarda el nuevo préstamo
            prestamos[contadorPrestamos++] = new Prestamo
            {
                NombreAgente = nombre,
                DNI = dni,
                Equipo = equipo,
                Caracteristicas = caracteristicas,
                Area = area,
                Fecha = DateTime.Now,  // Fecha actual
                Estado = "Prestado"   // Estado inicial
            };

            Console.WriteLine("✅ Préstamo registrado.");
        }

        // Muestra los préstamos según el rol del usuario
        public static void MostrarPrestamos(string rol)
        {
            Console.WriteLine("\n📋 LISTADO DE PRÉSTAMOS:");

            // Si no hay préstamos registrados
            if (contadorPrestamos == 0)
            {
                Console.WriteLine("No hay préstamos registrados aún.");
                return;
            }

            // Recorre todos los préstamos
            for (int i = 0; i < contadorPrestamos; i++)
            {
                var p = prestamos[i];

                // Muestra solo los préstamos del área del usuario (o todos si es admin)
                if (rol == "Administrador" || p.Area.Equals(rol, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"\n{i + 1}) Agente: {p.NombreAgente} | DNI: {p.DNI}");
                    Console.WriteLine($" Equipo: {p.Equipo} ({p.Caracteristicas})");
                    Console.WriteLine($" Área: {p.Area} | Fecha: {p.Fecha.ToString("dd/MM/yyyy HH:mm")} | Estado: {p.Estado}");

                    //Si el equipo ya fue devuelto, se muestra la fecha correspondiente
                    if (p.FechaDevolucion.HasValue)
                        Console.WriteLine($" Fecha de devolución: {p.FechaDevolucion:g}");
                }
            }
        }

        // Método para registrar la devolución de un equipo prestado
        public static void DevolverEquipo()
        {
            // Solicita al usuario el DNI del agente que devuelve el equipo
            Console.Write("\nIngrese el DNI del agente que devuelve el equipo: ");
            string dni = Console.ReadLine();

            // Bandera para rastrear si se encontró el préstamo
            bool encontrado = false;

            // Recorre todos los préstamos registrados
            for (int i = 0; i < contadorPrestamos; i++)
            {
                // Verifica que el préstamo exista, coincida el DNI y esté en estado "Prestado"
                if (prestamos[i] != null && prestamos[i].DNI == dni && prestamos[i].Estado == "Prestado")
                {
                    // Cambia el estado del préstamo a "Devuelto"
                    prestamos[i].Estado = "Devuelto";
                    prestamos[i].FechaDevolucion = DateTime.Now; // ✅ Guarda la fecha de devolución

                    // Informa al usuario que la operación fue exitosa
                    Console.WriteLine("✅ Equipo devuelto correctamente.");

                    // Marca como encontrado y sale del bucle
                    encontrado = true;
                    break;
                }
            }

            // Si no se encontró ningún préstamo activo con ese DNI
            if (!encontrado)
            {
                Console.WriteLine("❌ No se encontró un préstamo activo para ese DNI.");
            }
        }

    }
}