using System.Text.Json;

namespace TrabalhoAv2
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            MenuPrincipal();
        }

        static void MenuPrincipal()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== MENU PRINCIPAL ====");
                Console.WriteLine("1. Problema × Instância (JSON)");
                Console.WriteLine("2. Decisores (L_fim_b e L_mult3_b)");
                Console.WriteLine("3. Reconhecedor (não terminante)");
                Console.WriteLine("4. Detector de Loop + Reflexão");
                Console.WriteLine("5. Simulador de AFD");
                Console.WriteLine("0. Sair");
                Console.Write("\nEscolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1": Item1_ProblemaInstancia(); break;
                    case "2": Item2_Decisores(); break;
                    case "3": Item3_Reconhecedor(); break;
                    case "4": Item4_DetectorLoop(); break;
                    case "5": Item5_SimuladorAFD(); break;
                    case "0": return;
                    default: Console.WriteLine("Opção inválida."); break;
                }

                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
            }
        }

        static void Item1_ProblemaInstancia()
        {
            Console.Clear();
            Console.WriteLine("=== ITEM 1 — Problema × Instância ===\n");

            string json = """
            {
              "frases": [
                "Determinar se um número é primo.",
                "O número 13 é primo?",
                "Verificar se uma palavra termina em 'b'.",
                "A palavra 'aaab' termina com 'b'?"
              ]
            }
            """;

            var data = JsonSerializer.Deserialize<JsonData>(json);
            int acertos = 0;

            foreach (var frase in data!.frases)
            {
                Console.WriteLine($"Frase: {frase}");
                Console.Write("Digite (P/I): ");
                var resposta = Console.ReadLine()?.Trim().ToUpper();

                bool correta = (frase.Contains("?")) ? resposta == "I" : resposta == "P";

                if (correta)
                {
                    Console.WriteLine("✅ Correto!");
                    acertos++;
                }
                else
                {
                    Console.WriteLine("❌ Errado!");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Total de acertos: {acertos}/{data.frases.Count}");
        }

        class JsonData
        {
            public List<string> frases { get; set; } = new();
        }

        static void Item2_Decisores()
        {
            Console.Clear();
            Console.WriteLine("=== ITEM 2 — Decisores ===\n");
            Console.WriteLine("1. L_fim_b → Palavras que terminam em 'b'");
            Console.WriteLine("2. L_mult3_b → Quantidade de 'b' múltipla de 3");
            Console.Write("\nEscolha o decisor: ");

            var op = Console.ReadLine();

            Console.Write("Digite a palavra (Σ={a,b}): ");
            var w = Console.ReadLine() ?? "";

            bool resultado = op switch
            {
                "1" => w.EndsWith('b'),
                "2" => (w.Count(c => c == 'b') % 3 == 0),
                _ => false
            };

            Console.WriteLine(resultado ? "\n✅ Aceita." : "\n❌ Rejeitada.");
        }

        static void Item3_Reconhecedor()
        {
            Console.Clear();
            Console.WriteLine("=== ITEM 3 — Reconhecedor Não Terminante ===\n");
            Console.WriteLine("Linguagem: { a^n b^n | n ≥ 0 }");
            Console.Write("Digite a palavra (Σ={a,b}): ");
            string? w = Console.ReadLine();

            Console.Write("Limite de passos (ex: 20): ");
            int limite = int.Parse(Console.ReadLine() ?? "20");

            int passos = 0;
            int i = 0;

            while (true)
            {
                passos++;
                if (passos > limite)
                {
                    Console.WriteLine("\n⏹ Execução interrompida (limite atingido).");
                    return;
                }

                if (i < w!.Length && w[i] == 'a') i++;
                else break;
            }

            Console.WriteLine("\nReconhecimento finalizado.");
        }

        static void Item4_DetectorLoop()
        {
            Console.Clear();
            Console.WriteLine("=== ITEM 4 — Detector de Loop ===\n");

            Console.Write("Limite de passos: ");
            int limite = int.Parse(Console.ReadLine() ?? "20");

            var estados = new HashSet<int>();
            int estado = 0;
            Random rnd = new();

            for (int passo = 0; passo < limite; passo++)
            {
                if (estados.Contains(estado))
                {
                    Console.WriteLine($"⚠️ Possível laço detectado no estado {estado} (passo {passo}).");
                    break;
                }

                estados.Add(estado);
                estado = rnd.Next(0, 5);
            }

            Console.WriteLine("\nReflexão: o método pode gerar falsos positivos e negativos.");
        }

        static void Item5_SimuladorAFD()
        {
            Console.Clear();
            Console.WriteLine("=== ITEM 5 — Simulador de AFD ===\n");

            var afd = new AFD
            {
                Estados = new() { "q0", "q1" },
                Alfabeto = new() { 'a', 'b' },
                Transicoes = new()
                {
                    { ("q0", 'a'), "q1" },
                    { ("q1", 'b'), "q0" }
                },
                EstadoInicial = "q0",
                EstadosFinais = new() { "q0" }
            };

            Console.Write("Digite a palavra (Σ={a,b}): ");
            string entrada = Console.ReadLine() ?? "";

            string estadoAtual = afd.EstadoInicial;

            foreach (char simbolo in entrada)
            {
                Console.WriteLine($"Estado atual: {estadoAtual}, lendo '{simbolo}'");

                if (!afd.Transicoes.ContainsKey((estadoAtual, simbolo)))
                {
                    Console.WriteLine("❌ Transição inexistente — rejeitado.");
                    return;
                }

                estadoAtual = afd.Transicoes[(estadoAtual, simbolo)];
            }

            if (afd.EstadosFinais.Contains(estadoAtual))
                Console.WriteLine("✅ Palavra aceita!");
            else
                Console.WriteLine("❌ Palavra rejeitada!");
        }

        class AFD
        {
            public List<string> Estados { get; set; } = new();
            public List<char> Alfabeto { get; set; } = new();
            public Dictionary<(string, char), string> Transicoes { get; set; } = new();
            public string EstadoInicial { get; set; } = "";
            public List<string> EstadosFinais { get; set; } = new();
        }
    }
}
