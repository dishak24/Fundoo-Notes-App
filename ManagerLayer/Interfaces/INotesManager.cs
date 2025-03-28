using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interfaces
{
    public interface INotesManager
    {
        //Create Note
        public NotesEntity CreateNote(int userId, NotesModel notesModel);

        //Get All Notes
        public List<NotesEntity> GetAllNotes();
    }
}
