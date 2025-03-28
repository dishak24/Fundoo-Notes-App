using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class NotesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotesId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Remainder { get; set; }
        public string Colour { get; set; }
        public bool IsArchived { get; set; }
        public bool IsPinned { get; set; }
        public bool IsTrashed { get; set; }

        //Foreign key (UserId) references on UserEntity(UserId)
        [ForeignKey("NotesUser")]
        public int UserId { get; set; }

        // [JsonIgnore]--BCoz we want only UserId column.
        // If we not write JsonIgnote then it takes all coloumns from UserEmtity that we dont want.
        [JsonIgnore]
        public virtual UserEntity NotesUser { get; set; }


    }
}
