﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceManagement.Data;
using FinanceManagement.Models;
using System.Security.Claims;

namespace FinanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaLancamentosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContaLancamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUsuarioLogado()
        {
            return this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: api/ContaLancamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContaLancamento>>> GetContaLancamentos()
        {
            return await _context.ContaLancamentos.ToListAsync();
        }

        // GET: api/ContaLancamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContaLancamento>> GetContaLancamento(int id)
        {
            var contaLancamento = await _context.ContaLancamentos.FindAsync(id);

            if (contaLancamento == null)
            {
                return NotFound();
            }

            return contaLancamento;
        }

        // PUT: api/ContaLancamentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContaLancamento(int id, ContaLancamento contaLancamento)
        {
            contaLancamento.Id = id;
            _context.Entry(contaLancamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContaLancamentoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ContaLancamentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ContaLancamento>> PostContaLancamento(ContaLancamento contaLancamento)
        {
            _context.ContaLancamentos.Add(contaLancamento);
            await _context.SaveChangesAsync();

            var conta = await _context.Contas.FindAsync(contaLancamento.ContaId);
            var lancamento = await _context.Lancamentos.FindAsync(contaLancamento.LancamentoId);
            if ((contaLancamento.LancamentoId == lancamento.Id) && (conta.Id == contaLancamento.ContaId))
            {

                var parcelado = await _context.Parcelados.Where(x => x.Id == lancamento.ParceladoId).FirstOrDefaultAsync();
                if (lancamento.DespesaReceita == true)
                {
                    if (lancamento.ParceladoId != null)
                    {
                        var value = (-1) * lancamento.Valor * parcelado.Quantidade;
                        conta.Saldo -= value;
                    }
                    else
                    {
                        var value = (-1) * lancamento.Valor;
                        conta.Saldo -= value;
                    }

                }
                else
                {
                    if (lancamento.ParceladoId != null)
                    {
                        conta.Saldo += lancamento.Valor * parcelado.Quantidade;
                    }
                    else
                    {
                        conta.Saldo += lancamento.Valor;
                    }
                }


                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetContaLancamento", new { id = contaLancamento.Id }, contaLancamento);
        }

        // DELETE: api/ContaLancamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContaLancamento(int id)
        {
            var contaLancamento = await _context.ContaLancamentos.FindAsync(id);
            if (contaLancamento == null)
            {
                return NotFound();
            }

            _context.ContaLancamentos.Remove(contaLancamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContaLancamentoExists(int id)
        {
            return _context.ContaLancamentos.Any(e => e.Id == id);
        }
    }
}
