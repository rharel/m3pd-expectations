using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;

namespace rharel.M3PD.Expectations.Arrangement
{
    /// <summary>
    /// Helps make event composition easier via an interface that is closer to 
    /// natural language descriptions of expectation arrangements.
    /// </summary>
    /// <example>
    /// <code>
    /// using static rharel.Expectations.Program.DialogueEventNLI;
    /// 
    /// struct Greeting { Greeting(addressee) { ... } }
    /// ...
    /// Event("greeting", 
    ///     source: "Alice", 
    ///     properties: new Greeting(addressee: "Bob")
    /// );
    /// </code>
    /// </example>
    /// <remarks>
    /// NLI stands for Natural Language Interface.
    /// </remarks>
    public static class DialogueEventNLI
    {
        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="type">The realized move's type.</param>
        /// <param name="source">The move's source identifier.</param>
        /// <returns>A new dialogue event.</returns>
        public static DialogueEvent Event(
            string type, string source)
        {
            return new DialogueEvent(
                source,
                DialogueMoves.Create(type)
            );
        }
        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <typeparam name="T">The type of the move's properties.</typeparam>
        /// <param name="type">The realized move's type.</param>
        /// <param name="source">The move's source identifier.</param>
        /// <param name="properties">The move's properties.</param>
        /// <returns>A new dialogue event.</returns>
        public static DialogueEvent Event<T>(
            string type, string source, T properties)
        {
            return new DialogueEvent(
                source,
                DialogueMoves.Create(type, properties)
            );
        }
    }
}
