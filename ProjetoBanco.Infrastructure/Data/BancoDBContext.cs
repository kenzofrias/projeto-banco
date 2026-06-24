using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Core.Models;

namespace ProjetoBanco.Infrastructure.Data
{
    public class BancoDBContext : DbContext
    {
        public DbSet<Conta> Contas { get; set; }
        public DbSet<ContaCorrente> ContasCorrentes { get; set; }
        public DbSet<ContaPoupanca> ContasPoupancas { get; set; }
        public DbSet<HistoricoResposta> Historicos { get; set; }

        // Construtor para Injeção de Dependência (usado pela Web API e testes)
        public BancoDBContext(DbContextOptions<BancoDBContext> options) : base(options)
        {
        }

        // Construtor padrão para uso no ConsoleApp e migrations via 'dotnet ef'
        public BancoDBContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("A variável de ambiente 'CONNECTION_STRING' não está definida.");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Conta>().HasKey(c => c.Numero);

            modelBuilder.Entity<Conta>().Property(c => c.Numero).HasMaxLength(20);
            modelBuilder.Entity<Conta>().Property(c => c.Titular).HasMaxLength(100).IsRequired();

            modelBuilder.Entity<Conta>().Property(c => c.Saldo).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ContaCorrente>().Property(c => c.LimiteChequeEspecial).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ContaCorrente>().Property(c => c.TaxaManutencao).HasColumnType("decimal(18,2)");
            
            modelBuilder.Entity<ContaPoupanca>().Property(c => c.TaxaRendimento).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<HistoricoResposta>().HasKey(h => h.Id);
            modelBuilder.Entity<HistoricoResposta>().Property(h => h.Valor).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<HistoricoResposta>().Property(h => h.SaldoAnterior).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<HistoricoResposta>().Property(h => h.SaldoAtual).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Conta>()
                .HasMany(c => c.Historico)
                .WithOne()
                .HasForeignKey(h => h.NumeroConta)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Conta>()
                .HasDiscriminator<string>("TipoConta")
                .HasValue<ContaCorrente>("ContaCorrente")
                .HasValue<ContaPoupanca>("ContaPoupanca");
        }
    }
}