using rharel.Functional;
using rharel.M3PD.Common.Hashing;
using System;

namespace rharel.M3PD.Agency.Dialogue_Moves
{
    /// <summary>
    /// Represents a communicated meaning.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Dialogue moves are a way to refer to the act of communicating meanings 
    /// through behavior, independent of the concrete form(s) such behavior may 
    /// take.
    /// </para>
    /// <para>
    /// To illustrate: both a hand-wave as well as a verbal "hello" may be
    /// regarded as concrete forms of the same move: a greeting. The level of
    /// abstraction desired is dependent on the use-case.
    /// </para>
    /// <para>
    /// <see cref="DialogueMove{T}"/> for the generic version.
    /// </para>
    /// </remarks>
    public interface DialogueMove
    {
        /// <summary>
        /// Gets this move's type identifier.
        /// </summary>
        string Type { get; }
        /// <summary>
        /// Gets this move's properties.
        /// </summary>
        Optional<object> Properties { get; }
    }
    /// <summary>
    /// Represents a communicated meaning.
    /// </summary>
    /// <typeparam name="T">The type of the move's properties.</typeparam>
    /// <remarks>
    /// <see cref="DialogueMove"/> for the non-generic version and additional
    /// details.
    /// </remarks>
    public struct DialogueMove<T>: DialogueMove
    {
        /// <summary>
        /// Creates a new move of the specified type.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="type"/> is blank.
        /// </exception>
        public DialogueMove(string type): this(type, new None<T>()) { }
        /// <summary>
        /// Creates a new move of the specified type and properties.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <param name="properties">The move's properties.</param>
        /// <exception cref="ArgumentNullException">
        /// When either <paramref name="type"/> or 
        /// <paramref name="properties"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="type"/> is blank.
        /// </exception>
        public DialogueMove(string type, T properties): 
            this(type, new Some<T>(properties)) { }
        /// <summary>
        /// Creates a new move of the specified type and properties.
        /// </summary>
        /// <param name="type">The move's type identifier.</param>
        /// <param name="properties">The move's properties.</param>
        /// <exception cref="ArgumentNullException">
        /// When either <paramref name="type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="type"/> is blank.
        /// </exception>
        private DialogueMove(string type, Optional<T> properties)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (type.Trim().Length == 0)
            {
                throw new ArgumentException(nameof(type));
            }
            Type = type;
            Properties = properties;
        }

        /// <summary>
        /// Gets this move's type identifier.
        /// </summary>
        public string Type { get; }
        
        /// <summary>
        /// Gets this move's properties.
        /// </summary>
        /// <remarks>
        /// <see cref="DialogueMove.Properties"/> for the non-generic version.
        /// </remarks>
        public Optional<T> Properties { get; }
        /// <summary>
        /// Gets this move's properties.
        /// </summary>
        /// <remarks>
        /// <see cref="Properties"/> for the generic version.
        /// </remarks>
        Optional<object> DialogueMove.Properties => Properties.Cast<object>();

        /// <summary>
        /// Casts the properties of this and wraps it in a new move of the same
        /// type.
        /// </summary>
        /// <typeparam name="TResult">The type to cast to.</typeparam>
        /// <returns>
        /// A new move containing the cast properties of this one.
        /// </returns>
        public DialogueMove<TResult> Cast<TResult>()
        {
            return new DialogueMove<TResult>(Type, Properties.Cast<TResult>());
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>
        /// True iff the specified object is equal to this instance. 
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is DialogueMove other)
            {
                return other.Type.Equals(Type) &&
                       other.Properties.Equals(Properties);
            }
            return false;
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing 
        /// algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = HashCombiner.Initialize();
            hash = HashCombiner.Hash(hash, Type);
            hash = HashCombiner.Hash(hash, Properties);
            
            return hash;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            return $"{nameof(DialogueMove<T>)}{{ " +
                   $"{nameof(Type)} = '{Type}', " +
                   $"{nameof(Properties)} = {Properties} }}";
        }
    }
}
