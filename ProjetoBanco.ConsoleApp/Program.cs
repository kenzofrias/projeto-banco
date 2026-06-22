using ProjetoBanco.Core.Models;
using ProjetoBanco.Infrastructure.Data;
using ProjetoBanco.Infrastructure.Repositories;

// Console.WriteLine("--- Conectando ao Banco de Dados ---");
using var dbContext = new BancoDBContext();
var repositorio = new ContaRepositorio(dbContext);

// Função para buscar a conta no banco ou criá-la se não existir
async Task<Conta> ObterOuCriarContaAsync(Conta novaConta)
{
    try
    {
        return await repositorio.ObterContaPorNumeroAsync(novaConta.Numero);
    }
    catch (KeyNotFoundException)
    {
        await repositorio.AdicionarContaAsync(novaConta);
        return novaConta;
    }
}

//Contas Corrente
Conta c1 = await ObterOuCriarContaAsync(new ContaCorrente("12345", "João C O Silva", 1200m, 500m));
Conta c2 = await ObterOuCriarContaAsync(new ContaCorrente("54321", "Maria S O Silva", 1400m, 750m));

// Contas Poupança
Conta c3 = await ObterOuCriarContaAsync(new ContaPoupanca("67890", "Carlos A O Silva", 2000m, 5m));
Conta c4 = await ObterOuCriarContaAsync(new ContaPoupanca("09876", "Ana C O Silva", 1500m, 3m));

c1.ExibirExtrato();
c2.ExibirExtrato();
c3.ExibirExtrato();
c4.ExibirExtrato();

// c1.Depositar(1000m);
// c2.Depositar(1000m);
// c3.Depositar(1000m);
// c4.Depositar(1000m);

// c1.Transferir(c3, 500m);
// c2.Transferir(c4, 500m);

// await repositorio.AtualizarContaAsync(c1);
// await repositorio.AtualizarContaAsync(c2);
// await repositorio.AtualizarContaAsync(c3);
// await repositorio.AtualizarContaAsync(c4);

// Conta c1 = await ObterOuCriarContaAsync(new ContaCorrente("24680", "João S Silva", 1450m, 500m));
// // c1.Depositar(1221m); // 2671
// // c1.Sacar(1354.22m); // 1316.78
// // c1.Transferir(c2, 1550m); // - 233.22 e 1250
// c1.CalcularTarifaMensal(); // -253.22
// await repositorio.AtualizarContaAsync(c1);
// await repositorio.AtualizarContaAsync(c2);
// Console.WriteLine("--- Finalizando a Aplicação ---");



// // Mostrando titulares e saldos
// Console.WriteLine("Titulares e Saldos Iniciais:");
// Console.WriteLine(c1.ToString());
// Console.WriteLine(c2.ToString());
// Console.WriteLine(c3.ToString());
// Console.WriteLine(c4.ToString());

// // Realizando operações
// c1.Depositar(100m); // 1300
// c2.Depositar(200m); // 1600
// c3.Depositar(300m); // 2300
// c4.Depositar(400m); // 1900

// c1.Sacar(400m); // 900
// c2.Sacar(300m); // 1300
// c3.Sacar(200m); // 2100
// c4.Sacar(100m); // 1800

// c1.Transferir(c3, 200m); // C1 transfere 200 reais para C3 - C1: 700, C3: 2300
// c4.Transferir(c2, 400m); // C4 transfere 400 reais para C2 - C4: 1400, C2: 1700

// // Mostrando titulares e saldos pós-operações
// Console.WriteLine("\nTitulares e Saldos Pós-Operações:");
// Console.WriteLine(c1.ToString());
// Console.WriteLine(c2.ToString());
// Console.WriteLine(c3.ToString());
// Console.WriteLine(c4.ToString());

// // Aplicando cheque especial
// c1.Sacar(1000m); // C1 tenta sacar 1000 - C1: 700 - 1000 = -300 (dentro do limite de cheque especial: 500)
// c2.Sacar(2000m); // C2 tenta sacar 2000 - C2: 1700 - 2000 = -300 (dentro do limite de cheque especial: 750)

// // Mostrando titulares e saldos pós-cheque especial (Conta Corrente)
// Console.WriteLine("\nTitulares e Saldos Pós-Cheque Especial(Conta Corrente):");
// Console.WriteLine(c1.ToString());
// Console.WriteLine(c2.ToString());

// // Aplicando além do limite de cheque especial
// // c1.Sacar(300m); // Retorna exceção de saldo insuficiente
// // c2.Sacar(500m); // Retorna exceção de saldo insuficiente

// // Aplicando rendimento após três meses
// int contador = 1;
// while (contador <= 3)
// {
//     c3.AplicarRendimento(); // Aplica rendimento mensal de 5% para C3
//     c4.AplicarRendimento(); // Aplica rendimento mensal de 3% para C4
//     contador++;
// }

// // Mostrando titulares e saldos pós-rendimento (Conta Poupança)
// Console.WriteLine("\nTitulares e Saldos Pós-Rendimento (Conta Poupança):");
// Console.WriteLine(c3.ToString());
// Console.WriteLine(c4.ToString());

// // Listando todas as contas
// List<Conta> contas = new List<Conta> { c1, c2, c3, c4 };
// Console.WriteLine("\nListagem de Contas:");
// foreach (var conta in contas)
// {
//     Console.WriteLine(conta.ToString());
// }

// // Exibindo extratos individuais
// c1.ExibirExtrato();

// c2.ExibirExtrato();

// c3.ExibirExtrato();

// c4.ExibirExtrato();

// Console.WriteLine("\n");

// // Atualizando os dados modificados no banco de dados
// Console.WriteLine("--- Salvando alterações no Banco de Dados ---");
// await repositorio.AtualizarContaAsync(c1);
// await repositorio.AtualizarContaAsync(c2);
// await repositorio.AtualizarContaAsync(c3);
// await repositorio.AtualizarContaAsync(c4);
// Console.WriteLine("Alterações salvas com sucesso!");