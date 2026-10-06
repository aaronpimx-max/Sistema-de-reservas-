using System;
using System.Globalization;
using FlexSpace.DAL;
using FlexSpace.BLL;
using Microsoft.Win32;

namespace FlexSpace.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            ReservaBLL bll = new ReservaBLL();
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine(" SISTEMA DE GESTIÓN FLEXSPACE MENÚ ");
                Console.WriteLine("==================================================");
                Console.WriteLine("1. Registrar Nueva Reserva");
                Console.WriteLine("2. Cancelar Reserva");
                Console.WriteLine("3. Consultar Reservas Activas por Puesto");
                Console.WriteLine("4. Listar Clientes Sancionados");
                Console.WriteLine("5. Salir");
                Console.WriteLine("==================================================");
                Console.Write("Seleccione una opción: ");
                string opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            Console.Clear();
                            Console.WriteLine("-- REGISTRAR NUEVA RESERVA-- - ");
                            Console.Write("Ingrese Cliente ID (Ej: 1=VIP, 2=Sancionado, 3=Estandar): ");
                            int clienteId = int.Parse(Console.ReadLine());
                            Console.Write("Ingrese Puesto ID (Ej: 101, 102, 103): ");
                            int puestoId = int.Parse(Console.ReadLine());

                            Console.Write("Fecha y Hora Inicio (yyyy-MM-dd HH:mm): ");
                            DateTime inicio = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                            Console.Write("Fecha y Hora Fin (yyyy-MM-dd HH:mm): ");
                            DateTime fin = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                            Reserva res = bll.RegistrarReserva(clienteId, puestoId, inicio, fin);

                            Console.WriteLine("\n¡RESERVA CONFIRMADA EXITOSAMENTE!");
                            Console.WriteLine($"Reserva ID: {res.Id} | Costo Total Calculado: ${res.CostoTotal:N2}");
                            break;

                        case "2":
                            Console.Clear();
                            Console.WriteLine("-- CANCELAR RESERVA--- ");
                            Console.Write("Ingrese Reserva ID a cancelar: ");
                            int resId = int.Parse(Console.ReadLine());
                            bll.CancelarReserva(resId);
                            Console.WriteLine("\nReserva cancelada correctamente.");
                            break;

                        case "3":
                            Console.Clear();
                            Console.WriteLine("-- CONSULTAR RESERVAS ACTIVAS POR PUESTO-- - ");
                            Console.Write("Ingrese Código de Puesto (Ej: P-01, P-02, P-03): ");
                            string codPuesto = Console.ReadLine();
                            var listaReservas = bll.ConsultarReservasActivasPorPuesto(codPuesto);
                            Console.WriteLine($"\nReservas futuras para {codPuesto}:");
                            foreach (var r in listaReservas)
                            {
                                Console.WriteLine($"Reserva ID: {r.Id} | Inicio: {r.FechaInicio} | Fin: {r.FechaFin} | Monto: ${r.CostoTotal}");
                            }
                            if (listaReservas.Count == 0) Console.WriteLine("No hay reservas activas futuras.");
                            break;

                        case "4":
                            Console.Clear();
                            Console.WriteLine("-- CLIENTES SANCIONADOS-- - ");
                            var sancionados = bll.ListarClientesSancionados();
                            foreach (var c in sancionados)
                            {
                                Console.WriteLine($"ID: {c.Id} | Nombre: {c.Nombre} | Sanciones Activas: {c.SancionesActivas}");
                            }
                            break;
                        case "5":
                            salir = true;
                            Console.WriteLine("\n¡Gracias por usar FlexSpace!");
                            break;

                        default:
                            Console.WriteLine("\nOpción no válida.");
                            break;
                    }
                }
                catch (ClienteSancionadoException ex)
                {
                    Console.WriteLine($"\n[RECHAZADO POR SANCIONES]: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[ERROR]: {ex.Message}");
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }
    }
}