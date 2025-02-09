﻿using FinanceManagement.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceManagement.Models.ViewModels;

namespace FinanceManagement.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<CategoriaLancamento>()
                .HasOne(l => l.Lancamento)
                .WithMany(cl => cl.CategoriaLancamentos)
                .HasForeignKey(l => l.LancamentoId);
            modelBuilder.Entity<CategoriaLancamento>()
                .HasOne(c => c.Categoria)
                .WithMany(cl => cl.CategoriaLancamentos)
                .HasForeignKey(c => c.CategoriaId);

            modelBuilder.Entity<ContaLancamento>()
                .HasOne(l => l.Lancamento)
                .WithMany(cl => cl.ContaLancamentos)
                .HasForeignKey(l => l.LancamentoId);
            modelBuilder.Entity<ContaLancamento>()
                .HasOne(c => c.Conta)
                .WithMany(cl => cl.ContaLancamentos)
                .HasForeignKey(c => c.ContaId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Banco> Bancos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Conta> Contas { get; set; }

        public DbSet<Fixo> Fixos { get; set; }

        public DbSet<Lancamento> Lancamentos { get; set; }

        public DbSet<Parcelado> Parcelados { get; set; }

        public DbSet<Periodo> Periodos { get; set; }

        public DbSet<TipoConta> TipoContas { get; set; }

        public DbSet<ApplicationUser> Usuarios { get; set; }

        public DbSet<CategoriaLancamento> CategoriaLancamentos { get; set; }

        public DbSet<ContaLancamento> ContaLancamentos { get; set; }
    }
}
