using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    /// <summary>
    /// Группа параметров
    /// </summary>
    public class ParameterDescriptorGroupDescriptor
    {
        /// <summary>
        /// Идентификатор группы
        /// </summary>
        public string groupStringId { get; set; } = "";

        /// <summary>
        /// Ссылки на идентификаторы параметров
        /// </summary>
        public List<string> parameterStringIdList = new List<string>();

        /// <summary>
        /// Имя группы
        /// </summary>
        public string readableName { get; set; } = "";
    }
}
