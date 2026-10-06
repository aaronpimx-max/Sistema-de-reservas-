using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexSpace.DAL
{
    public class ClienteSancionadoException : Exception
    {
        public ClienteSancionadoException(string mensaje) : base(mensaje) { }
    }

    public enum TipoCliente { Estandar, VIP }
    public enum TipoPuesto { EscritorioIndividual, SalaReuniones, CabinaPrivada }
    public enum EstadoReserva { Confirmada, Cancelada, Finalizada }

    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public TipoCliente TipoCliente { get; set; }
        public int SancionesActivas { get; set; }
    }

    public class Puesto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public TipoPuesto TipoPuesto { get; set; }
        public decimal TarifaBasePorHora { get; set; }
    }

    public class Reserva
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int PuestoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public EstadoReserva Estado { get; set; }
        public decimal CostoTotal { get; set; }
    }
    public class FlexSpaceRepository
    {
        public static List<Cliente> Clientes = new List<Cliente>
{
new Cliente { Id = 1, Nombre = "Aaron Montaño", Email = "aaron@flex.com", TipoCliente = TipoCliente.VIP, SancionesActivas = 0 },
new Cliente { Id = 2, Nombre = "Carlos Pérez", Email = "carlos@flex.com", TipoCliente = TipoCliente.Estandar, SancionesActivas = 3 },
new Cliente { Id = 3, Nombre = "Maria Gómez", Email = "maria@flex.com", TipoCliente = TipoCliente.Estandar, SancionesActivas = 1 }
};

        public static List<Puesto> Puestos = new List<Puesto>
{
new Puesto { Id = 101, Codigo = "P-01", TipoPuesto = TipoPuesto.EscritorioIndividual, TarifaBasePorHora = 1000m },
new Puesto { Id = 102, Codigo = "P-02", TipoPuesto = TipoPuesto.CabinaPrivada, TarifaBasePorHora = 2000m },
new Puesto { Id = 103, Codigo = "P-03", TipoPuesto = TipoPuesto.SalaReuniones, TarifaBasePorHora = 3500m }
};

        public static List<Reserva> Reservas = new List<Reserva>();
    }
}