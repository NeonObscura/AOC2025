namespace puzzle1;

public enum Direction
{
    Left = -1,
    Right = 1
}

public record Instruction(Direction Direction, int Steps);

class Program
{
    const int FilePathIndex = 0;
    const int ArgumentsCount = 1;
    const int DirectionIndex = 0;
    const int StepsStartIndex = 1;
    const int MinInstructionLength = 2;
    const int InitialDialPosition = 50;
    const int DalSteps = 100;
    const int DialPasswordTargetPosition = 0;
    
    static void Main(string[] args)
    {
        if(args.Length != ArgumentsCount)
        {
            PrintHelp();
            return;
        }
        
        var lines = ReadLinesFromFile(args[FilePathIndex]);
        var instructions = ParseInstructions(lines);

        var password = 0;
        var currentDialPosition = InitialDialPosition;
        foreach (var instruction in instructions)
        {
            currentDialPosition = (currentDialPosition + ((int)instruction.Direction * instruction.Steps)) % DalSteps;
            if (currentDialPosition == DialPasswordTargetPosition)
                password++;
        }

        Console.WriteLine($"Password: {password}");
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Usage: puzzle1-1 <input_file_path>");
    }

    static string[] ReadLinesFromFile(string path)
    {
        if(!Path.Exists(path))
        {
            throw new FileNotFoundException($"The file at path {path} was not found.");
        }
        
        return File.ReadAllLines(path);
    }

    static IEnumerable<Instruction> ParseInstructions(string[] rawInstructions)
    {
        foreach (var rawInstruction in rawInstructions)
        {
            if (string.IsNullOrEmpty(rawInstruction))
            {
                continue;
            }

            var cleanedInstruction = rawInstruction.Trim();
            
            if (cleanedInstruction.Length < MinInstructionLength)
            {
                throw new ArgumentException("Bad instruction format: instruction is empty or whitespace.");
            }

            var direction = ParseDirection(cleanedInstruction[DirectionIndex]);
            var steps = ParseStepsCount(cleanedInstruction);
            
            yield return new Instruction(direction, steps);
        }
    }

    static Direction ParseDirection(char directionChar)
    {
        if(char.ToLower(directionChar) == 'l')
        {
            return Direction.Left;
        }
        
        if(char.ToLower(directionChar) == 'r')
        {
            return Direction.Right;
        }
        
        throw new ArgumentException($"Bad instruction format: unknown direction character '{directionChar}'.");
    }

    static int ParseStepsCount(string rawInstruction)
    {
        if (int.TryParse(rawInstruction.AsSpan(StepsStartIndex), out var steps))
        {
            return steps;
        }
        
        throw new ArgumentException("Bad instruction format: steps is not a valid integer.");
    }
}