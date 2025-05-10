using System;
using System.Numerics;
using SistemaPrestamos;

namespace LoginMenuApp
{
    class Program
    {
        static void Main()
        {
            // Bucle principal para permitir múltiples sesiones sin salir de la aplicación
            while (true)
            {
                // Limpia la consola al inicio de cada sesión
                Console.Clear();
                Console.WriteLine("🔐 INICIO DE SESIÓN");

                // Intento de inicio de sesión llamando al GestorLogin
                string usuario = GestorLogin.IniciarSesion();

                // Si el usuario es null (credenciales incorrectas), vuelve al inicio del bucle
                if (usuario == null) continue;

                // Obtiene el rol del usuario que inició sesión
                string rol = GestorLogin.ObtenerRol(usuario);

                // Bucle del menú interno para el usuario logueado
                while (true)
                {
                    // Menú básico para todos los usuarios
                    Console.WriteLine("\n--- MENÚ ---");
                    Console.WriteLine("1. Registrar préstamo");
                    Console.WriteLine("2. Ver préstamos");

                    // Opciones adicionales solo para administradores
                    if (rol == "Administrador")
                    {
                        Console.WriteLine("3. Devolver equipo");
                        Console.WriteLine("4. Crear nueva área");
                        Console.WriteLine("5. Asignar jefe de área");
                    }

                    // Opción para cerrar sesión
                    Console.WriteLine("0. Cerrar sesión");
                    Console.Write("Seleccione una opción: ");

                    // Lee la opción seleccionada por el usuario
                    string opcion = Console.ReadLine();

                    // Switch para manejar las diferentes opciones del menú
                    switch (opcion)
                    {
                        case "1": // Registrar préstamo
                            if (rol == "Administrador")
                                GestorPrestamos.RegistrarPrestamo();
                            else
                                Console.WriteLine("❌ Solo el administrador puede registrar préstamos.");
                            break;

                        case "2": // Ver préstamos
                            GestorPrestamos.MostrarPrestamos(rol);
                            break;

                        case "3": // Devolver equipo (solo admin)
                            if (rol == "Administrador")
                                GestorPrestamos.DevolverEquipo();
                            else
                                Console.WriteLine("❌ Solo el administrador puede devolver equipos.");
                            break;

                        case "4": // Crear nueva área (solo admin)
                            if (rol == "Administrador")
                            {
                                Console.Write("Ingrese el nombre del área: ");
                                string nuevaArea = Console.ReadLine();
                                GestorLogin.CrearArea(nuevaArea);
                            }
                            break;

                        case "5": // Asignar jefe de área (solo admin)
                            if (rol == "Administrador")
                            {
                                Console.Write("Ingrese el ID del jefe de área: ");
                                string id = Console.ReadLine();
                                Console.Write("Área a asignar: ");
                                string area = Console.ReadLine();
                                GestorLogin.AsignarJefe(id, area);
                            }
                            break;

                        case "0": // Cerrar sesión
                            Console.WriteLine("\n🔓 Cerrando sesión...");
                            // Pausa breve para visualización del mensaje
                            System.Threading.Thread.Sleep(1000); //instruccion que pausa el programa durante 1 segundo
                            Console.Clear();
                            break; // Sale del menú actual para volver al login

                        default: // Opción no válida
                            Console.WriteLine("❌ Opción no válida.");
                            break;
                    }

                    // Condición para salir del bucle del menú (cerrar sesión)
                    if (opcion == "0")
                        break; // Rompe solo el bucle del menú, no el bucle principal
                }
            }
        }
    }
}
