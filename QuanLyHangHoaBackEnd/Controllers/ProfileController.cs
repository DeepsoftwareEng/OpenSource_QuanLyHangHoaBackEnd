using Microsoft.AspNetCore.Mvc;
using QuanLyHangHoaEsteel_BackEnd.Models;
using QuanLyHangHoaEsteel_BackEnd.Repository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QuanLyHangHoaEsteel_BackEnd.Controllers
{
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : Controller
    {
        private readonly IProfileRepository profileRepository;

        public ProfileController(IProfileRepository profile)
        {
            this.profileRepository = profile;
        }
        [HttpGet]
        [Route("get-by-username")]
        public IActionResult GetByUserName(string userName)
        {
            try
            {
                var prof = profileRepository.GetByAccount(userName);
                return Ok(new
                {
                    response_code = 200,
                    response_data = prof
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    response_code = 300,
                    response_data = new Profile(),
                });
            }  
        }
        [HttpPost]
        [Route("edit-profile")]
        public IActionResult AddProfile(string userName, Profile profile)
        {
            var checkExists = profileRepository.GetByAccount(userName);
            if(checkExists == null)
            {
                var add = profileRepository.AddProfile(profile, userName);
                if (!add)
                {
                    return Ok(new
                    {
                        response_code = 300,
                        response_data = "Lỗi khi thêm hồ sơ"
                    });
                }
            }
            else
            {
                var update = profileRepository.UpdateProfile(profile);
                if (!update)
                {
                    return Ok(new
                    {
                        response_code = 300,
                        response_data = "Lỗi khi thêm hồ sơ"
                    });
                }
            }
            
            return Ok(new
            {
                response_code = 200,
                response_data = "Sửa hồ sơ thành công"
            });
        }
        [HttpDelete]
        [Route("delete-profile")]
        public IActionResult DeleteProfile(string userName)
        {
            var prof = profileRepository.GetByAccount(userName);
            var del = profileRepository.DeleteProfile((int)prof.Id);
            if (!del)
            {
                return Ok(new
                {
                    response_code = 300,
                    response_data = "Lỗi khi xóa hồ sơ"
                });
            }
            return Ok(new
            {
                response_code = 200,
                response_data = "Xóa hồ sơ thành công"
            });
        }
    }
}
