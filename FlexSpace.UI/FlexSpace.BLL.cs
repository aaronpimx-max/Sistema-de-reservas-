using System;
using System.Collections.Generic;
using FlexSpace.DAL;

namespace FlexSpace.BLL
{
    public class ReservaBLL
    {
        public bool HaySolapamiento(int puestoId, DateTime inicio, DateTime fin)
        {
            foreach (Reserva r in FlexSpaceRepository.Reservas)
            {
                if (r.PuestoId == puestoId && r.Estado == EstadoReserva.Confirmada)
                {
                    // Si los horarios se cruzan
                    if (inicio < r.FechaFin && fin > r.FechaInicio)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public decimal CalcularCostoTotal(Cliente cliente, Puesto puesto, DateTime inicio, DateTime fin)
        {
            if (cliente.SancionesActivas >= 3)
            {
                throw new ClienteSancionadoException("El cliente está bloqueado por tener 3 o más sanciones.");
            }
            decimal horas = (decimal)(fin - inicio).TotalHours;

            decimal total = horas * puesto.TarifaBasePorHora;

            if (cliente.SancionesActivas > 0)
            {
                return total * 1.20m;
            }

            if (inicio.DayOfWeek == DayOfWeek.Saturday || inicio.DayOfWeek == DayOfWeek.Sunday)
            {
                total = total + (total * 0.15m);
            }

            if (horas >= 5)
            {
                total = total - (total * 0.10m);
            }

            if (cliente.TipoCliente == TipoCliente.VIP)
            {
                total = total - (total * 0.05m);
            }
            return total;
        }

        public Reserva RegistrarReserva(int clienteId, int puestoId, DateTime inicio, DateTime fin)
        {
            Cliente cliente = null;
            foreach (Cliente c in FlexSpaceRepository.Clientes)
            {
                if (c.Id == clienteId) cliente = c;
            }

            Puesto puesto = null;
            foreach (Puesto p in FlexSpaceRepository.Puestos)
            {
                if (p.Id == puestoId) puesto = p;
            }

            if (cliente == null || puesto == null)
            {
                throw new Exception("Cliente o Puesto no encontrado.");
            }

            if (HaySolapamiento(puestoId, inicio, fin))
            {
                throw new Exception("El puesto ya está reservado en ese horario.");
            }

            decimal costo = CalcularCostoTotal(cliente, puesto, inicio, fin);

            Reserva nuevaReserva = new Reserva();
            nuevaReserva.Id = FlexSpaceRepository.Reservas.Count + 1;
            nuevaReserva.ClienteId = clienteId;
            nuevaReserva.PuestoId = puestoId;
            nuevaReserva.FechaInicio = inicio;
            nuevaReserva.FechaFin = fin;
            nuevaReserva.Estado = EstadoReserva.Confirmada;
            nuevaReserva.CostoTotal = costo;

            FlexSpaceRepository.Reservas.Add(nuevaReserva);
            return nuevaReserva;
        }

        public void CancelarReserva(int reservaId)
        {
            foreach (Reserva r in FlexSpaceRepository.Reservas)
            {
                if (r.Id == reservaId)
                {
                    r.Estado = EstadoReserva.Cancelada;

                    TimeSpan diferencia = r.FechaInicio - DateTime.Now;
                    if (diferencia.TotalHours < 2)
                    {
                        foreach (Cliente c in FlexSpaceRepository.Clientes)
                        {
                            if (c.Id == r.ClienteId)
                            {
                                c.SancionesActivas = c.SancionesActivas + 1;
                            }
                        }
                    }
                    return;
                }
            }
            throw new Exception("Reserva no encontrada.");
        }
        public List<Reserva> ConsultarReservasActivasPorPuesto(string codigoPuesto)
        {
            int puestoId = 0;
            foreach (Puesto p in FlexSpaceRepository.Puestos)
            {
                if (p.Codigo == codigoPuesto) puestoId = p.Id;
            }

            List<Reserva> resultado = new List<Reserva>();
            foreach (Reserva r in FlexSpaceRepository.Reservas)
            {
                if (r.PuestoId == puestoId && r.Estado == EstadoReserva.Confirmada)
                {
                    resultado.Add(r);
                }
            }
            return resultado;
        }
        public List<Cliente> ListarClientesSancionados()
        {
            List<Cliente> sancionados = new List<Cliente>();
            foreach (Cliente c in FlexSpaceRepository.Clientes)
            {
                if (c.SancionesActivas > 0)
                {
                    sancionados.Add(c);
                }
            }
            return sancionados;
        }
    }
}