using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Lib.Hardware.Interface
{
    /// <summary>
    /// Абстрактный тип памяти контроллера
    /// </summary>
    public enum AbstractMemoryType
    {
        /// <summary>
        /// D-память
        /// </summary>
        D = 1,

        /// <summary>
        /// Битовая память
        /// </summary>
        CIO = 2,

        /// <summary>
        /// W-память
        /// </summary>
        W = 3,
    }
}
