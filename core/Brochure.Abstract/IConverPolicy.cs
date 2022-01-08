using System;
using System.Text.Json;

namespace Brochure.Abstract
{
    /// <summary>
    /// The conver policy.
    /// </summary>
    public interface IConverPolicy
    {
        /// <summary>
        /// Convers the to.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A T2.</returns>
        T2 ConverTo<T1, T2>(T1 model) where T1 : class
        where T2 : class, new();
    }

    /// <summary>
    /// The conver policy.
    /// </summary>
    public interface IConverPolicy<T1, T2> where T1 : class
        where T2 : class, new()
    {
        /// <summary>
        /// Convers the to.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A T2.</returns>
        T2 ConverTo(T1 model);
    }

    /// <summary>
    /// The conver policy.
    /// </summary>
    public interface IConverPolicy<T> where T : class
    {
        /// <summary>
        /// Convers the to.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A T.</returns>
        T ConverTo(T obj = null);
    }
}