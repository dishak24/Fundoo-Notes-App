using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface INotesRepo
    {
        //Create note
        public NotesEntity CreateNote(int userId, NotesModel notesModel);

       
    }
}
