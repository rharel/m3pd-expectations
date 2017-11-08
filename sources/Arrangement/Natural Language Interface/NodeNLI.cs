using rharel.Debug;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Common.Collections;
using rharel.M3PD.Common.Delegates;
using System;
using System.Collections.Generic;

namespace rharel.M3PD.Expectations.Arrangement
{
    /// <summary>
    /// Helps make program composition easier via an interface that is closer to 
    /// natural language descriptions of expectation arrangements.
    /// </summary>
    /// <example>
    /// <code>
    /// var expect = new NodeNLI();
    /// expect.Sequence("some sequence",
    ///     expect.Event(...),
    ///     expect.Conjunction(...),
    ///     ...
    /// );
    /// </code>
    /// </example>
    /// <remarks>
    /// <para>
    /// If during the creation of a new node no identifier is supplied, one
    /// is generated automatically. Identifiers prefixed with an _underscore
    /// should be considered reserved for this purpose.
    /// </para>
    /// <para>
    /// NLI stands for Natural Language Interface.
    /// </para>
    /// </remarks>
    public sealed class NodeNLI
    {
        /// <summary>
        /// Creates a new program composition interface.
        /// </summary>
        public NodeNLI()
        {
            NodeIDs = new CollectionView<string>(_node_ids);
        }

        /// <summary>
        /// Gets node identifiers for nodes created via this instance.
        /// </summary>
        public ImmutableCollection<string> NodeIDs { get; }

        /// <summary>
        /// Expects a specified event indefinitely.
        /// </summary>
        /// <param name="event">The expected event.</param>
        /// <param name="id">The node's identifier.</param>
        /// <returns>A new expectation node.</returns>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// </exception>
        public IndefiniteEvent IndefiniteEvent(
            DialogueEvent @event, string id = null)
        {
            id = ProcessNewNode(typeof(IndefiniteEvent), id);
            return new IndefiniteEvent(id, @event);
        }

        /// <summary>
        /// Expects to satisfy a specified child only while a specified 
        /// condition holds.
        /// </summary>
        /// <param name="body">The node's only child.</param>
        /// <param name="condition">The condition to apply.</param>
        /// <param name="id">The node's identifier.</param>
        /// <returns>A new expectation node.</returns>
        /// <exception cref="ArgumentNullException">
        /// When either <paramref name="body"/> or 
        /// <paramref name="condition"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// </exception>
        public Conditional Conditional(
            Node body, Predicate condition, string id = null)
        {
            id = ProcessNewNode(typeof(Conditional), id);
            return new Conditional(id, body, condition);
        }

        /// <summary>
        /// Expects satisfaction of all children in no particular order.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="children">The node's children.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="children"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// When <paramref name="children"/> contains less than two members.
        /// </exception>
        /// <returns>A conjunctive expectation.</returns>
        public Conjunction Conjunction(
            string id = null, params Node[] children)
        {
            id = ProcessNewNode(typeof(Conjunction), id);
            return new Conjunction(id, children);
        }

        /// <summary>
        /// Expects satisfaction of at least one child.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="children">The node's children.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="children"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// When <paramref name="children"/> contains less than two members.
        /// </exception>
        /// <returns>A conjunctive expectation.</returns>
        public Disjunction Disjunction(
            string id = null, params Node[] children)
        {
            id = ProcessNewNode(typeof(Disjunction), id);
            return new Disjunction(id, children);
        }

        /// <summary>
        /// Expects to satisfy exactly one child. The child selected to be the 
        /// one satisfied is the first whose scope carrier chain's endpoint is 
        /// partially satisfied.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="children">The node's children.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="children"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// When <paramref name="children"/> contains less than two members.
        /// </exception>
        /// <returns>A conjunctive expectation.</returns>
        public Divergence Divergence(string id = null, params Node[] children)
        {
            id = ProcessNewNode(typeof(Divergence), id);
            return new Divergence(id, children);
        }

        /// <summary>
        /// Expects to repeatedly satisfy a specified child.
        /// </summary>
        /// <param name="body">The node's only child.</param>
        /// <param name="id">The node's identifier.</param>
        /// <returns>A new expectation node.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// </exception>
        public Repeat Repeat(Node body, string id = null)
        {
            id = ProcessNewNode(typeof(Repeat), id);
            return new Repeat(id, body);
        }

        /// <summary>
        /// Expects satisfaction of all children in sequence.
        /// </summary>
        /// <param name="id">The node's identifier.</param>
        /// <param name="children">The node's children.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="children"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// When <paramref name="children"/> contains less than two members.
        /// </exception>
        /// <returns>A conjunctive expectation.</returns>
        public Sequence Sequence(string id = null, params Node[] children)
        {
            id = ProcessNewNode(typeof(Sequence), id);
            return new Sequence(id, children);
        }

        /// <summary>
        /// Expects a specified event indefinitely.
        /// </summary>
        /// <param name="event">The expected event.</param>
        /// <param name="id">The node's identifier.</param>
        /// <returns>A new expectation node.</returns>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// </exception>
        /// <remarks>
        /// This is an alias for 
        /// <see cref="IndefiniteEvent(DialogueEvent, string)"/>.
        /// </remarks>
        public IndefiniteEvent Event(DialogueEvent @event, string id = null)
        {
            return IndefiniteEvent(@event, id);
        }

        /// <summary>
        /// Validates the specified node identifier.
        /// </summary>
        /// <param name="id">The identifier to process.</param>
        /// <param name="node_type">The node's type.</param>
        /// <returns>
        /// If <paramref name="id"/> is null, a new auto-generated identifier,
        /// otherwise <paramref name="id"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="node_type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="id"/> is blank or already taken.
        /// When <paramref name="node_type"/> is not recognized.
        /// </exception>
        private string ProcessNewNode(Type node_type, string id)
        {
            if (id == null)
            {
                id = GenerateID(node_type);
            }
            else if (id.Trim().Length == 0 || _node_ids.Contains(id))
            {
                throw new ArgumentException(nameof(id));
            }

            _node_ids.Add(id);
            _node_count_by_type[node_type] += 1;

            return id;
        }
        /// <summary>
        /// Generates an identifier for a node of the specified type.
        /// </summary>
        /// <param name="node_type">The node's type.</param>
        /// <returns>A unique identifier.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="node_type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="node_type"/> is not recognized.
        /// </exception>
        private string GenerateID(Type node_type)
        {
            Require.IsNotNull(node_type);
            Require.IsTrue(_node_count_by_type.ContainsKey(node_type));

            return $"_{node_type}_#{_node_count_by_type[node_type]}";
        }

        private readonly HashSet<string> _node_ids = new HashSet<string>();
        private readonly Dictionary<Type, int> _node_count_by_type = (
            new Dictionary<Type, int>
            {
                // Atomics
                { typeof(IndefiniteEvent), 0 },

                // Composites
                { typeof(Conditional), 0 },
                { typeof(Conjunction), 0 },
                { typeof(Disjunction), 0 },
                { typeof(Divergence), 0 },
                { typeof(Repeat), 0 },
                { typeof(Sequence), 0 },
            }
        );
    }
}
