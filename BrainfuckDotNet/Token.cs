namespace BrainfuckDotNet
{
    public struct Token
    {
        public OpCode OpCode { get; }
        public object Value { get; }

        public Token(OpCode opCode, object value = null)
        {
            OpCode = opCode;
            Value = value;
        }
    }
}
