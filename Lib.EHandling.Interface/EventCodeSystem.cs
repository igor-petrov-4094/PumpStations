using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.EHandling.Interface
{
    /// <summary>
    /// Система кодов событий
    /// </summary>
    public class EventCodeSystem
    {
        /// <summary>
        /// Имя
        /// </summary>
        public readonly string name;

        /// <summary>
        /// Текстовый Id
        /// </summary>
        public readonly string textId;

        /// <summary>
        /// Конструктор EventCodeSystem
        /// </summary>
        /// <param name="_textId">Идентификатор системы</param>
        /// <param name="_name">Имя системы. Если равно null, будет использован идентификатор</param>
        public EventCodeSystem(
            string _textId,
            string _name = null
        )
        {
            textId = _textId;
            name = _name ?? _textId;
        }
    }
}
