using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class NotesManager : INotesManager
    {
        private readonly INotesRepo notesRepo;

        public NotesManager(INotesRepo notesRepo)
        {
            this.notesRepo = notesRepo;
        }

        //Adding new Note
        public NotesEntity CreateNote(int userId, NotesModel notesModel)
        {
            return notesRepo.CreateNote(userId, notesModel);
        }

       
    }
}
