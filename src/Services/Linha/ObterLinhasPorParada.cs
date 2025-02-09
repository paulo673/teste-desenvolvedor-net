﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infra;
using Microsoft.EntityFrameworkCore;
using Services.Commons;

namespace Services.Linha
{
    public class ObterLinhasPorParada : BaseService
    {
        public ObterLinhasPorParada(ApplicationContext context) : base(context)
        {
        }

        public async Task<ICollection<Domain.Entities.Linha>> Executar(long id)
        {
            var linhas = await context.Paradas
                .Include(x => x.Linhas)
                .Where(x => x.Id == id)
                .Select(x => x.Linhas)
                .FirstOrDefaultAsync();

            return linhas;
        }

        public async Task<ICollection<Domain.Entities.Linha>> Executar(double latitude, double longitude)
        {
            var linhas = await context.Paradas
                .Include(x => x.Linhas)
                .Where(x => x.Localizacao.Latitude == latitude && x.Localizacao.Longitude == longitude)
                .Select(x => x.Linhas)
                .FirstOrDefaultAsync();

            return linhas;
        }
    }
}