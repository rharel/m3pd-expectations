﻿namespace rharel.M3PD.Agency.Dialogue_Moves
{
    /// <summary>
    /// Contains extension methods for dialogue moves.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Determines whether the specified move is the idle move.
        /// </summary>
        /// <param name="move">The move to test.</param>
        /// <returns>
        /// True iff <paramref name="move"/> equals <see cref="Idle.Instance"/>.
        /// </returns>
        public static bool IsIdle(this DialogueMove move)
        {
            return move.Equals(Idle.Instance);
        }
    }
}
