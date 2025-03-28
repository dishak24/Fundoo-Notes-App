using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class NotesRepo : INotesRepo
    {
        private readonly FundooDBContext context;

        public NotesRepo(FundooDBContext context)
        {
            this.context = context;
        }

        //Adding new Note
        public NotesEntity CreateNote(int userId, NotesModel notesModel)
        {
            NotesEntity notes = new NotesEntity();

            notes.UserId = userId;

            notes.Title = notesModel.Title;
            notes.Description = notesModel.Description;

            //adding values in DB
            context.Add(notes);
            context.SaveChanges();
            return notes;
        }

        
    }
}
