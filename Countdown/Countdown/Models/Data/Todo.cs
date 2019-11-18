using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Countdown.Models.Data
{

    public class Todo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        
        public string Title { get; set; }

        [StringLength(200)]
        public string Description { get; set; }


        [Required]
        
        public DateTime StartDate { get; set; }

      


        [Required]
        public DateTime DueDate { get; set; }


    

        [Required]
        public string AssignedTo { get; set; }

        [Required]
        public string Owner { get; set; }

       
    }
}