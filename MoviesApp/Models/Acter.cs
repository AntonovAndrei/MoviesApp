using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Models
{
    public class Acter
    {
        public Acter()
        {
            this.ActersList = new HashSet<ActerMovie>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime BirthdayDate { get; set; }

        //зачем делать виртуальными?
        public virtual ICollection<ActerMovie> ActersList { get; set; }
    }
}