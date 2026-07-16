using IoTSuper_API.Data;
using IoTSuper_API.DTO.Componentes;
using IoTSuper_API.Models;
using IoTSuper_API.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace IoTSuper_API.Services
{
    public class ComponenteService : IComponenteService
    {

        private readonly AppDBContext _context;

        public ComponenteService(AppDBContext context)
        {
            _context = context;
        }

        public async Task ActualizarComponenteAsync(ComponenteDTO componenteDTO)
        {
            await _context.Componentes.Where(c => c.IdComponente == componenteDTO.IdComponente)
                .ExecuteUpdateAsync(c => c
                    .SetProperty(p => p.Nombre, componenteDTO.Nombre)
                    .SetProperty(p => p.Topic, componenteDTO.Topic)
                    .SetProperty(p => p.PosicionX, componenteDTO.PosicionX)
                    .SetProperty(p => p.PosicionY, componenteDTO.PosicionY));

            if(componenteDTO.Termometro != null)
            {
                await _context.Termometros.Where(t => t.IdComponente == componenteDTO.IdComponente)
                    .ExecuteUpdateAsync(t => t
                        .SetProperty(p => p.Temperatura_Maxima, componenteDTO.Termometro.Temperatura_Maxima)
                        .SetProperty(p => p.Temperatura_Minima, componenteDTO.Termometro.Temperatura_Minima)
                        .SetProperty(p => p.EmailEmergencia, componenteDTO.Termometro.EmailEmergencia));
            }
            else if(componenteDTO.Etiqueta != null)
            {
                await _context.Etiquetas.Where(e => e.IdComponente == componenteDTO.IdComponente)
                    .ExecuteUpdateAsync(e => e
                        .SetProperty(p => p.Frase1, componenteDTO.Etiqueta.Frase1)
                        .SetProperty(p => p.Frase2, componenteDTO.Etiqueta.Frase2)
                        .SetProperty(p => p.Frase3, componenteDTO.Etiqueta.Frase3)
                        .SetProperty(p => p.Frase4, componenteDTO.Etiqueta.Frase4));
            }
            else if(componenteDTO.Stock != null)
            {
                await _context.Stocks.Where(s => s.IdComponente == componenteDTO.IdComponente)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(p => p.Stock_Maximo, componenteDTO.Stock.Stock_Maximo)
                        .SetProperty(p => p.Stock_Minimo, componenteDTO.Stock.Stock_Minimo)
                        .SetProperty(p => p.Peso_Unidad, componenteDTO.Stock.Peso_Unidad)
                        .SetProperty(p => p.EmailEmergencia, componenteDTO.Stock.EmailEmergencia));
            } else { throw new Exception("El componente debe tener un tipo específico (Termómetro, Stock o Etiqueta)."); }
        }

        public async Task<int> CrearComponenteAsync(ComponenteDTO componenteDTO)
        {
            Componente componente = new Componente
            {
                IdSeccion = componenteDTO.IdSeccion,
                Nombre = componenteDTO.Nombre,
                Topic = componenteDTO.Topic,
                PosicionX = componenteDTO.PosicionX,
                PosicionY = componenteDTO.PosicionY,
                Habilitado = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Componentes.AddAsync(componente);
            await _context.SaveChangesAsync();

            int nuevoId = componente.IdComponente;

            if (componenteDTO.Termometro != null)
            {
                await _context.Termometros.AddAsync(new Termometro
                {
                    IdComponente = nuevoId,
                    Temperatura_Maxima = componenteDTO.Termometro.Temperatura_Maxima,
                    Temperatura_Minima = componenteDTO.Termometro.Temperatura_Minima,
                    EmailEmergencia = componenteDTO.Termometro.EmailEmergencia
                });
            }
            else if(componenteDTO.Etiqueta != null)
            {
                await _context.Etiquetas.AddAsync(new Etiqueta
                {
                    IdComponente = nuevoId,
                    Frase1 = componenteDTO.Etiqueta.Frase1,
                    Frase2 = componenteDTO.Etiqueta.Frase2,
                    Frase3 = componenteDTO.Etiqueta.Frase3,
                    Frase4 = componenteDTO.Etiqueta.Frase4
                });
            }
            else if (componenteDTO.Stock != null)
            {
                await _context.Stocks.AddAsync(new Stock
                {
                    IdComponente = nuevoId,
                    Stock_Maximo = componenteDTO.Stock.Stock_Maximo,
                    Stock_Minimo = componenteDTO.Stock.Stock_Minimo,
                    Peso_Unidad = componenteDTO.Stock.Peso_Unidad,
                    EmailEmergencia = componenteDTO.Stock.EmailEmergencia
                });
            }
            else { throw new Exception("El componente debe tener un tipo específico (Termómetro, Stock o Etiqueta)."); }

            await _context.SaveChangesAsync();

            return nuevoId;
        }

        public async Task EliminarComponenteAsync(int idComponente)
        {
            await _context.Componentes.Where(c => c.IdComponente == idComponente)
                .ExecuteUpdateAsync(c => c
                    .SetProperty(p => p.Habilitado, false));
        }

        public async Task<ComponenteDTO> GetComponenteAsync(int idComponente)
        {
            ComponenteDTO componente = await _context.Componentes
                .Where(c => c.IdComponente == idComponente && c.Habilitado == true)
                .Select(c => new ComponenteDTO
                {
                    IdComponente = c.IdComponente,
                    IdSeccion = c.IdSeccion,
                    Nombre = c.Nombre,
                    Topic = c.Topic,
                    PosicionX = c.PosicionX,
                    PosicionY = c.PosicionY,
                    Termometro = _context.Termometros
                        .Where(t => t.IdComponente == c.IdComponente)
                        .Select(t => new TermometroDTO
                        {
                            IdTermometro = t.IdTermometro,
                            Temperatura_Maxima = t.Temperatura_Maxima,
                            Temperatura_Minima = t.Temperatura_Minima,
                            EmailEmergencia = t.EmailEmergencia
                        })
                        .FirstOrDefault(),
                    Stock = _context.Stocks
                        .Where(s => s.IdComponente == c.IdComponente)
                        .Select(s => new StockDTO
                        {
                            IdStock = s.IdStock,
                            Stock_Maximo = s.Stock_Maximo,
                            Stock_Minimo = s.Stock_Minimo,
                            Peso_Unidad = s.Peso_Unidad,
                            EmailEmergencia = s.EmailEmergencia
                        })
                        .FirstOrDefault(),
                    Etiqueta = _context.Etiquetas
                        .Where(e => e.IdComponente == c.IdComponente)
                        .Select(e => new EtiquetaDTO
                        {
                            IdEtiqueta = e.IdEtiqueta,
                            Frase1 = e.Frase1,
                            Frase2 = e.Frase2,
                            Frase3 = e.Frase3,
                            Frase4 = e.Frase4,
                            Visualizaciones = e.Visualizaciones
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            return componente ?? throw new Exception("No se encontró un componente con el ID proporcionado.");
        }

        public async Task<List<ComponenteDTO>> GetComponentesAsync(int seccion)
        {
            List<ComponenteDTO> componentes = await _context.Componentes
                .Where(c => c.IdSeccion == seccion && c.Habilitado == true)
                .Select(c => new ComponenteDTO
                {
                    IdComponente = c.IdComponente,
                    IdSeccion = c.IdSeccion,
                    Nombre = c.Nombre,
                    Topic = c.Topic,
                    PosicionX = c.PosicionX,
                    PosicionY = c.PosicionY,

                    Termometro = _context.Termometros
                        .Where(t => t.IdComponente == c.IdComponente)
                        .Select(t => new TermometroDTO
                        {
                            IdTermometro = t.IdTermometro,
                            Temperatura_Maxima = t.Temperatura_Maxima,
                            Temperatura_Minima = t.Temperatura_Minima,
                            EmailEmergencia = t.EmailEmergencia
                        })
                        .FirstOrDefault(),

                    Stock = _context.Stocks
                        .Where(s => s.IdComponente == c.IdComponente)
                        .Select(s => new StockDTO
                        {
                            IdStock = s.IdStock,
                            Stock_Maximo = s.Stock_Maximo,
                            Stock_Minimo = s.Stock_Minimo,
                            Peso_Unidad = s.Peso_Unidad,
                            EmailEmergencia = s.EmailEmergencia
                        })
                        .FirstOrDefault(),

                    Etiqueta = _context.Etiquetas
                        .Where(e => e.IdComponente == c.IdComponente)
                        .Select(e => new EtiquetaDTO
                        {
                            IdEtiqueta = e.IdEtiqueta,
                            Frase1 = e.Frase1,
                            Frase2 = e.Frase2,
                            Frase3 = e.Frase3,
                            Frase4 = e.Frase4,
                            Visualizaciones = e.Visualizaciones
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            return componentes.Count > 0 ? componentes : new List<ComponenteDTO>();
        }

        public Task SumarUnoVisualizacionAsync(string topic)
        {
            Componente? componente = _context.Componentes.FirstOrDefault(c => c.Topic == topic && c.Habilitado == true);

            Etiqueta? etiqueta = _context.Etiquetas.FirstOrDefault(e => e.IdComponente == componente.IdComponente);

            if (etiqueta != null)
            {
                etiqueta.Visualizaciones += 1;
                return _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("No se encontró una etiqueta asociada al componente con el topic proporcionado.");
            }
        }
    }
}
