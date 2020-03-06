using System;

namespace Common_Library.Random {

    /// <summary>
    /// Interface for Random Selector Builders.
    /// </summary>
    /// <typeparam name="T">Type of items that gets randomly returned</typeparam>
    public interface IRandomSelectorBuilder<out T> {

        IRandomSelector<T> Build(Int32 seed=-1);
    }    
}