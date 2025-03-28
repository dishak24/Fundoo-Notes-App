using CommonLayer.Models;
using ManagerLayer.Interfaces;
//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesManager notesManager;
        public NotesController(INotesManager notesManager)
        {
            this.notesManager = notesManager;
        }

        //API for Creating/adding new Note
        [HttpPost]
        [Route("AddNote")]
        public IActionResult CreateNote(NotesModel notesModel)
        {
            try
            {
               int userId = int.Parse(User.FindFirst("UserId").Value);

                var note = notesManager.CreateNote(userId, notesModel);
                if (note != null)
                {
                    return Ok(new ResponseModel<NotesEntity>
                    {
                        Success = true,
                        Message = "Done, Note Added successfully !",
                        Data = note

                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Adding Notes Failed !!!!!"

                    });
                }

            }
            catch(Exception e)
            {
                throw e;
            }
        }

        
    }
}
