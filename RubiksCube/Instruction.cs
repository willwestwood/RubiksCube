using System;
using System.Text;
using System.Linq;

namespace RubiksCube
{
    public class Instruction
    {
        public enum InstructionType { Rotate, Spin }

        public Instruction(
            InstructionType type,
            Position position,
            Direction direction,
            int numberOfRows,
            int repetitions)
        {
            Type = type;
            Position = position;
            Direction = direction;
            NumberOfRows = numberOfRows;
            Repetitions = repetitions;
        }

        public void Print()
        {
            const char space = ' ';

            StringBuilder output = new StringBuilder();
            output.Append(Type.ToString());
            output.Append(space);

            switch(Type)
            {
                case InstructionType.Rotate:
                    output.Append(Position.ToString().ToLowerInvariant());
                    output.Append(space);
                    output.Append("side");
                    output.Append(space);
                    output.Append(Direction.ToString().ToLowerInvariant());
                    output.Append(",");
                    output.Append(space);
                    output.Append(NumberOfRows.ToString());
                    output.Append(space);
                    output.Append(NumberOfRows > 1 ? "rows" : "row");
                    break;
                case InstructionType.Spin:
                    output.Append("cube");
                    output.Append(space);
                    output.Append(Position.ToString().ToLowerInvariant());
                    break;
            }

            Console.WriteLine(output.ToString());
        }

        InstructionType Type { get; }
        Position Position { get; }
        Direction Direction { get; }
        int NumberOfRows { get; }
        int Repetitions { get; }
    }
}
