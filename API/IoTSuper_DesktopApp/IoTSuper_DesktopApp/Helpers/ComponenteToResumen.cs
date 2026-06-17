using IoTSuper_DesktopApp.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace IoTSuper_DesktopApp.Helpers
{
    public static class ComponenteToResumen
    {
        public static ResumenDTO ConvierteAResumenDTO(ComponenteDTO dto, string nombreCentro, string nombreSeccion)
        {
            string tipo, ultimoDato, estado;

            if (dto.Termometro != null)
            {
                tipo = "Termómetro";
            }
            else if (dto.Stock != null)
            {
                tipo = "Stock";
            }
            else
            {
                tipo = "Etiqueta/Display";
            }

            return new ResumenDTO
            {
                IdComponente = dto.IdComponente,
                Nombre = dto.Nombre,
                Tipo = tipo,
                Centro = nombreCentro,
                Seccion = nombreSeccion,
                Estado = "Error",
                UltimoDato = "N/A",
                Actualizado = "N/A"
            };
        }
    }
}
