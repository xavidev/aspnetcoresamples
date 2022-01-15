using System.ComponentModel.DataAnnotations;

namespace DummyMVCApp.Controllers
{
    public class DummyModel
    {
        [Required]
        public string Id { get; set; }
    }
}