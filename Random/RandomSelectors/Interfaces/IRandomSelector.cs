using System.Collections;
using System.Collections.Generic;
using System;


namespace Common_Library.Random {

    /// <summary>
    /// Interface for Random selector
    /// </summary>
    /// <typeparam name="T">Type of items that gets randomly returned</typeparam>
    public interface IRandomSelector<out T> {

        T SelectRandomItem();
        T SelectRandomItem(Single randomValue);
    }
}