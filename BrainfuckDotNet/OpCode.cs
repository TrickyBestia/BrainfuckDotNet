namespace BrainfuckDotNet
{
    public enum OpCode
    {
        /// <summary>
        /// Switch to next memory cell.
        /// </summary>
        Next,
        /// <summary>
        /// Switch to previous memory cell.
        /// </summary>
        Prev,
        /// <summary>
        /// Increment current memory cell.
        /// </summary>
        Incr,
        /// <summary>
        /// Decrement current memory cell.
        /// </summary>
        Decr,
        /// <summary>
        /// Write to console current memory cell.
        /// </summary>
        Prnt,
        /// <summary>
        /// Read symbol from console to current memory cell.
        /// </summary>
        Read,
        /// <summary>
        /// [
        /// </summary>
        Whl_B,
        /// <summary>
        /// ]
        /// </summary>
        Whl_E,
    }
}
