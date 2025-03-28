using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace CommonLayer.Models
{
    public class NotesModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        
    }
}
