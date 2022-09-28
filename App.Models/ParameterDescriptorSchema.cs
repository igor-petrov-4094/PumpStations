using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    /// <summary>
    /// Схема параметров (набор групп параметров)
    /// </summary>
    public class ParameterDescriptorSchema
    {
        /// <summary>
        /// Имя схемы
        /// </summary>
        public string name { get; set; } = "";

        /// <summary>
        /// Ссылки на идентификаторы групп
        /// </summary>
        public List<string> groupStringIdList = new List<string>();
    }
}
