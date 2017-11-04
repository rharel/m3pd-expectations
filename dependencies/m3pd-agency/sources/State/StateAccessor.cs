namespace rharel.M3PD.Agency.State
{
    /// <summary>
    /// Allows access to state components.
    /// </summary>
    public interface StateAccessor
    {
        /// <summary>
        /// Gets the component corresponding to the specified type.
        /// </summary>
        /// <typeparam name="T">The type of component to get.</typeparam>
        /// <param name="id">The component's identifier.</param>
        /// <returns>
        /// The component corresponding to the specified type.
        /// </returns>
        T Get<T>(string id);
    }
}
