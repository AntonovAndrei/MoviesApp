using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels
{
    public class InputActerViewModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime BirthdayDate { get; set; }
    }
}