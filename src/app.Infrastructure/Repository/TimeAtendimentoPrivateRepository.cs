using app.Application.Interfaces;
using app.Application.Log;
using app.Domain.Entities;
using app.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Infrastructure.Repository;

public class TimeAtendimentoPrivateRepository : ITimeAtendimentoPrivateRepository
{
    private readonly TimeAtendimentoContext _context;
    private readonly ILoggerWorker<TimeAtendimentoPrivateRepository> _logger;

    public TimeAtendimentoPrivateRepository(TimeAtendimentoContext context, ILoggerWorker<TimeAtendimentoPrivateRepository> logger = null)
    {
        _context = context;
        _logger = logger;
    }

    public Task<bool> InserirTimeAtendimentoPrivate(TimeAtendimentoPrivate time)
    {
        string[] atendentes = CriarListaDeAtendentes(time);

        using (var trans = _context.Database.BeginTransaction())
        {
            try
            {
                RemoverTimeAtendimentoPrivatePeloId(time);
                InserirTimeAtendimentoPrivate(time, atendentes, trans);                
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                EfetuarRollback(trans, ex);
            }

        }
        return Task.FromResult(false);
    }

    private void EfetuarRollback(IDbContextTransaction trans, Exception ex)
    {
        _logger.LogException("Falha ao adicionar os registros", ex);
        trans.Rollback();
    }

    private static string[] CriarListaDeAtendentes(TimeAtendimentoPrivate time)
    {
        return new[]
                {
            time.TimeAtendimento.Banker,
            time.TimeAtendimento.Especialista,
            time.TimeAtendimento.Gerente
        };
    }

    private void InserirTimeAtendimentoPrivate(TimeAtendimentoPrivate time, string[] atendentes, IDbContextTransaction trans)
    {
        var lista = new List<TimeAtendimentoPrivate_TBL>();

        foreach (var atendente in atendentes)
        {
            var timeAtendimentoPrivate = new TimeAtendimentoPrivate_TBL
            {
                Cliente = time.Cliente,
                Atendente = atendente
            };
            lista.Add(timeAtendimentoPrivate);
        }

        _context.AddRange(lista);
        _context.SaveChanges();
        trans.Commit();
        _logger.LogInfo("Registros adicionados com sucesso");
    }

    private void RemoverTimeAtendimentoPrivatePeloId(TimeAtendimentoPrivate time)
    {
        /*var registrosParaRemover = _context.TimeAtendimentoPrivate_TBL
            .Where(t => t.Cliente == time.Cliente)
            .ToList();

        if(registrosParaRemover.Count > 0)
            _context.TimeAtendimentoPrivate_TBL.RemoveRange(registrosParaRemover);*/
        _context.Database.ExecuteSqlRaw("DELETE FROM tbl_time_atendimento where id_cliente = {0}", time.Cliente);
    }
}
