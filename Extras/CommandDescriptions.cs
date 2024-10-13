using System.Collections.Generic;

public class CommandDescriptions
{
    // Dictionary to store command names and their respective descriptions.
    private readonly Dictionary<string, string> commandDescriptions = new Dictionary<string, string>
{
    { "data", "Sintaxe: data [data]\nDescrição: Exibe ou define a data do sistema." },
    { "cls", "Sintaxe: cls\nDescrição: Limpa a tela de saída de comandos." },
    { "cd", "Sintaxe: cd [diretório]\nDescrição: Muda para o diretório especificado. Se nenhum diretório for especificado, muda para a raiz (C:\\)." },
    { "ls", "Sintaxe: ls [-o]\nDescrição: Lista arquivos e diretórios no diretório atual. Use -o para mostrar arquivos e diretórios ocultos." },
    { "mkdir", "Sintaxe: mkdir [diretório]\nDescrição: Cria um novo diretório com o nome especificado." },
    { "rmdir", "Sintaxe: rmdir [diretório]\nDescrição: Remove o diretório especificado se ele estiver vazio." },
    { "echo", "Sintaxe: echo [texto]\nDescrição: Exibe o texto fornecido na tela." },
    { "copy", "Sintaxe: copy [origem] [destino]\nDescrição: Copia um arquivo do local de origem para o destino." },
    { "move", "Sintaxe: move [origem] [destino]\nDescrição: Move um arquivo do local de origem para o destino." },
    { "del", "Sintaxe: del [arquivo] ou del -temp\nDescrição: Deleta o arquivo especificado." },
    { "ren", "Sintaxe: ren [arquivo] [novo_nome]\nDescrição: Renomeia o arquivo especificado." },
    { "ver", "Sintaxe: ver [arquivo]\nDescrição: Exibe o conteúdo de um arquivo." },
    { "find", "Sintaxe: find [texto] [arquivo]\nDescrição: Procura por um texto específico dentro de um arquivo." },
    { "tree", "Sintaxe: tree\nDescrição: Exibe a estrutura de diretórios em forma de árvore." },
    { "attr", "Sintaxe: attr [+|-atributo] [arquivo]\nDescrição: Modifica os atributos de um arquivo." },
    { "ping", "Sintaxe: ping [opções] [alvo]\nDescrição: Envia pacotes ICMP Echo Request para o destino especificado e exibe as respostas recebidas..." },
    { "ipconfig", "Sintaxe: ipconfig\nDescrição: Mostra as Interfaces de Rede." },
    { "lprocessos", "Sintaxe: lprocessos\nDescrição: Lista os Processos em Execução." },
    { "matar", "Sintaxe: matar <nome_do_processo | PID>\nDescrição: Fecha um Programa que esteja em Execução."},
    { "quem", "Sintaxe: quem\nDescrição: Exibe o nome do usuário atual." },
    { "recente", "Sintaxe: recente\nDescrição: Exibe o histórico de comandos digitados." },
    { "open", "Sintaxe: open .\nDescrição: Abre o diretório atual no Windows Explorer." },
    { "help", "Sintaxe: help\nDescrição: Lista todos os comandos disponíveis com suas respectivas sintaxes." },
    { "sair", "Sintaxe: sair\nDescrição: Fecha o aplicativo." }
};

    public string GetDescription(string command)
    {
        // Log message indicating the help description was loaded successfully.
        Logger.Log("Descrição da ajuda carregada com sucesso.");

        // If the command exists in the dictionary, return its description, otherwise return a default message.
        return commandDescriptions.TryGetValue(command, out var description) ? description : "Description not found.";
    }

    public IEnumerable<string> GetAllCommands()
    {
        // Return all the keys (command names) from the dictionary.
        return commandDescriptions.Keys;
    }
}