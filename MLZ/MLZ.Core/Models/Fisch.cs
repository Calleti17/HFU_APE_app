using SQLite;
using System;

namespace MLZ.Core.Models
{
    public class Fisch
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Lake { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Method { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public int Count { get; set; } 
        public string FormattedDate => Date.ToString("dd.MM.yyyy HH:mm:ss");

        
    }
}
