using System.ComponentModel.DataAnnotations;

namespace OnlineLearning.Service.ViewModels
{
    public class RoleFormViewModel
    {
        [Required, StringLength(256)]
        public string Name { get; set; }
    }
}