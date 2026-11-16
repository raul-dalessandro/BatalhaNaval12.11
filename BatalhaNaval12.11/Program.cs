char[,] jogador = new char[8, 8];
char[,] maquina = new char[8, 8];

char[,] tirosJogador = new char[8, 8];
char[,] tirosMaquina = new char[8, 8];

int[] tamanhosNavios = { 1, 1, 2, 2, 3, 3 };

int acertosJogador = 0;
int acertosMaquina = 0;

void PreencherTabuleiro(char[,] tabuleiro)
{
    for (int i = 0; i < 8; i++)
    {
        for (int j = 0; j < 8; j++)
        {
            tabuleiro[i, j] = '~';
        }
    }
}

void PosicionarNavios(char[,] tabuleiro)
{
    foreach (int tamanho in tamanhosNavios)
    {
        bool posicaoValida = false;

        while (!posicaoValida)
        {
            int linha = Random.Shared.Next(0, 8);
            int coluna = Random.Shared.Next(0, 8);
            int direcao = Random.Shared.Next(0, 2);//0 horizontal, 1 vertical

            posicaoValida = true;

            if (direcao == 0)
            {
                if (coluna + tamanho > 8)
                    posicaoValida = false;
                else
                {
                    for (int i = 0; i < tamanho; i++)
                    {
                        if (tabuleiro[linha, coluna + i] == 'N')
                            posicaoValida = false;
                    }
                }
            }
            else
            {
                if (linha + tamanho > 8)
                    posicaoValida = false;
                else
                {
                    for (int i = 0; i < tamanho; i++)
                    {
                        if (tabuleiro[linha + i, coluna] == 'N')
                            posicaoValida = false;
                    }
                }
            }

            if (posicaoValida)
            {
                for (int i = 0; i < tamanho; i++)
                {
                    if (direcao == 0)
                        tabuleiro[linha, coluna + i] = 'N';
                    else
                        tabuleiro[linha + i, coluna] = 'N';
                }
            }
        }
    }
}

void ExibirTabuleiroJogador()
{
    Console.WriteLine("\nSEU TABULEIRO");

    Console.Write("  ");
    for (int j = 0; j < 8; j++)
        Console.Write((char)('A' + j) + " "); //usa o codigo do char pra imprimir as letras

    Console.WriteLine();

    for (int i = 0; i < 8; i++)
    {
        Console.Write((i + 1) + " ");
            
        for (int j = 0; j < 8; j++)
        {
            if (tirosMaquina[i, j] != '~')
                Console.Write(tirosMaquina[i, j] + " ");
            else
                Console.Write(jogador[i, j] + " ");
        }

        Console.WriteLine();
    }
}

void ExibirAtaquesJogador()
{
    Console.WriteLine("\nSEUS ATAQUES");

    Console.Write("  ");
    for (int j = 0; j < 8; j++)
        Console.Write((char)('A' + j) + " ");

    Console.WriteLine();

    for (int i = 0; i < 8; i++)
    {
        Console.Write((i + 1) + " ");

        for (int j = 0; j < 8; j++)
            Console.Write(tirosJogador[i, j] + " ");

        Console.WriteLine();
    }
}

void Jogar()
{
    PreencherTabuleiro(jogador);
    PreencherTabuleiro(maquina);
    PreencherTabuleiro(tirosJogador);
    PreencherTabuleiro(tirosMaquina);

    PosicionarNavios(jogador);
    PosicionarNavios(maquina);

    while (acertosJogador < 12 && acertosMaquina < 12)
    {
        bool acertou;

        do
        {
            Console.Clear();

            ExibirTabuleiroJogador();
            ExibirAtaquesJogador();

            Console.Write("\nDigite a letra da coluna (A a H): ");
            char colunaLetra = char.ToUpper(Console.ReadLine()[0]);
            int coluna = colunaLetra - 'A';

            Console.Write("Digite o número da linha (1 a 8): ");
            int linha = int.Parse(Console.ReadLine()) - 1;

            if (linha < 0 || linha >= 8 || coluna < 0 || coluna >= 8)
            {
                Console.WriteLine("Coordenada inválida!");
                Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();

                acertou = false;
                continue;
            }

            if (tirosJogador[linha, coluna] != '~')
            {
                Console.WriteLine("Você já atacou essa posição!");
                Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();

                acertou = false;
                continue;
            }

            if (maquina[linha, coluna] == 'N')
            {
                tirosJogador[linha, coluna] = 'X';
                acertosJogador++;

                Console.WriteLine("Você acertou!");
                acertou = true;
            }
            else
            {
                tirosJogador[linha, coluna] = 'O';

                Console.WriteLine("Você errou!");
                acertou = false;
            }

            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();

        } while (acertou && acertosJogador < 12);

        if (acertosJogador == 12)
            break;

        bool ataquePC;

        do
        {
            Console.Clear();

            

            int linha = Random.Shared.Next(0, 8);
            int coluna = Random.Shared.Next(0, 8);

            while (tirosMaquina[linha, coluna] != '~')
            {
                linha = Random.Shared.Next(0, 8);
                coluna = Random.Shared.Next(0, 8);
            }

            Console.WriteLine("\nA máquina atacou " +
                (char)('A' + coluna) + (linha + 1));

            if (jogador[linha, coluna] == 'N')
            {
                tirosMaquina[linha, coluna] = 'X';
                acertosMaquina++;

                Console.WriteLine("A máquina acertou seu navio!");
                ataquePC = true;
            }
            else
            {
                tirosMaquina[linha, coluna] = 'O';

                Console.WriteLine("A máquina acertou a água!");
                ataquePC = false;
            }

            ExibirTabuleiroJogador();

            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();

        } while (ataquePC && acertosMaquina < 12);
    }

    Console.Clear();

    ExibirTabuleiroJogador();
    ExibirAtaquesJogador();

    if (acertosJogador == 12)
        Console.WriteLine("\nPARABÉNS! VOCÊ VENCEU!");
    else
        Console.WriteLine("\nA MÁQUINA VENCEU!");
}

Jogar();