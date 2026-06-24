﻿using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Core.Models;
using ProjetoBanco.Infrastructure.Data;
using ProjetoBanco.Infrastructure.Repositories;

Console.WriteLine("--- Iniciando Sistema Bancário ---");

// Aplicação de migrations
Console.WriteLine("Verificando e aplicando migrations do banco de dados...");
using (var dbContextMigration = new BancoDBContext())
{
    dbContextMigration.Database.Migrate();
}
Console.WriteLine("Migrations aplicadas com sucesso.");

using var dbContext = new BancoDBContext();
var repositorio = new ContaRepositorio(dbContext);

async Task<Conta> ObterOuCriarContaAsync(Conta novaConta)
{
    var contaExistente = await repositorio.ObterContaPorNumeroAsync(novaConta.Numero);
    if (contaExistente == null)
    {
        await repositorio.AdicionarContaAsync(novaConta);
        return novaConta;
    }
    return contaExistente;
}

Console.WriteLine("\n--- Criando ou Carregando Contas ---");
Conta c1 = await ObterOuCriarContaAsync(new ContaCorrente("12345", "João C O Silva", 1200m, 500m));
Conta c2 = await ObterOuCriarContaAsync(new ContaCorrente("54321", "Maria S O Silva", 1400m, 750m));
Conta c3 = await ObterOuCriarContaAsync(new ContaPoupanca("67890", "Carlos A O Silva", 2000m, 5m));
Conta c4 = await ObterOuCriarContaAsync(new ContaPoupanca("09876", "Ana C O Silva", 1500m, 3m));
Console.WriteLine("Contas carregadas com sucesso.");

Console.WriteLine("\n--- Realizando Operações ---");
c1.Depositar(100m);
c2.Sacar(200m);
c1.Transferir(c3, 50m);
c4.Transferir(c1, 100m);

await repositorio.SalvarAlteracoesAsync();
Console.WriteLine("Operações salvas no banco de dados.");

c1.ExibirExtrato();
c2.ExibirExtrato();
c3.ExibirExtrato();
c4.ExibirExtrato();

Console.WriteLine("\n--- Finalizando a Aplicação ---");