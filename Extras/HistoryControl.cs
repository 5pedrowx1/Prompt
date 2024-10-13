using System.Collections.Generic;

public class HistoryControl
{
    private readonly List<string> commandHistory;
    private int currentIndex;

    public HistoryControl()
    {
        commandHistory = new List<string>();
        currentIndex = -1;
    }

    // Adds a new command to the history, ensuring that duplicate consecutive commands are not added.
    public void AddCommand(string command)
    {
        // If the history is empty or the last command in the history is not the same as the new one, add it.
        if (commandHistory.Count == 0 || commandHistory[commandHistory.Count - 1] != command)
        {
            commandHistory.Add(command);
            // Update the current index to the end of the history list.
            currentIndex = commandHistory.Count;
        }
    }

    public bool HasPreviousCommand(int index)
    {
        // Returns true if the index is within the range of the history list.
        return index >= 0 && index < commandHistory.Count;
    }

    public string GetCommandFromHistory(int index)
    {
        // If the index is valid, return the command at that index.
        if (HasPreviousCommand(index))
        {
            return commandHistory[index];
        }
        return string.Empty;
    }

    public string GetPreviousCommand()
    {
        // If there is a previous command in the history, move the current index back and return the command.
        if (currentIndex > 0)
        {
            currentIndex--;
            return commandHistory[currentIndex];
        }
        return string.Empty;
    }

    public string GetNextCommand()
    {
        // If there is a next command in the history, move the current index forward and return the command.
        if (currentIndex < commandHistory.Count - 1)
        {
            currentIndex++;
            return commandHistory[currentIndex];
        }       
        return string.Empty;
    }

    public void ClearHistory()
    {
        commandHistory.Clear();
        currentIndex = -1;
    }

    // Returns a read-only view of  callommands in the history.
    public IEnumerable<string> GetAllCommands()
    {
        return commandHistory.AsReadOnly();
    }

    //Gets the total number of commands in the history.
    public int CommandCount => commandHistory.Count;
}