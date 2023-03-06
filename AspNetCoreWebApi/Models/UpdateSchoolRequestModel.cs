using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebApi.Models
{
    public class UpdateSchoolRequestModel
    {
        [Required(ErrorMessage = "School Name tidak boleh kosong.")]
        [StringLength(32, MinimumLength = 3, ErrorMessage = "School Name harus diantara 3 - 32 karakter.")]
        public string? SchoolName { get; set; }
    }
}
